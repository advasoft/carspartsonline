
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System.Windows;
    using System.Windows.Input;
    using DevExpress.Xpf.Grid;
    using ViewModels;

    public partial class PriceChangeReportsPage : Page
    {
        private PriceChangeReportsViewModel _viewModel;

        public PriceChangeReportsPage()
        {
            InitializeComponent();
            
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.OpenPriceChangeReportCommand.Execute(null);
        }

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            _viewModel.OpenPriceChangeReportCommand.Execute(null);
        }

        private void LayoutRoot_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as PriceChangeReportsViewModel;

        }

        private void ReceiptGridControl_OnCustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {

            }
        }
    }
}
