using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    public partial class Receipt : Page
    {
        public Receipt()
        {
            InitializeComponent();
            Loaded += Receipt_Loaded;
        }

        void Receipt_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            BarcodeEditText.Focus();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
