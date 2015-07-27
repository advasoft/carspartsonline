
namespace StoreAppTest.Reports
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DevExpress.XtraReports.UI;
    using Web.DataModel;

    public partial class WarehouseTransferRequestReport : XtraReport
    {
        public WarehouseTransferRequestReport()
        {
            InitializeComponent();          
        }

        #region Overrides of XtraReport

        protected override void BeforeReportPrint()
        {
            IEnumerable<WarehouseTransferRequest> documents = new List<WarehouseTransferRequest>();


            var context = new StoreDbContext();
            var parameter = Parameters["DocId"];
            if (parameter != null)
            {
                long id = long.Parse(parameter.Value.ToString());
                documents = context.WarehouseTransferRequests
                    .Include(i => i.Creator)
                    .Include(i => i.Customer)
                    .Include(i => i.LastChanged)
                    .Include(i => i.Supplier)
                    .Include(i => i.WarehouseTransferRequestItemItems)
                    .Include("WarehouseTransferRequestItemItems.PriceItem.Gear")
                    .Where(r => r.Id == id).ToList();
            }

            DataSource = documents;

            base.BeforeReportPrint();
        }

        #endregion


    }
}
