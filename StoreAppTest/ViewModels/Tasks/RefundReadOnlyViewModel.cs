
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Client;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;
    using Utilities;
    using RefundItem = Model.RefundItem;

    public class RefundReadOnlyViewModel : ViewModelBase
    {
        public Guid ViewId { get; private set; }

        private long _refundId;

        private IEventAggregator _agregator;
        private CloseViewNeedEvent _closeViewNeedEvent;

        private IList<RefundItem> _refunds = new List<RefundItem>();

        private Task _loadTask;

        public RefundReadOnlyViewModel(Guid viewId, long refundId)
        {
            ViewId = viewId;

            _refundId = refundId;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();

            RefundItems = new ObservableCollection<RefundItem>();

            InitCommands();
        }


        public string RefundNumber
        {
            get { return _refundNumber; }
            set
            {
                _refundNumber = value;
                OnPropertyChanged("RefundNumber");
            }
        }


        public decimal TotalRefund
        {
            get
            {
                return RefundItems.Sum(s => s.Amount);
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


        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                _Barcode = value;
                OnPropertyChanged("Barcode");
            }
        }
        private string _Barcode;

        public long SavedDocumentId { get; set; }


        private string _refundNumber;

        public ObservableCollection<RefundItem> RefundItems { get; set; }

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            _refunds = new List<RefundItem>();

            //string uri = string.Concat(
            //    Application.Current.Host.Source.Scheme, "://",
            //    Application.Current.Host.Source.Host, ":",
            //    Application.Current.Host.Source.Port,
            //    "/StoreAppDataService.svc/");

            RefundItems.Clear();
            _refunds.Clear();

            _loadTask = Task.Factory.StartNew(() =>
            {
                try
                {

                    //StoreDbContext ctx = new StoreDbContext(
                    //    new Uri(uri
                    //        , UriKind.Absolute));
                    var client = new StoreapptestClient();
                    int index = 1;


                    var refundPdDb =
                        //ctx.ExecuteSyncronous(ctx.RefundDocuments.Expand("RefundItems/PriceItem/Gear,RefundItems/PriceItem/Prices,RefundItems/SaleItem")
                        //.Where(rl => rl.Id == _refundId)).FirstOrDefault();
                        client.GetRefundDocument(_refundId);

                    foreach (var refundItem in refundPdDb.RefundItems)
                    {
                        var wholesalePrice = 0;
                        if (refundItem.PriceItem.Prices.Count > 0)
                        {
                            wholesalePrice = (int)
                                refundItem.PriceItem.Prices.Where(w => w.PriceDate <= refundPdDb.RefundDate).OrderByDescending(or => or.PriceDate)
                                    .First()
                                    .Price;
                        }

                        var item = new RefundItem()
                        {
                            Number = index++,
                            CatalogNumber = refundItem.PriceItem.Gear.CatalogNumber,
                            IsDuplicate = refundItem.PriceItem.Gear.IsDuplicate ? "*" : "",
                            Name = refundItem.PriceItem.Gear.Name,
                            RetailPrice = (int)refundItem.Price,
                            WhosalePrice = wholesalePrice,
                            SoldCount = (int)refundItem.Count,
                            Uom = refundItem.PriceItem.Uom_Id,
                            Discount = (int)refundItem.Discount,
                            SaledCount = (int)refundItem.SaleItem.Count
                        };
                        _refunds.Add(item);
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RefundItems.Add(item);
                        });
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }).ContinueWith(t =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    OnPropertyChanged("TotalRefund");
                });
            });
        }

        #endregion

        #region Commands

        public ICommand PrintReportCommand { get; set; }

        private void InitCommands()
        {

            PrintReportCommand = new UICommand(a =>
            {
                RefundDocumentReportControl control = new RefundDocumentReportControl(SavedDocumentId);
                control.Show();
            });
        }

        #endregion


            
    }
}
