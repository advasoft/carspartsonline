
namespace StoreAppTest.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DevExpress.XtraReports.UI;
    using Web.DataModel;
    using StoreAppTest.Utilities;

    public partial class RealizationPerDayReport : XtraReport
    {
        public RealizationPerDayReport()
        {
            InitializeComponent();
            
        }

        #region Overrides of XtraReport

        protected override void BeforeReportPrint()
        {
            IList<RealizationPerDayReportDocument> documents = new List<RealizationPerDayReportDocument>();
            int totalSales = 0;
            int totalRefund = 0;
            int subtotal = 0;
            int totalSalesProfit = 0;
            int totalRefundProfit = 0;
            int subtotalProfit = 0;
            int totalByPrice = 0;

            var context = new StoreDbContext();
            var parameter = Parameters["DocId"];
            if (parameter != null)
            {
                //var now = DateTimeHelper.GetNowKz();
                //var strt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                //var endd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);




                long id = long.Parse(parameter.Value.ToString());
                var salesDocuments = context.SaleDocumentsPerDays
                    .Include(i => i.Creator)
                    .Include(i => i.SalesPerDayItems)
                    .Include("SalesPerDayItems.SaleDocumentsPerDay")
                    .Include("SalesPerDayItems.SaleItem.PriceItem.Remainders")
                    .Include("SalesPerDayItems.SaleItem.PriceItem.Prices")
                    .Include("SalesPerDayItems.SaleItem.PriceItem.Gear")
                    .Include("SalesPerDayItems.SaleItem.PriceItem.PriceLists")
                    .Include("SalesPerDayItems.SaleItem.PriceItem.UnitOfMeasure")
                    .Include("SalesPerDayItems.SaleItem.SaleDocument")
                    .Where(r => r.Id == id).ToList();
                foreach (var saleDocumentsPerDay in salesDocuments)
                {

                    var strt = DateTimeHelper.GetStartDay(saleDocumentsPerDay.SaleDocumentsDate);
                    var endd = DateTimeHelper.GetEndDay(saleDocumentsPerDay.SaleDocumentsDate);

                    var dischargesSource = context.DebtDischargeDocuments
                        .Include(i => i.Debtor)
                        .Where(
                            w =>
                                w.DischargeDate >= strt && w.DischargeDate <= endd && w.IsDischarge &&
                                w.Creator_Id == saleDocumentsPerDay.Creator_Id).ToList();

                var discharges = (from d in dischargesSource
                                  group d by new
                                  {
                                      Debtor = d.Debtor_Id
                                  }
                                      into ds
                                      select new DebtDischargeDocument()
                                      {
                                          Amount = ds.Sum(s => s.Amount),
                                          Debtor_Id = ds.Key.Debtor
                                      }).ToList();


                    var refundItems =
                    context.RefundItems
                    .Include(i => i.RefundDocument.SaleDocument)
                    .Include(i => i.SaleItem.SaleDocument)
                    .Include(i => i.SaleItem.PriceItem.Gear)
                    .Include(i => i.SaleItem.PriceItem.PriceLists)
                    .Include(i => i.SaleItem.PriceItem.Prices)
                    
                        .Where(
                            r =>
                                r.RefundDocument.RefundDate >= strt && r.RefundDocument.RefundDate <= endd &&
                                r.RefundDocument.Creator_Id == saleDocumentsPerDay.Creator_Id).ToList();

                    var doc = new RealizationPerDayReportDocument();
                    doc.Barcode = saleDocumentsPerDay.Barcode;
                    doc.Date = saleDocumentsPerDay.SaleDocumentsDate;
                    doc.Number = saleDocumentsPerDay.Number;


                    var saleItems = saleDocumentsPerDay.SalesPerDayItems;
                    foreach (var salesPerDayItem in saleItems)
                    {

                        //var amouuntWithoutDebt = 0;
                        //var amountWholesalePriceWithoutDebt = 0;

                        //decimal refund = 0;

                        //refund =
                        //    refundItems.Where(wh => wh.SaleItem_Id == salesPerDayItem.SaleItem_Id && wh.RefundDocument.SaleDocument.IsInDebt == false)
                        //        .Sum(sum => sum.Count);

                        //if (!salesPerDayItem.SaleItem.SaleDocument.IsInDebt)
                        //{
                        //    var price =
                        //        salesPerDayItem.SaleItem.PriceItem.Prices.Where(p => p.PriceDate <= saleDocumentsPerDay.SaleDocumentsDate)
                        //            .OrderByDescending(o => o.PriceDate)
                        //            .FirstOrDefault();


                        //    amouuntWithoutDebt = (int)((salesPerDayItem.Price * (salesPerDayItem.Count - refund)) - (salesPerDayItem.Discount - ((salesPerDayItem.Discount / salesPerDayItem.Count) * refund)));
                        //    if (price != null)
                        //    {
                        //        amountWholesalePriceWithoutDebt =
                        //            (int)((price.Price * (salesPerDayItem.Count - refund)));
                        //    }

                        //}

                        var item = new RealizationPerDayReportItem()
                        {
                            Amount = (int) (salesPerDayItem.Amount),
                            CatalogNumber = salesPerDayItem.CatalogNumber ?? "",
                            Name = salesPerDayItem.Name ?? "",
                            Count = (int) salesPerDayItem.Count,
                            Discount = (int) salesPerDayItem.Discount,
                            Price = salesPerDayItem.Count == 0 ? 0 : (int)(salesPerDayItem.Amount / salesPerDayItem.Count),
                            Uom = salesPerDayItem.UnitOfMeasure,
                            Time = salesPerDayItem.SaleItem.SaleDocument.SaleDate,
                            SaleDocumentNumber = salesPerDayItem.SaleItem.SaleDocument.Number,
                            PriceListName = salesPerDayItem.SaleItem.PriceItem.PriceLists.FirstOrDefault().Name,
                            WholesalePrice = (int)salesPerDayItem.SaleItem.PriceItem.Prices.Where(p => p.PriceDate <= saleDocumentsPerDay.SaleDocumentsDate).OrderByDescending(o => o.PriceDate).First().Price,
                            SaleItemData = salesPerDayItem.SaleItem
                            //AmountWithoutDebt = amouuntWithoutDebt,
                            //AmountWithoutDebtByWholePrice = amountWholesalePriceWithoutDebt
                            
                        };

                        doc.Items.Add(item);
                    }


                    var realizationsByPriceList = (from q in doc.Items
                                                   group q by q.PriceListName
                                                       into grp
                                                       select grp).ToList();

                    var refundsByPriceList = (from r in refundItems
                                              group r by r.PriceItem.PriceLists.FirstOrDefault().Name
                                                  into grp
                                                  select grp).ToList();

                    foreach (var itm in realizationsByPriceList)
                    {
                        int refndVal = 0;
                        int refndValWh = 0;

                        var refnd = refundsByPriceList.Where(r => r.Key == itm.Key).FirstOrDefault();
                        if (refnd != null)
                        {
                            refndVal = (int)refnd.Sum(s => s.Amount);
                            refndValWh = (int)
                                refnd.Sum(
                                    s =>
                                        s.Count *
                                        (s.PriceItem.Prices.Where(p => p.PriceDate <= s.RefundDocument.RefundDate)
                                            .OrderByDescending(o => o.PriceDate)
                                            .FirstOrDefault() == null
                                            ? 0
                                            : s.PriceItem.Prices.Where(p => p.PriceDate <= s.RefundDocument.RefundDate)
                                                .OrderByDescending(o => o.PriceDate)
                                                .FirstOrDefault().Price)
                                    );
                        }

                        var realizationVal = (int)
                            itm.Where(r => !r.SaleItemData.SaleDocument.IsInDebt).Sum(s => s.Amount);

                        var realizationValWh =
                            (int)
                                itm.Where(r => !r.SaleItemData.SaleDocument.IsInDebt).Sum(
                                    s =>
                                        s.Count * s.WholesalePrice
                                    );

                        var frstItem = itm.FirstOrDefault();
                        if (frstItem != null)
                        {
                            frstItem.AmountWithoutDebt = realizationVal - refndVal;
                            frstItem.AmountWithoutDebtByWholePrice = realizationValWh - refndValWh;
                        }
                    }



                    foreach (var debtDischargeDocument in discharges)
                    {
                        var item = new RealizationPerDayReportItem()
                        {
                            Name = "Возврат долга " + debtDischargeDocument.Debtor_Id,
                            IsInDebd = true,
                            Amount = (int)debtDischargeDocument.Amount
                        };
                        doc.Items.Add(item);
                    }
                    documents.Add(doc);

                    totalSales = (int)saleDocumentsPerDay.TotalAmount;

                    totalRefund = (int)saleDocumentsPerDay.TotalRefund;

                    subtotal = totalSales - totalRefund;

                    totalSalesProfit = (int) saleDocumentsPerDay.TotalAmountProfit;
                    totalRefundProfit = (int) saleDocumentsPerDay.TotalRefundProfit;
                    subtotalProfit = (int)saleDocumentsPerDay.SubTotalProfit;

                }

            }

            xrTotal.Text = totalSales.ToString();
            xrRefund.Text = totalRefund.ToString();
            xrSubtotal.Text = subtotal.ToString();
            //xrTotalByPrice.Text = totalByPrice.ToString();
            xrTotalProfit.Text = totalSalesProfit.ToString();
            xrRefundProfit.Text = totalRefundProfit.ToString();
            xrSubtotalProfit.Text = subtotalProfit.ToString();

            DataSource = documents;

            base.BeforeReportPrint();
        }

        #endregion
    }

    public class RealizationPerDayReportItem
    {
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string Uom { get; set; }
        public int? Price { get; set; }
        public int? Count { get; set; }
        public int? Discount { get; set; }
        public int Amount { get; set; }
        public int AmountWithoutDebt { get; set; }
        public int AmountWithoutDebtByWholePrice { get; set; }

        public int PofitByPriceList
        {
            get
            {
                return AmountWithoutDebt - AmountWithoutDebtByWholePrice;
            }
        }

        public string Debtor { get; set; }
        public int DebtDischarge { get; set; }
        public bool IsInDebd { get; set; }
        public string PriceListName { get; set; }
        public int WholesalePrice { get; set; }
        public DateTime Time { get; set; }
        public string SaleDocumentNumber { get; set; }

        public int TotalByWholePrice
        {
            get
            {
                return (int) ((WholesalePrice*(Count ?? 0)));
            }
        }

        public int Profit
        {
            get
            {
                return Amount - TotalByWholePrice;
            }
        }

        public SaleItem SaleItemData { get; set; }
    }

    public class RealizationPerDayReportDocument
    {
        public RealizationPerDayReportDocument()
        {
            Items = new List<RealizationPerDayReportItem>();
        }

        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Barcode { get; set; }

        public IList<RealizationPerDayReportItem> Items { get; set; }

    }
}
