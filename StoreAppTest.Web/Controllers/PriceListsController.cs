
namespace StoreAppTest.Web.Controllers
{
    extern alias ef;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http.Results;
    using DataModel;
    using System.Web.Http;
    using ef::System.Data.Entity;
    using Kent.Boogaart.KBCsv;
    using NLog;
    using Utilities;
    using StoreAppTest.Utilities;

    public class PriceListsController : ApiController
    {
        public IHttpActionResult LoadPriceList(PriceList priceList)
        {
            //Saving pricelist
            using (StoreDbContext context = new StoreDbContext())
            {
                try
                {
                    var gears = priceList.PriceItems.Select(s => s.Gear).ToList().ToList();
                    var gearsFromDb = context.Gears.ToList();
                    foreach (var gear in gears.ToList())
                    {

                        if (!gearsFromDb.Any(g => g.Name == gear.Name && g.Articul == gear.Articul))
                            context.Gears.Add(gear);
                        //else
                        //{
                        //    var gearIndex = gears.IndexOf(gear);
                        //    var gearFromDb = gearsFromDb.FirstOrDefault(
                        //        g => g.Name == gear.Name && g.Articul == gear.Articul);

                        //    gears[gearIndex] = gearFromDb;
                        //}
                    }
                    context.SaveChanges();

                    var uoms = priceList.PriceItems.Select(s => s.UnitOfMeasure).DistinctBy(d => d.Name).ToList();
                    var uomsFromDb = context.UnitOfMeasures.ToList();
                    foreach (var uom in uoms.ToList())
                    {
                        if (!uomsFromDb.Any(u => u.Name == uom.Name))
                            context.UnitOfMeasures.Add(uom);
                        else
                        {
                            uoms[uoms.IndexOf(uom)] =
                                uomsFromDb.FirstOrDefault(
                                    u => u.Name == uom.Name);
                        }
                    }
                    context.SaveChanges();

                    var warehouse = priceList.UploadUser.Warehouse;
                    var warehouseFromDb = context.Warehouses.Where(w => w.Name == warehouse.Name).FirstOrDefault();
                    if (warehouseFromDb != null)
                        priceList.UploadUser.Warehouse = warehouseFromDb;
                    else
                    {
                        context.Warehouses.Add(warehouse);
                        context.SaveChanges();
                    }

                    var user = priceList.UploadUser;
                    var userFromDb = context.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
                    if (userFromDb != null)
                        priceList.UploadUser = userFromDb;
                    else
                    {
                        context.Users.Add(user);
                        context.SaveChanges();
                    }


                    var remainders = priceList.PriceItems.SelectMany(s => s.Remainders).ToList();
                    remainders.ForEach(f =>
                    {
                        f.Warehouse = warehouseFromDb;
                    });


                    RepairPriceItemUomReferences(priceList.PriceItems, uoms);
                    RepairPriceItemGearReferences(priceList.PriceItems, gears);

                    context.PriceLists.Add(priceList);
                    context.SaveChanges();

                }
                catch (Exception e)
                {
                    return new ExceptionResult(e, this);
                }
            }

            return new OkResult(this);
        }

        public IHttpActionResult UpdatePriceList(PriceList priceList)
        {
            using (StoreDbContext context = new StoreDbContext())
            {
                try
                {

                }
                catch (Exception e)
                {
                    return new ExceptionResult(e, this);
                }
            }

            return new OkResult(this);
        }

        private void RepairPriceItemUomReferences(IEnumerable<PriceItem> priceItems,
            IEnumerable<UnitOfMeasure> uoms)
        {
            IDictionary<string, UnitOfMeasure> uomDictionary = new Dictionary<string, UnitOfMeasure>();
            foreach (var uom in uoms)
            {
                uomDictionary.Add(uom.Name, uom);
            }


            foreach (var priceItem in priceItems)
            {
                var uom = uomDictionary[priceItem.Uom_Id];
                priceItem.UnitOfMeasure = uom;
            }
        }

        private void RepairPriceItemGearReferences(IEnumerable<PriceItem> priceItems,
            IEnumerable<Gear> gears)
        {
            //IDictionary<long, Gear> gearsDictionary = new Dictionary<long, Gear>();
            //foreach (var gear in gears.ToList())
            //{
            //    gearsDictionary.Add(gear.Id, gear);
            //}

            foreach (var priceItem in priceItems)
            {
                //var gear = gearsDictionary[priceItem.Gear_Id];
                //priceItem.Gear = gear;
                priceItem.Gear_Id = priceItem.Gear.Id;
            }
        }


        [HttpPost]
        public async Task<IHttpActionResult> LoadPriceFile(
            [FromUri]string filePath,
            [FromUri]string fileName,
            [FromUri]int packageCount,
            [FromUri]int packageNumber)
        {
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + filePath);
            var fullPath = Path.Combine(mappedPath, Path.GetFileName(fileName));


            FileMode fileMode = File.Exists(fullPath) && packageNumber > 0
                ? FileMode.Append
                : FileMode.Create;

            var stream = await ControllerContext.Request.Content.ReadAsStreamAsync();
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(File.Open(fullPath, fileMode)))
            {
                byte[] buffer = new byte[4096];
                int byteRead;
                while ((byteRead = reader.Read(buffer, 0, buffer.Length)) != 0)
                {
                   writer.Write(buffer, 0, byteRead); 
                }
            }

            return new OkResult(this);
        }

        [HttpPost]
        public IHttpActionResult LoadPriceListFile([FromUri]string priceListFileName)
        {
            var priceListName = Path.GetFileNameWithoutExtension(priceListFileName);

            var currentUserName = HttpContext.Current.Request.Headers["Authorization"].Replace("user=", "").Trim();

            var context = new StoreDbContext();

            try
            {
                var currentUser =
                    context.Users.Where(w => w.UserName == currentUserName).FirstOrDefault();


                var uoms = context.UnitOfMeasures.ToList();

                bool priceListIsNew = true;
                var priceList = context.PriceLists
                    //.Include("PriceItems")
                    //.Include("PriceItems.Gear")
                    //.Include("PriceItems.UnitOfMeasure")
                    //.Include("PriceItems.Remainders")
                    .Where(w => w.Name == priceListName).FirstOrDefault();

                if (priceList == null)
                {
                    priceList = new PriceList();
                    priceList.Name = priceListName;

                    priceList.UploadDate = DateTimeHelper.GetNowKz();
                    priceList.UploadUser_Id = currentUser.UserName;
                    context.PriceLists.Add(priceList);
                    context.SaveChanges();
                }
                else
                {
                    priceListIsNew = false;
                }


                var sb = new StringBuilder();
                sb.Append("SELECT  ");
                sb.Append("PR.Id AS PriceItem_Id, ");
                sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
                sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
                sb.Append("PR.Uom_Id AS Uom, ");
                sb.Append("GR.Id AS Gear_Id, ");
                sb.Append("GR.CatalogNumber AS CatalogNumber, ");
                sb.Append("GR.Articul AS Articul, ");
                sb.Append("GR.IsDuplicate AS IsDuplicate, ");
                sb.Append("GR.Name AS Gear_Name, ");
                sb.Append("0 AS RecommendedRemainder, ");
                sb.Append("0 AS LowerLimitRemainder, ");
                sb.Append("WHPSD.CreatedDate AS WholesalePriceDate, ");
                sb.Append("WHPS.Price AS WholesalePrice ");
                sb.Append("FROM [PriceItems] AS PR ");
                sb.Append("LEFT JOIN [Gears] AS GR ON PR.Gear_Id = GR.Id ");
                sb.Append("LEFT JOIN [UnitOfMeasures] AS UM ON PR.Uom_Id = UM.Name ");
                sb.Append("JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
                sb.Append("JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
                sb.Append("LEFT JOIN  ");
                sb.Append("( ");
	            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
	            sb.Append("FROM [WholesalePrices] ");
	            sb.Append("GROUP BY PriceItem_Id ");
                sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
                sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");
                //sb.Append("WHERE  PRPRL.PriceList_Name = @p0");

                var priceItems = context.Database.SqlQuery<PriceItemView>(sb.ToString()).ToList();



                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Upload");
                var fullPath = Path.Combine(mappedPath, Path.GetFileName(priceListFileName));

                using (var freader = File.OpenRead(fullPath))
                using (var reader = new CsvReader(freader, Encoding.GetEncoding(1251)))
                {
                    reader.ValueSeparator = ';';
                    reader.ReadHeaderRecord();

                    while (reader.HasMoreRecords)
                    {
                        var record = reader.ReadDataRecord();

                        string catalogNumber = string.Empty;
                        bool isDupl = false;
                        string name = string.Empty;
                        decimal buyPriceRur = 0.00m;
                        decimal buyPriceTn = 0.00m;
                        string uom = string.Empty;
                        decimal wholesalePrice = 0.00m;
                        decimal remainders = 0;
                        string articul = string.Empty;

                        catalogNumber = record["Номер детали"];
                        var isDublStr = record["Дубл."];
                        if (isDublStr != string.Empty)
                            isDupl = true;
                        name = record["Наименование"];
                        var buyR = record["Цена закуп (руб, ю, тг)"];
                        if (buyR != string.Empty)
                            decimal.TryParse(buyR, out buyPriceRur);
                        var buyT = record["Цена закуп (тг)"];
                        if (buyT != string.Empty)
                            decimal.TryParse(buyT, out buyPriceTn);
                        uom = record["Ед. Измерения"];
                        var priceS = record["Цена"];
                        if (priceS != string.Empty)
                            decimal.TryParse(priceS, out wholesalePrice);
                        var remainderS = record["Остаток"];
                        if (remainderS != string.Empty)
                            decimal.TryParse(remainderS, out remainders);
                        articul = record["артикул"];


                        if (name == string.Empty || uom == string.Empty) continue;

                        var priceItem = priceItems.Where(w => w.Articul == articul
                                                                              && w.CatalogNumber == catalogNumber
                                                                              && w.Gear_Name == name).FirstOrDefault();

                        PriceItem findedPriceListItem = null;

                        if (priceItem == null)
                        {
                            findedPriceListItem = new PriceItem();

                            var findedGear = new Gear();
                            findedGear.Articul = articul;
                            findedGear.CatalogNumber = catalogNumber;
                            findedGear.Category_Id = "Новинки"; //TODO: переделать на чето типа "Обычные"
                            findedGear.IsDuplicate = isDupl;
                            findedGear.Name = name;
                            context.Gears.Add(findedGear);
                            context.SaveChanges();

                            //priceItem.Gear_Id = findedGear.Id;
                            //findedPriceListItem.Gear = findedGear;
                            findedPriceListItem.Gear_Id = findedGear.Id;

                            var findedUom = uoms.Where(w => w.Name == uom).FirstOrDefault();
                            if (findedUom == null)
                            {
                                findedUom = new UnitOfMeasure();
                                findedUom.Name = uom;
                                context.UnitOfMeasures.Add(findedUom);
                                context.SaveChanges();
                                uoms.Add(findedUom);
                            }
                            findedPriceListItem.Uom_Id = findedUom.Name;
                            findedPriceListItem.BuyPriceRur = buyPriceRur;
                            findedPriceListItem.BuyPriceTng = buyPriceTn;
                            

                            //findedPriceListItem.PriceList_Id = priceList.Name;
                            context.PriceItems.Add(findedPriceListItem);
                            context.SaveChanges();

                            findedPriceListItem.PriceLists.Add(priceList);
                            priceList.PriceItems.Add(findedPriceListItem);
                            context.SaveChanges();


                            Remainder rm = new Remainder();
                            rm.Amount = remainders;
                            rm.Warehouse_Id = currentUser.Warehouse.Name;
                            //rm.Warehouse = currentUser.Warehouse;
                            rm.RemainderDate = DateTimeHelper.GetNowKz();
                            rm.PriceItem_Id = findedPriceListItem.Id;
                            context.Remainders.Add(rm);
                            context.SaveChanges();


                            WholesalePrice price = new WholesalePrice();
                            price.Price = wholesalePrice;
                            price.PriceDate = DateTimeHelper.GetNowKz();
                            //price.PriceItem = findedPriceListItem;
                            price.PriceItem_Id = findedPriceListItem.Id;
                            context.WholesalePrices.Add(price);

                            context.SaveChanges();

                            //findedPriceListItem.Remainders.Add(rm);
                            //findedPriceListItem.Prices.Add(price);
                            
                        }
                        else
                        {
                            findedPriceListItem = context.PriceItems.Include("Prices").Where(w => w.Gear_Id == priceItem.Gear_Id).First();
                            findedPriceListItem.BuyPriceRur = buyPriceRur;
                            findedPriceListItem.BuyPriceTng = buyPriceTn;
                            context.SaveChanges();

                            if(priceListIsNew)
                                findedPriceListItem.PriceLists.Add(priceList);

                            var lastPrice =
                                findedPriceListItem.Prices.OrderByDescending(o => o.PriceDate).FirstOrDefault();

                            if (lastPrice != null)
                            {
                                if (priceItem.WholesalePrice != lastPrice.Price)
                                {
                                    WholesalePrice price = new WholesalePrice();
                                    price.Price = wholesalePrice;
                                    price.PriceDate = DateTimeHelper.GetNowKz();
                                    //price.PriceItem = findedPriceListItem;
                                    price.PriceItem_Id = findedPriceListItem.Id;
                                    context.WholesalePrices.Add(price);
                                    findedPriceListItem.Prices.Add(price);

                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                WholesalePrice price = new WholesalePrice();
                                price.Price = wholesalePrice;
                                price.PriceDate = DateTimeHelper.GetNowKz();
                                //price.PriceItem = findedPriceListItem;
                                price.PriceItem_Id = findedPriceListItem.Id;
                                context.WholesalePrices.Add(price);
                                findedPriceListItem.Prices.Add(price);

                                context.SaveChanges();
                            }
                            
                            //findedPriceListItem.Remainders.Where(r => r.Warehouse_Id == currentUser.Warehouse.Name)
                            //    .FirstOrDefault()
                            //    .Amount = remainders;
                            if (!findedPriceListItem.Remainders.Any(r => r.Warehouse_Id == currentUser.Warehouse.Name))
                            {
                                Remainder rm = new Remainder();
                                rm.Amount = remainders;
                                rm.Warehouse_Id = currentUser.Warehouse.Name;
                                //rm.Warehouse = currentUser.Warehouse;
                                rm.RemainderDate = DateTimeHelper.GetNowKz();
                                rm.PriceItem_Id = findedPriceListItem.Id;
                                context.Remainders.Add(rm);
                                context.SaveChanges();
                            }
                            else
                            {
                                var findedReminders =
                                    findedPriceListItem.Remainders.Where(
                                        r => r.Warehouse_Id == currentUser.Warehouse.Name).FirstOrDefault();
                                findedReminders.Amount = remainders;
                                findedReminders.RemainderDate = DateTimeHelper.GetNowKz();
                                context.Entry(findedReminders).State = EntityState.Modified;
                                context.SaveChanges();

                            }

                        }

                        //findedPriceListItem.BuyPriceRur = buyPriceRur;
                        //findedPriceListItem.BuyPriceTng = buyPriceTn;

                        //context.SaveChanges();
                        //findedPriceListItem.WholesalePrice = wholesalePrice;
                    }

                    //context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                //return new ExceptionResult(exception, this);
                return new StatusCodeResult(HttpStatusCode.ExpectationFailed, this);
            }

            return new OkResult(this);        
        }



        [HttpPost]
        public HttpResponseMessage LogIn([FromUri]string userName, [FromUri]string passwordHash)
        {
            var context = new StoreDbContext();

            var user = context.Users.Where(w => w.UserName == userName).FirstOrDefault();
            if(user == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);//new StatusCodeResult(HttpStatusCode.Unauthorized, this);

            //var securePwd = SecUtility.Encrypt("store_app_test", passwordHash);
            var pwdString = Encoding.Unicode.GetString(user.PasswordHash ?? new byte[] { });
            var pwdBytesString = pwdString != "" ? SecUtility.Decrypt("store_app_test", pwdString) : "";

            if (pwdBytesString != (passwordHash ?? ""))
                return Request.CreateResponse(HttpStatusCode.Unauthorized);//new StatusCodeResult(HttpStatusCode.Unauthorized, this);

            //HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user.UserName), new string[]{} );
            var response = Request.CreateResponse(HttpStatusCode.Accepted, user);
            return response;
        }


        [HttpPost]
        public HttpResponseMessage Register([FromUri]string userName, [FromUri]string displayName, [FromUri]string passwordHash, [FromUri]string warehouse = "")
        {
            var context = new StoreDbContext();
            var user = context.Users.Where(w => w.UserName == userName).FirstOrDefault();
            if (user != null)
                return Request.CreateResponse(HttpStatusCode.Ambiguous);//new StatusCodeResult(HttpStatusCode.Unauthorized, this);

            user = new User();
            user.DisplayName = displayName;
            user.UserName = userName;
            if (warehouse != null)
                user.Warehouse_Id = warehouse;

            var securePwd = SecUtility.Encrypt("store_app_test", passwordHash);
            var pwdBytes = Encoding.Unicode.GetBytes(securePwd);

            user.PasswordHash = pwdBytes;

            context.Users.Add(user);
            context.SaveChanges();

            var response = Request.CreateResponse(HttpStatusCode.Accepted, user);
            return response;

        }


        [HttpPost]
        public HttpResponseMessage SetPassword([FromUri]string userName, [FromUri]string passwordHash)
        {

            var context = new StoreDbContext();
            var user = context.Users.Where(w => w.UserName == userName).FirstOrDefault();
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);//new StatusCodeResult(HttpStatusCode.Unauthorized, this);

            
            var securePwd = SecUtility.Encrypt("store_app_test", passwordHash);
            var pwdBytes = Encoding.Unicode.GetBytes(securePwd);

            user.PasswordHash = pwdBytes;
            context.SaveChanges();

            //HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user.UserName), new string[]{} );
            var response = Request.CreateResponse(HttpStatusCode.Accepted, user);
            return response;
        }


        [HttpGet]

        public IEnumerable<PriceItemRemainderView> GetAllPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("RM.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("RM.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("RM.Amount AS Remainders, ");
            sb.Append("WHPS.Price AS WholesalePrice, ");
            sb.Append("'' AS Supplier ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("LEFT JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
            sb.Append("LEFT JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");
            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("WHERE PRPRL.PriceList_Name = @p0 ");
                parameters[0] = priceListName;
            }

            var context = new StoreDbContext();
            var query = context.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query.AsNoTracking().ToList();
        }

        [HttpGet]

        public IEnumerable<PriceItemRemainderView> GetNewsPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("RM.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("RM.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("RM.Amount AS Remainders, ");
            sb.Append("WHPS.Price AS WholesalePrice, ");
            sb.Append("'' AS Supplier ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("LEFT JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT Gear_Id, MAX(CreatedDate) AS CreatedDate FROM [dbo].[GearNews]  ");
            sb.Append("GROUP BY Gear_Id ");
            sb.Append(") AS GRN ON GR.Id = GRN.Gear_Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");

            sb.Append(" WHERE YEAR(GRN.CreatedDate) = YEAR(GETDATE()) AND MONTH(GRN.CreatedDate) >= MONTH(DATEADD(MONTH, -1 , GETDATE())) ");

            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND PRPRL.PriceList_Name = @p0 ");
                parameters[0] = priceListName;
            }

            var context = new StoreDbContext();
            var query = context.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query.AsNoTracking().ToList();
        }


        [HttpGet]

        public IEnumerable<PriceItemRemainderView> GetSucksPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            //sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("RM.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("RM.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("RM.Amount AS Remainders, ");
            sb.Append("WHPS.Price AS WholesalePrice, ");
            sb.Append("'' AS Supplier ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("LEFT JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");

            sb.Append("WHERE YEAR(RM.RemainderDate) = YEAR(GETDATE()) AND MONTH(RM.RemainderDate) = MONTH(DATEADD(MONTH, -3 , GETDATE())) AND RM.Amount = 0 ");

            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND PRPRL.PriceList_Name = @p0 ");
                parameters[0] = priceListName;
            }

            var context = new StoreDbContext();
            var query = context.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query.AsNoTracking().ToList();
        }


        [HttpGet]
        public IEnumerable<PriceItemRemainderView> GetLowerLimitPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id,  ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur,  ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng,  ");
            sb.Append("PR.Uom_Id AS Uom,  ");
            sb.Append("GR.Id AS Gear_Id,  ");
            sb.Append("GR.CatalogNumber AS CatalogNumber,  ");
            sb.Append("GR.Articul AS Articul,  ");
            sb.Append("GR.IsDuplicate AS IsDuplicate,  ");
            sb.Append("GR.Name AS Gear_Name,  ");
            sb.Append("RM.RecommendedRemainder AS RecommendedRemainder,  ");
            sb.Append("RM.LowerLimitRemainder AS LowerLimitRemainder,  ");
            sb.Append("RM.Id AS Remainder_Id,  ");
            sb.Append("RM.Warehouse_Id AS Warehouse,  ");
            sb.Append("RM.RemainderDate AS RemainderDate,  ");
            sb.Append("RM.Amount AS Remainders,  ");
            sb.Append("WHPS.Price AS WholesalePrice  ");
            sb.Append(",INC.Supplier_Id AS Supplier  ");

            sb.Append("FROM [dbo].[PriceItems] AS PR  ");
            sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id  ");
            sb.Append("LEFT JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name  ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id  ");
            sb.Append("LEFT JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id  ");
            sb.Append("LEFT JOIN   ");
            sb.Append("(  ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate  ");
            sb.Append("FROM [WholesalePrices]  ");
            sb.Append("GROUP BY PriceItem_Id  ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id  ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate  ");
            sb.Append("LEFT JOIN [dbo].[IncomeItems] AS INCI ON PR.Id = INCI.PriceItem_Id ");
            sb.Append("LEFT JOIN [dbo].[Incomes] AS INC_T ON INCI.Income_Id = INC_T.Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT Supplier_Id ");
            sb.Append("FROM [dbo].[Incomes] ");
            sb.Append("WHERE IsAccept  = 1 ");
            sb.Append("GROUP BY Supplier_Id ");

            sb.Append(") AS INC ON INC_T.Supplier_Id = INC.Supplier_Id ");
            sb.Append("WHERE RM.Amount <= RM.LowerLimitRemainder AND RM.Amount > 0 ");



            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND PRPRL.PriceList_Name = @p0 ");
                parameters[0] = priceListName;
            }
            sb.Append("GROUP BY PR.Id, PR.BuyPriceRur, PR.BuyPriceTng, PR.Uom_Id, GR.Id, GR.Id, GR.CatalogNumber,  ");
            sb.Append("GR.Articul, GR.IsDuplicate, GR.Name, RM.RecommendedRemainder, RM.LowerLimitRemainder, RM.Id,  ");
            sb.Append("RM.Warehouse_Id, RM.RemainderDate, RM.Amount, WHPS.Price, INC.Supplier_Id ");

            var context = new StoreDbContext();
            var query = context.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query.AsNoTracking().ToList();
        }


        [HttpGet]
        public IEnumerable<PriceItemRemainderView> GetIncomesPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();


sb.Append("SELECT  ");

sb.Append("PR.Id AS PriceItem_Id,  ");
sb.Append("PR.BuyPriceRur AS BuyPriceRur,  ");
sb.Append("PR.BuyPriceTng AS BuyPriceTng,  ");
sb.Append("PR.Uom_Id AS Uom, ");
sb.Append("GR.Id AS Gear_Id,  ");
sb.Append("GR.CatalogNumber AS CatalogNumber,  ");
sb.Append("GR.Articul AS Articul,  ");
sb.Append("GR.IsDuplicate AS IsDuplicate,  ");
sb.Append("GR.Name AS Gear_Name, ");
sb.Append("RM.RecommendedRemainder AS RecommendedRemainder,  ");
sb.Append("RM.LowerLimitRemainder AS LowerLimitRemainder, ");
sb.Append("RM.Id AS Remainder_Id, ");
sb.Append("RM.Warehouse_Id AS Warehouse, ");
sb.Append("RM.RemainderDate AS RemainderDate, ");
sb.Append("RM.Amount AS Remainders, ");
sb.Append("WHPS.Price AS WholesalePrice, ");
sb.Append("INCO.Supplier_Id AS Supplier ");

sb.Append("FROM ( ");
		sb.Append("SELECT MAX(Id) AS Id ");
		sb.Append("FROM [dbo].[Incomes]  ");
		//sb.Append("GROUP BY Id	  ");
	 sb.Append(") AS LSTINC ");
sb.Append("JOIN [dbo].[Incomes] AS INCO ON LSTINC.Id = INCO.Id ");
sb.Append("LEFT JOIN [dbo].[IncomeItems] AS INC ON INCO.Id = INC.Income_Id ");
sb.Append("LEFT JOIN [dbo].[PriceItems] AS PR ON INC.PriceItem_Id = PR.Id ");
sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
sb.Append("LEFT JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
sb.Append("LEFT JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
sb.Append("LEFT JOIN  (  ");
			sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate  ");
			sb.Append("FROM [WholesalePrices] GROUP BY PriceItem_Id  ");
			sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id  ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");
            sb.Append("WHERE PRPRL.PriceList_Name = @p0 ");
            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND INCO.IsAccept = 1 ");
                parameters[0] = priceListName;
            }

            var context = new StoreDbContext();
            var query = context.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query.AsNoTracking().ToList();


        }


        [HttpGet]
        public IEnumerable<PriceIncomeTotalItemView> GetTotalIncomes(string priceListName)
        {

            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            //sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("0 AS RecommendedRemainder, ");
            sb.Append("0 AS LowerLimitRemainder, ");
            sb.Append("RM.Amount AS Remainders ");
            sb.Append(",INCI.Amount AS Incomes ");
            sb.Append(",INCI.Income_Id AS Income_Id ");
            sb.Append(",INC.IsAccept ");
            sb.Append(",INCI.NewPrice AS NewPrice, ");
            sb.Append("PRS.Price AS WholesalePrice ");


            sb.Append("FROM [IncomeItems] AS INCI ");
            sb.Append("JOIN [Incomes] AS INC ON INCI.Income_Id = INC.Id ");
            sb.Append("JOIN [PriceItems] AS PR ON INCI.PriceItem_Id = PR.Id ");
            sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("LEFT JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN ( ");
            sb.Append("SELECT PriceItem_Id, SUM(Amount) AS Amount ");
            sb.Append("FROM [Remainders] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN [Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT MAX(PriceDate) AS PriceDate, PriceItem_Id ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS PRSD ON PR.Id = PRSD.PriceItem_Id ");
            sb.Append("LEFT JOIN [WholesalePrices] AS PRS ON PRSD.PriceDate = PRS.PriceDate AND PRSD.PriceItem_Id = PRS.PriceItem_Id ");

            sb.Append("WHERE PRPRL.PriceList_Name = @p0 ");

            var context = new StoreDbContext();
            var query = context.Set<PriceIncomeTotalItemView>()
                .SqlQuery(sb.ToString(), priceListName);


            return query.AsNoTracking().ToList();
        }


        [HttpGet]
        public IEnumerable<PriceIncomeTotalItemView> GetIncomes(string priceListName, string warehouse, string creator, string from, string to)
        {

            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            //sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("RMW.RecommendedRemainder AS RecommendedRemainder, ");
                sb.Append("RMW.LowerLimitRemainder AS LowerLimitRemainder, ");
            }
            else
            {
                sb.Append("0 AS RecommendedRemainder, ");
                sb.Append("0 AS LowerLimitRemainder, ");
            }
            sb.Append("RM.Amount AS Remainders ");           
            sb.Append(",INCI.Amount AS Incomes ");
            sb.Append(",INCI.Income_Id AS Income_Id ");
            sb.Append(",INC.IsAccept ");
            sb.Append(",INCI.NewPrice AS NewPrice, ");
            sb.Append("PRS.Price AS WholesalePrice ");
            sb.Append("FROM [IncomeItems] AS INCI ");
            sb.Append("JOIN [Incomes] AS INC ON INCI.Income_Id = INC.Id ");
            sb.Append("JOIN [PriceItems] AS PR ON INCI.PriceItem_Id = PR.Id ");
            sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("LEFT JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN ( ");
            sb.Append("SELECT PriceItem_Id, SUM(Amount) AS Amount, Warehouse_Id ");
            sb.Append("FROM [Remainders] ");
            sb.Append("GROUP BY PriceItem_Id, Warehouse_Id ");
            sb.Append(") AS RM ON PR.Id = RM.PriceItem_Id ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("LEFT JOIN [dbo].[Remainders] AS RMW ON RM.PriceItem_Id = RMW.PriceItem_Id AND RM.Warehouse_Id = RMW.Warehouse_Id ");
            }
            sb.Append("LEFT JOIN [Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT MAX(PriceDate) AS PriceDate, PriceItem_Id ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS PRSD ON PR.Id = PRSD.PriceItem_Id ");
            sb.Append("LEFT JOIN [WholesalePrices] AS PRS ON PRSD.PriceDate = PRS.PriceDate AND PRSD.PriceItem_Id = PRS.PriceItem_Id ");

            sb.Append("WHERE PRPRL.PriceList_Name = @p0 AND INC.Creator_Id = @p1 AND RM.Warehouse_Id = @p2 AND INC.IncomeDate BETWEEN @p3 AND @p4");
            sb.Append(" ORDER BY INC.IncomeDate DESC ");

            var context = new StoreDbContext();
            var query = context.Set<PriceIncomeTotalItemView>()
                .SqlQuery(sb.ToString(), priceListName, creator, warehouse, DateTime.Parse(from), DateTime.Parse(to));


            return query.AsNoTracking().ToList();
        }

        [HttpGet]
        public IEnumerable<PriceListItemRemainderView> GetAllPriceListItems(string warehouse = "")
        {

            var parameters = new ArrayList() { };

            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT  ");

            sb.Append("PRL.Name AS PriceList_Name, ");
            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("WHPS.Price AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("ISNULL(RMW.RecommendedRemainder, CAST(0 AS DECIMAL)) AS RecommendedRemainder, ");
                sb.Append("ISNULL(RMW.LowerLimitRemainder, CAST(0 AS DECIMAL)) AS LowerLimitRemainder, ");
            }
            else
            {
                sb.Append("CAST(0 AS DECIMAL) AS RecommendedRemainder, ");
                sb.Append("CAST(0 AS DECIMAL) AS LowerLimitRemainder, ");
            }
            sb.Append("CAST(0 AS BIGINT) AS Remainder_Id, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("RM.Warehouse_Id AS Warehouse, ");
            }
            else
            {
                sb.Append("'' AS Warehouse, ");
            }
            sb.Append("convert(datetime,'01.01.2015') AS RemainderDate, ");
            sb.Append("ISNULL(RM.Amount, 0) AS Remainders, ");
            sb.Append("'' AS Supplier ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN ( ");
            sb.Append("SELECT PriceItem_Id, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("Warehouse_Id, ");
            }
            sb.Append(" SUM(Amount) AS Amount ");

            sb.Append("FROM [dbo].[Remainders] ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("WHERE Warehouse_Id = @p0 ");
                parameters.Add(warehouse);
            }
            sb.Append("GROUP BY PriceItem_Id ");

            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append(",Warehouse_Id ");
            }
            sb.Append(") AS RM ON PR.Id = RM.PriceItem_Id ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("LEFT JOIN [dbo].[Remainders] AS RMW ON RM.PriceItem_Id = RMW.PriceItem_Id AND RM.Warehouse_Id = RMW.Warehouse_Id ");
            }


            sb.Append("JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");

            var context = new StoreDbContext();
            var query = context.Set<PriceListItemRemainderView>()
                .SqlQuery(sb.ToString(), warehouse);


            return query.AsNoTracking().ToList();

        }

    }
}
