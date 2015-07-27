
namespace StoreAppTest.Reports
{
    using System.Collections.Generic;
    using System.Data.Entity;
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
                documents = context.Incomes
                    .Include(i => i.Accepter)
                    .Include(i => i.Creator)
                    .Include(i => i.IncomeItems)
                    .Include(i => i.Supplier)
                    .Include("IncomeItems.PriceItem.Gear")
                    .Where(r => r.Id == id).ToList();
            }

            var firstDocument = documents.FirstOrDefault();

            DataSource = documents;

            base.BeforeReportPrint();
        }

        #endregion

    }
}
