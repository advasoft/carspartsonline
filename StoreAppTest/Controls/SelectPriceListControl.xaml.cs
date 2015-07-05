

namespace StoreAppTest.Controls
{
    using System.Windows;
    using Model;

    public partial class SelectPriceListControl
    {
        private SelectPriceListControlViewModel _viewModel;

        public bool IsMultiselect { get; set; }

        public SelectPriceListControl()
        {
            IsMultiselect = false;
            InitializeComponent();
            Loaded += SelectPriceControl_Loaded;
        }

        void SelectPriceControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as SelectPriceListControlViewModel;
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

        private void PriceListComboBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (_viewModel != null)
            //    _viewModel.UpdatePriceList();
        }
    }
}
