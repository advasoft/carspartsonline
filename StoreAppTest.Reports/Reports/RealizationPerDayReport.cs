
namespace StoreAppTest.Reports
{
    using System;
    using System.Collections.Generic;
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
                var now = DateTimeHelper.GetNowKz();
                var strt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var endd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

                var dischargesSource = context.DebtDischargeDocuments
                            .Where(w => w.DischargeDate >= strt && w.DischargeDate <= endd && w.IsDischarge).ToList();
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



                long id = long.Parse(parameter.Value.ToString());
                var salesDocuments = context.SaleDocumentsPerDays.Where(r => r.Id == id).ToList();
                foreach (var saleDocumentsPerDay in salesDocuments)
                {


                    var doc = new RealizationPerDayReportDocument();
                    doc.Barcode = saleDocumentsPerDay.Barcode;
                    doc.Date = saleDocumentsPerDay.SaleDocumentsDate;
                    doc.Number = saleDocumentsPerDay.Number;


                    var saleItems = saleDocumentsPerDay.SalesPerDayItems;
                    foreach (var salesPerDayItem in saleItems)
                    {
                        var item = new RealizationPerDayReportItem()
                        {
                            Amount = (int) salesPerDayItem.Amount,
                            CatalogNumber = salesPerDayItem.CatalogNumber,
                            Name = salesPerDayItem.Name,
                            Count = (int) salesPerDayItem.Count,
                            Discount = (int) salesPerDayItem.Discount,
                            Price = (int) (salesPerDayItem.Amount / salesPerDayItem.Count),
                            Uom = salesPerDayItem.UnitOfMeasure,
                            PriceListName = salesPerDayItem.SaleItem.PriceItem.PriceLists.FirstOrDefault().Name,
                            WholesalePrice = (int)salesPerDayItem.SaleItem.PriceItem.Prices.Where(p => p.PriceDate <= saleDocumentsPerDay.SaleDocumentsDate).OrderByDescending(o => o.PriceDate).First().Price
                            
                        };

                        doc.Items.Add(item);
                    }
                    foreach (var debtDischargeDocument in discharges)
                    {
                        var item = new RealizationPerDayReportItem()
                        {
                            Name = debtDischargeDocument.Debtor_Id,
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
        public string Debtor { get; set; }
        public int DebtDischarge { get; set; }
        public bool IsInDebd { get; set; }
        public string PriceListName { get; set; }
        public int WholesalePrice { get; set; }

        public int TotalByWholePrice
        {
            get
            {
                return (int) ((WholesalePrice*Count));
            }
        }

        public int Profit
        {
            get
            {
                return Amount - TotalByWholePrice;
            }
        }
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
