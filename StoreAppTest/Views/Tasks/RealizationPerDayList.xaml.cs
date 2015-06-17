using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System.Windows.Input;
    using DevExpress.Xpf.Grid;
    using ViewModels;

    public partial class RealizationPerDayList : Page
    {
        private RealizationPerDyaListViewModel _viewModel;

        public RealizationPerDayList()
        {
            InitializeComponent();
            Loaded += RealizationPerDayList_Loaded;
        }

        void RealizationPerDayList_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel = DataContext as RealizationPerDyaListViewModel;
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.ViewRealizationPerDayCommand.Execute(null);
            }
        }

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            _viewModel.ViewRealizationPerDayCommand.Execute(null);
        }

    }
}
