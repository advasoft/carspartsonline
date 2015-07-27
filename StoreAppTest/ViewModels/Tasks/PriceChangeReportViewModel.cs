
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Client;
    using Client.Model;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;
    using Utilities;
    using IncomeItem = Model.IncomeItem;
    using PriceChangeReportItem = Client.Model.PriceChangeReportItem;

    public class PriceChangeReportViewModel : ViewModelBase
    {
        private string _priceListName;
        private IEventAggregator _agregator;
        private CloseViewNeedEvent _closeViewNeedEvent;
        private IList<IncomeItem> _incomes;

        public PriceChangeReportViewModel(string priceListName, IList<IncomeItem> incomes)
        {

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();

            _priceListName = priceListName;
            _incomes = incomes;

            PriceChangeReportCollection = new PriceChangeReportCollection();

            InitCommands();
        }


        public PriceChangeReportCollection PriceChangeReportCollection
        {
            get { return _priceChangeReportCollection; }
            set
            {
                _priceChangeReportCollection = value;
                OnPropertyChanged("PriceChangeReportCollection");
            }
        }
        private PriceChangeReportCollection _priceChangeReportCollection;


        public Guid ViewId { get; set; }


        private Views.PriceChangeReport _view;
        public Views.PriceChangeReport View
        {
            get { return _view; }
            set { _view = value; }
        }



        public decimal TotalUp
        {
            get
            {
                return
                    PriceChangeReportCollection.Where(w => w.NewPrice > w.PreviewsPrice)
                        .Sum(s => (s.NewPrice - s.PreviewsPrice)*s.Remainders);

                    //ReceiptItems.Sum(s => s.Price * s.SoldCount);
            }
        }
        public decimal TotalDown
        {
            get
            {
                return 
                    PriceChangeReportCollection.Where(w => w.NewPrice < w.PreviewsPrice)
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


        private string _PriceReportNumber;

        public string PriceReportNumber
        {
            get { return _PriceReportNumber; }
            set
            {
                _PriceReportNumber = value;
                OnPropertyChanged("PriceReportNumber");
            }
        }


        public bool Saved
        {
            get
            {
                return _Saved;
            }
            set
            {
                _Saved = value;
                OnPropertyChanged("Saved");
            }
        }
        private bool _Saved;


        public long SavedDocumentId { get; set; }



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

                    Model.PriceChangeReportItem check = new Model.PriceChangeReportItem();

                    PriceChangeReportCollection source = new PriceChangeReportCollection();

                    int number = 1;
                    foreach (var incomeItem in _incomes)
                    {

                        //var rems =
                        //    ctx.ExecuteSyncronous(ctx.Remainders.Where(w => w.PriceItem_Id == incomeItem.PriceItem_Id))
                        //        .ToList();

                        //var prices =
                        //    ctx.ExecuteSyncronous(ctx.WholesalePrices.Where(r => r.PriceItem_Id == incomeItem.PriceItem_Id))
                        //        .ToList();
                        var rems = client.GetRemaindersByPriceItem(incomeItem.PriceItem_Id);
                        var prices = client.GetPricesByPriceItem(incomeItem.PriceItem_Id).ToList();


                        var columnsRemainders =
                            from p in rems
                            group p by p.Warehouse_Id into grp
                            select grp.Key;

                        var columns = columnsRemainders;
                        foreach (var column in columns)
                        {
                            if (!check.GetProperties().Any(p => p.Name == column))
                            {
                                Model.PriceChangeReportItem.AddProperty(column, typeof(decimal));

                            }
                        }

                        decimal previousPrice = 0;
                        WholesalePrice currentRetailPrice = prices.OrderByDescending(d => d.PriceDate).First(); 
                        WholesalePrice previousRetailPrice = null;
                        if (prices.Count > 1)
                        {
                            previousRetailPrice = prices.OrderByDescending(d => d.PriceDate).Skip(1).First();
                            previousPrice = previousRetailPrice.Price;

                        }
                        else
                        {
                            previousRetailPrice = currentRetailPrice;
                        }

                        Model.PriceChangeReportItem item = new Model.PriceChangeReportItem()
                        {
                            Number = number++,
                            Articul = incomeItem.Articul,
                            BuyPriceRur = incomeItem.BuyPriceRur,
                            BuyPriceTng = incomeItem.BuyPriceTng,
                            CatalogNumber = incomeItem.CatalogNumber,
                            Incomes = incomeItem.Incomes,
                            Income_Id = incomeItem.Income_ID,
                            IsDuplicate = incomeItem.IsDuplicate,
                            Gear_Id = incomeItem.Gear_Id,
                            Gear_Name = incomeItem.Name,
                            LowerLimitRemainder = (int)incomeItem.LowerLimitRemainder,
                            RecommendedRemainder = (int)incomeItem.RecommendedRemainder,
                            NewPrice = incomeItem.NewPrice,
                            PreviewsPrice = (int)previousPrice,
                            PriceItem_Id = incomeItem.PriceItem_Id,
                            Uom = incomeItem.Uom,
                            WholesalePrice = incomeItem.WholesalePrice,
                            IsAccept = incomeItem.IsAccepted,
                            NewPrice_Id = currentRetailPrice == null ? 0 : currentRetailPrice.Id,
                            PreviewsPrice_Id = previousRetailPrice == null ? 0 : previousRetailPrice.Id,
                            Remainders = (int)rems.Sum(s => s.Amount)
                        };

                        foreach (var rem in rems)
                        {
                            item.SetPropertyValue(rem.Warehouse_Id, rem.Amount);
                        }

                        source.Add(item);

                    }
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        PriceChangeReportCollection = source;


                    });


                    //PriceChangeReportCollection = InittCollection(_incomes);

                }
                catch (Exception exception)
                {
                    throw;
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


        #region Commands

        public ICommand SendReportCommand { get; set; }

        public ICommand PrintReportCommand { get; set; }

        private void InitCommands()
        {


            #region SendReportCommand

            SendReportCommand = new UICommand(o =>
            {
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

                        var now = DateTimeHelper.GetNowKz();


                        PriceChangeReport rep = new PriceChangeReport();
                        rep.Creator_Id = App.CurrentUser.UserName;
                        rep.ReportDate = now;
                        rep.ReportNumber = PriceReportNumber;

                        //ctx.AddToPriceChangeReports(rep);
                        //ctx.SaveChangesSynchronous();


                        foreach (var incomeItem in PriceChangeReportCollection)
                        {

                            PriceChangeReportItem repItem = new PriceChangeReportItem()
                            {
                                PriceItem_Id = incomeItem.PriceItem_Id,
                                PreviousPrice_Id = incomeItem.PreviewsPrice_Id,
                                NewPrice_Id = incomeItem.NewPrice_Id,
                                PriceChangeReport_Id = rep.Id
                            };

                            //ctx.AddToPriceChangeReportItems(repItem);

                            rep.PriceChangeReportItems.Add(repItem);
                        }
                        //ctx.SaveChangesSynchronous();

                        var client = new StoreapptestClient();
                        client.AddPriceChangeReport(rep);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //_closeViewNeedEvent.Publish(ViewId);
                            SavedDocumentId = rep.Id;
                            Saved = true;

                        });
                    }
                    catch (Exception exception)
                    {
                        throw;
                    }
                });
            });

            #endregion

            #region PrintReportCommand

            PrintReportCommand = new UICommand(a =>
            {
                PriceChangeReportControl control = new PriceChangeReportControl(SavedDocumentId);
                control.Show();
            });

            #endregion

        }

        #endregion

    }
}
