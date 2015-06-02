
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
                        int debtDisc = 0;
                        var debtor =
                            discharges.Where(w => w.Debtor_Id == salesPerDayItem.SaleItem.SaleDocument.Customer_Name)
                                .FirstOrDefault();
                        if (debtor != null)
                        {
                            debtDisc = (int)debtor.Amount;
                        }
                        var item = new RealizationPerDayReportItem()
                        {
                            Amount = (int) salesPerDayItem.SaleItem.Amount,
                            CatalogNumber = salesPerDayItem.SaleItem.PriceItem.Gear.CatalogNumber,
                            Name = salesPerDayItem.SaleItem.PriceItem.Gear.Name,
                            Count = (int) salesPerDayItem.SaleItem.Count,
                            IsInDebd = salesPerDayItem.SaleItem.SaleDocument.IsInDebt,
                            //DebtDischarge = salesPerDayItem.SaleItem.SaleDocument.IsInDebt ? debtDisc : 0,
                            //Debtor =
                            //    salesPerDayItem.SaleItem.SaleDocument.IsInDebt
                            //        ? salesPerDayItem.SaleItem.SaleDocument.Customer_Name
                            //        : "",
                            Discount = (int) salesPerDayItem.SaleItem.Discount,
                            Price = (int) salesPerDayItem.SaleItem.Price,
                            Uom = salesPerDayItem.SaleItem.PriceItem.Uom_Id
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

                    totalSales = (int)
                        (saleDocumentsPerDay.SalesPerDayItems.Where(w => !w.SaleItem.SaleDocument.IsInDebt)
                            .Sum(s => s.SaleItem.Amount) + discharges.Sum(d => d.Amount));

                    totalRefund = (int)
                        saleDocumentsPerDay.RefundsPerDayItems.Where(
                            w => !w.RefundItem.RefundDocument.SaleDocument.IsInDebt).Sum(s => s.RefundItem.Amount);

                    subtotal = totalSales - totalRefund;

                    totalByPrice =
                        (int) saleDocumentsPerDay.SalesPerDayItems.Where(w => !w.SaleItem.SaleDocument.IsInDebt)
                            .Sum(s => s.SaleItem.Count * s.SaleItem.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price);
                }
                //totalSales = documents.Sum(s => s.SalesPerDayItems.Sum(d => d.SaleItem.Amount));
                //totalRefund = documents.Sum(s => s.RefundsPerDayItems.Sum(d => d.RefundItem.Amount));
                //subtotal = totalSales - totalRefund;
                //totalByPrice =
                //    documents.Sum(
                //        s =>
                //            s.SalesPerDayItems.Sum(
                //                d =>
                //                    d.SaleItem.Count*
                //                    d.SaleItem.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price));

            }

            //xrRefund.Text = 
            xrTotal.Text = totalSales.ToString();
            xrRefund.Text = totalRefund.ToString();
            xrSubtotal.Text = subtotal.ToString();
            xrTotalByPrice.Text = totalByPrice.ToString();

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
