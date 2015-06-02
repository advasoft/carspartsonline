
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
    using RefundItem = StoreAppDataService.RefundItem;

    public class RealizationPerDayViewModel : ViewModelBase
    {
        public Guid ViewId { get; private set; }

        private IList<RefundItem> _refundItems;


        private IEventAggregator _agregator;
        private CloseViewNeedEvent _closeViewNeedEvent;

        private IList<RealizationItem> realization = new List<RealizationItem>();
        private IList<DebtDischargeDocument> discharges = new List<DebtDischargeDocument>();
 
        public RealizationPerDayViewModel(Guid viewId)
        {
            ViewId = viewId;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();

            RealizationItems = new ObservableCollection<RealizationItem>();
            _refundItems = new List<RefundItem>();

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
                var total = realization.Where(w => !w.SaleItemData.SaleDocument.IsInDebt).Sum(s => s.Amount);
                return total + totalDisc;
            }
        }
        public decimal TotalRefund
        {
            get
            {
                return _refundItems.Where(w => !w.RefundDocument.SaleDocument.IsInDebt).Sum(s => s.Amount);
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

                decimal total = realization.Where(w => !w.SaleItemData.SaleDocument.IsInDebt).Sum(
                    s => s.SoldCount*s.WholePrice);
                return TotalAmount - (total + totalDisc);
            }
        }
        public decimal TotalRefundProfit
        {
            get
            {
                decimal total = _refundItems.Where(w => !w.RefundDocument.SaleDocument.IsInDebt).Sum(
                    s => s.Count * s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price);

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
                
                var total = realization.Where(w => !w.SaleItemData.SaleDocument.IsInDebt && !_refundItems.Any( a=> a.SaleItem_Id == w.SaleItemData.Id)).Sum(s => s.SoldCount * s.WholePrice);
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

            var now = DateTimeHelper.GetNowKz();

            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            RealizationItems.Clear();

            Task.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                    int index = 1;


                    var realizationPdDb =
                        ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.OrderByDescending(s => s.Id)).FirstOrDefault();
                    if (realizationPdDb != null)
                    {
                        int lastNumber = 0;
                        if (int.TryParse(realizationPdDb.Number, out lastNumber))
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                RealizationNumber = (++lastNumber).ToString();
                            });
                        }
                    }
                    else
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RealizationNumber = "1";
                        });
                    }

                    //var now = DateTimeHelper.GetNowKz();
                    var strt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    var endd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

                    var dischargesSource =
                        ctx.ExecuteSyncronous(
                            ctx.DebtDischargeDocuments.Expand("Debtor")
                                .Where(w => w.DischargeDate >= strt && w.DischargeDate <= endd && w.IsDischarge && w.Creator_Id == App.CurrentUser.UserName)).ToList();
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


                    var closedSalesPerDay = ctx.ExecuteSyncronous(
                        ctx.SalesPerDayItems.Expand("SaleDocumentsPerDay,SaleItem")
                            .Where(
                                w =>
                                    w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                                    && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                                    && w.SaleDocumentsPerDay.IsClosed &&
                                    w.SaleDocumentsPerDay.Creator_Id == App
                                        .CurrentUser.UserName)).ToList().Select(s => s.SaleItem.Id).ToList();


                    var salesQuery = ctx.ExecuteSyncronous(
                        ctx.SaleItems.Expand("SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices")
                            .Where(
                                w =>
                                    w.SaleDocument.SaleDate >= startDate 
                                    && w.SaleDocument.SaleDate <= endDate
                                    && !w.SaleDocument.IsOrder
                                    //&& !closedSalesPerDay.Contains(w.Id)
                                    && w.SaleDocument.Creator_Id == App
                                        .CurrentUser.UserName)).ToList();

                    var salesItems = salesQuery.Where(w => !closedSalesPerDay.Contains(w.Id)).ToList();

                    salesItems.ForEach(s =>
                    {
                        
                        //string debtor = "";
                        //int debtDisc = 0;

                        //var findedDebtor =
                        //    discharges.Where(w => w.Debtor_Id == s.SaleDocument.Customer_Name).FirstOrDefault();
                        //if (findedDebtor != null)
                        //{
                        //    debtor = findedDebtor.Debtor_Id;
                        //    debtDisc = (int)findedDebtor.Amount;
                        //}

                        realization.Add(new RealizationItem()
                        {
                            
                            CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                            IsDuplicate = s.PriceItem.Gear.IsDuplicate? "*" : "",
                            Name = s.PriceItem.Gear.Name,
                            Price = (int)s.Price,
                            Remainders =
                                (int)s.PriceItem.Remainders.First().Amount,
                            SoldCount = (int)s.Count,
                            Uom = s.PriceItem.UnitOfMeasure.Name,
                            WholePrice = (int)s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                            Amount = (int)((s.Price * s.Count) - s.Discount),
                            Discount = (int)s.Discount,
                            SaleItemData = s,
                            Customer = s.SaleDocument.IsInDebt ? s.SaleDocument.Customer_Name : "",
                            //DebtDischarge = s.SaleDocument.IsInDebt ? debtDisc : 0,
                            IsInDebt = s.SaleDocument.IsInDebt

                        });
                    });

                    var query = from r in realization
                                group r by new
                                {
                                    CatalogNumber = r.CatalogNumber,
                                    IsDuplicate = r.IsDuplicate,
                                    Name = r.Name,
                                    Price = r.Price,
                                    //Remainders = r.Remainders,
                                    //SoldCount = r.SoldCount,
                                    Uom = r.Uom,

                                    //Customer = r.Customer,
                                    //IsInDebt = r.IsInDebt,
                                    //SaleItemData = r.SaleItemData,
                                    //Amount = r.Amount,
                                    //Discount = r.Discount,
                                    //WholePrice = r.WholePrice
                                }
                                    into groups
                                    select new RealizationItem()
                                    {
                                        Number = index++,
                                        CatalogNumber = groups.Key.CatalogNumber,
                                        IsDuplicate = groups.Key.IsDuplicate,
                                        Name = groups.Key.Name,
                                        Price = (int)groups.Key.Price,
                                        Remainders = (int)groups.Average(s => s.Remainders),
                                        SoldCount = groups.Sum(s => s.SoldCount),
                                        Uom = groups.Key.Uom,
                                        Amount = groups.Sum(s => s.Amount),
                                        Discount = groups.Sum(s => s.Discount),
                                        WholePrice = (int)groups.Average(a => a.WholePrice),
                                        //Customer = groups.Key.Customer,
                                        DebtDischarge = (int)groups.Average(s => s.DebtDischarge),
                                        //IsInDebt = groups.Key.IsInDebt,
                                        //SaleItemData = groups.Key.SaleItemData
                                    };

                    var closedRefundPerDay = ctx.ExecuteSyncronous(
                        ctx.RefundsPerDayItems.Expand("SaleDocumentsPerDay,RefundItem")
                            .Where(
                                w =>
                                    w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                                    && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                                    && w.SaleDocumentsPerDay.IsClosed &&
                                    w.SaleDocumentsPerDay.Creator_Id == App
                                        .CurrentUser.UserName)).ToList().Select(s => s.RefundItem.Id).ToList();

                    
                    var refundQuery = ctx.ExecuteSyncronous(
                        ctx.RefundItems.Expand("RefundDocument/SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices")
                            .Where(
                                w =>
                                    w.RefundDocument.RefundDate >= startDate 
                                    && w.RefundDocument.RefundDate <= endDate
                                    //&& !closedRefundPerDay.Contains(w.Id)
                                    && w.RefundDocument.Creator_Id == App
                                        .CurrentUser.UserName)).ToList();

                    _refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();

                    foreach (var realizationItem in query)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RealizationItems.Add(realizationItem);
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

        public ICommand SaveRealizationPerDayCommand { get; set; }
        public ICommand PrintReportCommand { get; set; }

        private void InitCommands()
        {
            SaveRealizationPerDayCommand = new UICommand(o =>
            {
                string uri = string.Concat(
                    Application.Current.Host.Source.Scheme, "://",
                    Application.Current.Host.Source.Host, ":",
                    Application.Current.Host.Source.Port,
                    "/StoreAppDataService.svc/");

                Task.Factory.StartNew(() =>
                {
                    try
                    {

                        StoreDbContext ctx = new StoreDbContext(
                            new Uri(uri
                                , UriKind.Absolute));

                        var realizationPdDb =
                           ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.OrderByDescending(s => s.Id)).FirstOrDefault();
                        if (realizationPdDb != null)
                        {
                            int lastNumber = 0;
                            if (int.TryParse(realizationPdDb.Number, out lastNumber))
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    RealizationNumber = (++lastNumber).ToString();
                                });
                            }
                        }
                        else
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                RealizationNumber = "1";
                            });
                        }

                        
                        var salePerDay = new SaleDocumentsPerDay();
                        //receipt.Creator = App.CurrentUser;
                        salePerDay.Creator_Id = App.CurrentUser.UserName;
                        salePerDay.IsClosed = true;
                        salePerDay.Number = RealizationNumber;
                        salePerDay.SaleDocumentsDate = DateTimeHelper.GetNowKz();
                        salePerDay.Barcode = GetBarcode();

                        ctx.AddToSaleDocumentsPerDays(salePerDay);
                        ctx.SaveChangesSynchronous();


                        foreach (var receiptItem in realization)
                        {
                            var item = new SalesPerDayItem()
                            {
                                SaleDocumentsPerDay_Id = salePerDay.Id,
                                SaleItem_Id = receiptItem.SaleItemData.Id

                            };

                            salePerDay.SalesPerDayItems.Add(item);
                            ctx.AddToSalesPerDayItems(item);
                        }


                        foreach (var refundItem in _refundItems)
                        {
                            var item = new RefundsPerDayItem()
                            {
                                SaleDocumentsPerDay_Id = salePerDay.Id,
                                RefundItem_Id = refundItem.Id

                            };

                            ctx.AddToRefundsPerDayItems(item);
                        }

                        ctx.SaveChangesSynchronous();

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //_closeViewNeedEvent.Publish(ViewId);
                            SavedDocumentId = salePerDay.Id;
                            Barcode = salePerDay.Barcode;
                            Saved = true;
                        });
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }


                }).ContinueWith(c =>
                {

                });
            });

            PrintReportCommand = new UICommand(a =>
            {
                RealizationPerDayReportControl control = new RealizationPerDayReportControl(SavedDocumentId);
                control.Show();
            });
        }

        private string GetBarcode()
        {
            string pref = "RE";
            int lenAll = 4;
            int lenNum = RealizationNumber.Length;
            int zeroCount = lenAll - lenNum;

            StringBuilder builder = new StringBuilder(pref);
            for (int i = 0; i < zeroCount; i++)
            {
                builder.Append("0");
            }

            builder.Append(RealizationNumber);

            return builder.ToString();
        }

        #endregion

    }
}
