
namespace StoreAppTest.Print
{
    using DevExpress.Xpf.Printing;

    public partial class SalesInvoiceReportControl
    {
        private long _salesDocumentId;
        private bool _isRefund;


        public SalesInvoiceReportControl(long salesDocumentId, bool isRefund)
        {
            _salesDocumentId = salesDocumentId;
            _isRefund = isRefund;

            InitializeComponent();

            DocumentPreview.Loaded += DocumentPreview_Loaded;
            
        }

        void DocumentPreview_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //DocumentPreview.Model.CreateDocument();
        }

        private void ChildWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var model = DocumentPreview.Model as ReportPreviewModel;

            //long value = 1;
            model.Parameters["DocId"].Value = _salesDocumentId.ToString();
            if(_isRefund)
                model.Parameters["Refund"].Value = _isRefund;

            DocumentPreview.Model.CreateDocument();
        }
    }
}
