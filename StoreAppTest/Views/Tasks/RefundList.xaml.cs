using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System.Windows.Input;
    using DevExpress.Xpf.Grid;
    using ViewModels;

    public partial class RefundList : Page
    {
        private RefundListViewModel _viewModel;

        public RefundList()
        {
            InitializeComponent();
            Loaded += RefundList_Loaded;
        }

        void RefundList_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel = DataContext as RefundListViewModel;
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.ViewRefundCommand.Execute(null);
            }
        }

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            _viewModel.ViewRefundCommand.Execute(null);
        }

    }
}
