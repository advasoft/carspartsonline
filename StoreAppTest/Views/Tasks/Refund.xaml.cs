using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System.Linq;
    using DevExpress.Xpf.Grid;
    using DevExpress.XtraEditors.DXErrorProvider;
    using Model;
    using ViewModels;

    public partial class Refund : Page
    {
        private RefundViewModel _viewModel;

        public Refund()
        {
            InitializeComponent();
            Loaded += Refund_Loaded;
        }

        void Refund_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel = DataContext as RefundViewModel;
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void GridColumn_OnValidate(object sender, GridCellValidationEventArgs e)
        {
            var row = e.Row as RefundItem;
            if (e.Value != null)
            {
                int val = int.Parse(e.Value.ToString());
                
                if (val > row.PreviousSoldCount)
                {
                    e.IsValid = false;
                    e.ErrorType = ErrorType.Critical;
                    e.ErrorContent = "Количество возвращаемого товара больше чем продано";
                }
                else if (val == 0)
                {
                    e.IsValid = false;
                    e.ErrorType = ErrorType.Critical;
                    e.ErrorContent = "Количество должно быть больше 0";
                }
                else
                {
                    e.IsValid = true;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorType = ErrorType.Critical;
                e.ErrorContent = "Не указано количество возвращаетмого товара";
            }

        }
    }
}
