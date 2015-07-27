
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
    using Controls;
    using DevExpress.Data.PLinq.Helpers;
    using GalaSoft.MvvmLight.Threading;
    using Utilities;

    public class PriceChangeReportsViewModel : ViewModelBase
    {

        public PriceChangeReportsViewModel()
        {
            PriceChangeReports = new ObservableCollection<PriceChangeReportItemModel>();
            InitCommands();
        }

        public ObservableCollection<PriceChangeReportItemModel> PriceChangeReports { get; set; }

        public PriceChangeReportItemModel SelectedPriceChangeReport
        {
            get { return _SelectedPriceChangeReport; }
            set
            {
                _SelectedPriceChangeReport = value;
                OnPropertyChanged("SelectedPriceChangeReport");
            }
        }
        private PriceChangeReportItemModel _SelectedPriceChangeReport;


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
            IsLoading = true;
            PriceChangeReports.Clear();

                //        string uri = string.Concat(
                //Application.Current.Host.Source.Scheme, "://",
                //Application.Current.Host.Source.Host, ":",
                //Application.Current.Host.Source.Port,
                //"/StoreAppDataService.svc/");


            Task.Factory.StartNew(() =>
            {
                try
                {

                    //StoreDbContext ctx = new StoreDbContext(
                    //    new Uri(uri
                    //        , UriKind.Absolute));

                    var client = new StoreapptestClient();

                    //var reports = ctx.ExecuteSyncronous(ctx.PriceChangeReports.Expand("Creator,PriceChangeReportItems/NewPrice,PriceChangeReportItems/PreviousPrice,PriceChangeReportItems/PriceItem/Remainders")).ToList();
                    var reports = client.GetPriceChangeReports();
                    foreach (var source in reports.OrderByDescending(o => o.ReportDate))
                    {
                        PriceChangeReportItemModel item = new PriceChangeReportItemModel();
                        item.Date = source.ReportDate;
                        item.Number = source.ReportNumber;
                        item.PriceChangeReport_Id = source.Id;

                        int up = 0;
                        int down = 0;

                        IList<Remainder> remainders = source.PriceChangeReportItems.SelectMany(s => s.PriceItem.Remainders.Where(r => r.Warehouse_Id == App.CurrentUser.Warehouse_Id)).ToList();
                        //IList<WholesalePrice> prices =
                        //    source.PriceChangeReportItems.SelectMany(s => s.PriceItem.Prices).ToList();

                        foreach (var priceChangeReportItem in source.PriceChangeReportItems)
                        {
                            if (remainders != null &&
                                remainders.Any(a => a.PriceItem_Id == priceChangeReportItem.PriceItem.Id))
                            {
                                var rem =
                                    remainders.Where(r => r.PriceItem_Id == priceChangeReportItem.PriceItem.Id)
                                        .FirstOrDefault();

                                if (priceChangeReportItem.NewPrice.Price > priceChangeReportItem.PreviousPrice.Price)
                                    up += (int) ((priceChangeReportItem.NewPrice.Price -
                                                  priceChangeReportItem.PreviousPrice.Price)*rem.Amount);

                                else if (priceChangeReportItem.NewPrice.Price < priceChangeReportItem.PreviousPrice.Price)
                                    down += (int)((priceChangeReportItem.PreviousPrice.Price -
                                                    priceChangeReportItem.NewPrice.Price) * rem.Amount);
                            }
                        }
                        //item.Up = (int)source.PriceChangeReportItems.Where(w => w.NewPrice.Price > w.PreviousPrice.Price)
                        //.Sum(a => (a.NewPrice.Price - a.PreviousPrice.Price) * a.PriceItem.Remainders.Sum(s => s.Amount));
                        //item.Down = (int)source.PriceChangeReportItems.Where(w => w.NewPrice.Price < w.PreviousPrice.Price)
                        //.Sum(a => (a.PreviousPrice.Price - a.NewPrice.Price) * a.PriceItem.Remainders.Sum(s => s.Amount));
                        item.Up = up;
                        item.Down = down;
                        item.Total = item.Up - item.Down;

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            PriceChangeReports.Add(item);
                        });
                    }
                }
                catch (Exception exception)
                {
                    throw;
                }
            }).ContinueWith(t =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsLoading = false;
                });
            });
        }

        #endregion



        #region Commands

        public ICommand OpenPriceChangeReportCommand { get; set; }


        private void InitCommands()
        {
            OpenPriceChangeReportCommand = new UICommand(a =>
            {
                if(SelectedPriceChangeReport == null) return;

                PriceChangesReportControl ps = new PriceChangesReportControl();
                PriceChangesReportsItemViewModel psv = new PriceChangesReportsItemViewModel(SelectedPriceChangeReport.PriceChangeReport_Id);
                ps.DataContext = psv;

                ps.Show();

                psv.LoadView();

            });
        }

        #endregion
    }

    public class PriceChangeReportItemModel
    {
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public long PriceChangeReport_Id { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }
        public int Total { get; set; }

    }
}
