namespace StoreAppTest.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Client;
    using Client.Model;
    using GalaSoft.MvvmLight.Threading;
    using Utilities;
    using ViewModels;

    public class SelectPriceControlViewModel : ViewModelBase
    {
        public SelectPriceControlViewModel()
        {
            PriceItems = new ObservableCollection<PriceItemRemainderModel>();
            PriceLists = new ObservableCollection<PriceList>();
        }

        public ObservableCollection<PriceItemRemainderModel> PriceItems { get; set; }
        public ObservableCollection<PriceList> PriceLists { get; set; }



        public string PriceListName
        {
            get { return _PriceListName; }
            set
            {
                _PriceListName = value;
                OnPropertyChanged("PriceListName");
                UpdatePriceList();
            }
        }
        private string _PriceListName;

        public string Warehouse { get; set; }


        private PriceItemRemainderModel _SelectedPriceItem;
        public PriceItemRemainderModel SelectedPriceItem
        {
            get { return _SelectedPriceItem; }
            set
            {
                _SelectedPriceItem = value;
                OnPropertyChanged("SelectedPriceItem");
            }
        }


        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        private bool _IsLoading;



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

            //UpdatePriceList();
        }

        #endregion



        public void UpdatePriceList()
        {
            IsLoading = true;

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            PriceItems.Clear();

            Task.Factory.StartNew(() =>
            {
                try
                {

                    //StoreDbContext ctx = new StoreDbContext(
                    //    new Uri(uri
                    //        , UriKind.Absolute));


                    if (string.IsNullOrEmpty(PriceListName))
                    {
                        return;
                    }
                    IList<PriceItemRemainderView> pricelistitems = new List<PriceItemRemainderView>();

                    var client = new StoreapptestClient();

                    string queryString = "";
                    if (!string.IsNullOrEmpty(Warehouse))
                    {
                        //queryString = string.Format("{0}GetLightPriceItemList?priceListName='{1}'&warehouse='{2}'", uri,
                        //    PriceListName, App.CurrentUser.Warehouse.Name);
                        pricelistitems = client.GetIncomes(PriceListName, App.CurrentUser.Warehouse.Name).ToList();
                    }
                    else
                    {
                        //queryString = string.Format("{0}GetLightPriceItemList?priceListName='{1}'", uri, PriceListName);
                        pricelistitems = client.GetIncomes(PriceListName, "").ToList();
                    }
                    //var qr = ctx.ExecuteSyncronous<PriceItemRemainderView>(
                    //    new Uri(queryString,
                    //        UriKind.Absolute));

                    //pricelistitems = qr.ToList();

                    foreach (var priceItemRemainderView in pricelistitems)
                    {
                        var model = new PriceItemRemainderModel()
                        {
                            PriceItem_Id = priceItemRemainderView.PriceItem_Id,
                            BuyPriceRur = priceItemRemainderView.BuyPriceRur,
                            BuyPriceTng = priceItemRemainderView.BuyPriceTng,
                            WholesalePrice = (int)priceItemRemainderView.WholesalePrice,
                            Uom = priceItemRemainderView.Uom,
                            Gear_Id = priceItemRemainderView.Gear_Id,
                            CatalogNumber = priceItemRemainderView.CatalogNumber,
                            Articul = priceItemRemainderView.Articul,
                            IsDuplicate = priceItemRemainderView.IsDuplicate,
                            Gear_Name = priceItemRemainderView.Gear_Name,
                            RecommendedRemainder = priceItemRemainderView.RecommendedRemainder,
                            LowerLimitRemainder = priceItemRemainderView.LowerLimitRemainder,
                            Remainder_Id = priceItemRemainderView.Remainder_Id,
                            Warehouse = priceItemRemainderView.Warehouse,
                            RemainderDate = priceItemRemainderView.RemainderDate,
                            Remainders = (int)priceItemRemainderView.Remainders
                        };

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            PriceItems.Add(model);
                        });
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }


            }).ContinueWith(r =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsLoading = false;
                });

            });
        }



    }


    public class PriceItemRemainderModel
    {
        public bool Selected { get; set; }

        public long PriceItem_Id { get; set; }
        public decimal BuyPriceRur { get; set; }
        public decimal BuyPriceTng { get; set; }
        public int WholesalePrice { get; set; }
        public string Uom { get; set; }
        public long Gear_Id { get; set; }
        public string CatalogNumber { get; set; }
        public string Articul { get; set; }
        public bool IsDuplicate { get; set; }
        public string Gear_Name { get; set; }
        public decimal RecommendedRemainder { get; set; }
        public decimal LowerLimitRemainder { get; set; }
        public long Remainder_Id { get; set; }
        public string Warehouse { get; set; }
        public DateTime RemainderDate { get; set; }
        public int Remainders { get; set; }

    }
}
