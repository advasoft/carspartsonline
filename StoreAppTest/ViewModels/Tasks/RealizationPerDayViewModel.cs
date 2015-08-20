
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
    using Client.Model;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;
    using Utilities;
    using Views;
    using RefundItem = Client.Model.RefundItem;

    public class RealizationPerDayViewModel : ViewModelBase
    {
        public Guid ViewId { get; private set; }

        private IList<RefundItem> _refundItems;


        private IEventAggregator _agregator;
        private CloseViewNeedEvent _closeViewNeedEvent;

        private IList<RealizationItem> realization = new List<RealizationItem>();
        private IList<DebtDischargeDocument> discharges = new List<DebtDischargeDocument>();

        private RealizationPerDay _view;

        public RealizationPerDayViewModel(Guid viewId, RealizationPerDay view)
        {
            ViewId = viewId;

            _view = view;

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
                //
                //return _refundItems.Where(w => !w.RefundDocument.SaleDocument.IsInDebt).Sum(s => s.Amount);
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
                    s => s.SoldCount * s.WholePrice);
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

        public bool Saving
        {
            get
            {
                return _IsSaving;
            }
            set
            {
                _IsSaving = value;
                OnPropertyChanged("Saving");
            }
        }
        private bool _IsSaving;

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
            //realization = new List<RealizationItem>();

            //string uri = string.Concat(
            //    Application.Current.Host.Source.Scheme, "://",
            //    Application.Current.Host.Source.Host, ":",
            //    Application.Current.Host.Source.Port,
            //    "/StoreAppDataService.svc/");

            RealizationItems.Clear();

            Task.Factory.StartNew(() =>
            {
                try
                {

                    //StoreDbContext ctx = new StoreDbContext(
                    //    new Uri(uri
                    //        , UriKind.Absolute));

                    int index = 1;

                    var client = new StoreapptestClient();
                    //var realizationPdDb =
                        //ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.OrderByDescending(s => s.Id)).FirstOrDefault();

                    var lastNum = client.GetLastSaleDocumentsPerDaysNumber();
                    if (!string.IsNullOrEmpty(lastNum))
                    {
                        int lastNumber = 0;
                        if (int.TryParse(lastNum, out lastNumber))
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

                    var now = DateTimeHelper.GetNowKz();
                    var strt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    var endd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

                    var dischargesSource =
                        //ctx.ExecuteSyncronous(
                        //    ctx.DebtDischargeDocuments.Expand("Debtor")
                        //        .Where(w => w.DischargeDate >= strt && w.DischargeDate <= endd && w.IsDischarge && w.Creator_Id == App.CurrentUser.UserName)).ToList();
                        client.GetUserDebtDischargeDocumentsByDate(strt, endd, App.CurrentUser.UserName).ToList();

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


                    ////StoreDbContext refundCtx = new StoreDbContext(
                    ////    new Uri(uri
                    ////        , UriKind.Absolute));


                    //var closedRefundPerDay = refundCtx.ExecuteSyncronous(
                    //    refundCtx.RefundsPerDayItems.Expand("SaleDocumentsPerDay,RefundItem")
                    //        .Where(
                    //            w =>
                    //                w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                    //                && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                    //                && w.SaleDocumentsPerDay.IsClosed &&
                    //                w.SaleDocumentsPerDay.Creator_Id == App
                    //                    .CurrentUser.UserName)).ToList().Select(s => s.RefundItem.Id).ToList();


                    //var refundQuery = refundCtx.ExecuteSyncronous(
                    //    refundCtx.RefundItems.Expand("RefundDocument/SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices,SaleItem")
                    //        .Where(
                    //            w =>
                    //                w.RefundDocument.RefundDate >= startDate
                    //                && w.RefundDocument.RefundDate <= endDate
                    //                //&& !closedRefundPerDay.Contains(w.Id)
                    //                && w.RefundDocument.Creator_Id == App
                    //                    .CurrentUser.UserName)).ToList();

                    //_refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();

                    //var closedSalesPerDay = ctx.ExecuteSyncronous(
                    //    ctx.SalesPerDayItems.Expand("SaleDocumentsPerDay,SaleItem")
                    //        .Where(
                    //            w =>
                    //                w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                    //                && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                    //                && w.SaleDocumentsPerDay.IsClosed &&
                    //                w.SaleDocumentsPerDay.Creator_Id == App
                    //                    .CurrentUser.UserName)).ToList().Select(s => s.SaleItem.Id).ToList();


                    //var salesQuery = ctx.ExecuteSyncronous(
                    //    ctx.SaleItems.Expand("SaleDocument,PriceItem/PriceLists,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices")
                    //        .Where(
                    //            w =>
                    //                w.SaleDocument.SaleDate >= startDate
                    //                && w.SaleDocument.SaleDate <= endDate
                    //                && !w.SaleDocument.IsOrder
                    //                //&& !closedSalesPerDay.Contains(w.Id)
                    //                && w.SaleDocument.Creator_Id == App
                    //                    .CurrentUser.UserName)).ToList();

                    //var salesItems = salesQuery.Where(w => !closedSalesPerDay.Contains(w.Id)).ToList();

                    //salesItems.ForEach(s =>
                    //{
                        
                    //    //string debtor = "";
                    //    //int debtDisc = 0;

                    //    //var findedDebtor =
                    //    //    discharges.Where(w => w.Debtor_Id == s.SaleDocument.Customer_Name).FirstOrDefault();
                    //    //if (findedDebtor != null)
                    //    //{
                    //    //    debtor = findedDebtor.Debtor_Id;
                    //    //    debtDisc = (int)findedDebtor.Amount;
                    //    //}
                    //    var remainders = 0;
                    //    var rems =
                    //        s.PriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                    //            .FirstOrDefault();
                    //    if (rems != null)
                    //        remainders = (int)rems.Amount;

                    //    var amouuntWithoutDebt = 0;
                    //    var amountWholesalePriceWithoutDebt = 0;

                    //    if (!s.SaleDocument.IsInDebt)
                    //    {
                    //        var price =
                    //            s.PriceItem.Prices.Where(p => p.PriceDate <= DateTimeHelper.GetNowKz())
                    //                .OrderByDescending(o => o.PriceDate)
                    //                .FirstOrDefault();

                    //        amouuntWithoutDebt = (int) ((s.Price*s.Count) - s.Discount);
                    //        if (price != null)
                    //        {
                    //            amountWholesalePriceWithoutDebt =
                    //                (int)((price.Price * s.Count));
                    //        }
                    //    }
                    //    var itemRefunds =
                    //        _refundItems.Where(wh => wh.SaleItem_Id == s.Id && wh.RefundDocument.SaleDocument.IsInDebt == true)
                    //            .Sum(sum => sum.Count);

                    //    var count = s.Count - itemRefunds;
                    //    if (count != 0)
                    //    {
                    //        realization.Add(new RealizationItem()
                    //        {

                    //            CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                    //            IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
                    //            Name = s.PriceItem.Gear.Name,
                    //            Price = (int) s.Price,
                    //            Remainders = remainders,
                    //            SoldCount = (int)count,
                    //            Uom = s.PriceItem.UnitOfMeasure.Name,
                    //            WholePrice = (int) s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                    //            //Amount = (int)((s.Price * count) - s.Discount),
                    //            Amount = (int)((s.Price - (s.Discount / s.Count)) * count),
                    //            AmountWithoutDebt = amouuntWithoutDebt,
                    //            AmountWithoutDebtProfit = amouuntWithoutDebt - amountWholesalePriceWithoutDebt,
                    //            Discount = (int) s.Discount,
                    //            SaleItemData = s,
                    //            Customer = s.SaleDocument.IsInDebt ? s.SaleDocument.Customer_Name : "",
                    //            //DebtDischarge = s.SaleDocument.IsInDebt ? debtDisc : 0,
                    //            IsInDebt = s.SaleDocument.IsInDebt,
                    //            PriceListName = s.PriceItem.PriceLists.First().Name,
                    //            SaledDate = s.SaleDocument.SaleDate.ToString("T"),
                    //            SaledCount = (int)s.Count,

                    //        });
                    //    }
                    //});

                    //var query = from r in realization
                    //            group r by new
                    //            {
                    //                CatalogNumber = r.CatalogNumber,
                    //                IsDuplicate = r.IsDuplicate,
                    //                Name = r.Name,
                    //                Price = r.Price,
                    //                //Remainders = r.Remainders,
                    //                //SoldCount = r.SoldCount,
                    //                Uom = r.Uom,

                    //                //Customer = r.Customer,
                    //                IsInDebt = r.IsInDebt,
                    //                PriceListName = r.PriceListName,
                    //                SaledDate = r.SaledDate
                    //                //SaleItemData = r.SaleItemData,
                    //                //Amount = r.Amount,
                    //                //Discount = r.Discount,
                    //                //WholePrice = r.WholePrice
                    //            }
                    //                into groups
                    //                select new RealizationItem()
                    //                {
                    //                    Number = index++,
                    //                    CatalogNumber = groups.Key.CatalogNumber,
                    //                    IsDuplicate = groups.Key.IsDuplicate,
                    //                    Name = groups.Key.Name,
                    //                    Price = (int)groups.Key.Price,
                    //                    Remainders = (int)groups.Average(s => s.Remainders),
                    //                    SoldCount = groups.Sum(s => s.SoldCount),
                    //                    Uom = groups.Key.Uom,
                    //                    Amount = groups.Sum(s => s.Amount),
                    //                    AmountWithoutDebt = groups.Sum(s => s.AmountWithoutDebt),
                    //                    AmountWithoutDebtProfit = groups.Sum(s => s.AmountWithoutDebtProfit),
                    //                    Discount = groups.Sum(s => s.Discount),
                    //                    WholePrice = (int)groups.Average(a => a.WholePrice),
                    //                    //Customer = groups.Key.Customer,
                    //                    DebtDischarge = (int)groups.Average(s => s.DebtDischarge),
                    //                    IsInDebt = groups.Key.IsInDebt,
                    //                    PriceListName = groups.Key.PriceListName,
                    //                    SaledDate = groups.Key.SaledDate,
                    //                    SaledCount = groups.Sum(s => s.SaledCount)
                    //                    //SaleItemData = groups.Key.SaleItemData
                    //                };
                    

                    var refndItems = client.GetTodayRefundItems(App.CurrentUser.UserName).ToList();
                    _refundItems = refndItems;

                    var query = client.GetTodayRealizationItems(App.CurrentUser.UserName, App.CurrentUser.Warehouse_Id).ToList();

                    realization = client.GetTodayRealizationItemsRaw(App.CurrentUser.UserName,
                        App.CurrentUser.Warehouse_Id).ToList();

                    foreach (var realizationItem in query)
                    {
                        if (realizationItem.IsInDebt)
                            realizationItem.Additional = "Продан в долг";
                        var item = realizationItem;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RealizationItems.Add(item);
                        });
                    }

                    var realizationsByPriceList = (from q in realization
                                                   group q by q.PriceListName
                                                       into grp
                                                       select grp).ToList();

                    var refundsByPriceList = (from r in refndItems
                                              group r by r.PriceItem.PriceLists.FirstOrDefault().Name
                                                  into grp
                                                  select grp).ToList();

                    foreach (var itm in realizationsByPriceList)
                    {
                        int refndVal = 0;
                        int refndValWh = 0;

                        var refnd = refundsByPriceList.Where(r => r.Key == itm.Key).FirstOrDefault();
                        if (refnd != null)
                        {
                            refndVal = (int)refnd.Sum(s => s.Amount);
                            refndValWh = (int)
                                refnd.Sum(
                                    s =>
                                        s.Count *
                                        (s.PriceItem.Prices.Where(p => p.PriceDate <= s.RefundDocument.RefundDate)
                                            .OrderByDescending(o => o.PriceDate)
                                            .FirstOrDefault() == null
                                            ? 0
                                            : s.PriceItem.Prices.Where(p => p.PriceDate <= s.RefundDocument.RefundDate)
                                                .OrderByDescending(o => o.PriceDate)
                                                .FirstOrDefault().Price)
                                    );
                        }

                        var realizationVal = (int)
                            itm.Where(r => !r.SaleItemData.SaleDocument.IsInDebt).Sum(s => s.Amount);

                        var realizationValWh =
                            (int)
                                itm.Where(r => !r.SaleItemData.SaleDocument.IsInDebt).Sum(
                                    s =>
                                        s.SaledCount * s.WholePrice
                                    );

                        var frstItem = itm.FirstOrDefault();
                        if (frstItem != null)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                //var findedRealizationItem =
                                //    RealizationItems.Where(w => w.SaleItemData.Id == frstItem.SaleItemData.Id)
                                //        .FirstOrDefault();
                                var findedRealizationItem =
                                    RealizationItems.Where(i => i.PriceListName == frstItem.PriceListName)
                                        .FirstOrDefault();

                                findedRealizationItem.AmountWithoutDebt = realizationVal - refndVal;
                                findedRealizationItem.AmountWithoutDebtProfit = findedRealizationItem.AmountWithoutDebt - (realizationValWh - refndValWh);
                            });
                        }
                    }



                    foreach (var debtDischargeDocument in discharges)
                    {
                        var document = debtDischargeDocument;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RealizationItems.Add(new RealizationItem()
                            {
                                Number = index++,
                                Name = "Погашение " + document.Debtor_Id,
                                Price = (int)document.Amount,
                                IsInDebt = true
                            });
                        });
                    }
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
                    OnPropertyChanged("TotalByPrice");

                    _view.ReceiptGridControl.RefreshData();
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
                Saving = true;
                //string uri = string.Concat(
                //    Application.Current.Host.Source.Scheme, "://",
                //    Application.Current.Host.Source.Host, ":",
                //    Application.Current.Host.Source.Port,
                //    "/StoreAppDataService.svc/");

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var client = new StoreapptestClient();
                        //StoreDbContext ctx = new StoreDbContext(
                        //    new Uri(uri
                        //        , UriKind.Absolute));

                        //var realizationPdDb =
                        //   ctx.ExecuteSyncronous(ctx.SaleDocumentsPerDays.OrderByDescending(s => s.Id)).FirstOrDefault();
                        var lastNum = client.GetLastSaleDocumentsPerDaysNumber();
                        if (!string.IsNullOrWhiteSpace(lastNum))
                        {
                            int lastNumber = 0;
                            if (int.TryParse(lastNum, out lastNumber))
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
                        salePerDay.TotalAmount = TotalAmount;
                        salePerDay.TotalRefund = TotalRefund;
                        salePerDay.SubTotal = SubTotal;
                        salePerDay.TotalAmountProfit = TotalAmountProfit;
                        salePerDay.TotalRefundProfit = TotalRefundProfit;
                        salePerDay.SubTotalProfit = SubTotalProfit;

                        //ctx.AddToSaleDocumentsPerDays(salePerDay);
                        //ctx.SaveChangesSynchronous();


                        foreach (var receiptItem in realization)
                        {
                            var item = new SalesPerDayItem()
                            {
                                SaleDocumentsPerDay_Id = salePerDay.Id,
                                SaleItem_Id = receiptItem.SaleItemData.Id

                            };

                            item.Amount = receiptItem.Amount;
                            item.CatalogNumber = receiptItem.CatalogNumber;
                            item.Count = receiptItem.SoldCount;
                            item.Discount = receiptItem.Discount;
                            item.IsDuplicate = receiptItem.IsDuplicate == "*" ? true : false;
                            item.Name = receiptItem.Name;
                            item.Price = receiptItem.Price;
                            item.UnitOfMeasure = receiptItem.Uom;
                            item.Remainders = receiptItem.Remainders;

                            salePerDay.SalesPerDayItems.Add(item);
                            //ctx.AddToSalesPerDayItems(item);
                        }

                        //foreach (var receiptItem in realization)
                        //{
                        //    var item = new SalesPerDayItem()
                        //    {
                        //        SaleDocumentsPerDay_Id = salePerDay.Id,
                        //        SaleItem_Id = receiptItem.SaleItemData.Id

                        //    };
                        //    item.Amount = receiptItem.Amount;
                        //    item.CatalogNumber = receiptItem.CatalogNumber;
                        //    item.Count = receiptItem.SoldCount;
                        //    item.Discount = receiptItem.Discount;
                        //    item.IsDuplicate = receiptItem.IsDuplicate == "*" ? true : false;
                        //    item.Name = receiptItem.Name;
                        //    item.Price = receiptItem.Price;
                        //    item.UnitOfMeasure = receiptItem.Uom;
                            

                        //    salePerDay.SalesPerDayItems.Add(item);
                        //    //ctx.AddToSalesPerDayItems(item);
                        //}


                        //foreach (var refundItem in _refundItems)
                        //{
                        //    var item = new RefundsPerDayItem()
                        //    {
                        //        SaleDocumentsPerDay_Id = salePerDay.Id,
                        //        RefundItem_Id = refundItem.Id

                        //    };

                        //    ctx.AddToRefundsPerDayItems(item);
                        //}

                        //ctx.SaveChangesSynchronous();
                        var realizationId = client.AddRealizationPerDay(salePerDay);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //_closeViewNeedEvent.Publish(ViewId);
                            SavedDocumentId = realizationId;
                            Barcode = salePerDay.Barcode;
                            Saved = true;
                        });
                    }
                    catch (Exception e)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Saving = false;
                            MessageBox.Show(e.Message);
                        });
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
