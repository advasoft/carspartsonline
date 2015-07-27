
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Client;
    using GalaSoft.MvvmLight.Threading;
    using Utilities;

    public class PriceChangesReportsItemViewModel : ViewModelBase
    {

        private long _priceReport;

        public PriceChangesReportsItemViewModel(long priceReport)
        {
            _priceReport = priceReport;
            PriceChanges = new ObservableCollection<Model.PriceChangeReportItem>();
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


        public ObservableCollection<Model.PriceChangeReportItem> PriceChanges { get; set; }



        public decimal TotalUp
        {
            get
            {
                return
                    PriceChanges.Where(w => w.NewPrice > w.PreviewsPrice)
                        .Sum(s => (s.NewPrice - s.PreviewsPrice) * s.Remainders);

                //ReceiptItems.Sum(s => s.Price * s.SoldCount);
            }
        }
        public decimal TotalDown
        {
            get
            {
                return
                    PriceChanges.Where(w => w.NewPrice < w.PreviewsPrice)
                        .Sum(s => (s.PreviewsPrice - s.NewPrice) * s.Remainders);

            }
        }
        public decimal Total
        {
            get
            {
                return TotalUp - TotalDown;
            }
        }


        #region Overrides of ViewModelBase



        protected override void LoadViewHandler()
        {
            IsLoading = true;
            //string uri = string.Concat(
            //    Application.Current.Host.Source.Scheme, "://",
            //    Application.Current.Host.Source.Host, ":",
            //    Application.Current.Host.Source.Port,
            //    "/StoreAppDataService.svc/");



            Task.Factory.StartNew(() =>
            {
                try
                {

                    //StoreDbContext ctx = new StoreDbContext(
                    //    new Uri(uri
                    //        , UriKind.Absolute));

                    var client = new StoreapptestClient();
                    //var reportItems =
                    //    ctx.ExecuteSyncronous(ctx.PriceChangeReportItems
                    //    .Expand("PriceItem,PreviousPrice,NewPrice,PriceItem/Gear,PriceItem/UnitOfMeasure,")
                    //    .Where(w => w.PriceChangeReport_Id == _priceReport)).ToList();
                    var reportItems = client.GetPriceChangeReportItems(_priceReport);

                    int index = 1;
                    foreach (var reportItem in reportItems)
                    {
                        Model.PriceChangeReportItem item = new Model.PriceChangeReportItem();
                        item.Number = index++;
                        item.Articul = reportItem.PriceItem.Gear.Articul;
                        item.BuyPriceRur = (int)reportItem.PriceItem.BuyPriceRur;
                        item.BuyPriceTng = (int)reportItem.PriceItem.BuyPriceTng;
                        item.CatalogNumber = reportItem.PriceItem.Gear.CatalogNumber;
                        item.Incomes = 0;
                        item.Income_Id = 0;
                        item.IsDuplicate = reportItem.PriceItem.Gear.IsDuplicate ? "*" : "";
                        item.Gear_Id = reportItem.PriceItem.Gear.Id;
                        item.Gear_Name = reportItem.PriceItem.Gear.Name;
                        item.LowerLimitRemainder = 0;//(int)reportItem.PriceItem.Gear.LowerLimitRemainder;
                        item.RecommendedRemainder = 0;//(int)reportItem.PriceItem.Gear.RecommendedRemainder;

                        decimal newPrice = 0;
                        long newPriceId = 0;

                        if (reportItem.NewPrice != null)
                        {
                            newPrice = reportItem.NewPrice.Price;
                            newPriceId = reportItem.NewPrice.Id;
                        }

                        decimal previousPrice = 0;
                        long previouPriceId = newPriceId;

                        if (reportItem.PreviousPrice != null)
                        {
                            previousPrice = reportItem.PreviousPrice.Price;
                            previouPriceId = reportItem.PreviousPrice.Id;
                        }


                        item.NewPrice = (int)newPrice;
                        item.PreviewsPrice = (int)previousPrice;

                        item.NewPrice_Id = newPriceId;
                        item.PreviewsPrice_Id = previouPriceId;

                        item.PriceItem_Id = reportItem.PriceItem_Id;
                        item.Uom = reportItem.PriceItem.Uom_Id;
                        //item.WholesalePrice = reportItem.PriceItem.WholesalePrice;

                        //var rems =
                        //    ctx.ExecuteSyncronous(ctx.Remainders.Where(w => w.PriceItem_Id == reportItem.PriceItem_Id && w.Warehouse_Id == App.CurrentUser.Warehouse_Id))
                        //        .ToList();
                        var rems = client.GetRemaindersByPriceItem(reportItem.PriceItem_Id, App.CurrentUser.Warehouse_Id);

                        item.Remainders = (int)rems.Sum(s => s.Amount);
                        item.IsAccept = true;


                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            PriceChanges.Add(item);
                        });

                    }
                }
                catch (Exception exception)
                {

                }
            }).ContinueWith(c =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsLoading = false;
                    OnPropertyChanged("TotalUp");
                    OnPropertyChanged("TotalDown");
                    OnPropertyChanged("Total");

                });
            });
        }

        #endregion
    }
}
