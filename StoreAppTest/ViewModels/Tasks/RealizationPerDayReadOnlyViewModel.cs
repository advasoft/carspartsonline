
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
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;using StoreAppDataService;
    using Utilities;

    public class RealizationPerDayReadOnlyViewModel : ViewModelBase
    {
        public Guid ViewId { get; private set; }

        private long _realizationPerDayId;

        private IEventAggregator _agregator;
        private CloseViewNeedEvent _closeViewNeedEvent;

        private IList<RealizationItem> realization = new List<RealizationItem>();
        private IList<RefundsPerDayItem>  _refundItems = new List<RefundsPerDayItem>();
        private IList<DebtDischargeDocument> discharges = new List<DebtDischargeDocument>();

        public RealizationPerDayReadOnlyViewModel(Guid viewId, long realizationPerDayId)
        {
            ViewId = viewId;

            _realizationPerDayId = realizationPerDayId;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();

            RealizationItems = new ObservableCollection<RealizationItem>();

            InitCommands();
        }


        public string RealizationNumber
        {
            get { return _realizationNumber; }
            set
            {
                _realizationNumber = value;
                OnPropertyChanged("RealizationNumber");
            }
        }

        public decimal TotalAmount
        {
            get
            {
                var totalDisc = discharges.Sum(s => s.Amount);
                var total = realization.Sum(s => s.Amount);
                return total + totalDisc;
            }
        }
        public decimal TotalRefund
        {
            get
            {
                return _refundItems.Sum(s => s.RefundItem.Amount);
            }
        }
        public decimal SubTotal
        {
            get
            {
                return TotalAmount - TotalRefund;
            }
        }
        public decimal TotalAmountProfit
        {
            get
            {
                var totalDisc = discharges.Sum(s => s.Amount);

                decimal total = realization.Sum(
                    s => s.SoldCount*s.WholePrice);
                return TotalAmount - (total + totalDisc);
            }
        }
        public decimal TotalRefundProfit
        {
            get
            {
                decimal total = _refundItems.Sum(
                    s => s.RefundItem.Count * s.RefundItem.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price);

                return TotalRefund - total;
            }
        }
        public decimal SubTotalProfit
        {
            get
            {
                return TotalAmountProfit - TotalRefundProfit;
            }
        }
        public decimal TotalByPrice
        {
            get
            {
                //var totalDisc = discharges.Sum(s => s.Amount);

                var total = realization.Where(w => !_refundItems.Any(a => a.RefundItem.SaleItem_Id == w.SaleItemData.Id)).Sum(s => s.SoldCount * s.WholePrice);
                return total;
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


        private string _realizationNumber;

        public ObservableCollection<RealizationItem> RealizationItems { get; set; }

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            realization = new List<RealizationItem>();

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            RealizationItems.Clear();
            _refundItems.Clear();
            realization.Clear();

            Task.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                    int index = 1;


                    var realizationPdDb =
                        ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.Expand("SalesPerDayItems/SaleItem/PriceItem/Prices,SalesPerDayItems/SaleItem/PriceItem/Gear,SalesPerDayItems/SaleItem/PriceItem/Remainders,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Prices,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Gear,RefundsPerDayItems/RefundItem/SaleItem/PriceItem/Remainders,RefundsPerDayItems/RefundItem/PriceItem/Prices,RefundsPerDayItems/RefundItem/PriceItem/Gear,RefundsPerDayItems/RefundItem/PriceItem/Remainders")
                        .Where(rl => rl.Id == _realizationPerDayId)).FirstOrDefault();

                    var startDate = DateTimeHelper.GetStartDay(realizationPdDb.SaleDocumentsDate);
                    var endDate = DateTimeHelper.GetEndDay(realizationPdDb.SaleDocumentsDate);

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




                    _refundItems = realizationPdDb.RefundsPerDayItems.ToList();

                    foreach (var realizationItem in realizationPdDb.SalesPerDayItems)
                    {

                        var remainders = 0;
                        var rems =
                            realizationItem.SaleItem.PriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                                .FirstOrDefault();
                        if (rems != null)
                            remainders = (int)rems.Amount;

                        var item = new RealizationItem()
                        {

                            CatalogNumber = realizationItem.SaleItem.PriceItem.Gear.CatalogNumber,
                            IsDuplicate = realizationItem.SaleItem.PriceItem.Gear.IsDuplicate ? "*" : "",
                            Name = realizationItem.SaleItem.PriceItem.Gear.Name,
                            Price = (int)realizationItem.SaleItem.Price,
                            Remainders = remainders,
                            SoldCount = (int)realizationItem.SaleItem.Count,
                            Uom = realizationItem.SaleItem.PriceItem.Uom_Id,
                            WholePrice = (int)realizationItem.SaleItem.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                            Amount = (int)((realizationItem.SaleItem.Price * realizationItem.SaleItem.Count) - realizationItem.SaleItem.Discount),
                            Discount = (int)realizationItem.SaleItem.Discount,
                            SaleItemData = realizationItem.SaleItem,
                            //Customer = realizationItem.SaleItem.SaleDocument.IsInDebt ? realizationItem.SaleItem.SaleDocument.Customer_Name : "",
                            ////DebtDischarge = s.SaleDocument.IsInDebt ? debtDisc : 0,
                            //IsInDebt = realizationItem.SaleItem.SaleDocument.IsInDebt

                        };
                        realization.Add(item);
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RealizationItems.Add(item);
                        });
                    }

                    foreach (var debtDischargeDocument in discharges)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RealizationItems.Add(new RealizationItem()
                            {
                                Number = index++,
                                Name = "Погашение " + debtDischargeDocument.Debtor_Id,
                                Price = (int)debtDischargeDocument.Amount,
                                IsInDebt = true
                            });
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
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("TotalRefund");
                    OnPropertyChanged("SubTotal");
                    OnPropertyChanged("TotalAmountProfit");
                    OnPropertyChanged("TotalRefundProfit");
                    OnPropertyChanged("SubTotalProfit");
                    OnPropertyChanged("TotalByPrice");
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
                RealizationPerDayReportControl control = new RealizationPerDayReportControl(SavedDocumentId);
                control.Show();
            });
        }

        #endregion

    }
}
