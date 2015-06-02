
namespace StoreAppTest.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using DevExpress.XtraReports.UI;
    using Web.DataModel;

    public partial class RefundDocumentReport : XtraReport
    {
        public RefundDocumentReport()
        {
            InitializeComponent();
            
        }

        #region Overrides of XtraReport

        protected override void BeforeReportPrint()
        {
            IEnumerable<RefundDocument> documents = new List<RefundDocument>();

            var context = new StoreDbContext();
            var parameter = Parameters["DocId"];
            if (parameter != null)
            {
                long id = long.Parse(parameter.Value.ToString());
                documents = context.RefundDocuments.Where(r => r.Id == id).ToList();
            }
            DataSource = documents;

            base.BeforeReportPrint();
        }

        #endregion
    }
}
