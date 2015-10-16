
namespace StoreAppTest.Client
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Xml;
    using System.Xml.Serialization;
    using Controls;
    using GalaSoft.MvvmLight.Threading;
    using Model;
    using Newtonsoft.Json;
    using StoreAppTest.Model;
    using Utilities;
    using ViewModels;
    using IncomeItem = StoreAppTest.Model.IncomeItem;
    using PriceChangeReportItem = Model.PriceChangeReportItem;
    using PriceItem = Model.PriceItem;
    using RefundItem = Model.RefundItem;

    public class StoreapptestClient
    {

        public IEnumerable<PriceItemRemainderView> GetIncomes(string priceListName, string warehouse = "")
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLightPriceItemList?priceListName={0}&warehouse={1}"
                        , priceListName, warehouse))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<PriceItemRemainderView>>(uri);
        }

        public string GetLastInvoiceNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastInvoiceNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public string GetLastReceiptNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastReceiptNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public string GetLastOrderNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastOrderNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public string GetLastRefundNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastRefundNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public string GetLastIncomeNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastIncomeNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public string GetLastPriceChangeReportNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastPriceChangeReportNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public string GetLastSaleDocumentsPerDaysNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastSaleDocumentsPerDaysNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public string GetLastWarehouseTransferRequestNumber()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetLastWarehouseTransferRequestNumber"))
                , UriKind.Absolute);

            return GetInternalData<string>(uri);
        }

        public IEnumerable<Customer> GetCustomers(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUserCustomers?userName={0}"
                        , userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<Customer>>(uri);
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetSuppliers"))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<Supplier>>(uri);
        }

        public IEnumerable<SaleDocument> GetUserDebtReceipts(string userName, string customerName = "")
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUserDebtReceipts?userName={0}&customer={1}"
                        , userName, customerName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<SaleDocument>>(uri);
        }


        public IEnumerable<RefundDocument> GetUserDebtRefunds(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUserDebtRefunds?userName={0}"
                        , userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<RefundDocument>>(uri);
        }

        public IEnumerable<DebtDischargeDocument> GetUserDebtDischargeDocuments(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUserDebtDischargeDocuments?userName={0}"
                        , userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<DebtDischargeDocument>>(uri);
        }

        public IEnumerable<DebtDischargeDocument> GetUserDebtDischargeDocuments(DateTime from, DateTime to,
            string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUserDebtDischargeDocuments?from={0}?to={1}?userName={2}"
                        , from, to, userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<DebtDischargeDocument>>(uri);
        }

        public IEnumerable<DebtDischargeDocument> GetUserDebtDischargeDocumentsByDate(DateTime from, DateTime to,
            string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUserDebtDischargeDocumentsByDate?from={0}&to={1}&userName={2}"
                        , from, to, userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<DebtDischargeDocument>>(uri);
        }

        public IEnumerable<Warehouse> GetWarehouses()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetWarehouses"))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<Warehouse>>(uri);
        }

        public IEnumerable<User> GetUsers()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUsers"))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<User>>(uri);
        }



        public void AddDebt(DebtDischargeDocument document)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddDebtDischarge"))
                , UriKind.Absolute);

            SaveInternalData(uri, document);
        }

        public void AddUser(User user)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddUser"))
                , UriKind.Absolute);

            SaveInternalData(uri, user);
        }

        public void SaveUser(User user)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveUser"))
                , UriKind.Absolute);

            SaveInternalData(uri, user);
        }

        public void RemoveUser(User user)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/RemoveUser"))
                , UriKind.Absolute);

            SaveInternalData(uri, user);
        }

        public void AddWarehouse(Warehouse warehouse)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddWarehouse"))
                , UriKind.Absolute);

            SaveInternalData(uri, warehouse);
        }

        public void SaveWarehouse(Warehouse warehouse)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveWarehouse"))
                , UriKind.Absolute);

            SaveInternalData(uri, warehouse);
        }

        public void RemoveWarehouse(Warehouse warehouse)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/RemoveWarehouse"))
                , UriKind.Absolute);

            SaveInternalData(uri, warehouse);
        }

        public void SetPassword(string userName, string passwordHash)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SetPassword?userName={0}&passwordHash={1}"
                        , userName, passwordHash))
                , UriKind.Absolute);

            GetInternalData<HttpWebResponseMessage>(uri);
        }


        public IEnumerable<CashFlowItemModel> GetCashFlowItems(DateTime AtFromDate, DateTime AtToDate,
            DateTime PreviousAtFromDate, DateTime PreviousAtToDate, string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format(
                        "/api/PriceLists/GetCashFlowItems?AtFromDate={0}&AtToDate={1}&PreviousAtFromDate={2}&PreviousAtToDate={3}&userName={4}",
                        AtFromDate, AtToDate, PreviousAtFromDate, PreviousAtToDate, userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<CashFlowItemModel>>(uri);
        }

        public Income GetIncomeByKey(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetIncomeByKey?id={0}",
                        id.ToString()))
                , UriKind.Absolute);

            return GetInternalData<Income>(uri);
        }

        public IEnumerable<IncomeItem> GetIncomeItems(DateTime AtFromDate, DateTime AtToDate,
            string priceListName, string userName, string warehouse = "")
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format(
                        "/api/PriceLists/GetIncomeItems?AtFromDate={0}&AtToDate={1}&priceListName={2}&userName={3}&warehouse={4}",
                        AtFromDate, AtToDate, priceListName, userName, warehouse))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<IncomeItem>>(uri);
        }

        public void SaveIncome(Income income)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveIncome"))
                , UriKind.Absolute);

            SaveInternalData(uri, income);
        }

        public void SavePriceItem(PriceItem priceItem)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SavePriceItem"))
                , UriKind.Absolute);

            SaveInternalData(uri, priceItem);
        }

        public void AcceptIncome(AcceptIncomeContainer incomeContainer)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AcceptIncome"))
                , UriKind.Absolute);

            SaveInternalData(uri, incomeContainer);
        }

        public IEnumerable<UnitOfMeasure> GetUnitOfMeasures()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetUnitOfMeasures"))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<UnitOfMeasure>>(uri);
        }

        public PriceList GetPriceListByName(string name)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetPriceListByName?name={0}",
                        name))
                , UriKind.Absolute);

            return GetInternalData<PriceList>(uri);
        }

        public long AddPriceItem(PriceItem priceItem)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddPriceItem"))
                , UriKind.Absolute);

            return Convert.ToInt64(SaveInternalDataWithString(uri, priceItem));
        }

        public PriceItem GetPriceItemById(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetPriceItemById?id={0}",
                        id))
                , UriKind.Absolute);

            return GetInternalData<PriceItem>(uri);
        }

        public PriceItem GetPriceItemByBarcode(string barcode)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetPriceItemByBarcode?barcode={0}",
                        barcode))
                , UriKind.Absolute);

            return GetInternalData<PriceItem>(uri);
        }

        public void SaveGear(Gear gear)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveGear"))
                , UriKind.Absolute);

            SaveInternalData(uri, gear);
        }

        public long AddRemainder(Remainder remainder)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddRemainder"))
                , UriKind.Absolute);

            return Convert.ToInt64(SaveInternalDataWithString(uri, remainder));
        }

        public void SaveRemainder(Remainder remainder)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveRemainder"))
                , UriKind.Absolute);

            SaveInternalData(uri, remainder);
        }

        public void AddWholesalePrice(WholesalePrice price)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddWholesalePrice"))
                , UriKind.Absolute);

            SaveInternalData(uri, price);
        }

        public long AddIncome(Income income)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddIncome"))
                , UriKind.Absolute);

            return Convert.ToInt64(SaveInternalDataWithString(uri, income));
        }

        public void AddPriceChangeReport(PriceChangeReport report)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddPriceChangeReport"))
                , UriKind.Absolute);

            SaveInternalData(uri, report);
        }

        public IEnumerable<PriceChangeReport> GetPriceChangeReports()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetPriceChangeReports"))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<PriceChangeReport>>(uri);
        }

        public IEnumerable<Remainder> GetRemaindersByPriceItem(long priceItemId, string warehouse = "")
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRemaindersByPriceItem?priceItemId={0}&warehouse={1}", priceItemId,
                        warehouse))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<Remainder>>(uri);
        }

        public IEnumerable<WholesalePrice> GetPricesByPriceItem(long priceItemId)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetPricesByPriceItem?priceItemId={0}", priceItemId))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<WholesalePrice>>(uri);
        }

        public IEnumerable<PriceChangeReportItem> GetPriceChangeReportItems(long priceChangeReportId)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetPriceChangeReportItems?priceChangeReportId={0}",
                        priceChangeReportId))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<PriceChangeReportItem>>(uri);
        }

        public SaleDocumentsPerDay GetSaleDocumentsPerDayById(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetSaleDocumentsPerDayById?id={0}", id))
                , UriKind.Absolute);

            return GetInternalData<SaleDocumentsPerDay>(uri);
        }

        public IEnumerable<RealizationItem> GetTodayRealizationItems(string userName, string warehouse)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRealizationItems?userName={0}&warehouse={1}", userName,
                        warehouse))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<RealizationItem>>(uri);
        }

        public IEnumerable<TodayRealizationItem> GetTodayTotalRealizationItems(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayTotalRealizationItems?userName={0}", userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<TodayRealizationItem>>(uri);
        }
        
        public IEnumerable<RealizationItem> GetTodayRealizationItemsRaw(string userName, string warehouse)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRealizationItemsRaw?userName={0}&warehouse={1}", userName,
                        warehouse))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<RealizationItem>>(uri);
        }
        public IEnumerable<TodayRealizationsTotal> GetTodayRecipeTotalsByPriceListName(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRecipeTotalsByPriceListName?userName={0}", userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<TodayRealizationsTotal>>(uri);
        }

        public IEnumerable<RefundItem> GetTodayRefundItems(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRefundItems?userName={0}", userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<RefundItem>>(uri);
        }
        public TodayRefundsTotalItem GetTodayRefundTotals(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRefundTotals?userName={0}", userName))
                , UriKind.Absolute);

            return GetInternalData<TodayRefundsTotalItem>(uri);
        }
        public IEnumerable<TodayRefundsTotal> GetTodayRefundTotalsByPriceListName(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRefundTotalsByPriceListName?userName={0}", userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<TodayRefundsTotal>>(uri);
        }
        public Dictionary<int, decimal> GetTodayRefundItemsDictionary(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRefundItemsDictionary?userName={0}", userName))
                , UriKind.Absolute);

            return GetInternalData<Dictionary<int, decimal>>(uri);
        }
        public IEnumerable<int> GetTodayRefundedSalesItemIds(string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetTodayRefundedSalesItemIds?userName={0}", userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<int>>(uri);
        }

        public IEnumerable<RefundItem> GetRefundItemsByDate(DateTime from, DateTime to, string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRefundItemsByDate?from={0}&to={1}&userName={2}"
                    , from, to, userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<RefundItem>>(uri);
        }
        public long AddRealizationPerDay(SaleDocumentsPerDay saleDocumentsPerDay)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddRealizationPerDay"))
                , UriKind.Absolute);

            return Convert.ToInt64(SaveInternalDataWithString(uri, saleDocumentsPerDay));
        }

        public IEnumerable<SaleDocumentsPerDay> GetSaleDocumentsPerDays(DateTime from, DateTime to, string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetSaleDocumentsPerDays?from={0}&to={1}&userName={2}", from, to,
                        userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<SaleDocumentsPerDay>>(uri);
        }

        public SaleDocumentsPerDay GetSaleDocumentsPerDayByBarcode(string barcode)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetSaleDocumentsPerDayByBarcode?barcode={0}", barcode))
                , UriKind.Absolute);

            return GetInternalData<SaleDocumentsPerDay>(uri);
        }

        public IEnumerable<PriceList> GetPriceLists()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetPriceLists"))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<PriceList>>(uri);
        }

        public long SaveRecipe(SaleDocument saleDocument)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveRecipe"))
                , UriKind.Absolute);

            return Convert.ToInt64(SaveInternalDataWithString(uri, saleDocument));
        }


        public IEnumerable<RefundDocument> GetRefundDocuments(DateTime startDate, DateTime endDate, string userName)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRefundDocuments?startDate={0}&endDate={1}&userName={2}"
                        , startDate, endDate, userName))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<RefundDocument>>(uri);
        }

        public RefundDocument GetRefundDocument(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRefundDocument?id={0}"
                        , id))
                , UriKind.Absolute);

            return GetInternalData<RefundDocument>(uri);
        }

        public long SaveRefund(RefundDocument document)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveRefundNew"))
                , UriKind.Absolute);

            return Convert.ToInt64(SaveInternalDataWithString(uri, document));
        }

        public SaleDocument GetRecipe(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRecipe?id={0}"
                        , id))
                , UriKind.Absolute);

            return GetInternalData<SaleDocument>(uri);
        }

        public IEnumerable<RefundDocument> GetRefundDocumentsByRecipId(long recipeId)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRefundDocumentsByRecipId?recipeId={0}"
                        , recipeId))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<RefundDocument>>(uri);
        }

        public IEnumerable<SaleDocument> GetRecipesByUser(string user)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRecipesByUser?user={0}"
                        , user))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<SaleDocument>>(uri);
        }

        public void AddRemaindersUserChange(RemaindersUserChange remaindersUserChange)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddRemaindersUserChange"))
                , UriKind.Absolute);

            SaveInternalData(uri, remaindersUserChange);
        }

        public Remainder GetRemainder(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetRemainder?id={0}"
                        , id))
                , UriKind.Absolute);

            return GetInternalData<Remainder>(uri);
        }



        public void DeleteRemainderUserChange(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/DeleteRemainderUserChange"))
                , UriKind.Absolute);

            SaveInternalData(uri, id);
        }

        public void DeleteRemainder(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/DeleteRemainder"))
                , UriKind.Absolute);

            SaveInternalData(uri, id);
        }

        public void DeleteWholesalePrice(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/DeleteWholesalePrice"))
                , UriKind.Absolute);

            SaveInternalData(uri, id);
        }

        public bool DeletePriceItem(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/DeletePriceItem?id={0}", id))
                , UriKind.Absolute);

            return DeleteInternalData(uri);
        }

        public void DeleteGearNew(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/DeleteGearNew"))
                , UriKind.Absolute);

            SaveInternalData(uri, id);
        }

        public void DeleteGear(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/DeleteGear"))
                , UriKind.Absolute);

            SaveInternalData(uri, id);
        }

        public IEnumerable<WarehouseTransferRequest> GetWarehouseTransferRequests()
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetWarehouseTransferRequests"))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<WarehouseTransferRequest>>(uri);
        }


        public IEnumerable<WarehouseTransferRequestItem> GetWarehouseTransferRequestItems(long warehouseRequest_id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetWarehouseTransferRequestItems?warehouseRequest_id={0}"
                        , warehouseRequest_id))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<WarehouseTransferRequestItem>>(uri);
        }

        public IEnumerable<WarehouseTransferRequest> GetReceivedWarehouseTransferRequests(DateTime from, DateTime to,
            string warehouse)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetReceivedWarehouseTransferRequests?from={0}&to={1}&warehouse={2}"
                        , from, to, warehouse))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<WarehouseTransferRequest>>(uri);
        }

        public IEnumerable<WarehouseTransferRequest> GetSendedWarehouseTransferRequests(DateTime from, DateTime to,
            string warehouse)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetSendedWarehouseTransferRequests?from={0}&to={1}&warehouse={2}"
                        , from, to, warehouse))
                , UriKind.Absolute);

            return GetInternalData<IEnumerable<WarehouseTransferRequest>>(uri);
        }

        public WarehouseTransferRequest GetWarehouseTransferRequest(long id)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/GetWarehouseTransferRequest?id={0}"
                        , id))
                , UriKind.Absolute);

            return GetInternalData<WarehouseTransferRequest>(uri);
        }

        public void SaveWarehouseTransferRequest(WarehouseTransferRequest request)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveWarehouseTransferRequest"))
                , UriKind.Absolute);

            SaveInternalData(uri, request);
        }

        public void AcceptWarehouseTransferRequest(WarehouseTransferRequest request)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AcceptWarehouseTransferRequest"))
                , UriKind.Absolute);

            SaveInternalData(uri, request);
        }

        public long AddWarehouseTransferRequest(WarehouseTransferRequest request)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddWarehouseTransferRequest"))
                , UriKind.Absolute);

            return Convert.ToInt64(SaveInternalDataWithString(uri, request));
        }

        public void AddCustomer(Customer customer)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddCustomer"))
                , UriKind.Absolute);

            SaveInternalData(uri, customer);
        }

        public void SaveCustomer(Customer customer)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveCustomer"))
                , UriKind.Absolute);

            SaveInternalData(uri, customer);
        }

        public void AddSupplier(Supplier supplier)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/AddSupplier"))
                , UriKind.Absolute);

            SaveInternalData(uri, supplier);
        }

        public void SaveSupplier(Supplier supplier)
        {
            var uri = new Uri(
                string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/SaveSupplier"))
                , UriKind.Absolute);

            SaveInternalData(uri, supplier);
        }

        private T GetInternalData<T>(Uri uri)
        {
            EventWaitHandle handler = new AutoResetEvent(false);

            T result = default(T);

            new Thread(() =>
            {

                var request =

                    WebRequest.Create(uri);
                request.Method = "GET";

                request.BeginGetResponse((r) =>
                {
                    HttpWebResponse response;

                    try
                    {

                        response =

                            (HttpWebResponse) request.EndGetResponse(r);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {

                            Stream stream = response.GetResponseStream();
                            StreamReader sr = new StreamReader(stream);

                            var json = sr.ReadToEnd();
                            result = JsonConvert.DeserializeObject<T>(json);
                            //handler.Set();
                        }
                    }
                    catch (Exception exception)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            MessageChildWindow msch = new MessageChildWindow();
                            msch.Title = "Ошибка";
                            msch.Message = exception.Message;
                            msch.Show();
                        });
                    }
                    finally
                    {
                        handler.Set();
                    }
                },
                request);

            }) {IsBackground = true}.Start();


            handler.WaitOne();
            return result;

        }

        private void SaveInternalData(Uri uri, object data)
        {
            EventWaitHandle handler = new AutoResetEvent(false);

            new Thread(() =>
            {

                var request =

                    WebRequest.Create(uri);
                request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json; charset=utf-8";

                request.BeginGetRequestStream(i =>
                {

                    // End the operation
                    Stream postStream = request.EndGetRequestStream(i);
                    //var postData = JsonConvert.SerializeObject(data);
                    // Convert the string into a byte array.
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    };
                    var serializer = JsonSerializer.Create(settings);
                    var sb = new StringBuilder();
                    var sw = new StringWriter(sb);
                    serializer.Serialize(sw, data);
                    sw.Close();

                    var postData = sb.ToString();
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    // Write to the request stream.
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Close();

                    // Start the asynchronous operation to get the response
                    request.BeginGetResponse(ac =>
                    {
                        // End the operation
                        HttpWebResponse response;
                        try
                        {


                            response = (HttpWebResponse) request.EndGetResponse(ac);
                            Stream streamResponse = response.GetResponseStream();
                            StreamReader streamRead = new StreamReader(streamResponse);
                            string responseString = streamRead.ReadToEnd();
                            // Close the stream object
                            streamResponse.Close();
                            streamRead.Close();

                            // Release the HttpWebResponse
                            response.Close();
                        }
                        catch (Exception exception)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                MessageChildWindow msch = new MessageChildWindow();
                                msch.Title = "Ошибка";
                                msch.Message = exception.Message;
                                msch.Show();
                            });
                        }
                        finally
                        {
                            handler.Set();
                        }
                        
                    }, request);
                }, request);

            }) {IsBackground = true}.Start();

            handler.WaitOne(10000);
        }

        private string SaveInternalDataWithString(Uri uri, object data)
        {
            EventWaitHandle handler = new AutoResetEvent(false);
            string responseString = "";

            new Thread(() =>
            {
                var request =

                    WebRequest.Create(uri);
                request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json; charset=utf-8";
                //request.ContentType = "application/xml";

                request.BeginGetRequestStream(i =>
                {

                    var sb = new StringBuilder();
                    var sw = new StringWriter(sb);
                    var jw = new JsonTextWriter(sw);

                    // End the operation
                    Stream postStream = request.EndGetRequestStream(i);
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    };
                    var serializer = JsonSerializer.Create(settings);
                    serializer.Serialize(jw, data);
                    jw.Close();
                    sw.Close();
                    var postData = sb.ToString();

                    ////var sb = new StringBuilder();
                    ////var sw = new StringWriter(sb);
                    //var postData = "";
                    //XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings()
                    //{
                    //    // If set to true XmlWriter would close MemoryStream automatically and using would then do double dispose
                    //    // Code analysis does not understand that. That's why there is a suppress message.
                    //    CloseOutput = false,
                    //    Encoding = Encoding.UTF8,
                    //    OmitXmlDeclaration = false,
                    //    Indent = true
                    //};
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(ms, xmlWriterSettings))
                    //    {
                    //        var serializer = new XmlSerializer(data.GetType());
                    //        serializer.Serialize(xw, data);
                    //        //sw.Close();
                    //    }
                    //    postData = //JsonConvert.SerializeObject(data);
                    //    Encoding.UTF8.GetString(ms.ToArray(), 0, (int) ms.Length);
                    //}



                    // Convert the string into a byte array.
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    // Write to the request stream.
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Close();

                    // Start the asynchronous operation to get the response
                    request.BeginGetResponse(ac =>
                    {
                        try
                        {
                            // End the operation
                            HttpWebResponse response = (HttpWebResponse) request.EndGetResponse(ac);
                            Stream streamResponse = response.GetResponseStream();
                            StreamReader streamRead = new StreamReader(streamResponse);
                            responseString = streamRead.ReadToEnd();

                            // Close the stream object
                            streamResponse.Close();
                            streamRead.Close();

                            // Release the HttpWebResponse
                            response.Close();
                        }
                        catch (WebException)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                MessageChildWindow msch = new MessageChildWindow();
                                msch.Title = "Ошибка";
                                msch.Message = "Внутренняя ошибка сервера";
                                msch.Show();
                            });
                        }
                        catch (Exception ex)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                MessageChildWindow msch = new MessageChildWindow();
                                msch.Title = "Ошибка";
                                msch.Message = ex.Message;
                                msch.Show();
                            });
                        }
                        handler.Set();
                    }, request);
                }, request);

            }) {IsBackground = true}.Start();

            handler.WaitOne(10000);

            return responseString;
        }

        private bool DeleteInternalData(Uri uri)
        {
            EventWaitHandle handler = new AutoResetEvent(false);
            bool result = false;

            //T result = default(T);

            new Thread(() =>
            {

                var request =

                    WebRequest.Create(uri);
                request.Method = "DELETE";

                request.BeginGetResponse((r) =>
                {
                    HttpWebResponse response;

                    try
                    {

                        response =

                            (HttpWebResponse)request.EndGetResponse(r);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            result = true;
                        }
                    }
                    catch (WebException exception)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            var content = string.Empty;

                            if (exception.Response != null)
                            {
                                var excResponse = exception.Response;
                                using (StreamReader rd = new StreamReader(excResponse.GetResponseStream()))
                                {
                                    content = rd.ReadToEnd();
                                }
                            }
                            MessageChildWindow msch = new MessageChildWindow();
                            msch.Title = "Ошибка";
                            if (!string.IsNullOrEmpty(content))
                                msch.Message = content;
                            else
                                msch.Message = exception.Message;
                            msch.Show();
                        });
                    }
                    finally
                    {
                        handler.Set();
                    }
                },
                request);

            }) { IsBackground = true }.Start();


            handler.WaitOne();

            return result;

        }

    }

    [Serializable]
    [DataContract]
    public class AcceptIncomeContainer
    {
        [DataMember]
        public Income Income { get; set; }
        [DataMember]
        public IEnumerable<StoreAppTest.Model.IncomeItem> IncomeItems { get; set; }
        [DataMember]
        public string Warehouse { get; set; }
    }
}
