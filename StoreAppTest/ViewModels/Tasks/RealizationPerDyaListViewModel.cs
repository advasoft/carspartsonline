
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Controls;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using StoreAppDataService;
    using Utilities;

    public class RealizationPerDyaListViewModel : ViewModelBase
    {

        private bool _showonlyfrocurrentuser;

        private IEventAggregator _agregator;
        private RealizationPerDayOpenNeedEvent _realizationPerDayOpenNeedEvent;

        public RealizationPerDyaListViewModel(bool showonlyfrocurrentuser = false)
        {
            _showonlyfrocurrentuser = showonlyfrocurrentuser;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _realizationPerDayOpenNeedEvent = _agregator.GetEvent<RealizationPerDayOpenNeedEvent>();

            RealizationPerDayDocuments = new ObservableCollection<RealizationPerDayDocumentModel>();

            InitCommands();
        }

        public DateTime AtFromDate
        {
            get { return _atFromDate; }
            set
            {
                _atFromDate = DateTimeHelper.GetStartDay(value);
                OnPropertyChanged("AtFromDate");
            }
        }
        private DateTime _atFromDate;


        public DateTime AtToDate
        {
            get { return _atToDate; }
            set
            {
                _atToDate = DateTimeHelper.GetEndDay(value);
                OnPropertyChanged("AtToDate");
            }
        }
        private DateTime _atToDate;


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


        public ObservableCollection<RealizationPerDayDocumentModel> RealizationPerDayDocuments
        {
            get { return _RealizationPerDayDocuments; }
            set
            {
                _RealizationPerDayDocuments = value;
                OnPropertyChanged("RealizationPerDayDocuments");
            }
        }
        private ObservableCollection<RealizationPerDayDocumentModel> _RealizationPerDayDocuments;


        public RealizationPerDayDocumentModel SelectedRealizationPerDayDocument
        {
            get { return _SelectedRealizationPerDayDocument; }
            set
            {
                _SelectedRealizationPerDayDocument = value;
                OnPropertyChanged("SelectedRealizationPerDayDocument");
            }
        }
        private RealizationPerDayDocumentModel _SelectedRealizationPerDayDocument;


        public string Barcode
        {
            get { return _Barcode; }
            set
            {
                _Barcode = value;
                OnPropertyChanged("Barcode");
            }
        }
        private string _Barcode;


        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            var now = DateTime.Now;
            AtFromDate = DateTimeHelper.GetStartMonth(now);
            AtToDate = DateTimeHelper.GetEndMonth(now);

            RefreshCommand.Execute(null);
        }

        #endregion



        #region Commands


        public ICommand RefreshCommand { get; set; }
        public ICommand ViewRealizationPerDayCommand { get; set; }

        private void InitCommands()
        {

            #region ViewWarehouseTransferRequestCommand

            ViewRealizationPerDayCommand = new UICommand(a =>
            {
                if (SelectedRealizationPerDayDocument != null)
                {
                    try
                    {
                        _realizationPerDayOpenNeedEvent.Publish(SelectedRealizationPerDayDocument);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            });

            #endregion

            #region RefreshCommand

            RefreshCommand = new UICommand(o =>
            {
                IsLoading = true;

                var receivedUri = GetContextUri();

                RealizationPerDayDocuments.Clear();

                Task.Factory.StartNew(() =>
                {

                    try
                    {
                        var ctx = new StoreDbContext(receivedUri);

                        IList<SaleDocumentsPerDay> salesPerDayDocuments = new List<SaleDocumentsPerDay>();

                        if (_showonlyfrocurrentuser)
                        {
                            salesPerDayDocuments =
                                ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.Expand(
                                    "Creator,SalesPerDayItems/SaleItem/PriceItem/Prices,SalesPerDayItems/SaleItem/PriceItem/Gear,SalesPerDayItems/SaleItem/PriceItem/Remainders,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Prices,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Gear,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Remainders,RefundsPerDayItems/RefundItem/PriceItem/Prices,RefundsPerDayItems/RefundItem/PriceItem/Gear,RefundsPerDayItems/RefundItem/PriceItem/Remainders")
                                    .Where(rl => rl.SaleDocumentsDate >= AtFromDate
                                    && rl.SaleDocumentsDate <= AtToDate
                                    && rl.Creator_Id == App.CurrentUser.UserName)
                                    .OrderByDescending(or => or.SaleDocumentsDate)).ToList();
                        }
                        else
                        {

                            salesPerDayDocuments =
                                ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.Expand(
                                    "Creator,SalesPerDayItems/SaleItem/PriceItem/Prices,SalesPerDayItems/SaleItem/PriceItem/Gear,SalesPerDayItems/SaleItem/PriceItem/Remainders,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Prices,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Gear,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Remainders,RefundsPerDayItems/RefundItem/PriceItem/Prices,RefundsPerDayItems/RefundItem/PriceItem/Gear,RefundsPerDayItems/RefundItem/PriceItem/Remainders")
                                    .Where(rl => rl.SaleDocumentsDate >= AtFromDate 
                                    && rl.SaleDocumentsDate <= AtToDate)
                                    .OrderByDescending(or => or.SaleDocumentsDate)).ToList();
                        }

                        var tempRealization = new List<RealizationPerDayDocumentModel>();
                        foreach (var document in salesPerDayDocuments)
                        {
                            var model = new RealizationPerDayDocumentModel();
                            model.DocumentDate = document.SaleDocumentsDate;
                            model.DocumentNumber = document.Number;
                            model.Barcode = document.Barcode;
                            model.Id = document.Id;
                            model.RefundAmount = (int)document.RefundsPerDayItems.Sum(s => s.RefundItem.Amount);

                            var discharges = new List<DebtDischargeDocument>();

                            var startDate = DateTimeHelper.GetStartDay(document.SaleDocumentsDate);
                            var endDate = DateTimeHelper.GetEndDay(document.SaleDocumentsDate);

                            var dischargesSource =
                                ctx.ExecuteSyncronous(
                                    ctx.DebtDischargeDocuments.Expand("Debtor")
                                        .Where(w => w.DischargeDate >= startDate && w.DischargeDate <= endDate && w.IsDischarge && w.Creator_Id == App.CurrentUser.UserName)).ToList();
                            discharges = (from d in dischargesSource
                                          group d by new
                                          {
                                              Debtor = d.Debtor_Id
                                          }
                                              into ds
                                              select new DebtDischargeDocument()
                                              {
                                                  Amount = ds.Sum(s => s.Amount),
                                                  Debtor_Id = ds.Key.Debtor
                                              }).ToList();



                            var totalDisc = discharges.Sum(s => s.Amount);
                            var total = document.SalesPerDayItems.Sum(s => s.SaleItem.Amount);


                            model.TotalAmount = (int)(total + totalDisc);
                            model.Total = model.TotalAmount - model.RefundAmount;

                            model.UserName = document.Creator.DisplayName;

                            tempRealization.Add(model);

                        }
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            foreach (var realizationPerDayDocumentModel in tempRealization)
                            {
                                RealizationPerDayDocuments.Add(realizationPerDayDocumentModel);
                            }
                        });


                    }
                    catch (Exception exception)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        });
                    }
                }).ContinueWith(c =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        IsLoading = false;
                    });
                });
            });

            #endregion

        }

        #endregion



        private Uri GetContextUri()
        {
            return new Uri(string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/"));

        }

    }


    public class RealizationPerDayDocumentModel : Notified
    {
        public long Id { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime DocumentDate { get; set; }

        public string UserName { get; set; }

        private int _TotalAmount;
        public int TotalAmount
        {
            get
            {
                return _TotalAmount;
            }
            set
            {
                _TotalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }



        private int _RefundAmount;
        public int RefundAmount
        {
            get
            {
                return _RefundAmount;
            }
            set
            {
                _RefundAmount = value;
                OnPropertyChanged("RefundAmount");
            }
        }

        private int _Total;
        public int Total
        {
            get
            {
                return _Total;
            }
            set
            {
                _Total = value;
                OnPropertyChanged("Total");
            }
        }

        public string Barcode { get; set; }
    }

}
