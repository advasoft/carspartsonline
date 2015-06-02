
namespace StoreAppTest.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using DevExpress.XtraReports.UI;
    using Web.DataModel;

    public partial class SalesDocumentReport : XtraReport
    {
        public SalesDocumentReport()
        {
            InitializeComponent();
            
        }

        #region Overrides of XtraReport

        protected override void BeforeReportPrint()
        {
            IEnumerable<SaleDocument> documents = new List<SaleDocument>();


            var context = new StoreDbContext();
            var parameter = Parameters["DocId"];
            if (parameter != null)
            {
                long id = long.Parse(parameter.Value.ToString());
                documents = context.SaleDocuments.Where(r => r.Id == id).ToList();
            }

            var firstDocument = documents.FirstOrDefault();
            if (firstDocument != null)
            {
                if (firstDocument.IsInDebt)
                {
                    xrDebtorLabel.Visible = true;
                    xrDebtor.Visible = true;
                }
                else
                {
                    xrDebtorLabel.Visible = false;
                    xrDebtor.Visible = false;
                }
                if (firstDocument.IsOrder)
                {
                    xrTitle.Text = "Счет";
                }
                else if (firstDocument.IsInvoice)
                {
                    xrTitle.Text = "Счет-фактура";
                }
                else
                {
                    xrTitle.Text = "Товарный чек";
                }
            }
            DataSource = documents;

            base.BeforeReportPrint();
        }

        #endregion

        private void bindingSource1_CurrentChanged(object sender, System.EventArgs e)
        {

        }
    }
}
