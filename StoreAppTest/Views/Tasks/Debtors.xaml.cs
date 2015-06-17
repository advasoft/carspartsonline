
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System.Windows;
    using DevExpress.XtraEditors.DXErrorProvider;
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

        private void CustomerLookUpEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = ErrorType.Default;
                e.ErrorContent = "Нужно выбрать должника.";
            }
            else
            {
                e.IsValid = true;
                e.ErrorType = ErrorType.None;
                e.ErrorContent = "";
            }
        }
    }
}
