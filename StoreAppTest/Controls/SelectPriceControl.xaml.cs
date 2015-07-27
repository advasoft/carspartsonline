

namespace StoreAppTest.Controls
{
    using System.Windows;
    using System.Windows.Input;
    using Model;

    public partial class SelectPriceControl
    {
        private SelectPriceControlViewModel _viewModel;

        public bool IsMultiselect { get; set; }

        public SelectPriceControl()
        {
            IsMultiselect = false;
            InitializeComponent();
            Loaded += SelectPriceControl_Loaded;
        }

        void SelectPriceControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as SelectPriceControlViewModel;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsMultiselect = true;
            DialogResult = true;

        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }

        private void TableView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    DialogResult = true;
            //}
        }

        private void TableView_RowDoubleClick(object sender, DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {
            DialogResult = true;
        }

        private void PriceListComboBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (_viewModel != null)
            //    _viewModel.UpdatePriceList();
        }
    }
}
