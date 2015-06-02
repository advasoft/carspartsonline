
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System.Windows;
    using ViewModels;

    public partial class Debtors : Page
    {
        private DebtorsViewModel _viewModel;

        public Debtors()
        {
            _viewModel = new DebtorsViewModel(this);
            DataContext = _viewModel;
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _viewModel.LoadView();
            
        }

        private void Debtors_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadView();
        }
    }
}
