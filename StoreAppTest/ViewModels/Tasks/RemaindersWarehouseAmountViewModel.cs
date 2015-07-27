

namespace StoreAppTest.ViewModels.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Client.Model;
    using GalaSoft.MvvmLight.Threading;
    using Utilities;

    public class RemaindersWarehouseAmountViewModel : ViewModelBase
    {

        public RemaindersWarehouseAmountViewModel()
        {
        }


        public int Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
                OnPropertyChanged("Amount");
            }
        }
        private int _Amount;


        public IList<PriceListItemRemainderView> PriceItems { get; private set; }


        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");



            Task.Factory.StartNew(() =>
            {

                RemaindersClient client = new RemaindersClient(App.CurrentUser.Warehouse.Name);

                PriceItems = client.GetAllPriceListItems().ToList();

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Amount = (int)PriceItems.Sum(s => s.Remainders * s.WholesalePrice);

                });
            });
        }

        #endregion
    }
}
