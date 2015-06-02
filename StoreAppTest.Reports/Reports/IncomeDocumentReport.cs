
namespace StoreAppTest.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using DevExpress.XtraReports.UI;
    using Web.DataModel;

    public partial class IncomeDocumentReport : XtraReport
    {
        public IncomeDocumentReport()
        {
            InitializeComponent();
            
        }

        #region Overrides of XtraReport

        protected override void BeforeReportPrint()
        {
            IEnumerable<Income> documents = new List<Income>();


            var context = new StoreDbContext();
            var parameter = Parameters["DocId"];
            if (parameter != null)
            {
                long id = long.Parse(parameter.Value.ToString());
                documents = context.Incomes.Where(r => r.Id == id).ToList();
            }

            var firstDocument = documents.FirstOrDefault();

            DataSource = documents;

            base.BeforeReportPrint();
        }

        #endregion

    }
}
