
namespace StoreAppTest.Utilities
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Client.Model;
    using GalaSoft.MvvmLight.Threading;
    using Kent.Boogaart.KBCsv;
    using MSPToolkit.Encodings;
    using Newtonsoft.Json;

    public class PriceListLoader : IDisposable
    {
        private string _filePath;
        private Action<IAsyncResult> _callback;
        private Action<IAsyncResult> _errorCallback;

        public PriceListLoader(string filePath)
        {
            _filePath = filePath;
        }


        public IAsyncResult LoadAsync(Action<IAsyncResult> callback, bool overwrite = false, Action<IAsyncResult> errorCallback = null)
        {
            _callback = callback;
            _errorCallback = errorCallback;

            var task = Task.Factory.StartNew(() =>
            {
                using (var reader = new CsvReader(_filePath, new Windows1251Encoding()))
                {
                    reader.ValueSeparator = ';';
                    reader.ReadHeaderRecord();

                    var priceName = Path.GetFileNameWithoutExtension(_filePath);

                    var lst = new PriceList();
                    lst.Name = priceName;


                    lst.UploadDate = DateTimeHelper.GetNowKz();
                    lst.UploadUser_Id = App.CurrentUser.UserName;
                    lst.UploadUser = App.CurrentUser;


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
                        uom = record["Ед. измерения"];
                        var priceS = record["Цена"];
                        if (priceS != string.Empty)
                            decimal.TryParse(priceS, out wholesalePrice);
                        var remainderS = record["Остаток"];
                        if (remainderS != string.Empty)
                            decimal.TryParse(remainderS, out remainders);
                        articul = record["артикул"];


                        if(name == string.Empty || uom == string.Empty) continue;



                        PriceItem priceItem = new PriceItem();
                        priceItem.BuyPriceRur = buyPriceRur;
                        priceItem.BuyPriceTng = buyPriceTn;

                        var findedGear = new Gear();
                        findedGear.Articul = articul;
                        findedGear.CatalogNumber = catalogNumber;
                        findedGear.Category_Id = "Новинки"; //TODO: переделать на чето типа "Обычные"
                        findedGear.IsDuplicate = isDupl;
                        findedGear.Name = name;

                        priceItem.Gear_Id = findedGear.Id;
                        priceItem.Gear = findedGear;

                        var findedUom = new UnitOfMeasure();
                        findedUom.Name = uom;

                        priceItem.Uom_Id = findedUom.Name;
                        priceItem.UnitOfMeasure = findedUom;

                        //priceItem.WholesalePrice = wholesalePrice;


                        Remainder rm = new Remainder();
                        rm.Amount = remainders;
                        rm.Warehouse_Id = App.CurrentUser.Warehouse.Name;
                        rm.Warehouse = App.CurrentUser.Warehouse;
                        rm.RemainderDate = DateTimeHelper.GetNowKz();

                        priceItem.Remainders.Add(rm);
                        //priceItem.PriceList_Id = lst.Name;

                        lst.PriceItems.Add(priceItem);

                    }


                    //Construct the URI to our Web API
                    string action = "LoadPriceList";
                    if (overwrite)
                        action = "UpdatePriceList";

                    Uri apiUri = default(Uri);
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        apiUri = new Uri(
                            string.Concat(
                            Application.Current.Host.Source.Scheme, "://",
                            Application.Current.Host.Source.Host, ":",
                            Application.Current.Host.Source.Port,
                            "/api/PriceLists/", action), UriKind.Absolute);
                    });
                    WebClient client = new WebClient();
                    client.Headers["Accept"] = "application/json";
                    client.Headers["Content-Type"] = "application/json";

                    var listString = JsonConvert.SerializeObject(lst);
                    bool uploadCompleted = false;
                    client.UploadStringCompleted += (sender, args) => { uploadCompleted = true; };
                    client.UploadStringAsync(apiUri, listString);
                    while (!uploadCompleted)
                    {
                        Thread.Sleep(100);
                    }

                }

                //XLSXReader xlsxReader = new XLSXReader(_fileInfo);
                //var readedData = xlsxReader.GetData("Каталог");
                //string priceListName = _fileInfo.Name.Replace(_fileInfo.Extension, "");
                ////new Thread(() =>
                ////{

                //StoreDbContext ctx = new StoreDbContext(
                //    new Uri(
                //        string.Concat(
                //    Application.Current.Host.Source.Scheme, "://", 
                //    Application.Current.Host.Source.Host, ":", 
                //    Application.Current.Host.Source.Port,
                //    "/StoreAppDataService.svc/"), UriKind.Relative));
                //    //var gears = ctx.Gears.ExecuteSyncronous().ToList();
                //    //var uoms = ctx.UnitOfMeasures.ExecuteSyncronous().ToList();

                //    var lst = new PriceList();
                //    lst.Name = priceListName;

                //    var warehouse = new Warehouse();
                //    warehouse.Name = "Склад";

                //    User usr = new User();
                //usr.DisplayName = "DD";
                //usr.UserName = "dd";
                //usr.Warehouse_Id = warehouse.Name;
                //usr.Warehouse = warehouse;

                //lst.UploadDate = DateTime.Now;
                //lst.UploadUser_Id = usr.UserName;
                //lst.User = usr;

                //    //ctx.AddToPriceLists(lst);

                //StringBuilder log = new StringBuilder();
                //log.AppendFormat("| N\t | CatalogN\t | *\t| Name\t| Buy RUR\t| Buy TNG\t| UOM\t| Price\t| Remndrs\t| Articul\t|\n\r");
                //    foreach (var row in readedData)
                //    {

                //        string catalogNumber = string.Empty;
                //        bool isDupl = false;
                //        int stringNumber = 0;
                //        string name = string.Empty;
                //        decimal buyPriceRur = 0.00m;
                //        decimal buyPriceTn = 0.00m;
                //        string uom = string.Empty;
                //        decimal wholesalePrice = 0.00m;
                //        decimal remainders = 0;
                //        string articul = string.Empty;

                //        if (row.Contains("B"))
                //        {
                //            if ((string) row["B"] == "Номер детали")
                //                continue;
                //            catalogNumber = (string) row["B"];
                //        }
                //        else
                //            continue;
                //        if (row.Contains("A"))
                //        {
                //            int.TryParse((string) row["A"], out stringNumber);
                //        }
                //        if (row.Contains("C"))
                //        {
                //            bool.TryParse((string) row["C"], out isDupl);
                //        }
                //        if (row.Contains("D"))
                //        {
                //            name = (string) row["D"];
                //        }
                //        if (row.Contains("E"))
                //        {
                //            decimal.TryParse((string) row["E"], out buyPriceRur);
                //        }
                //        if (row.Contains("F"))
                //        {
                //            decimal.TryParse((string) row["F"], out buyPriceTn);
                //        }
                //        if (row.Contains("G"))
                //        {
                //            uom = (string) row["G"];
                //        }
                //        if (row.Contains("H"))
                //        {
                //            decimal.TryParse((string) row["H"], out wholesalePrice);
                //        }
                //        if (row.Contains("I"))
                //        {
                //            decimal.TryParse((string) row["I"], out remainders);
                //        }
                //        if (row.Contains("P"))
                //        {
                //            articul = (string) row["P"];
                //        }

                //        log.AppendFormat("| {9}\t | {0}\t | {1}\t | {2}\t | {3}\t | {4}\t | {5}\t | {6}\t | {7}\t | {8}\t |\n\r",
                //            catalogNumber,isDupl == true ? "*" : "", name, buyPriceRur, buyPriceTn, uom, wholesalePrice, remainders, articul, stringNumber);
                        
                //        PriceItem priceItem = new PriceItem();
                //        priceItem.BuyPriceRur = buyPriceRur;
                //        priceItem.BuyPriceTng = buyPriceTn;

                //        //var findedGear = ctx.ExecuteSyncronous(ctx.Gears.Where(f => f.CatalogNumber == catalogNumber)).FirstOrDefault();
                //        //var findedGear = gears.FirstOrDefault(f => f.CatalogNumber == catalogNumber);
                //        //if (findedGear == null)
                //        //{
                //            var findedGear = new Gear();
                //            findedGear.Articul = articul;
                //            findedGear.CatalogNumber = catalogNumber;
                //            findedGear.Category_Id = "Новинки"; //TODO: переделать на чето типа "Обычные"
                //            findedGear.IsDuplicate = isDupl;
                //            findedGear.Name = name;

                //            //ctx.AddToGears(findedGear);
                //        //}
                //        priceItem.Gear_Id = findedGear.CatalogNumber;
                //        priceItem.Gear = findedGear;

                //        //var findedUom = uoms.FirstOrDefault(f => f.Name == uom);
                //        //if (findedUom == null)
                //        //{
                //            var findedUom = new UnitOfMeasure();
                //            findedUom.Name = uom;

                //            //ctx.AddToUnitOfMeasures(findedUom);
                //        //}
                //        priceItem.Uom_Id = findedUom.Name;
                //        priceItem.UnitOfMeasure = findedUom;

                //        priceItem.WholesalePrice = wholesalePrice;


                //        Remainder rm = new Remainder();
                //        rm.Amount = remainders;
                //        rm.Warehouse_Id = warehouse.Name;
                //        rm.Warehouse = warehouse;
                //        rm.RemainderDate = DateTime.Now;
                //        //rm.PriceItem = priceItem;

                //        priceItem.Remainders.Add(rm);
                //        priceItem.PriceList_Id = lst.Name;
                //        //priceItem.PriceList = lst;

                //        lst.PriceItems.Add(priceItem);
                //        //ctx.AddToPriceItems(priceItem);
                //    }

                //    //ctx.BeginSaveChanges(a =>
                //    //{

                //    //}, new object());

                //var logResult = log.ToString();
                //    string action = "LoadPriceList";
                //    if (overwrite)
                //        action = "UpdatePriceList";

                //    //Construct the URI to our Web API
                
                //Uri apiUri = new Uri(
                //    string.Concat(
                //    Application.Current.Host.Source.Scheme, "://",
                //    Application.Current.Host.Source.Host, ":",
                //    Application.Current.Host.Source.Port,
                //    "/api/PriceLists/", action), UriKind.Relative);
                //    WebClient client = new WebClient();
                //    client.Headers["Accept"] = "application/json";
                //    client.Headers["Content-Type"] = "application/json";

                //    var listString = JsonConvert.SerializeObject(lst);
                //    bool uploadCompleted = false;
                //    client.UploadStringCompleted += (sender, args) => { uploadCompleted = true; };
                //    client.UploadStringAsync(apiUri, listString);
                //    while (!uploadCompleted)
                //    {
                //        Thread.Sleep(100);
                //    }
                ////}) {IsBackground = true}.Start();
            })
            .ContinueWith(t =>
            {
                if(_errorCallback != null)
                    _errorCallback.Invoke(t);
            }, TaskContinuationOptions.OnlyOnFaulted)
            .ContinueWith(t =>
            {
                if(_callback != null)
                    callback.Invoke(t);
            });

            return task;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            
        }

        #endregion
    }
}
