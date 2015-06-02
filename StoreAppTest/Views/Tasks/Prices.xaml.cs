using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using DevExpress.Xpf.Grid;
    using DevExpress.Xpf.Printing;
    using DevExpress.XtraEditors.DXErrorProvider;
    using ViewModels;

    public partial class Prices : Page
    {
        private PricesViewModel _viewModel;
        public Prices()
        {
            InitializeComponent();
            Loaded += Prices_Loaded;
        }

        void Prices_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //_viewModel = (PricesViewModel) DataContext;
            //_viewModel.LoadView();
            //((TableView)PricesGridControl.View).BestFitColumns();
            _viewModel = (PricesViewModel)DataContext;
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //_viewModel.LoadView();
        }

        private void CustomerLookUpEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null && InDebtCheckEdit != null && InDebtCheckEdit.IsChecked == true)
            {
                e.IsValid = false;
                e.ErrorType = ErrorType.Default;
                e.ErrorContent = "Нужно выбрать должника.";
            }
            else
            {
                e.IsValid = true;
                e.ErrorType = ErrorType.None;
                e.ErrorContent = "";
            }
        }

        private void InDebtCheckEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CustomerLookUpEdit.DoValidate();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DocumentPreviewWindow preview = new DocumentPreviewWindow();
            PrintableControlLink link = new PrintableControlLink(PricesGridControl.View as DevExpress.Xpf.Printing.IPrintableControl);
            link.ExportServiceUri = "/StoreExportService.svc";
            link.PrintingSystem.ExportOptions.PrintPreview.ShowOptionsBeforeExport = false;
            LinkPreviewModel model = new LinkPreviewModel(link);
            preview.Model = model;
            link.CreateDocument(false);
            preview.ShowDialog();
        }


    }
}
