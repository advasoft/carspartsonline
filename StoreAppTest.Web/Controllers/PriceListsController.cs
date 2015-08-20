
namespace StoreAppTest.Web.Controllers
{
    extern alias ef;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;
    using System.Web;
    using System.Web.Http.Results;
    using DataModel;
    using System.Web.Http;
    using System.Xml.Serialization;
    using ef::System.Data.Entity;
    using Kent.Boogaart.KBCsv;
    using Newtonsoft.Json;
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

            var user = context.Users.Include(i => i.Warehouse).Where(w => w.UserName == userName).FirstOrDefault();
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


        [HttpGet]
        public HttpResponseMessage SetPassword(string userName, string passwordHash)
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

        [HttpGet]
        public IEnumerable<PriceItemRemainderView> GetLightPriceItemList(string priceListName, string warehouse = "")
        {

            var parameters = new ArrayList() { priceListName };

            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT  ");

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
                sb.Append("WHERE Warehouse_Id = @p1 ");
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

            sb.Append("WHERE PRPRL.PriceList_Name = @p0 ");



            var context = new StoreDbContext();
            var query = context.Set<PriceItemRemainderView>()
          .SqlQuery(sb.ToString(), parameters.ToArray());


            return query;

        }

        [HttpGet]
        public IEnumerable<Customer> GetUserCustomers(string userName = "")
        {
            var context = new StoreDbContext();
            var query = context.Customers.Where(c => c.Creator_Id == userName || c.Creator_Id == "admin");
            return query;
        }

        [HttpGet]
        public IEnumerable<SaleDocument> GetUserDebtReceipts(string userName = "", string customer = "")
        {
            var context = new StoreDbContext();
            var query = context.SaleDocuments
                .Include(i => i.Customer)
                .Include(i => i.SaleItems)
                .Where(c => c.Creator_Id == userName && !c.IsOrder && c.IsInDebt);
            if (!string.IsNullOrEmpty(customer))
                query = query.Where(c => c.Customer_Name == customer);

            return query;
        }

        [HttpGet]
        public IEnumerable<RefundDocument> GetUserDebtRefunds(string userName = "")
        {
            var context = new StoreDbContext();
            var query = context.RefundDocuments
                .Include(i => i.RefundItems)
                .Include(i => i.SaleDocument)
                .Where(c => c.Creator_Id == userName && !c.SaleDocument.IsOrder && c.SaleDocument.IsInDebt);

            return query;
        }


        [HttpGet]
        public IEnumerable<DebtDischargeDocument> GetUserDebtDischargeDocuments(string userName = "")
        {
            var context = new StoreDbContext();
            var query = context.DebtDischargeDocuments
                .Include(i => i.Debtor)
                .Where(c => c.Creator_Id == userName);

            return query;
        }

        [HttpGet]
        public IEnumerable<DebtDischargeDocument> GetUserDebtDischargeDocumentsByDate(string from, string to, string userName = "")
        {
            var fromDate = DateTime.Parse(from);
            var toDate = DateTime.Parse(to);

            var context = new StoreDbContext();
            var query = context.DebtDischargeDocuments
                .Include(i => i.Debtor)
                .Where(c => c.Creator_Id == userName && c.DischargeDate >= fromDate && c.DischargeDate <= toDate && c.IsDischarge);

            return query;
        }

        [HttpGet]
        public IEnumerable<Warehouse> GetWarehouses()
        {
            var context = new StoreDbContext();
            var query = context.Warehouses;

            return query;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            var context = new StoreDbContext();
            var query = context.Users.Include(i => i.Warehouse);

            return query;
        }

        [HttpGet]
        public IEnumerable<Supplier> GetSuppliers()
        {
            var context = new StoreDbContext();
            var query = context.Suppliers;

            return query;
        }

        [HttpPost]
        public HttpResponseMessage AddDebtDischarge(DebtDischargeDocument debtDischarge)
        {
            var context = new StoreDbContext();
            context.DebtDischargeDocuments.Add(debtDischarge);
            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage AddUser(User user)
        {
            var context = new StoreDbContext();
            context.Users.Add(user);
            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage SaveUser(User user)
        {
            var context = new StoreDbContext();
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            
            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage RemoveUser(User user)
        {
            var context = new StoreDbContext();
            context.Users.Remove(user);
            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage AddWarehouse(Warehouse warehouse)
        {
            var context = new StoreDbContext();
            context.Warehouses.Add(warehouse);
            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage SaveWarehouse(Warehouse warehouse)
        {
            var context = new StoreDbContext();
            context.Warehouses.Attach(warehouse);
            context.Entry(warehouse).State = EntityState.Modified;

            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage RemoveWarehouse(Warehouse warehouse)
        {
            var context = new StoreDbContext();
            context.Warehouses.Remove(warehouse);
            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpGet]
        public string GetLastInvoiceNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [Number] ");
            sb.Append("FROM [dbo].[SaleDocuments] ");
            sb.Append("WHERE [IsInvoice] = 1 ");
            sb.Append("ORDER BY [SaleDate] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public string GetLastReceiptNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [Number] ");
            sb.Append("FROM [dbo].[SaleDocuments] ");
            sb.Append("WHERE [IsInvoice] = 0 AND [IsOrder] = 0 ");
            sb.Append("ORDER BY [SaleDate] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public string GetLastOrderNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [Number] ");
            sb.Append("FROM [dbo].[SaleDocuments] ");
            sb.Append("WHERE [IsOrder] = 1 ");
            sb.Append("ORDER BY [SaleDate] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public string GetLastRefundNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [RefundNumber] ");
            sb.Append("FROM dbo.[RefundDocuments] ");
            sb.Append("ORDER BY [RefundDate] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public string GetLastIncomeNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [IncomeNumber] ");
            sb.Append("FROM dbo.[Incomes] ");
            sb.Append("ORDER BY [Id] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public string GetLastPriceChangeReportNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [ReportNumber] ");
            sb.Append("FROM dbo.[PriceChangeReports] ");
            sb.Append("ORDER BY [Id] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public string GetLastSaleDocumentsPerDaysNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [Number] ");
            sb.Append("FROM dbo.[SaleDocumentsPerDays] ");
            sb.Append("ORDER BY [Id] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public string GetLastWarehouseTransferRequestNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [RequestNumber] ");
            sb.Append("FROM dbo.[WarehouseTransferRequests] ");
            sb.Append("ORDER BY [Id] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        [HttpGet]
        public IEnumerable<CashFlowItemModel> GetCashFlowItems(string AtFromDate, string AtToDate, 
            string PreviousAtFromDate, string PreviousAtToDate, string userName = "")
        {
            StoreDbContext ctx = new StoreDbContext();

            var fromDate = DateTime.Parse(AtFromDate);
            var toDate = DateTime.Parse(AtToDate);
            var previousFromDate = DateTime.Parse(PreviousAtFromDate);
            var previousToDate = DateTime.Parse(PreviousAtToDate);

            //"SaleDocumentsPerDay,SaleItem/SaleDocument/Creator,SaleItem/PriceItem/Prices"
            var realizations = ctx.SalesPerDayItems

                .Where(w => w.SaleDocumentsPerDay.SaleDocumentsDate >= fromDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= toDate);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                realizations = realizations.Where(w => w.SaleDocumentsPerDay.Creator_Id == userName);
            }
            realizations = realizations
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.SaleItem.SaleDocument.Creator)
                .Include(i => i.SaleItem.PriceItem.Prices);
            var previousRealizations = ctx.SalesPerDayItems
                .Where(w => w.SaleDocumentsPerDay.SaleDocumentsDate >= previousFromDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= previousToDate);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                previousRealizations = previousRealizations.Where(w => w.SaleDocumentsPerDay.Creator_Id == userName);
            }
            previousRealizations = previousRealizations
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.SaleItem.SaleDocument.Creator)
                .Include(i => i.SaleItem.PriceItem.Prices);

            var tempGroupedRealzation =
                from r in realizations
                select new
                {
                    Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                    Sales = r.Amount,
                    WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                    ? r.SaleItem.PriceItem.Prices.Where(
                                        p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                        .OrderByDescending(or => or.PriceDate)
                                        .FirstOrDefault()
                                        .Price
                                      : 0)
                };

            var groupedRealization = from r in tempGroupedRealzation
                                     group r by r.Cashier
                                         into cshrs
                                         select new
                                         {
                                             Cashier = cshrs.Key,
                                             Sales = cshrs.Sum(s => s.Sales),
                                             WholesaleSales = cshrs.Sum(s => s.WholesaleSales)
                                         };


            var tempGroupedPreviousRealization =
                from r in previousRealizations
                select new
                {
                    Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                    Sales = r.Amount,
                    WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                    ? r.SaleItem.PriceItem.Prices.Where(
                                        p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                        .OrderByDescending(or => or.PriceDate)
                                        .FirstOrDefault()
                                        .Price
                                      : 0)
                };
            var groupedPreviousRealization = from r in tempGroupedPreviousRealization
                                             group r by r.Cashier
                                                 into cshrs
                                                 select new
                                                 {
                                                     Cashier = cshrs.Key,
                                                     Sales = cshrs.Sum(s => s.Sales),
                                                     WholesaleSales = cshrs.Sum(s => s.WholesaleSales)
                                                 };

            var realization = (from r in groupedRealization
                              join pr in groupedPreviousRealization on r.Cashier equals pr.Cashier into prvs
                              from previous in prvs.DefaultIfEmpty()
                              select new RealizationTEMP
                              {
                                  Cashier = r.Cashier,
                                  Sales = r.Sales,
                                  WholesaleSales = r.WholesaleSales,
                                  PreviousSales = previous != null ? previous.Sales : 0,
                                  PreviousWholesaleSales = previous != null ? previous.WholesaleSales : 0
                              }).ToList();

            //"RefundDocument/SaleDocument,SaleItem/SaleDocument/Creator,PriceItem/Prices"
            var refunds = ctx.RefundItems
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= fromDate
                            && w.RefundDocument.RefundDate <= toDate
                            && !w.RefundDocument.SaleDocument.IsInDebt);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                refunds = refunds.Where(w => w.RefundDocument.Creator_Id == userName);
            }
            refunds = refunds
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.SaleItem.SaleDocument.Creator)
                .Include(i => i.PriceItem.Prices);

            var previousRefunds = 
                ctx.RefundItems
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= previousFromDate
                            && w.RefundDocument.RefundDate <= previousToDate
                            && !w.RefundDocument.SaleDocument.IsInDebt);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                previousRefunds = previousRefunds.Where(w => w.RefundDocument.Creator_Id == userName);
            }

            previousRefunds = previousRefunds
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.SaleItem.SaleDocument.Creator)
                .Include(i => i.PriceItem.Prices);

            var tempGroupedRefunds =
                from r in refunds
                select new
                {
                    Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                    Refunds = r.Amount,
                    WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                    ? r.SaleItem.PriceItem.Prices.Where(
                                        p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                        .OrderByDescending(or => or.PriceDate)
                                        .FirstOrDefault()
                                        .Price
                                      : 0)
                };
            var groupedRefunds = from r in tempGroupedRefunds
                                 group r by r.Cashier
                                     into cshrs
                                     select new
                                     {
                                         Cashier = cshrs.Key,
                                         Refunds = cshrs.Sum(s => s.Refunds),
                                         WholesaleRefunds = cshrs.Sum(s => s.WholesaleSales)

                                     };


            var tempGroupedPreviousRefunds =
                from r in previousRefunds
                select new
                {
                    Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                    Refunds = r.Amount,
                    WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                    ? r.SaleItem.PriceItem.Prices.Where(
                                        p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                        .OrderByDescending(or => or.PriceDate)
                                        .FirstOrDefault()
                                        .Price
                                      : 0)
                };

            var groupedPreviousRefunds = from r in tempGroupedPreviousRefunds
                                         group r by r.Cashier
                                             into cshrs
                                             select new
                                             {
                                                 Cashier = cshrs.Key,
                                                 Refunds = cshrs.Sum(s => s.Refunds),
                                                 WholesaleRefunds = cshrs.Sum(s => s.WholesaleSales)

                                             };

            var refund = (from r in groupedRefunds
                         join pr in groupedPreviousRefunds on r.Cashier equals pr.Cashier into prvs
                         from previous in prvs.DefaultIfEmpty()
                         select new RefundTEMP
                         {
                             Cashier = r.Cashier,
                             Refunds = r.Refunds,
                             WholesaleRefunds = r.WholesaleRefunds,
                             PreviousRefunds = previous != null ? previous.Refunds : 0,
                             PreviousWholesaleRefunds = previous != null ? previous.WholesaleRefunds : 0
                         }).ToList();


            var debts = realizations.Where(r => r.SaleItem.SaleDocument.IsInDebt);

            var previousDebts = previousRealizations.Where(r => r.SaleItem.SaleDocument.IsInDebt);

            var tempGroupedDebts =
                from r in debts
                select new
                {
                    Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                    Debts = r.Amount,
                    WholesaleDebts =
                        r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                        ? r.SaleItem.PriceItem.Prices.Where(
                                            p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                            .OrderByDescending(or => or.PriceDate)
                                            .FirstOrDefault()
                                            .Price
                                        : 0)
                };
            var groupedDebts = from r in tempGroupedDebts
                               group r by r.Cashier
                                   into cshrs
                                   select new
                                   {
                                       Cashier = cshrs.Key,
                                       Debts = cshrs.Sum(s => s.Debts),
                                       WholesaleDebts = cshrs.Sum(s => s.WholesaleDebts)
                                   };

            var tempPreviousGroupedDebts =
                from r in previousDebts
                select new
                {
                    Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                    Debts = r.Amount,
                    WholesaleDebts =
                        r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                        ? r.SaleItem.PriceItem.Prices.Where(
                                            p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                            .OrderByDescending(or => or.PriceDate)
                                            .FirstOrDefault()
                                            .Price
                                        : 0)
                };

            var groupedPreviousDebts = from r in tempPreviousGroupedDebts
                                       group r by r.Cashier
                                           into cshrs
                                           select new
                                           {
                                               Cashier = cshrs.Key,
                                               Debts = cshrs.Sum(s => s.Debts),
                                               WholesaleDebts = cshrs.Sum(s => s.WholesaleDebts)
                                           };

            var debt = (from r in groupedDebts
                        join pr in groupedPreviousDebts on r.Cashier equals pr.Cashier into prvs
                        from previous in prvs.DefaultIfEmpty()
                        select new DebtTEMP
                        {
                            Cashier = r.Cashier,
                            Debts = r.Debts,
                            WholesaleDebts = r.WholesaleDebts,
                            PreviousDebts = previous != null ? previous.Debts : 0,
                            PreviousWholesaleDebts = previous != null ? previous.WholesaleDebts : 0
                        }).ToList();

            var additionalDebts = 
               ctx.DebtDischargeDocuments
                   .Where(w => !w.IsDischarge
                               && w.DischargeDate >= fromDate
                               && w.DischargeDate <= toDate);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                additionalDebts = additionalDebts.Where(w => w.Creator_Id == userName);
            }
            additionalDebts = additionalDebts
                .Include(i => i.Creator);

            var previousAdditionalDebts = 
               ctx.DebtDischargeDocuments
                   .Where(w => !w.IsDischarge
                               && w.DischargeDate >= previousFromDate
                               && w.DischargeDate <= previousToDate);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                previousAdditionalDebts = previousAdditionalDebts.Where(w => w.Creator_Id == userName);
            }
            previousAdditionalDebts = previousAdditionalDebts
                .Include(i => i.Creator);

            var groupedAdditionaDebts = from r in additionalDebts
                                        group r by r.Creator.DisplayName
                                            into cshrs
                                            select new
                                            {
                                                Cashier = cshrs.Key,
                                                Debts = cshrs.Sum(s => s.Amount)
                                            };


            var groupedPreviousAdditionaDebts = from r in previousAdditionalDebts
                                                group r by r.Creator.DisplayName
                                                    into cshrs
                                                    select new
                                                    {
                                                        Cashier = cshrs.Key,
                                                        Debts = cshrs.Sum(s => s.Amount)
                                                    };

            var additionalDebt = (from r in groupedAdditionaDebts
                                 join pr in groupedPreviousAdditionaDebts on r.Cashier equals pr.Cashier into prvs
                                 from previous in prvs.DefaultIfEmpty()
                                 select new DebtTEMP
                                 {
                                     Cashier = r.Cashier,
                                     Debts = r.Debts,
                                     WholesaleDebts = r.Debts,
                                     PreviousDebts = previous != null ? previous.Debts : 0,
                                     PreviousWholesaleDebts = previous != null ? previous.Debts : 0
                                 }).ToList();


            debt.AddRange(additionalDebt);

            var totalDebt = (from d in debt
                            group d by d.Cashier
                                into dbt
                                select new DebtTEMP
                                {
                                    Cashier = dbt.Key,
                                    Debts = dbt.Sum(s => s.Debts),
                                    WholesaleDebts = dbt.Sum(s => s.WholesaleDebts),
                                    PreviousDebts = dbt.Sum(s => s.PreviousDebts),
                                    PreviousWholesaleDebts = dbt.Sum(s => s.PreviousWholesaleDebts)
                                }).ToList();

            var debtDischarges = 
                ctx.DebtDischargeDocuments
                    .Where(w => w.IsDischarge
                                && w.DischargeDate >= fromDate
                                && w.DischargeDate <= toDate);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                debtDischarges = debtDischarges.Where(w => w.Creator_Id == userName);
            }
            debtDischarges = debtDischarges
                .Include(i => i.Creator);

            var previousDebtDischarges = 
                ctx.DebtDischargeDocuments
                    .Where(w => w.IsDischarge
                                && w.DischargeDate >= previousFromDate
                                && w.DischargeDate <= previousToDate);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                previousDebtDischarges = previousDebtDischarges.Where(w => w.Creator_Id == userName);
            }
            previousDebtDischarges = previousDebtDischarges
                .Include(i => i.Creator);

            var groupedDebtDischarges = from r in debtDischarges
                                        group r by r.Creator.DisplayName
                                            into cshrs
                                            select new
                                            {
                                                Cashier = cshrs.Key,
                                                Discharges = cshrs.Sum(s => s.Amount)
                                            };

            var groupedPreviousDebtDischarges = from r in previousDebtDischarges
                                                group r by r.Creator.DisplayName
                                                    into cshrs
                                                    select new
                                                    {
                                                        Cashier = cshrs.Key,
                                                        Discharges = cshrs.Sum(s => s.Amount)
                                                    };

            var debtDischarge = (from r in groupedDebtDischarges
                                join pr in groupedPreviousDebtDischarges on r.Cashier equals pr.Cashier into prvs
                                from previous in prvs.DefaultIfEmpty()
                                select new DebtDischargeTEMP
                                {
                                    Cashier = r.Cashier,
                                    DebDischargests = r.Discharges,
                                    PreviousDischarges = previous != null ? previous.Discharges : 0
                                }).ToList();

            IList<CashFlowItemModel> totalData = (from s in realization
                            join r in refund on s.Cashier equals r.Cashier into rfnds
                            join d in totalDebt on s.Cashier equals d.Cashier into dbts
                            join dd in debtDischarge on s.Cashier equals dd.Cashier into dschrgs
                            from rfndsItem in rfnds.DefaultIfEmpty()
                            from dbtsItem in dbts.DefaultIfEmpty()
                            from dschrgsItem in dschrgs.DefaultIfEmpty()
                            select new CashFlowItemModel
                            {
                                Cashier = s.Cashier,
                                Sales = (int)s.Sales,
                                SalesByWholesales = (int)s.WholesaleSales,
                                PreviousSales = (int)s.PreviousSales,
                                PreviousSalesByWholesales = (int)s.PreviousWholesaleSales,
                                Refunds = (int)(rfndsItem != null ? rfndsItem.Refunds : 0),
                                RefundsByWholesales = (int)(rfndsItem != null ? rfndsItem.WholesaleRefunds : 0),
                                PreviousRefunds = (int)(rfndsItem != null ? rfndsItem.PreviousRefunds : 0),
                                PreviousRefundsByWholesales = (int)(rfndsItem != null ? rfndsItem.PreviousWholesaleRefunds : 0),
                                Debds = (int)(dbtsItem != null ? dbtsItem.Debts : 0),
                                DebdsByWholesales = (int)(dbtsItem != null ? dbtsItem.WholesaleDebts : 0),
                                PreviousDebds = (int)(dbtsItem != null ? dbtsItem.PreviousDebts : 0),
                                PreviousDebdsByWholesales = (int)(dbtsItem != null ? dbtsItem.PreviousWholesaleDebts : 0),
                                DebdDischarges = (int)(dschrgsItem != null ? dschrgsItem.DebDischargests : 0),
                                PreviousDebdDischarges = (int)(dschrgsItem != null ? dschrgsItem.PreviousDischarges : 0)
                            }).ToList();

            return totalData;
        }

        [HttpGet]
        public Income GetIncomeByKey(long id)
        {
            var context = new StoreDbContext();
            var income = context.Incomes.Where(i => i.Id == id).FirstOrDefault();

            return income;
        }

        [HttpGet]
        public IEnumerable<IncomeItem> GetIncomeItems(string AtFromDate, string AtToDate, 
            string priceListName, string userName, string warehouse = "")
        {
            
            var qr = GetIncomes(priceListName, warehouse, userName, AtFromDate, AtToDate);
            
            //foreach (var priceItemRemainderView in 
            int index = 1;

            var query = from p in qr
                        group p by new
                        {
                            Articul = p.Articul,
                            CatalogNumber = p.CatalogNumber,
                            Income_ID = p.Income_Id,
                            IsDuplicate = p.IsDuplicate ? "*" : "",
                            Gear_Id = p.Gear_Id,
                            Name = p.Gear_Name,
                            PriceItem_Id = p.PriceItem_Id,
                            Uom = p.Uom,
                            IsAccepted = p.IsAccept
                        }
                            into grp
                            select new IncomeItem()

                            //).Select(new IncomeItem()
                            {
                                Number = index++,
                                Articul = grp.Key.Articul,
                                BuyPriceRur = (int)grp.Average(a => a.BuyPriceRur),
                                BuyPriceTng = (int)grp.Average(a => a.BuyPriceTng),
                                CatalogNumber = grp.Key.CatalogNumber,
                                Incomes = (int)grp.Average(s => s.Incomes),
                                Income_ID = grp.Key.Income_ID,
                                IsDuplicate = grp.Key.IsDuplicate,
                                Gear_Id = grp.Key.Gear_Id,
                                Name = grp.Key.Name,
                                LowerLimitRemainder = grp.Average(a => a.LowerLimitRemainder),
                                RecommendedRemainder = grp.Average(a => a.RecommendedRemainder),
                                NewPrice = (int)grp.Average(a => a.NewPrice),
                                PriceItem_Id = grp.Key.PriceItem_Id,
                                Uom = grp.Key.Uom,
                                WholesalePrice = (int)grp.Average(a => a.WholesalePrice),
                                Remainders = (int)grp.Average(s => s.Remainders),
                                IsAccepted = grp.Key.IsAccepted,
                            };

            var incomeItems = query as IList<IncomeItem> ?? query.ToList();
            foreach (var grp in incomeItems.GroupBy(g => g.Income_ID))
            {
                var grp1 = grp;
                //var inc = ctx.ExecuteSyncronous(ctx.Incomes.Where(w => w.Id == grp1.Key)).FirstOrDefault();
                var inc = GetIncomeByKey(grp1.Key);

                foreach (var incomeItem in grp1)
                {
                    incomeItem.Income = string.Format("Оприходование № {0} от {1:dd.MM.yyyy}",
                        inc.IncomeNumber, inc.IncomeDate);
                    incomeItem.IncomeDate = inc.IncomeDate;
                }
            }

            return incomeItems.OrderByDescending(o => o.IncomeDate);
        }
            
        [HttpPost]
        public HttpResponseMessage SaveIncome(Income income)
        {
            var context = new StoreDbContext();
            context.Incomes.Attach(income);
            context.Entry(income).State = EntityState.Modified;

            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage AddIncome(Income income)
        {
            income.Accepter = null;
            income.Creator = null;
            income.Supplier = null;

            var items = income.IncomeItems;

            income.IncomeItems = null;

            var context = new StoreDbContext();

            using (var trans = context.Database.BeginTransaction())
            {

                try
                {
                    context.Incomes.Add(income);

                    context.SaveChanges();

                    foreach (var item in items)
                    {
                        item.Income = null;
                        item.PriceItem = null;

                        item.Income_Id = income.Id;

                        context.IncomeItems.Add(item);
                    }
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception exception)
                {
                    trans.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(income.Id.ToString())
            };
        }

        [HttpPost]
        public HttpResponseMessage SavePriceItem(PriceItem priceItem)
        {
            priceItem.Gear = null;
            priceItem.PriceLists = null;
            priceItem.Prices = null;
            priceItem.Remainders = null;
            priceItem.UnitOfMeasure = null;

            try
            {
                var context = new StoreDbContext();
                context.PriceItems.Attach(priceItem);
                context.Entry(priceItem).State = EntityState.Modified;

                context.SaveChanges();
            }
            catch (Exception exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }


            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage AcceptIncome(AcceptIncomeContainer incomeContainer)
        {
            var context = new StoreDbContext();
            //context.PriceItems.Attach(priceItem);
            //context.Entry(priceItem).State = EntityState.Modified;

            //context.SaveChanges();
            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    var incomeDb = incomeContainer.Income;

                    //ctx.ChangeState(incomeDb, EntityStates.Modified);
                    //ctx.SaveChangesSynchronous();
                    context.Incomes.Attach(incomeDb);
                    context.Entry(incomeDb).State = EntityState.Modified;

                    foreach (var ii in incomeDb.IncomeItems)
                    {
                        ii.PriceItem.BuyPriceRur = ii.BuyPriceRur;
                        ii.PriceItem.BuyPriceTng = ii.BuyPriceTng;

                        context.PriceItems.Attach(ii.PriceItem);
                        context.Entry(ii.PriceItem).State = EntityState.Modified;
                    }
                    //ctx.SaveChangesSynchronous();
                    List<Remainder> changesRemainders = new List<Remainder>();

                    foreach (var acceptIncomeItem in incomeContainer.IncomeItems)
                    {

                        var rem = context.Remainders.Where(
                                    r =>
                                        r.PriceItem_Id == acceptIncomeItem.PriceItem_Id &&
                                        r.Warehouse_Id == incomeContainer.Warehouse).FirstOrDefault();

                        if (rem == null)
                        {
                            rem = new Remainder();
                            rem.Amount = acceptIncomeItem.Incomes;
                            rem.RemainderDate = DateTimeHelper.GetNowKz();
                            rem.PriceItem_Id = acceptIncomeItem.PriceItem_Id;
                            rem.Warehouse_Id = incomeContainer.Warehouse;
                            context.Remainders.Add(rem);
                        }
                        else
                        {
                            rem.Amount += acceptIncomeItem.Incomes;
                            rem.RemainderDate = DateTimeHelper.GetNowKz();
                            context.Remainders.Attach(rem);
                            context.Entry(rem).State = EntityState.Modified;
                        }
                        changesRemainders.Add(rem);

                        WholesalePrice rp = new WholesalePrice();
                        rp.Price = acceptIncomeItem.NewPrice;
                        rp.PriceDate = DateTimeHelper.GetNowKz();
                        rp.PriceItem_Id = acceptIncomeItem.PriceItem_Id;
                        context.WholesalePrices.Add(rp);

                    }
                    context.SaveChanges();
                    //ctx.SaveChangesSynchronous();
                    //CreatePriceChangeReport(incomeDb.Id);

                    //Save price change report

                    var now = DateTimeHelper.GetNowKz();

                    //save changes
                    foreach (var remainder in changesRemainders)
                    {
                        var newRemChanges = new RemainderChanges();
                        newRemChanges.Remainder_Id = remainder.Id;
                        newRemChanges.ChangesDate = now;
                        context.RemaindersChanges.Add(newRemChanges);
                    }
                    context.SaveChanges();

                    PriceChangeReport rep = new PriceChangeReport();
                    rep.Creator_Id = incomeDb.Accepter_Id;
                    rep.ReportDate = now;

                    //var priceReportDb =
                    //    ctx.ExecuteSyncronous(ctx.PriceChangeReports.OrderByDescending(s => s.Id)).FirstOrDefault();
                    var lastNum = GetLastPriceChangeReportNumber();

                    if (!string.IsNullOrEmpty(lastNum))
                    {
                        int lastNumber = 0;
                        if (int.TryParse(lastNum, out lastNumber))
                        {
                            rep.ReportNumber = (++lastNumber).ToString();
                        }
                    }
                    else
                    {
                        rep.ReportNumber = "1";
                    }
                    context.PriceChangeReports.Add(rep);

                    foreach (var acceptIncomeItem in incomeContainer.IncomeItems)
                    {
                        var item = acceptIncomeItem;
                        var prices = context.WholesalePrices.Where(r => r.PriceItem_Id == item.PriceItem_Id).ToList();

                        decimal previousPrice = 0;
                        WholesalePrice currentRetailPrice = prices.OrderByDescending(d => d.PriceDate).First();
                        WholesalePrice previousRetailPrice = null;
                        if (prices.Count > 1)
                        {
                            previousRetailPrice = prices.OrderByDescending(d => d.PriceDate).Skip(1).First();
                            previousPrice = previousRetailPrice.Price;

                            if (previousRetailPrice.Price == 0)
                            {
                                previousRetailPrice.Price = currentRetailPrice.Price;
                            }
                        }
                        else
                        {
                            previousRetailPrice = currentRetailPrice;
                        }

                        PriceChangeReportItem repItem = new PriceChangeReportItem()
                        {
                            PriceItem_Id = acceptIncomeItem.PriceItem_Id,
                            PreviousPrice_Id = previousRetailPrice.Id,
                            NewPrice_Id = currentRetailPrice.Id,
                            PriceChangeReport = rep
                        };

                        context.PriceChangeReportItems.Add(repItem);

                    }

                    context.SaveChanges();
                    tr.Commit();
                }
                catch (Exception exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }


            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage AddPriceChangeReport(PriceChangeReport report)
        {
            var context = new StoreDbContext();

            //var priceReportDb =
            //    ctx.ExecuteSyncronous(ctx.PriceChangeReports.OrderByDescending(s => s.Id)).FirstOrDefault();
            var lastNum = GetLastPriceChangeReportNumber();
            if (!string.IsNullOrEmpty(lastNum))
            {
                int lastNumber = 0;
                if (int.TryParse(lastNum, out lastNumber))
                {
                    report.ReportNumber = (++lastNumber).ToString();
                }
            }
            else
            {
                report.ReportNumber = "1";
            }

            context.PriceChangeReports.Add(report);

            foreach (var item in report.PriceChangeReportItems)
            {
                var prevId = item.PreviousPrice_Id;
                var currId = item.NewPrice_Id;

                var prev = context.WholesalePrices.Where(p => p.Id == prevId).FirstOrDefault();
                var curr = context.WholesalePrices.Where(p => p.Id == currId).FirstOrDefault();

                if (curr != null && prev != null)
                {
                    if (prev.Price == 0)
                    {
                        prev.Price = curr.Price;
                    }
                }

                context.PriceChangeReportItems.Add(item);
            }
            context.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpGet]
        public IEnumerable<UnitOfMeasure> GetUnitOfMeasures()
        {
            var context = new StoreDbContext();
            return context.UnitOfMeasures;
        }

        [HttpGet]
        public PriceList GetPriceListByName(string name)
        {
            var context = new StoreDbContext();
            var priceList = context.PriceLists.Where(i => i.Name == name).FirstOrDefault();

            return priceList;
        }

        [HttpPost]
        public HttpResponseMessage AddPriceItem(PriceItem priceItem)
        {
            var gear = priceItem.Gear;
            priceItem.Gear = null;

            priceItem.UnitOfMeasure = null;

            var remainders = priceItem.Remainders;
            priceItem.Remainders = null;

            var prices = priceItem.Prices;
            priceItem.Prices = null;

            var priceLists = priceItem.PriceLists;
            priceItem.PriceLists = null;

            var context = new StoreDbContext();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    gear.GearCategory = null;
                    gear.Category_Id = "Обычные";
                    context.Gears.Add(gear);
                    context.SaveChanges();

                    var newsGear = new GearNew();
                    newsGear.CreatedDate = DateTimeHelper.GetNowKz();
                    newsGear.Gear_Id = gear.Id;
                    context.GearNews.Add(newsGear);

                    var findedUom = context.UnitOfMeasures.Where(w => w.Name == priceItem.Uom_Id).FirstOrDefault();
                    if (findedUom == null)
                    {
                        var newUom = new UnitOfMeasure();
                        newUom.Name = priceItem.Uom_Id;
                        context.UnitOfMeasures.Add(newUom);
                    }

                    priceItem.Gear_Id = gear.Id;

                    context.PriceItems.Add(priceItem);
                    context.SaveChanges();

                    foreach (var remainder in remainders)
                    {
                        remainder.Warehouse = null;
                        remainder.RemaindersUserChanges = null;
                        remainder.PriceItem = null;

                        remainder.PriceItem_Id = priceItem.Id;
                        context.Remainders.Add(remainder);
                    }
                    foreach (var price in prices)
                    {
                        price.PriceItem = null;

                        price.PriceItem_Id = priceItem.Id;
                        context.WholesalePrices.Add(price);
                    }
                    context.SaveChanges();

                    foreach (var prl in priceLists)
                    {
                        var findedPrl = context.PriceLists.Find(prl.Name);
                        priceItem.PriceLists = new List<PriceList>();
                        priceItem.PriceLists.Add(findedPrl);
                    }
                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(priceItem.Id.ToString())
            };
        }

        [HttpGet]
        public PriceItem GetPriceItemById(long id)
        {
            //Gear,Remainders,UnitOfMeasure,Prices
            var context = new StoreDbContext();
            var priceItem = context.PriceItems
                .Include(i => i.Gear)
                .Include(i => i.Remainders)
                .Include(i => i.Prices)
                .Include(i => i.UnitOfMeasure)
                .Where(i => i.Id == id).FirstOrDefault();

            return priceItem;
        }

        [HttpGet]
        public PriceItem GetPriceItemByBarcode(string barcode)
        {
            var context = new StoreDbContext();
            var priceItem = context.PriceItems.Where(i => i.Barcode1 == barcode || i.Barcode2 == barcode || i.Barcode3 == barcode).FirstOrDefault();

            return priceItem;
        }

        [HttpPost]
        public HttpResponseMessage SaveGear(Gear gear)
        {
            gear.Category_Id = "Обычные";
            var context = new StoreDbContext();


            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    context.Gears.Attach(gear);
                    context.Entry(gear).State = EntityState.Modified;

                    context.SaveChanges();

                    var gearChanges = new GearChanges();
                    var now = DateTimeHelper.GetNowKz();
                    gearChanges.ChangesDate = now;
                    gearChanges.Gear_Id = gear.Id;
                    context.GearsChanges.Add(gearChanges);
                    context.SaveChanges();

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }


            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage AddRemainder(Remainder remainder)
        {
            var context = new StoreDbContext();

            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    remainder.PriceItem = null;
                    context.Remainders.Add(remainder);

                    context.SaveChanges();

                    var remainderChanges = new RemainderChanges();
                    remainderChanges.Remainder_Id = remainder.Id;
                    var now = DateTimeHelper.GetNowKz();

                    remainderChanges.ChangesDate = now;
                    context.RemaindersChanges.Add(remainderChanges);
                    context.SaveChanges();

                    tr.Commit();
                }
                catch (Exception exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);                    
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(remainder.Id.ToString())
            };
        }

        [HttpPost]
        public HttpResponseMessage SaveRemainder(Remainder remainder)
        {
            var context = new StoreDbContext();

            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    context.Remainders.Attach(remainder);
                    context.Entry(remainder).State = EntityState.Modified;

                    context.SaveChanges();

                    var remainderChanges = new RemainderChanges();
                    remainderChanges.Remainder_Id = remainder.Id;
                    var now = DateTimeHelper.GetNowKz();

                    remainderChanges.ChangesDate = now;
                    context.RemaindersChanges.Add(remainderChanges);
                    context.SaveChanges();

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage AddWholesalePrice(WholesalePrice price)
        {
            var context = new StoreDbContext();
            context.WholesalePrices.Add(price);

            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpGet]
        public IEnumerable<PriceChangeReport> GetPriceChangeReports()
        {
            var context = new StoreDbContext();
            return context.PriceChangeReports
                .Include(i => i.Creator)
                .Include(i => i.PriceChangeReportItems)
                .Include("PriceChangeReportItems.NewPrice")
                .Include("PriceChangeReportItems.PreviousPrice")
                .Include("PriceChangeReportItems.PriceItem")
                .Include("PriceChangeReportItems.PriceItem.Remainders");
        }

        [HttpGet]
        public IEnumerable<Remainder> GetRemaindersByPriceItem(long priceItemId, string warehouse = "")
        {
            var context = new StoreDbContext();
            var result = context.Remainders
                .Where(r => r.PriceItem_Id == priceItemId);
            if (!string.IsNullOrEmpty(warehouse))
                result = result.Where(w => w.Warehouse_Id == warehouse);
            result = result.Include(i => i.PriceItem);
            return result;
        }

        [HttpGet]
        public IEnumerable<WholesalePrice> GetPricesByPriceItem(long priceItemId)
        {
            var context = new StoreDbContext();
            return context.WholesalePrices
                .Include(i => i.PriceItem)
                .Where(r => r.PriceItem_Id == priceItemId);
        }

        [HttpGet]
        public IEnumerable<PriceChangeReportItem> GetPriceChangeReportItems(long priceChangeReportId)
        {
            var context = new StoreDbContext();
            return context.PriceChangeReportItems
                .Include(i => i.NewPrice)
                .Include(i => i.PreviousPrice)
                .Include(i => i.PriceItem.Gear)
                .Include(i => i.PriceItem.Remainders)
                .Where(p => p.PriceChangeReport_Id == priceChangeReportId);
        }

        [HttpGet]
        public SaleDocumentsPerDay GetSaleDocumentsPerDayById(long id)
        {
            var context = new StoreDbContext();
            var priceItem = context.SaleDocumentsPerDays
                .Include(i => i.SalesPerDayItems)
                .Include("SalesPerDayItems.SaleItem.SaleDocument")
                .Include("SalesPerDayItems.SaleItem.PriceItem.PriceLists")
                .Include("SalesPerDayItems.SaleItem.PriceItem.Prices")
                .Where(i => i.Id == id).FirstOrDefault();

            return priceItem;
        }

        [HttpGet]
        public IEnumerable<RealizationItem> GetTodayRealizationItems(string userName, string warehouse)
        {
            var context = new StoreDbContext();

            var now = DateTimeHelper.GetNowKz();
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);


            //"SaleDocumentsPerDay,RefundItem"
            var closedRefundPerDay = context.RefundsPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.RefundItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed &&
                            w.SaleDocumentsPerDay.Creator_Id == userName).Select(s => s.RefundItem.Id).ToList();


            //"RefundDocument/SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices,SaleItem"
            var refundQuery = 
                context.RefundItems
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.PriceItem.Gear)
                .Include(i => i.PriceItem.UnitOfMeasure)
                .Include(i => i.PriceItem.Remainders)
                .Include(i => i.PriceItem.Prices)
                .Include(i => i.PriceItem.PriceLists)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= startDate
                            && w.RefundDocument.RefundDate <= endDate
                            //&& !closedRefundPerDay.Contains(w.Id)
                            && w.RefundDocument.Creator_Id == userName).ToList();

            var refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();

            //"SaleDocumentsPerDay,SaleItem"
            var closedSalesPerDay =
                context.SalesPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed
                            && w.SaleDocumentsPerDay.Creator_Id == userName).Select(s => s.SaleItem.Id).ToList();

            //"SaleDocument,PriceItem/PriceLists,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices"
            var salesQuery = 
                context.SaleItems
                    .Include(i => i.SaleDocument)
                    .Include(i => i.PriceItem.PriceLists)
                    .Include(i => i.PriceItem.Gear)
                    .Include(i => i.PriceItem.UnitOfMeasure)
                    .Include(i => i.PriceItem.Remainders)
                    .Include(i => i.PriceItem.Prices)
                    .Include(i => i.PriceItem.PriceLists)
                    .Where(
                        w =>
                            w.SaleDocument.SaleDate >= startDate
                            && w.SaleDocument.SaleDate <= endDate
                            && !w.SaleDocument.IsOrder
                            //&& !closedSalesPerDay.Contains(w.Id)
                            && w.SaleDocument.Creator_Id == userName).ToList();

            var salesItems = salesQuery.Where(w => !closedSalesPerDay.Contains(w.Id)).ToList();
            var realization = new List<RealizationItem>();
            salesItems.ForEach(s =>
            {

                //string debtor = "";
                //int debtDisc = 0;

                //var findedDebtor =
                //    discharges.Where(w => w.Debtor_Id == s.SaleDocument.Customer_Name).FirstOrDefault();
                //if (findedDebtor != null)
                //{
                //    debtor = findedDebtor.Debtor_Id;
                //    debtDisc = (int)findedDebtor.Amount;
                //}
                var remainders = 0;
                var rems =
                    s.PriceItem.Remainders.Where(w => w.Warehouse_Id == warehouse)
                        .FirstOrDefault();
                if (rems != null)
                    remainders = (int)rems.Amount;

                //var amouuntWithoutDebt = 0;
                //var amountWholesalePriceWithoutDebt = 0;

                //var refnd =
                //    refundItems.Where(wh => wh.SaleItem_Id == s.Id && wh.RefundDocument.SaleDocument.IsInDebt == false)
                //        .Sum(sum => sum.Count);

                //if (!s.SaleDocument.IsInDebt)
                //{
                //    var price =
                //        s.PriceItem.Prices.Where(p => p.PriceDate <= now)
                //            .OrderByDescending(o => o.PriceDate)
                //            .FirstOrDefault();

                //    amouuntWithoutDebt = (int)((s.Price * (s.Count - refnd)) - (s.Discount - ((s.Discount / s.Count) * refnd)));
                //    if (price != null)
                //    {
                //        amountWholesalePriceWithoutDebt =
                //            (int)((price.Price * (s.Count - refnd)));
                //    }
                //}
                var itemRefunds =
                    refundItems.Where(wh => wh.SaleItem_Id == s.Id && wh.RefundDocument.SaleDocument.IsInDebt == true)
                        .Sum(sum => sum.Count);


                
                var count = s.Count - itemRefunds;
                if (count != 0)
                {
                    realization.Add(new RealizationItem()
                    {

                        CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                        IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
                        Name = s.PriceItem.Gear.Name,
                        Price = (int)s.Price,
                        Remainders = remainders,
                        SoldCount = (int)count,
                        Uom = s.PriceItem.UnitOfMeasure.Name,
                        WholePrice = (int)s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                        //Amount = (int)((s.Price * count) - s.Discount),
                        Amount = (int)((s.Price - (s.Discount / s.Count)) * count),
                        //AmountWithoutDebt = amouuntWithoutDebt,
                        //AmountWithoutDebtProfit = amouuntWithoutDebt - amountWholesalePriceWithoutDebt,
                        Discount = (int)s.Discount,
                        SaleItemData = s,
                        Customer = s.SaleDocument.IsInDebt ? s.SaleDocument.Customer_Name : "",
                        //DebtDischarge = s.SaleDocument.IsInDebt ? debtDisc : 0,
                        IsInDebt = s.SaleDocument.IsInDebt,
                        PriceListName = s.PriceItem.PriceLists.First().Name,
                        SaledDate = s.SaleDocument.SaleDate.ToString("T"),
                        SaledNumber = s.SaleDocument.Number,
                        SaledCount = (int)s.Count,

                    });
                }
            });
            int index = 1;
            var query = from r in realization
                        group r by new
                        {
                            CatalogNumber = r.CatalogNumber,
                            IsDuplicate = r.IsDuplicate,
                            Name = r.Name,
                            Price = r.Price,
                            //Remainders = r.Remainders,
                            //SoldCount = r.SoldCount,
                            Uom = r.Uom,

                            //Customer = r.Customer,
                            IsInDebt = r.IsInDebt,
                            PriceListName = r.PriceListName,
                            SaledDate = r.SaledDate,
                            SaledNumber = r.SaledNumber,
                            //SaleItemData = r.SaleItemData
                            //Amount = r.Amount,
                            //Discount = r.Discount,
                            //WholePrice = r.WholePrice
                        }
                            into groups
                            select new RealizationItem()
                            {
                                Number = index++,
                                CatalogNumber = groups.Key.CatalogNumber,
                                IsDuplicate = groups.Key.IsDuplicate,
                                Name = groups.Key.Name,
                                Price = (int)groups.Key.Price,
                                Remainders = (int)groups.Average(s => s.Remainders),
                                SoldCount = groups.Sum(s => s.SoldCount),
                                Uom = groups.Key.Uom,
                                Amount = groups.Sum(s => s.Amount),
                                AmountWithoutDebt = groups.Sum(s => s.AmountWithoutDebt),
                                AmountWithoutDebtProfit = groups.Sum(s => s.AmountWithoutDebtProfit),
                                Discount = groups.Sum(s => s.Discount),
                                WholePrice = (int)groups.Average(a => a.WholePrice),
                                //Customer = groups.Key.Customer,
                                DebtDischarge = (int)groups.Average(s => s.DebtDischarge),
                                IsInDebt = groups.Key.IsInDebt,
                                PriceListName = groups.Key.PriceListName,
                                SaledDate = groups.Key.SaledDate,
                                SaledNumber = groups.Key.SaledNumber,
                                SaledCount = groups.Sum(s => s.SaledCount),
                                //SaleItemData = groups.Key.SaleItemData
                            };
            return query;
        }

        [HttpGet]
        public IEnumerable<RealizationItem> GetTodayRealizationItemsRaw(string userName, string warehouse)
        {

            var context = new StoreDbContext();

            var now = DateTimeHelper.GetNowKz();
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);


            //"SaleDocumentsPerDay,RefundItem"
            var closedRefundPerDay = context.RefundsPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.RefundItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed &&
                            w.SaleDocumentsPerDay.Creator_Id == userName).Select(s => s.RefundItem.Id).ToList();


            //"RefundDocument/SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices,SaleItem"
            var refundQuery =
                context.RefundItems
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.PriceItem.Gear)
                .Include(i => i.PriceItem.UnitOfMeasure)
                .Include(i => i.PriceItem.Remainders)
                .Include(i => i.PriceItem.Prices)
                .Include(i => i.PriceItem.PriceLists)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= startDate
                            && w.RefundDocument.RefundDate <= endDate
                            //&& !closedRefundPerDay.Contains(w.Id)
                            && w.RefundDocument.Creator_Id == userName).ToList();

            var refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();

            //"SaleDocumentsPerDay,SaleItem"
            var closedSalesPerDay =
                context.SalesPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed &&
                            w.SaleDocumentsPerDay.Creator_Id == userName).Select(s => s.SaleItem.Id).ToList();

            //"SaleDocument,PriceItem/PriceLists,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices"
            var salesQuery =
                context.SaleItems
                    .Include(i => i.SaleDocument)
                    .Include(i => i.PriceItem.PriceLists)
                    .Include(i => i.PriceItem.Gear)
                    .Include(i => i.PriceItem.UnitOfMeasure)
                    .Include(i => i.PriceItem.Remainders)
                    .Include(i => i.PriceItem.Prices)
                    .Include(i => i.PriceItem.PriceLists)
                    .Where(
                        w =>
                            w.SaleDocument.SaleDate >= startDate
                            && w.SaleDocument.SaleDate <= endDate
                            && !w.SaleDocument.IsOrder
                            //&& !closedSalesPerDay.Contains(w.Id)
                            && w.SaleDocument.Creator_Id == userName).ToList();

            var salesItems = salesQuery.Where(w => !closedSalesPerDay.Contains(w.Id)).ToList();
            var realization = new List<RealizationItem>();
            salesItems.ForEach(s =>
            {

                //string debtor = "";
                //int debtDisc = 0;

                //var findedDebtor =
                //    discharges.Where(w => w.Debtor_Id == s.SaleDocument.Customer_Name).FirstOrDefault();
                //if (findedDebtor != null)
                //{
                //    debtor = findedDebtor.Debtor_Id;
                //    debtDisc = (int)findedDebtor.Amount;
                //}
                var remainders = 0;
                var rems =
                    s.PriceItem.Remainders.Where(w => w.Warehouse_Id == warehouse)
                        .FirstOrDefault();
                if (rems != null)
                    remainders = (int)rems.Amount;

                //var amouuntWithoutDebt = 0;
                //var amountWholesalePriceWithoutDebt = 0;

                //if (!s.SaleDocument.IsInDebt)
                //{
                //    var price =
                //        s.PriceItem.Prices.Where(p => p.PriceDate <= now)
                //            .OrderByDescending(o => o.PriceDate)
                //            .FirstOrDefault();

                //    amouuntWithoutDebt = (int)((s.Price * s.Count) - s.Discount);
                //    if (price != null)
                //    {
                //        amountWholesalePriceWithoutDebt =
                //            (int)((price.Price * s.Count));
                //    }
                //}
                var itemRefunds =
                    refundItems.Where(wh => wh.SaleItem_Id == s.Id && wh.RefundDocument.SaleDocument.IsInDebt == true)
                        .Sum(sum => sum.Count);


                var count = s.Count - itemRefunds;
                if (count != 0)
                {
                    realization.Add(new RealizationItem()
                    {

                        CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                        IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
                        Name = s.PriceItem.Gear.Name,
                        Price = (int)s.Price,
                        Remainders = remainders,
                        SoldCount = (int)count,
                        Uom = s.PriceItem.UnitOfMeasure.Name,
                        WholePrice = (int)s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                        //Amount = (int)((s.Price * count) - s.Discount),
                        Amount = (int)((s.Price - (s.Discount / s.Count)) * count),
                        //AmountWithoutDebt = amouuntWithoutDebt,
                        //AmountWithoutDebtProfit = amouuntWithoutDebt - amountWholesalePriceWithoutDebt,
                        Discount = (int)s.Discount,
                        SaleItemData = s,
                        Customer = s.SaleDocument.IsInDebt ? s.SaleDocument.Customer_Name : "",
                        //DebtDischarge = s.SaleDocument.IsInDebt ? debtDisc : 0,
                        IsInDebt = s.SaleDocument.IsInDebt,
                        PriceListName = s.PriceItem.PriceLists.First().Name,
                        SaledDate = s.SaleDocument.SaleDate.ToString("T"),
                        SaledNumber = s.SaleDocument.Number,
                        SaledCount = (int)s.Count,

                    });
                }
            });

            return realization;
            //salesItems.ForEach(s =>
            //{

            //    //string debtor = "";
            //    //int debtDisc = 0;

            //    //var findedDebtor =
            //    //    discharges.Where(w => w.Debtor_Id == s.SaleDocument.Customer_Name).FirstOrDefault();
            //    //if (findedDebtor != null)
            //    //{
            //    //    debtor = findedDebtor.Debtor_Id;
            //    //    debtDisc = (int)findedDebtor.Amount;
            //    //}
            //    var remainders = 0;
            //    var rems =
            //        s.PriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
            //            .FirstOrDefault();
            //    if (rems != null)
            //        remainders = (int)rems.Amount;

            //    var amouuntWithoutDebt = 0;
            //    var amountWholesalePriceWithoutDebt = 0;

            //    if (!s.SaleDocument.IsInDebt)
            //    {
            //        var price =
            //            s.PriceItem.Prices.Where(p => p.PriceDate <= DateTimeHelper.GetNowKz())
            //                .OrderByDescending(o => o.PriceDate)
            //                .FirstOrDefault();

            //        amouuntWithoutDebt = (int) ((s.Price*s.Count) - s.Discount);
            //        if (price != null)
            //        {
            //            amountWholesalePriceWithoutDebt =
            //                (int)((price.Price * s.Count));
            //        }
            //    }
            //    var itemRefunds =
            //        _refundItems.Where(wh => wh.SaleItem_Id == s.Id && wh.RefundDocument.SaleDocument.IsInDebt == true)
            //            .Sum(sum => sum.Count);

            //    var count = s.Count - itemRefunds;
            //    if (count != 0)
            //    {
            //        realization.Add(new RealizationItem()
            //        {

            //            CatalogNumber = s.PriceItem.Gear.CatalogNumber,
            //            IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
            //            Name = s.PriceItem.Gear.Name,
            //            Price = (int) s.Price,
            //            Remainders = remainders,
            //            SoldCount = (int)count,
            //            Uom = s.PriceItem.UnitOfMeasure.Name,
            //            WholePrice = (int) s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
            //            //Amount = (int)((s.Price * count) - s.Discount),
            //            Amount = (int)((s.Price - (s.Discount / s.Count)) * count),
            //            AmountWithoutDebt = amouuntWithoutDebt,
            //            AmountWithoutDebtProfit = amouuntWithoutDebt - amountWholesalePriceWithoutDebt,
            //            Discount = (int) s.Discount,
            //            SaleItemData = s,
            //            Customer = s.SaleDocument.IsInDebt ? s.SaleDocument.Customer_Name : "",
            //            //DebtDischarge = s.SaleDocument.IsInDebt ? debtDisc : 0,
            //            IsInDebt = s.SaleDocument.IsInDebt,
            //            PriceListName = s.PriceItem.PriceLists.First().Name,
            //            SaledDate = s.SaleDocument.SaleDate.ToString("T"),
            //            SaledCount = (int)s.Count,

            //        });
            //    }
            //});
        }
            
        [HttpGet]
        public IEnumerable<RefundItem> GetTodayRefundItems(string userName)
        {
            var now = DateTimeHelper.GetNowKz();
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            var context = new StoreDbContext();

            var closedRefundPerDay = context.RefundsPerDayItems
                .Where(
                    w =>
                        w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                        && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                        && w.SaleDocumentsPerDay.IsClosed &&
                        w.SaleDocumentsPerDay.Creator_Id == userName).Select(s => s.RefundItem.Id).ToList();
            //"RefundDocument/SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices,SaleItem"
            var refundQuery =
                context.RefundItems
                .Include(i => i.RefundDocument)
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.PriceItem.Gear)
                .Include(i => i.PriceItem.UnitOfMeasure)
                .Include(i => i.PriceItem.Remainders)
                .Include(i => i.PriceItem.Prices)
                .Include(i => i.PriceItem.PriceLists)
                .Include(i => i.SaleItem.SaleDocument)
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= startDate
                            && w.RefundDocument.RefundDate <= endDate
                            //&& !closedRefundPerDay.Contains(w.Id)
                            && w.RefundDocument.Creator_Id == userName).ToList();

            var refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();
            return refundItems;
        }

        [HttpGet]
        public IEnumerable<RefundItem> GetRefundItemsByDate(string from, string to, string userName)
        {
            var startDate = DateTime.Parse(from);
            var endDate = DateTime.Parse(to);


            var context = new StoreDbContext();

            var closedRefundPerDay = context.RefundsPerDayItems
                .Where(
                    w =>
                        w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                        && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                        && w.SaleDocumentsPerDay.IsClosed &&
                        w.SaleDocumentsPerDay.Creator_Id == userName).Select(s => s.RefundItem.Id).ToList();
            //"RefundDocument/SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices,SaleItem"
            
            //context.Database.ExecuteSqlCommand("SET ARITHABORT OFF SET QUERY_GOVERNOR_COST_LIMIT 0;");
            //var refundQuery =
            //    context.RefundItems
                
            //        .Where(
            //            w =>
            //                w.RefundDocument.RefundDate >= startDate
            //                && w.RefundDocument.RefundDate <= endDate
            //                //&& !closedRefundPerDay.Contains(w.Id)
            //                && w.RefundDocument.Cr i.PriceItem.Prices)
            //    .Include(i => i.PriceItem.PriceLists)
            //    .Include(i => i.SaleItem.SaleDocument).ToList();

            var refundQuery =
                context.RefundDocuments
                    .Where(
                        w =>
                            w.RefundDate >= startDate
                            && w.RefundDate <= endDate
                            && w.Creator_Id == userName)
                    .SelectMany(s => s.RefundItems)
                    .Include(i => i.RefundDocument)
                    .Include(i => i.PriceItem.Gear)
                    .Include(i => i.PriceItem.UnitOfMeasure)
                    .Include(i => i.PriceItem.Remainders)
                    .Include(i => i.PriceItem.Prices)
                    .Include(i => i.PriceItem.PriceLists)
                    .Include(i => i.SaleItem.SaleDocument);
                    //.Include(i => i.SaleDocument)
                    //.Include("RefundItems.PriceItem")
                    //.Include("RefundItems.PriceItem.Gear")
                    //.Include("RefundItems.PriceItem.UnitOfMeasure")
                    //.Include("RefundItems.PriceItem.Remainders")
                    //.Include("RefundItems.PriceItem.Prices")
                    //.Include("RefundItems.PriceItem.PriceLists")
                    //.Include("RefundItems.SaleItem.SaleDocument");

            var refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();
            return refundItems;
        }

        [HttpPost]
        public HttpResponseMessage AddRealizationPerDay(SaleDocumentsPerDay saleDocumentsPerDay)
        {
            
            
            //clearvalues
            saleDocumentsPerDay.Creator = null;

            var items = saleDocumentsPerDay.SalesPerDayItems;
            saleDocumentsPerDay.SalesPerDayItems = null;


            var context = new StoreDbContext();
            using (var tr = context.Database.BeginTransaction()) 
            {
                try
                {
                    context.SaleDocumentsPerDays.Add(saleDocumentsPerDay);
                    context.SaveChanges();

                    foreach (var item in items)
                    {
                        item.SaleDocumentsPerDay_Id = saleDocumentsPerDay.Id;

                        item.SaleDocumentsPerDay = null;
                        item.SaleItem = null;

                        context.SalesPerDayItems.Add(item);
                    }
                    context.SaveChanges();
                    tr.Commit();
                }
                catch (Exception exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(saleDocumentsPerDay.Id.ToString())
            };
        }

        [HttpGet]
        public IEnumerable<SaleDocumentsPerDay> GetSaleDocumentsPerDays(string from, string to, string userName = "")
        {
            var strt = DateTime.Parse(from);
            var end = DateTime.Parse(to);

            var startDate = new DateTime(strt.Year, strt.Month, strt.Day, 0, 0, 0);
            var endDate = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            var context = new StoreDbContext();

            var result = 
                context.SaleDocumentsPerDays
                                    .Where(rl => rl.SaleDocumentsDate >= startDate
                                    && rl.SaleDocumentsDate <= endDate);
            if (!string.IsNullOrEmpty(userName))
                result = result.Where(rl => rl.Creator_Id == userName);

            result = result
                .Include(i => i.Creator)
                .Include(i => i.SalesPerDayItems);
            return result.OrderByDescending(or => or.SaleDocumentsDate);
        }

        [HttpGet]
        public SaleDocumentsPerDay GetSaleDocumentsPerDayByBarcode(string barcode)
        {
            var context = new StoreDbContext();
            var priceItem = context.SaleDocumentsPerDays.Where(i => i.Barcode == barcode).FirstOrDefault();

            return priceItem;
        }

        [HttpGet]
        public IEnumerable<PriceList> GetPriceLists()
        {
            var context = new StoreDbContext();
            return context.PriceLists;
        }

        [HttpPost]
        public HttpResponseMessage SaveRecipe(SaleDocument saleDocument)
        {
            var logger = LogManager.GetLogger("*");
            

            var creator = saleDocument.Creator;

            logger.Info("-------------------------------------------------------------------------");
            logger.Info("Создан товарный чек №{0} от {1} пользователем {2}", saleDocument.Number, saleDocument.SaleDate, creator.UserName);
            logger.Info("Позиции документа:");

            //clear references
            saleDocument.Customer = null;
            saleDocument.Creator = null;
            saleDocument.LastChanger = null;

            var salesItems = saleDocument.SaleItems;
            saleDocument.SaleItems = null;

            var context = new StoreDbContext();

            using (var tr = context.Database.BeginTransaction())
            {

                try
                {

                    context.SaleDocuments.Add(saleDocument);
                    context.SaveChanges();

                    var rems = new List<Remainder>();

                    foreach (var item in salesItems)
                    {
                        item.PriceItem = null;
                        item.SaleDocument_Id = saleDocument.Id;

                        var rem =
                            context.Remainders.Where(
                                w =>
                                    w.Warehouse_Id == creator.Warehouse_Id &&
                                    w.PriceItem_Id == item.PriceItem_Id)
                                .FirstOrDefault();

                        if (!saleDocument.IsOrder)
                        {
                            if (rem == null)
                            {
                                rem = new Remainder();
                                rem.PriceItem_Id = item.PriceItem_Id;
                                rem.RemainderDate = DateTimeHelper.GetNowKz();
                                rem.Warehouse_Id = creator.Warehouse_Id;
                                rem.Amount -= item.Count;
                                context.Remainders.Add(rem);
                            }
                            else
                            {
                                //var rem =
                                //    item.PriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                                //        .FirstOrDefault();
                                rem.Amount -= item.Count;
                                rem.RemainderDate = DateTimeHelper.GetNowKz();
                                //context.Remainders.Attach(rem);
                                //context.Entry(rem).State = EntityState.Modified;

                            }
                            rems.Add(rem);
                            //ctx.AttachTo("Remainders", rem);

                        }

                        //item.PriceItem.Remainders.Add(rem);

                        //receipt.SaleItems.Add(item);
                        //ctx.AddToSaleItems(item);
                        context.SaleItems.Add(item);
                        logger.Info(" - {0}  {1}", item.CatalogNumber, item.Name);

                    }

                    context.SaveChanges();

                    foreach (var rem in rems)
                    {
                        var remainderChanges = new RemainderChanges();
                        remainderChanges.Remainder_Id = rem.Id;
                        var now = DateTimeHelper.GetNowKz();

                        remainderChanges.ChangesDate = now;
                        context.RemaindersChanges.Add(remainderChanges);
                    }
                    context.SaveChanges();

                    tr.Commit();
                    logger.Info("-------------------------------------------------------------------------");

                }
                catch (Exception exception)
                {
                    logger.Error("Произошла ошибка!", exception);
                    tr.Rollback();
                    throw;
                }
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = new StringContent(saleDocument.Id.ToString())
            };
        }

        [HttpGet]
        public IEnumerable<RefundDocument> GetRefundDocuments(string startDate, string endDate, string userName = "")
        {
            var context = new StoreDbContext();

            var from = DateTime.Parse(startDate);
            var to = DateTime.Parse(endDate);

            var refunds = context.RefundDocuments
                    .Where(rl => rl.RefundDate >= from
                    && rl.RefundDate <= to);
            if (!string.IsNullOrEmpty(userName))
                refunds = refunds.Where(rl => rl.Creator_Id == userName);

            refunds = refunds
                .Include(i => i.SaleDocument)
                .Include(i => i.RefundItems);
            return refunds.OrderByDescending(or => or.RefundDate);
        }

        [HttpGet]
        public RefundDocument GetRefundDocument(long id)
        {
            var context = new StoreDbContext();
            return context.RefundDocuments
                .Include("RefundItems.PriceItem.Gear")
                .Include("RefundItems.PriceItem.Prices")
                .Include("RefundItems.SaleItem")
                .Where(r => r.Id == id).FirstOrDefault();
        }
        //RefundItems/PriceItem/Gear,RefundItems/PriceItem/Prices,RefundItems/SaleItem
        [HttpPost]
        public HttpResponseMessage SaveRefundNew(RefundDocument document)
        {
            var creator = document.Creator;
            //clearing refs
            document.Creator = null;
            document.LastChanger = null;
            document.SaleDocument = null;

            var items = document.RefundItems;
            document.RefundItems = null;

            var context = new StoreDbContext();
            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    var lastNum = GetLastRefundNumber();

                    if (!string.IsNullOrEmpty(lastNum))
                    {
                        int lastNumber = 0;
                        if (int.TryParse(lastNum, out lastNumber))
                        {
                            document.RefundNumber = (++lastNumber).ToString();
                        }
                    }
                    else
                    {
                        document.RefundNumber = "1";
                    }
                    context.RefundDocuments.Add(document);
                    context.SaveChanges();

                    var rems = new List<Remainder>();

                    foreach (var item in items)
                    {
                        item.PriceItem = null;
                        item.RefundDocument = null;
                        item.SaleItem = null;

                        item.RefundDocument_Id = document.Id;

                        var rem =
                            context.Remainders.Where(w => w.Warehouse_Id == creator.Warehouse_Id
                                                          && w.PriceItem_Id == item.PriceItem_Id)
                                .FirstOrDefault();

                        if (rem != null)
                        {
                            rem.Amount += item.Count;
                            rem.RemainderDate = DateTimeHelper.GetNowKz();
                        }
                        rems.Add(rem);

                        context.RefundItems.Add(item);
                        //item.PriceItem.Remainders.Add(rem);
                    }
                    context.SaveChanges();

                    foreach (var rem in rems)
                    {
                        var remainderChanges = new RemainderChanges();
                        remainderChanges.Remainder_Id = rem.Id;
                        var now = DateTimeHelper.GetNowKz();

                        remainderChanges.ChangesDate = now;
                        context.RemaindersChanges.Add(remainderChanges);
                    }
                    context.SaveChanges();

                    tr.Commit();

                }
                catch (Exception exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(document.Id.ToString())
            };
        }

        [HttpPost]
        public HttpResponseMessage SaveRefund(string request)
        {
            string res = request;
            //var refundDocument = document;//container.Refund;
            ////clearing refs
            //refundDocument.Creator = null;
            //refundDocument.LastChanger = null;
            //refundDocument.SaleDocument = null;

            //refundDocument.RefundItems = null;

            //var context = new StoreDbContext();

            //var lastNum = GetLastRefundNumber();

            //if (!string.IsNullOrEmpty(lastNum))
            //{
            //    int lastNumber = 0;
            //    if (int.TryParse(lastNum, out lastNumber))
            //    {
            //        refundDocument.RefundNumber = lastNumber.ToString();
            //    }
            //}
            //context.RefundDocuments.Add(refundDocument);
            //context.SaveChanges();

            //////foreach (var item in container.Items)
            //////{
            ////var refundN = container.Items;
            ////var item = new RefundItem();
            ////item.Amount = refundN.Amount;
            ////item.Count = refundN.Count;
            ////item.Discount = refundN.Discount;
            ////item.Id = refundN.Id;
            ////item.Price = refundN.Price;
            ////item.PriceItem_Id = refundN.PriceItem_Id;
            ////item.RefundDocument_Id = refundN.RefundDocument_Id;
            ////item.SaleItem_Id = refundN.SaleItem_Id;

            //////var item = container.Items;
            //////    item.PriceItem = null;
            //////    item.RefundDocument = null;
            //////    item.SaleItem = null;

            ////    item.RefundDocument_Id = refundDocument.Id;

            ////    var rem =
            ////        context.Remainders.Where(w => w.Warehouse_Id == refundDocument.Creator.Warehouse_Id
            ////            && w.PriceItem_Id == item.PriceItem_Id)
            ////            .FirstOrDefault();

            ////    rem.Amount += item.Count;
            ////    rem.RemainderDate = DateTimeHelper.GetNowKz();

            ////    context.RefundItems.Add(item);
            ////    //item.PriceItem.Remainders.Add(rem);

            //////}
            //context.SaveChanges();
            //return new HttpResponseMessage(HttpStatusCode.Accepted)
            //{
            //    Content = new StringContent(refundDocument.Id.ToString())
            //};

            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }

        [HttpGet]
        public SaleDocument GetRecipe(long id)
        {
            var context = new StoreDbContext();
            return context.SaleDocuments
                .Include(i => i.Creator)
                .Include(i => i.Customer)
                .Include(i => i.SaleItems)
                .Include("SaleItems.PriceItem")
                .Include("SaleItems.PriceItem.Gear")
                .Include("SaleItems.PriceItem.Prices")
                .Where(r => r.Id == id).FirstOrDefault();
        }

        [HttpGet]
        public IEnumerable<RefundDocument> GetRefundDocumentsByRecipId(long recipeId)
        {
            var context = new StoreDbContext();

            var refunds = context.RefundDocuments
                .Include(i => i.Creator)
                .Include(i => i.LastChanger)
                .Include(i => i.SaleDocument)
                .Include(i => i.RefundItems)
                .Include("RefundItems.PriceItem")
                .Include("RefundItems.SaleItem")
                .Where(rl => rl.SaleDocument_Id == recipeId);

            return refunds.OrderByDescending(or => or.RefundDate);
        }

        [HttpGet]
        public IEnumerable<SaleDocument> GetRecipesByUser(string user)
        {
            var context = new StoreDbContext();

            return context.SaleDocuments
                .Include(i => i.SaleItems)
                .Include(i => i.Customer)
                .Include(i => i.Creator)
                .Where(f => !f.IsOrder && f.Creator_Id == user)
                    .OrderByDescending(or => or.SaleDate);
        }

        [HttpPost]
        public HttpResponseMessage AddRemaindersUserChange(RemaindersUserChange remaindersUserChange)
        {

            var context = new StoreDbContext();

            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    context.RemaindersUserChanges.Add(remaindersUserChange);
                    context.SaveChanges();

                    context.SaveChanges();

                    var remainderChanges = new RemainderChanges();
                    remainderChanges.Remainder_Id = remaindersUserChange.Remainder_Id;
                    var now = DateTimeHelper.GetNowKz();

                    remainderChanges.ChangesDate = now;
                    context.RemaindersChanges.Add(remainderChanges);
                    context.SaveChanges();

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpGet]
        public Remainder GetRemainder(long id)
        {
            var context = new StoreDbContext();
            return context.Remainders.Where(r => r.Id == id).FirstOrDefault();
        }

        [HttpPost]
        public HttpResponseMessage DeleteRemainderUserChange(long id)
        {
            var context = new StoreDbContext();
            var finded = context.RemaindersUserChanges.Where(r => r.Id == id).FirstOrDefault();
            if (finded != null)
            {
                context.RemaindersUserChanges.Remove(finded);
                context.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage DeleteRemainder(long id)
        {
            var context = new StoreDbContext();
            var finded = context.Remainders.Where(r => r.Id == id).FirstOrDefault();
            if (finded != null)
            {
                context.Remainders.Remove(finded);
                context.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage DeleteWholesalePrice(long id)
        {
            var context = new StoreDbContext();
            var finded = context.WholesalePrices.Where(r => r.Id == id).FirstOrDefault();
            if (finded != null)
            {
                context.WholesalePrices.Remove(finded);
                context.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpGet]
        public PriceItem DeletePriceItem(long id)
        {
            var context = new StoreDbContext();
            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    var finded = context.PriceItems
                        .Include(i => i.Remainders)
                        .Include(i => i.Prices)
                        .Include("Remainders.RemaindersUserChanges")
                        .Where(r => r.Id == id).FirstOrDefault();
                    if (finded != null)
                    {
                        var remlist = finded.Remainders.ToList();
                        //finded.Remainders.Clear();
                        //context.SaveChanges();

                        foreach (var item in remlist)
                        {

                            var remchlist = item.RemaindersUserChanges.ToList();
                            //item.RemaindersUserChanges.Clear();
                            //context.SaveChanges();

                            foreach (var rem in remchlist)
                            {
                        
                                context.RemaindersUserChanges.Remove(rem);
                            }
                            context.SaveChanges();

                            var remchanges = context.RemaindersChanges.Where(w => w.Remainder_Id == item.Id).ToList();
                            foreach (var remchange in remchanges)
                            {
                                context.RemaindersChanges.Remove(remchange);
                            }
                            context.SaveChanges();

                            context.Remainders.Remove(item);
                        }
                        context.SaveChanges();

                        var prses = finded.Prices.ToList();
                        //finded.Prices.Clear();
                        //context.SaveChanges();

                        foreach (var price in prses)
                        {
                            var chngOldPrices =
                                context.PriceChangeReportItems.Where(w => w.PreviousPrice_Id == price.Id).ToList();
                            foreach (var chngOld in chngOldPrices)
                            {
                                context.PriceChangeReportItems.Remove(chngOld);
                            }
                            context.SaveChanges();

                            var chngNewPrices =
                                context.PriceChangeReportItems.Where(w => w.NewPrice_Id == price.Id).ToList();
                            foreach (var chngNew in chngNewPrices)
                            {
                                context.PriceChangeReportItems.Remove(chngNew);
                            }
                            context.SaveChanges();

                            context.WholesalePrices.Remove(price);
                        }
                        context.SaveChanges();

                        finded.PriceLists.Clear();

                        context.SaveChanges();

                        var news = context.GearNews.Where(g => g.Gear_Id == finded.Gear_Id).ToList();
                        foreach (var n in news)
                        {
                            context.GearNews.Remove(n);
                        }
                        context.SaveChanges();

                        var gearchngs = context.GearsChanges.Where(g => g.Gear_Id == finded.Gear_Id).ToList();
                        foreach (var n in gearchngs)
                        {
                            context.GearsChanges.Remove(n);
                        }
                        context.SaveChanges();

                        long gearId = finded.Gear_Id;

                        var incomeItems = context.IncomeItems.Where(w => w.PriceItem_Id == finded.Id).ToList();
                        foreach (var incomeItem in incomeItems)
                        {
                            context.IncomeItems.Remove(incomeItem);
                        }
                        context.SaveChanges();

                        context.PriceItems.Remove(finded);
                        context.SaveChanges();

                        context.Gears.Remove(context.Gears.Where(g => g.Id == gearId).FirstOrDefault());

                        context.SaveChanges();
                        tr.Commit();
                            
                    }
                }
                catch (Exception exception)
                {
                    tr.Rollback();
                    throw;
                }            
            }
            return new PriceItem();
        }

        [HttpPost]
        public HttpResponseMessage DeleteGearNew(long id)
        {
            var context = new StoreDbContext();
            var finded = context.GearNews.Where(r => r.Id == id).FirstOrDefault();
            if (finded != null)
            {
                context.GearNews.Remove(finded);
                context.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage DeleteGear(long id)
        {
            var context = new StoreDbContext();
            var finded = context.Gears.Where(r => r.Id == id).FirstOrDefault();
            if (finded != null)
            {
                context.Gears.Remove(finded);
                context.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpGet]
        public IEnumerable<WarehouseTransferRequest> GetWarehouseTransferRequests()
        {
            var context = new StoreDbContext();

            return context.WarehouseTransferRequests
                .Include(i => i.WarehouseTransferRequestItemItems)
                    .OrderByDescending(or => or.Id);
        }

        [HttpGet]
        public IEnumerable<WarehouseTransferRequestItem> GetWarehouseTransferRequestItems(long warehouseRequest_id)
        {
            var context = new StoreDbContext();

            return context.WarehouseTransferRequestItems
                .Include(i => i.PriceItem.Gear)
                .Where(r => r.WarehouseTransferRequest_Id == warehouseRequest_id)
                    .OrderByDescending(or => or.Id);
        }

        [HttpGet]
        public IEnumerable<WarehouseTransferRequest> GetReceivedWarehouseTransferRequests(string from, string to, string warehouse)
        {
            var context = new StoreDbContext();

            var dateFrom = DateTime.Parse(from);
            var dateTo = DateTime.Parse(to);
            var result = context.WarehouseTransferRequests
                .Include(i => i.WarehouseTransferRequestItemItems)
                .Where(w => w.Supplier_Id == warehouse
                                && w.RequestDate >= dateFrom && w.RequestDate <= dateTo)
                    .OrderByDescending(or => or.Id);
            return result;
        }

        [HttpGet]
        public IEnumerable<WarehouseTransferRequest> GetSendedWarehouseTransferRequests(string from, string to, string warehouse)
        {
            var context = new StoreDbContext();

            var dateFrom = DateTime.Parse(from);
            var dateTo = DateTime.Parse(to);
            var result = context.WarehouseTransferRequests
                .Include(i => i.WarehouseTransferRequestItemItems)
                .Where(w => w.Customer_Id == warehouse
                                && w.RequestDate >= dateFrom && w.RequestDate <= dateTo)
                    .OrderByDescending(or => or.Id);

            return result;
        }

        [HttpGet]
        public WarehouseTransferRequest GetWarehouseTransferRequest(long id)
        {
            var context = new StoreDbContext();

            return context.WarehouseTransferRequests
                .Include(i => i.WarehouseTransferRequestItemItems)
                    .Where(or => or.Id == id).FirstOrDefault();
        }

        [HttpPost]
        public HttpResponseMessage SaveWarehouseTransferRequest(WarehouseTransferRequest request)
        {

            var context = new StoreDbContext();

            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    context.WarehouseTransferRequests.Attach(request);
                    context.Entry(request).State = EntityState.Modified;
                    foreach (var warehouseTransferRequestModelItem in request.WarehouseTransferRequestItemItems)
                    {

                        context.WarehouseTransferRequestItems.Attach(warehouseTransferRequestModelItem);
                        context.Entry(warehouseTransferRequestModelItem).State = EntityState.Modified;

                    }
                    context.SaveChanges();
                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }


            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage AcceptWarehouseTransferRequest(WarehouseTransferRequest request)
        {

            var logger = LogManager.GetLogger("*");
            logger.Info("========================================================");
            logger.Info("Подтверждение перемещения...");

            var context = new StoreDbContext();

            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    context.WarehouseTransferRequests.Attach(request);
                    context.Entry(request).State = EntityState.Modified;

                    var rems = new List<Remainder>();

                    foreach (var warehouseTransferRequestModelItem in request.WarehouseTransferRequestItemItems)
                    {

                        context.WarehouseTransferRequestItems.Attach(warehouseTransferRequestModelItem);
                        context.Entry(warehouseTransferRequestModelItem).State = EntityState.Modified;

                        var sourceRem =
                            context.Remainders
                            .Include(i => i.PriceItem.Gear)
                            .Where(
                                    w =>
                                        w.PriceItem_Id == warehouseTransferRequestModelItem.PriceItem_Id &&
                                        w.Warehouse_Id == request.Supplier_Id).FirstOrDefault();

                        if (sourceRem != null)
                        {
                            logger.Info("\tОстатки {0} у отправителя {1} {2}шт.", sourceRem.PriceItem.Gear.Name + ", "
                                + sourceRem.PriceItem.Gear.CatalogNumber,
                                request.Supplier_Id, sourceRem.Amount);
                            //if (sourceRem.Amount >= warehouseTransferRequestModelItem.CountAccepted)
                            //{

                                var targetRem =
                                    context.Remainders
                                    .Include(i => i.PriceItem.Gear)
                                    .Where(
                                        w =>

                                            w.PriceItem_Id == warehouseTransferRequestModelItem.PriceItem_Id &&
                                            w.Warehouse_Id == request.Customer_Id).FirstOrDefault();

                                if (targetRem == null)
                                {


                                    logger.Info("\tОстатки {0} у получателя {1} 0шт.", sourceRem.PriceItem.Gear.Name + ", "
                                        + sourceRem.PriceItem.Gear.CatalogNumber,
                                        request.Supplier_Id);

                                    targetRem = new Remainder();
                                    targetRem.Amount = warehouseTransferRequestModelItem.CountAccepted;
                                    targetRem.PriceItem_Id = warehouseTransferRequestModelItem.PriceItem_Id;
                                    targetRem.RemainderDate = DateTimeHelper.GetNowKz();
                                    targetRem.Warehouse_Id = request.Customer_Id;
                                    context.Remainders.Add(targetRem);

                                    logger.Info("\tПринято: {0}", warehouseTransferRequestModelItem.CountAccepted);
                                }
                                else
                                {

                                    logger.Info("\tОстатки {0} у получателя {1} {2}шт.", targetRem.PriceItem.Gear.Name + ", "
                                        + targetRem.PriceItem.Gear.CatalogNumber,
                                        request.Supplier_Id, targetRem.Amount);

                                    targetRem.Amount += warehouseTransferRequestModelItem.CountAccepted;
                                    targetRem.RemainderDate = DateTimeHelper.GetNowKz();
                                    context.Remainders.Attach(targetRem);
                                    context.Entry(targetRem).State = EntityState.Modified;

                                    logger.Info("\tПринято: {0}", warehouseTransferRequestModelItem.CountAccepted);
                                }
                                rems.Add(targetRem);

                                sourceRem.RemainderDate = DateTimeHelper.GetNowKz();
                                sourceRem.Amount -= warehouseTransferRequestModelItem.CountAccepted;
                                context.Remainders.Attach(sourceRem);
                                context.Entry(sourceRem).State = EntityState.Modified;
                                logger.Info("\tСписано: {0}", warehouseTransferRequestModelItem.CountAccepted);
                                //rems.Add(sourceRem);
                            //}
                        }
                        else
                        {
                            var targetRem =
                                context.Remainders.Where(
                                    w =>
                                        w.PriceItem_Id == warehouseTransferRequestModelItem.PriceItem_Id &&
                                        w.Warehouse_Id == request.Customer_Id).FirstOrDefault();

                            var priceItem =
                                context.PriceItems
                                    .Include(i => i.Gear)
                                    .Where(
                                        pr => pr.Id == warehouseTransferRequestModelItem.PriceItem_Id)
                                    .FirstOrDefault();

                            if (targetRem == null)
                            {


                                logger.Info("\tОстатки {0} у получателя {1} 0шт.", priceItem.Gear.Name + ", "
                                    + priceItem.Gear.CatalogNumber,
                                    request.Supplier_Id);

                                targetRem = new Remainder();
                                targetRem.Amount = warehouseTransferRequestModelItem.CountAccepted;
                                targetRem.PriceItem_Id = warehouseTransferRequestModelItem.PriceItem_Id;
                                targetRem.RemainderDate = DateTimeHelper.GetNowKz();
                                targetRem.Warehouse_Id = request.Customer_Id;
                                context.Remainders.Add(targetRem);
                            }
                            else
                            {
                                logger.Info("\tОстатки {0} у получателя {1} {2}шт.", priceItem.Gear.Name + ", "
                                    + priceItem.Gear.CatalogNumber,
                                    request.Supplier_Id, targetRem.Amount);

                                targetRem.Amount += warehouseTransferRequestModelItem.CountAccepted;
                                targetRem.RemainderDate = DateTimeHelper.GetNowKz();
                                context.Remainders.Attach(targetRem);
                                context.Entry(targetRem).State = EntityState.Modified;
                            }
                            rems.Add(targetRem);
                        }

                    }
                    context.SaveChanges();

                    foreach (var rem in rems)
                    {
                        var remainderChanges = new RemainderChanges();
                        remainderChanges.Remainder_Id = rem.Id;
                        var now = DateTimeHelper.GetNowKz();

                        remainderChanges.ChangesDate = now;
                        context.RemaindersChanges.Add(remainderChanges);
                    }
                    context.SaveChanges();

                    tr.Commit();
                }
                catch (Exception exception)
                {
                    logger.Error(exception.Message, exception);
                    logger.Info("========================================================");
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            logger.Info("========================================================");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        public HttpResponseMessage AddWarehouseTransferRequest(WarehouseTransferRequest request)
        {

            request.Creator = null;
            request.Customer = null;
            request.LastChanged = null;
            request.Supplier = null;

            var items = request.WarehouseTransferRequestItemItems;
            request.WarehouseTransferRequestItemItems = null;

            var context = new StoreDbContext();

            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    context.WarehouseTransferRequests.Add(request);
                    context.SaveChanges();

                    foreach (var item in items)
                    {
                        item.PriceItem = null;
                        item.WarehouseTransferRequest_Id = request.Id;

                        context.WarehouseTransferRequestItems.Add(item);
                    }
                    context.SaveChanges();

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
            }


            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(request.Id.ToString())
            };
        }
        [HttpPost]
        public HttpResponseMessage AddCustomer(Customer customer)
        {

            var context = new StoreDbContext();
            context.Customers.Add(customer);
            context.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
        [HttpPost]
        public HttpResponseMessage SaveCustomer(Customer customer)
        {

            var context = new StoreDbContext();
            context.Customers.Attach(customer);
            context.Entry(customer).State = EntityState.Modified;
            
            context.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public HttpResponseMessage AddSupplier(Supplier supplier)
        {

            var context = new StoreDbContext();
            context.Suppliers.Add(supplier);
            context.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
        [HttpPost]
        public HttpResponseMessage SaveSupplier(Supplier supplier)
        {

            var context = new StoreDbContext();
            context.Suppliers.Attach(supplier);
            context.Entry(supplier).State = EntityState.Modified;

            context.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }


        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDbContext"].ConnectionString);

            return connection;
        }
    }

    [Serializable]
    [DataContract]
    public class CashFlowItemModel
    {
        [DataMember]
        public string Cashier { get; set; }
        [DataMember]
        public int Sales { get; set; }
        [DataMember]
        public int PreviousSales { get; set; }
        [DataMember]
        public int SalesByWholesales { get; set; }
        [DataMember]
        public int PreviousSalesByWholesales { get; set; }


        public decimal SalesDifference
        {
            get
            {
                if (PreviousSales == 0)
                    return 100;
                //return (((decimal)PreviousSales) / ((decimal)Sales)) * 100;
                return (((decimal)Sales) / ((decimal)PreviousSales)) * 100;
            }
        }
        [DataMember]
        public int Refunds { get; set; }
        [DataMember]
        public int PreviousRefunds { get; set; }
        [DataMember]
        public int RefundsByWholesales { get; set; }
        [DataMember]
        public int PreviousRefundsByWholesales { get; set; }

        public decimal RefundsDifference
        {
            get
            {
                if (PreviousRefunds == 0)
                    return 100;
                //return (((decimal)PreviousRefunds) / ((decimal)Refunds)) * 100;
                return (((decimal)Refunds) / ((decimal)PreviousRefunds)) * 100;
            }
        }
        [DataMember]
        public int DebdsByWholesales { get; set; }
        [DataMember]
        public int PreviousDebdsByWholesales { get; set; }
        [DataMember]
        public int Debds { get; set; }
        [DataMember]
        public int PreviousDebds { get; set; }


        public decimal DebdsDifference
        {
            get
            {
                if (PreviousDebds == 0)
                    return 100;
                //return (((decimal)PreviousDebds) / ((decimal)Debds)) * 100;
                return (((decimal)Debds) / ((decimal)PreviousDebds)) * 100;
            }
        }
        [DataMember]
        public int DebdDischarges { get; set; }
        [DataMember]
        public int PreviousDebdDischarges { get; set; }

        public decimal DebdDischargesDifference
        {
            get
            {
                if (PreviousDebdDischarges == 0)
                    return 100;
                //return (((decimal)PreviousDebdDischarges) / ((decimal)DebdDischarges)) * 100;
                return (((decimal)DebdDischarges) / ((decimal)PreviousDebdDischarges)) * 100;
            }
        }

        public int Totals
        {
            get { return (Sales - Refunds - Debds) + DebdDischarges; }
        }

        public int PreviousTotals
        {
            get { return (PreviousSales - PreviousRefunds - PreviousDebds) + PreviousDebdDischarges; }
        }
        public decimal TotalsDifference
        {
            get
            {
                if (PreviousTotals == 0)
                    return 100;
                //return (((decimal)PreviousTotals) / ((decimal)Totals)) * 100;
                return (((decimal)Totals) / ((decimal)PreviousTotals)) * 100;
            }
        }

        public int AdvancedProfit
        {
            get { return Totals - ((SalesByWholesales - RefundsByWholesales - DebdsByWholesales) + DebdDischarges); }
        }

        public int PreviousAdvancedProfit
        {
            get { return PreviousTotals - ((PreviousSalesByWholesales - PreviousRefundsByWholesales - PreviousDebdsByWholesales) + PreviousDebdDischarges); }
        }
        public decimal AdvancedProfitDifference
        {
            get
            {
                if (PreviousAdvancedProfit == 0)
                    return 100;
                return (((decimal)AdvancedProfit) / ((decimal)PreviousAdvancedProfit)) * 100;
            }
        }
    }

    
    public enum PeriodTypes
    {
        Day,
        Week,
        Month,
        HalfYear,
        Year,
        Custom
    }

    [Serializable]
    [DataContract]
    public class IncomeItem 
    {
        [DataMember]
        public string Articul { get; set; }
        [DataMember]
        public string CatalogNumber { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string IsDuplicate { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public long Gear_Id { get; set; }
        [DataMember]
        public string Uom { get; set; }
        [DataMember]
        public long PriceItem_Id { get; set; }
        [DataMember]
        public decimal RecommendedRemainder { get; set; }
        [DataMember]
        public decimal LowerLimitRemainder { get; set; }
        [DataMember]
        public int WholesalePrice { get; set; }
        [DataMember]
        public int BuyPriceRur { get; set; }
        [DataMember]
        public int BuyPriceTng { get; set; }
        [DataMember]
        public int Remainders { get; set; }
        [DataMember]
        public long Income_ID { get; set; }
        [DataMember]
        public string Income { get; set; }
        [DataMember]
        public int Incomes { get; set; }
        [DataMember]
        public bool IsAccepted { get; set; }
        [DataMember]
        public int NewPrice { get; set; }
        [DataMember]
        public DateTime IncomeDate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class AcceptIncomeContainer
    {
        [DataMember]
        public Income Income { get; set; }
        [DataMember]
        public IEnumerable<IncomeItem> IncomeItems { get; set; }
        [DataMember]
        public string Warehouse { get; set; }
    }

    [DataContract]
    public class RealizationItem
    {
        [DataMember]
        public string Articul { get; set; }
        [DataMember]
        public string CatalogNumber { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string IsDuplicate { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string Uom { get; set; }
        [DataMember]
        public string PriceListName { get; set; }
        [DataMember]
        public string SaledDate { get; set; }
        [DataMember]
        public string SaledNumber { get; set; }
        [DataMember]
        public bool IsInDebt { get; set; }
        [DataMember]
        public string Customer
        {
            get;
            set;
        }
        [DataMember]
        public int DebtDischarge
        {
            get;
            set;
        }
        [DataMember]
        public SaleItem SaleItemData { get; set; }

        private int _price = 0;
        [DataMember]
        public int Price
        {
            get { return _price; } // - (Discount / SoldCount); }
            set
            {
                _price = value;
            }
        }

        public int PriceWithDiscount
        {
            get { return _price - (SaledCount == 0 ? 0 : Discount / SaledCount); }
        }

        private int _wholePrice = 0;
        [DataMember]
        public int WholePrice
        {
            get { return _wholePrice; }
            set
            {
                _wholePrice = value;
            }
        }
        [DataMember]
        public int Remainders { get; set; }
        [DataMember]
        public int SoldCount { get; set; }
        [DataMember]
        public int SaledCount { get; set; }
        [DataMember]
        public int Amount { get; set; }
        [DataMember]
        public int AmountWithoutDebt { get; set; }
        [DataMember]
        public int AmountWithoutDebtProfit { get; set; }
        [DataMember]
        public int Discount { get; set; }
        [DataMember]
        public string Additional { get; set; }
    }


    #region Temp cashflow classes

    public class RefundTEMP
    {
        public string Cashier { get; set; }
        public decimal Refunds { get; set; }
        public decimal WholesaleRefunds { get; set; }
        public decimal PreviousRefunds { get; set; }
        public decimal PreviousWholesaleRefunds { get; set; }
    }

    public class RealizationTEMP
    {
        public string Cashier { get; set; }
        public decimal Sales { get; set; }
        public decimal WholesaleSales { get; set; }
        public decimal PreviousSales { get; set; }
        public decimal PreviousWholesaleSales { get; set; }
    }

    public class DebtTEMP
    {
        public string Cashier { get; set; }
        public decimal Debts { get; set; }
        public decimal WholesaleDebts { get; set; }
        public decimal PreviousDebts { get; set; }
        public decimal PreviousWholesaleDebts { get; set; }
    }

    public class DebtDischargeTEMP
    {
        public string Cashier { get; set; }
        public decimal DebDischargests { get; set; }
        public decimal PreviousDischarges { get; set; }
    }
    #endregion

}
