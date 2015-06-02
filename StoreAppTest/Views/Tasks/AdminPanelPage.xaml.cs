
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using DevExpress.Xpf.Grid;
    using ViewModels;

    public partial class AdminPanelPage : Page
    {
        private AdminPanelViewModel _viewModel;
        public AdminPanelPage()
        {
            _viewModel = new AdminPanelViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _viewModel.LoadView();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            
        }
    }
}
