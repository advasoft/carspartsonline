
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Client;
    using Client.Model;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Utilities;
    using Views;

    public class DebtorsViewModel : ViewModelBase
    {
        private IEventAggregator _agregator;
        private DebtorSelectedCustomerEvent _newRealizationEvent;

        private Debtors _view;
 
        public DebtorsViewModel(Debtors view)
        {
            _view = view;

            CustomerList = new ObservableCollection<Customer>();

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _newRealizationEvent = _agregator.GetEvent<DebtorSelectedCustomerEvent>();

            Init();
        }


        public ObservableCollection<Customer> CustomerList { get; set; }


        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
                _newRealizationEvent.Publish(_SelectedCustomer);
            }
        }


        #region Commands

        public ICommand ActListCommand { get; set; }
        public ICommand AmortizationDebtorsListCommand { get; set; }
        public ICommand ShowDebtorsListCommand { get; set; }

        public ICommand ClearCustomerCommand { get; set; }

        private void Init()
        {
            ActListCommand = new UICommand(o =>
            {
                var vm = new ActDebtorsViewModel(SelectedCustomer);
                _view.DebtorsFrame.Content = new ActDeborsPage() { DataContext = vm };
                vm.LoadView();
            });
            AmortizationDebtorsListCommand = new UICommand(o =>
            {
            });
            ShowDebtorsListCommand = new UICommand(o =>
            {
                var vm = new DebtorListViewModel(SelectedCustomer);
                _view.DebtorsFrame.Content = new DebtorsListPage() { DataContext = vm };
                vm.LoadView();
            });
            ClearCustomerCommand = new UICommand(o =>
            {
                SelectedCustomer = null;
            });

        }

        #endregion

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            UpdateCustomersList();
            ShowDebtorsListCommand.Execute(null);
        }

        #endregion

        private void UpdateCustomersList()
        {

            //string uri = string.Concat(
            //    Application.Current.Host.Source.Scheme, "://",
            //    Application.Current.Host.Source.Host, ":",
            //    Application.Current.Host.Source.Port,
            //    "/StoreAppDataService.svc/");

            Task.Factory.StartNew(() =>
            {
                //StoreDbContext ctx = new StoreDbContext(
                //    new Uri(uri
                //        , UriKind.Absolute));

                //var customersDb =
                //    ctx.ExecuteSyncronous(ctx.Customers.Where(
                //            c =>
                //                c.Creator_Id == App.CurrentUser.UserName || c.Creator_Id == "admin" ||
                //                (c.Name == "Розничный покупатель" || c.Name == "Клиент интернет-магазина"))).ToList();
                var client = new StoreapptestClient();

                var customersDb = client.GetCustomers(App.CurrentUser.UserName);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    CustomerList.Clear();

                    customersDb.ForEach(i =>
                    {
                        CustomerList.Add(i);
                    });

                    //SelectedCustomer = CustomerList.FirstOrDefault(f => f.Name == "Розничный покупатель");

                });
            });


        }

    }
}
