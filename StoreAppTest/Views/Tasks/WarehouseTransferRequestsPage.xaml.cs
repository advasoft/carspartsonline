using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System.Windows.Input;
    using DevExpress.Xpf.Grid;
    using ViewModels;

    public partial class WarehouseTransferRequestsPage : Page
    {
        private WarehouseTransferRequestsViewModel _viewModel;

        public WarehouseTransferRequestsPage()
        {
            InitializeComponent();
            Loaded += WarehouseTransferRequestsPage_Loaded;
        }

        void WarehouseTransferRequestsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel = DataContext as WarehouseTransferRequestsViewModel;
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.ViewWarehouseTransferRequestCommand.Execute(null);
            }
        }

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            _viewModel.ViewWarehouseTransferRequestCommand.Execute(null);
        }

        private void UIElement1_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.ViewWarehouseTransferRequestSendedCommand.Execute(null);
            }
        }

        private void TableView1_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            _viewModel.ViewWarehouseTransferRequestSendedCommand.Execute(null);
        }

    }
}
