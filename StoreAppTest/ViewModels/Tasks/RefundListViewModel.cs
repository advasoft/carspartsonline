
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

    public class RefundListViewModel : ViewModelBase
    {

        private bool _showonlyfrocurrentuser;

        private IEventAggregator _agregator;
        private RefundOpenNeedEvent _refundOpenNeedEvent;

        public RefundListViewModel(bool showonlyfrocurrentuser = false)
        {
            _showonlyfrocurrentuser = showonlyfrocurrentuser;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _refundOpenNeedEvent = _agregator.GetEvent<RefundOpenNeedEvent>();

            RefundDocuments = new ObservableCollection<RefundDocumentModel>();

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


        public ObservableCollection<RefundDocumentModel> RefundDocuments
        {
            get { return _RefundDocuments; }
            set
            {
                _RefundDocuments = value;
                OnPropertyChanged("RefundDocuments");
            }
        }
        private ObservableCollection<RefundDocumentModel> _RefundDocuments;


        public RefundDocumentModel SelectedRefund
        {
            get { return _SelectedRefund; }
            set
            {
                _SelectedRefund = value;
                OnPropertyChanged("SelectedRefund");
            }
        }
        private RefundDocumentModel _SelectedRefund;


        public string Barcode
        {
            get { return _Barcode; }
            set
            {
                _Barcode = value;
                OnPropertyChanged("Barcode");
                //SearchByBarcode();

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
        public ICommand ViewRefundCommand { get; set; }

        private void InitCommands()
        {

            #region ViewRefundCommand

            ViewRefundCommand = new UICommand(a =>
            {
                if (SelectedRefund != null)
                {
                    try
                    {
                        _refundOpenNeedEvent.Publish(SelectedRefund);
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

                RefundDocuments.Clear();

                Task.Factory.StartNew(() =>
                {

                    try
                    {
                        var ctx = new StoreDbContext(receivedUri);

                        IList<RefundDocument> refundDocuments = new List<RefundDocument>();

                        if (_showonlyfrocurrentuser)
                        {
                            refundDocuments =
                                ctx.ExecuteSyncronous(ctx.RefundDocuments.Expand(
                                    "SaleDocument,RefundItems")
                                    .Where(rl => rl.RefundDate >= AtFromDate
                                    && rl.RefundDate <= AtToDate
                                    && rl.Creator_Id == App.CurrentUser.UserName)
                                    .OrderByDescending(or => or.RefundDate)).ToList();
                        }
                        else
                        {

                            refundDocuments =
                                ctx.ExecuteSyncronous(ctx.RefundDocuments.Expand(
                                    "SaleDocument,RefundItems")
                                    .Where(rl => rl.RefundDate >= AtFromDate 
                                    && rl.RefundDate <= AtToDate)
                                    .OrderByDescending(or => or.RefundDate)).ToList();
                        }

                        var tempRefund = new List<RefundDocumentModel>();
                        foreach (var document in refundDocuments)
                        {
                            var model = new RefundDocumentModel();
                            model.DocumentDate = document.RefundDate;
                            model.DocumentNumber = document.RefundNumber;
                            //model.Barcode = document.Barcode;
                            model.Id = document.Id;
                            model.TotalAmount = (int)document.RefundItems.Sum((s => s.Amount));
                            model.CustomerName = document.SaleDocument.Customer_Name;

                            tempRefund.Add(model);

                        }
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            foreach (var refundDocumentModel in tempRefund)
                            {
                                RefundDocuments.Add(refundDocumentModel);
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

        //private void SearchByBarcode()
        //{
        //    string uri = string.Concat(
        //       Application.Current.Host.Source.Scheme, "://",
        //       Application.Current.Host.Source.Host, ":",
        //       Application.Current.Host.Source.Port,
        //       "/StoreAppDataService.svc/");
        //    string barcode = Barcode;
        //    Task.Factory.StartNew(() =>
        //    {
        //        try
        //        {

        //            StoreDbContext ctx = new StoreDbContext(
        //                new Uri(uri
        //                    , UriKind.Absolute));


        //            var findedDocument = ctx.ExecuteSyncronous(ctx.RefundDocuments
        //                .Where(rl => rl.Barcode == barcode)).FirstOrDefault();

        //            if (findedDocument != null)
        //            {
        //                DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //                {
        //                    _realizationPerDayOpenNeedEvent.Publish(
        //                        new RealizationPerDayDocumentModel() {Id = findedDocument.Id, DocumentNumber = findedDocument.Number, Barcode = findedDocument.Barcode});
        //                    Barcode = string.Empty;
        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    });
        //}


        private Uri GetContextUri()
        {
            return new Uri(string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/"));

        }


    }


    public class RefundDocumentModel : Notified
    {
        public long Id { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime DocumentDate { get; set; }

        public string CustomerName { get; set; }

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

        public string Barcode { get; set; }
    }

}
