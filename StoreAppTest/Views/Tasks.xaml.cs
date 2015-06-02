namespace StoreAppTest.Views
{
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using ViewModels;

    public partial class TasksView : Page
    {
        private TasksViewModel _viewModel;

        public TasksView()
		{
            _viewModel = new TasksViewModel(this);
            DataContext = _viewModel;
			InitializeComponent();
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
            _viewModel.LoadView();
		}

    }
}