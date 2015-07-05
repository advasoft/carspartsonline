
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
        private IList<DebtDischargeDocument> discharges = new List<DebtDischargeDocument>();

        private Task _loadTask;

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
                return _TotalAmount;
            }
            set
            {
                _TotalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        private decimal _TotalAmount;

        public decimal TotalRefund
        {
            get
            {
                return _TotalRefund;
            }
            set
            {
                _TotalRefund = value;
                OnPropertyChanged("TotalRefund");
            }
        }

        private decimal _TotalRefund;


        public decimal SubTotal
        {
            get
            {
                return _SubTotal;
            }
            set
            {
                _SubTotal = value;
                OnPropertyChanged("SubTotal");
            }
        }

        private decimal _SubTotal;

        public decimal TotalAmountProfit
        {
            get
            {

                return _TotalAmountProfit;
            }
            set
            {
                _TotalAmountProfit = value;
                OnPropertyChanged("TotalAmountProfit");
            }
        }

        private decimal _TotalAmountProfit;

        public decimal TotalRefundProfit
        {
            get
            {
                return _TotalRefundProfit;
            }
            set
            {
                _TotalRefundProfit = value;
                OnPropertyChanged("TotalRefundProfit");
            }
        }

        private decimal _TotalRefundProfit;


        public decimal SubTotalProfit
        {
            get
            {
                return _SubTotalProfit;
            }
            set
            {
                _SubTotalProfit = value;
                OnPropertyChanged("SubTotalProfit");
            }
        }

        private decimal _SubTotalProfit;



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
            realization.Clear();

            _loadTask = Task.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                    int index = 1;


                    var realizationPdDb =
                        ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.Expand("SalesPerDayItems/SaleItem/SaleDocument,SalesPerDayItems/SaleItem/PriceItem/PriceLists,SalesPerDayItems/SaleItem/PriceItem/Prices")
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



                    foreach (var realizationItem in realizationPdDb.SalesPerDayItems)
                    {
                        var amouuntWithoutDebt = 0;
                        var amountWholesalePriceWithoutDebt = 0;

                        if (!realizationItem.SaleItem.SaleDocument.IsInDebt)
                        {
                            var price =
                                realizationItem.SaleItem.PriceItem.Prices.Where(p => p.PriceDate <= DateTimeHelper.GetNowKz())
                                    .OrderByDescending(o => o.PriceDate)
                                    .FirstOrDefault();


                            amouuntWithoutDebt = (int)((realizationItem.Price * realizationItem.Count) - realizationItem.Discount);
                            if (price != null)
                            {
                                amountWholesalePriceWithoutDebt =
                                    (int)((price.Price * realizationItem.Count));
                            }

                        }

                        var item = new RealizationItem()
                        {
                            Number = index++,
                            CatalogNumber = realizationItem.CatalogNumber,
                            IsDuplicate = realizationItem.IsDuplicate ? "*" : "",
                            Name = realizationItem.Name,
                            Price = (int)realizationItem.Price,
                            Remainders = (int)realizationItem.Remainders,
                            SoldCount = (int)realizationItem.Count,
                            Uom = realizationItem.UnitOfMeasure,
                            Amount = (int)realizationItem.Amount,
                            Discount = (int)realizationItem.Discount,
                            IsInDebt = realizationItem.SaleItem.SaleDocument.IsInDebt,
                            PriceListName = realizationItem.SaleItem.PriceItem.PriceLists.First().Name,
                            SaledDate = realizationItem.SaleItem.SaleDocument.SaleDate.ToString("T"),
                            AmountWithoutDebt = amouuntWithoutDebt,
                            AmountWithoutDebtProfit = amouuntWithoutDebt - amountWholesalePriceWithoutDebt,
                            SaledCount = (int)realizationItem.SaleItem.Count,
                            Additional = realizationItem.SaleItem.SaleDocument.IsInDebt ? "Продан в долг" : ""
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
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        TotalAmount = realizationPdDb.TotalAmount;
                        TotalAmountProfit = realizationPdDb.TotalAmountProfit;
                        TotalRefund = realizationPdDb.TotalRefund;
                        TotalRefundProfit = realizationPdDb.TotalRefundProfit;
                        SubTotal = realizationPdDb.SubTotal;
                        SubTotalProfit = realizationPdDb.SubTotalProfit;
                    });
                }
                catch (Exception e)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageBox.Show(e.Message);
                    });
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
                    //OnPropertyChanged("TotalByPrice");
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
