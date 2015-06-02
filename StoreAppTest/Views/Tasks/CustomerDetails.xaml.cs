
using System.Windows.Controls;

namespace StoreAppTest.Views
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using StoreAppDataService;
    using Utilities;

    public partial class CustomerDetails : UserControl
    {
        public CustomerDetails()
        {
            InitializeComponent();
        }

        public event EventHandler OkButtonClicked = (sender, args) => { };
        public event EventHandler CancelButtonClicked = (sender, args) => { };

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //TODO id okey raise event
            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            Customer cs = DataContext as Customer;
            DataContext = null;
            //Task.Factory.StartNew(() =>
            //{
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));
                    cs.Creator_Id = App.CurrentUser.UserName;
                    ctx.AddToCustomers(cs);
                    ctx.SaveChangesSynchronous();
                    
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Во время записи клиента в базу данных произошла ошибка");
                }

            //}).Wait();
            OkButtonClicked(sender, e);
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            CancelButtonClicked(sender, e);
        }
    }
}
