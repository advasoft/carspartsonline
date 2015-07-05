
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
                SearchByBarcode();

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
                                    "Creator,SalesPerDayItems")
                                    .Where(rl => rl.SaleDocumentsDate >= AtFromDate
                                    && rl.SaleDocumentsDate <= AtToDate
                                    && rl.Creator_Id == App.CurrentUser.UserName)
                                    .OrderByDescending(or => or.SaleDocumentsDate)).ToList();
                        }
                        else
                        {

                            salesPerDayDocuments =
                                ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.Expand(
                                    "Creator,SalesPerDayItems")
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
                            model.RefundAmount = (int)document.TotalRefund;

                            model.TotalAmount = (int)document.TotalAmount;
                            model.Total = (int) document.SubTotal;

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

        private void SearchByBarcode()
        {
            string uri = string.Concat(
               Application.Current.Host.Source.Scheme, "://",
               Application.Current.Host.Source.Host, ":",
               Application.Current.Host.Source.Port,
               "/StoreAppDataService.svc/");
            string barcode = Barcode;
            Task.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));


                    var findedDocument = ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays
                        .Where(rl => rl.Barcode == barcode)).FirstOrDefault();

                    if (findedDocument != null)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _realizationPerDayOpenNeedEvent.Publish(
                                new RealizationPerDayDocumentModel() {Id = findedDocument.Id, DocumentNumber = findedDocument.Number, Barcode = findedDocument.Barcode});
                            Barcode = string.Empty;
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }


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
