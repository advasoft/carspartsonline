
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using ViewModels;

    public partial class CashFlowPage : Page
    {
        //private CashFlowViewModel _vm;

        public CashFlowPage()
        {
            InitializeComponent();
            Loaded += CashFlowPage_Loaded;
        }

        void CashFlowPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //_vm = DataContext as CashFlowViewModel;
            //if (_vm != null)
            //    _vm.LoadView();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
