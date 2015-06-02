
namespace StoreAppTest.Print
{
    using DevExpress.Xpf.Printing;

    public partial class IncomeDocumentReportControl
    {
        private long _salesDocumentId;


        public IncomeDocumentReportControl(long salesDocumentId)
        {
            _salesDocumentId = salesDocumentId;
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

            DocumentPreview.Model.CreateDocument();
        }
    }
}
