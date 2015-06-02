
namespace StoreAppTest.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DevExpress.XtraReports.UI;
    using Web.DataModel;

    public partial class PriceChangeDocumentReport : XtraReport
    {
        public PriceChangeDocumentReport()
        {
            InitializeComponent();
            
        }

        #region Overrides of XtraReport

        protected override void BeforeReportPrint()
        {
            IList<ReportChangeReport> documents = new List<ReportChangeReport>();

            int totalUp = 0;
            int totalDown = 0;
            int total = 0;

            var context = new StoreDbContext();
            var parameter = Parameters["DocId"];
            if (parameter != null)
            {
                long id = long.Parse(parameter.Value.ToString());
                var docs = context.PriceChangeReports.Where(r => r.Id == id).ToList();

                foreach (var priceChangeReport in docs)
                {
                    ReportChangeReport report = new ReportChangeReport();
                    report.Date = priceChangeReport.ReportDate;
                    report.Number = priceChangeReport.ReportNumber;

                    foreach (var item in priceChangeReport.PriceChangeReportItems)
                    {
                        ReportChangeReportItem ii = new ReportChangeReportItem();
                        ii.CatalogNumber = item.PriceItem.Gear.CatalogNumber;
                        ii.Name = item.PriceItem.Gear.Name;
                        ii.Uom = item.PriceItem.Uom_Id;
                        ii.NewPrice = (int)item.NewPrice.Price;
                        ii.PreviousPrice = (int)item.PreviousPrice.Price;
                        ii.Remainders = (int) item.PriceItem.Remainders.Sum(s => s.Amount);

                        report.Items.Add(ii);
                    }
                    documents.Add(report);

                    totalUp = (int)priceChangeReport.PriceChangeReportItems.Where(w => w.NewPrice.Price > w.PreviousPrice.Price)
                    .Sum(a => (a.NewPrice.Price - a.PreviousPrice.Price) * a.PriceItem.Remainders.Sum(s => s.Amount));
                    totalDown = (int)priceChangeReport.PriceChangeReportItems.Where(w => w.NewPrice.Price < w.PreviousPrice.Price)
                    .Sum(a => (a.PreviousPrice.Price - a.NewPrice.Price) * a.PriceItem.Remainders.Sum(s => s.Amount));
                    total = totalUp - totalDown;
                }
            }

            DataSource = documents;

            xrUp.Text = totalUp.ToString();
            xrDown.Text = totalDown.ToString();
            xrTotal.Text = total.ToString();

            base.BeforeReportPrint();
        }

        #endregion

    }


    public class ReportChangeReport
    {
        public ReportChangeReport()
        {
            Items = new List<ReportChangeReportItem>();
        }

        public string Number { get; set; }
        public DateTime Date { get; set; }

        public IList<ReportChangeReportItem> Items { get; set; }
    }

    public class ReportChangeReportItem
    {
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string Uom { get; set; }

        public int NewPrice { get; set; }
        public int PreviousPrice { get; set; }
        public int Remainders { get; set; }
    }
}
