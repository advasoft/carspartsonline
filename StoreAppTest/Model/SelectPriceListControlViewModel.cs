namespace StoreAppTest.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Client;
    using Client.Model;
    using Utilities;
    using ViewModels;

    public class SelectPriceListControlViewModel : ViewModelBase
    {
        public SelectPriceListControlViewModel()
        {
            PriceLists = new ObservableCollection<PriceList>();
        }

        public ObservableCollection<PriceList> PriceLists { get; set; }



        public string PriceListName
        {
            get { return _PriceListName; }
            set
            {
                _PriceListName = value;
                OnPropertyChanged("PriceListName");
            }
        }
        private string _PriceListName;

        public string Warehouse { get; set; }

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");


            //StoreDbContext ctx = new StoreDbContext(
            //    new Uri(uri
            //        , UriKind.Absolute));

            var client = new StoreapptestClient();

            //PriceLists = new ObservableCollection<PriceList>(ctx.ExecuteSyncronous(ctx.PriceLists).ToList());
            PriceLists = new ObservableCollection<PriceList>(client.GetPriceLists());
            if (string.IsNullOrEmpty(PriceListName))
            {
                if (PriceLists.Count > 0)
                {
                    PriceListName = PriceLists.First().Name;
                }
            }

        }

        #endregion


    }

}
