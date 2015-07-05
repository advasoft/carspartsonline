
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Controls;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;
    using StoreAppDataService;
    using Utilities;
    using PriceItem = Model.PriceItem;
    using RefundItem = Model.RefundItem;

    public class RefundViewModel : ViewModelBase
    {
        public Guid ViewId { get; private set; }


        private IEventAggregator _agregator;
        private CloseViewNeedEvent _closeViewNeedEvent;
        private ChangeRemaindersEvent _changeRemaindersEvent;

        private Refund _durtyRefund;

        private long _SelectedSaleDocumentId;

        public RefundViewModel(Refund durtyRefund, Guid viewId)
        {
            _durtyRefund = durtyRefund;

            ViewId = viewId;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();
            _changeRemaindersEvent = _agregator.GetEvent<ChangeRemaindersEvent>();

            RefundItems = new ObservableCollection<RefundItem>(_durtyRefund.RefundItems);
            RefundItems.ForEach(i =>
            {
                i.RefundItemChanged += i_RefundItemChanged;    
            });

            //SaleDocuments = new ObservableCollection<LookupSalesDocumentModel>();

            InitCommands();
        }

        void i_RefundItemChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged("TotalRefund");
        }

        public string RefundNumber
        {
            get { return _durtyRefund.RefundNumber; }
            set
            {
                _durtyRefund.RefundNumber = value;
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

        public long SavedDocumentId { get; set; }


        private string _receiptNumber;

        public ObservableCollection<RefundItem> RefundItems { get; set; }
        //public ObservableCollection<LookupSalesDocumentModel> SaleDocuments { get; set; }

        //private LookupSalesDocumentModel _SelectedSaleDocument;

        //public LookupSalesDocumentModel SelectedSaleDocument
        //{
        //    get
        //    {
        //        return _SelectedSaleDocument;
        //    }
        //    set
        //    {
        //        _SelectedSaleDocument = value;
        //        OnPropertyChanged("SelectedSaleDocument");

        //        if (_SelectedSaleDocument != null)
        //        {
        //            FindReceiptCommand.Execute(null);
        //        }
        //    }
        //}

        public RefundItem SelectedRefundItem
        {
            get
            {
                return _SelectedRefundItem;
            }
            set
            {
                _SelectedRefundItem = value;
                OnPropertyChanged("SelectedRefundItem");
            }
        }
        private RefundItem _SelectedRefundItem;

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            //UpdateCustomersList();
        }

        #endregion

        #region Commands

        public ICommand SaveRefundCommand { get; set; }
        //public ICommand FindReceiptCommand { get; set; }

        public ICommand PrintReportCommand { get; set; }

        public ICommand RemoveRefundItemCommand { get; set; }
        public ICommand SelectSalesDocumentCommand { get; set; }

        private void InitCommands()
        {
            #region Save command
            SaveRefundCommand = new UICommand(o =>
            {
                if (RefundItems.Count == 0) return;
                
                RefundItems.ForEach(i =>
                {
                    i.RefundItemChanged -= i_RefundItemChanged;
                });



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

                        var refundDb =
                        ctx.ExecuteSyncronous(ctx.RefundDocuments.OrderByDescending(s => s.Id)).FirstOrDefault();
                        if (refundDb != null)
                        {
                            int lastNumber = 0;
                            if (int.TryParse(refundDb.RefundNumber, out lastNumber))
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    RefundNumber = (++lastNumber).ToString();
                                });
                            }
                        }




                        var refund = new RefundDocument();
                        refund.Creator_Id = App.CurrentUser.UserName;
                        refund.RefundDate = _durtyRefund.RefundDate;
                        refund.SaleDocument_Id = _SelectedSaleDocumentId;                        
                        refund.LastChanger_Id = App.CurrentUser.UserName;
                        refund.RefundNumber = RefundNumber;

                        ctx.AddToRefundDocuments(refund);
                        ctx.SaveChangesSynchronous();

                        foreach (var refundItem in RefundItems)
                        {
                            //decimal discount = 0;

                            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            //{
                            //    discount = SelectedSaleDocument.SaleDocumentData.SaleItems.FirstOrDefault(f => f.PriceItem_Id == refundItem.PriceItemData.Id).Discount;
                            //});
                            var item = new StoreAppDataService.RefundItem()
                            {
                                Count = refundItem.SoldCount,
                                Discount = refundItem.Discount,
                                Price = refundItem.RetailPrice,
                                PriceItem_Id = refundItem.PriceItemData.Id,
                                //PriceItem = refundItem.PriceItemData,
                                Amount = refundItem.Amount,
                                RefundDocument_Id = refund.Id,
                                RefundDocument = refund,
                                SaleItem_Id = refundItem.SaleItem_Id
                                
                            };

                            //var rem =
                            //    item.PriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                            //        .FirstOrDefault();

                            var rem =
                                ctx.ExecuteSyncronous(ctx.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id && w.PriceItem_Id == item.PriceItem_Id))
                                    .FirstOrDefault();

                            rem.Amount += item.Count;
                            rem.RemainderDate = DateTimeHelper.GetNowKz();

                            //tx.AttachTo("Remainders", rem);
                            ctx.ChangeState(rem, EntityStates.Modified);


                            refund.RefundItems.Add(item);
                            ctx.AddToRefundItems(item);

                            refundItem.PriceItemData.Remainders.Add(rem);
                        }

                        ctx.SaveChangesSynchronous();


                        var receiptItems = new List<PriceItem>();
                        foreach (var refundItem in RefundItems)
                        {
                            receiptItems.Add(new PriceItem()
                            {
                                PriceItemData = refundItem.PriceItemData,
                                Remainders = (int)refundItem.PriceItemData.Remainders.First().Amount
                            });
                        }

                        _changeRemaindersEvent.Publish(receiptItems);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //_closeViewNeedEvent.Publish(ViewId);
                            SavedDocumentId = refund.Id;
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

            #endregion

            #region SelectSalesDocumentCommand
            SelectSalesDocumentCommand = new UICommand(o =>
            {

                RefundItems.Clear();



                SelectSalesDocumentControl ctrl = new SelectSalesDocumentControl();

                SelectSalesDocumentControlViewModel vm = new SelectSalesDocumentControlViewModel();

                ctrl.DataContext = vm;

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        int number = 1;
                        _SelectedSaleDocumentId = vm.SelectedSaleDocument.Id;

                        foreach (var source in vm.SalesDocumentItems.Where(p => p.Selected))
                        {
                            var item = new RefundItem()
                            {
                                Articul = source.Articul,
                                CatalogNumber = source.CatalogNumber,
                                IsDuplicate = source.IsDuplicate,
                                Name = source.Gear_Name,
                                Number = number++,
                                RetailPrice = (int)source.RetailPrice,
                                SoldCount = source.SoldCount,
                                PreviousSoldCount = source.PreviousSoldCount,
                                Uom = source.Uom,
                                WhosalePrice =
                                    (int)source.WhosalePrice,
                                PriceItemData = source.PriceItemData,
                                Discount = (int)source.Discount,
                                SaleItem_Id = source.SaleItem_Id,
                                SaledCount = source.SaledCount

                            };
                            RefundItems.Add(item);
                            item.RefundItemChanged += i_RefundItemChanged;
                        }

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            OnPropertyChanged("TotalRefund");
                        });

                    }
                };

                ctrl.Show();
                vm.LoadView();



            });

            #endregion

            //#region Find receipt command
            //FindReceiptCommand = new UICommand(o =>
            //{
            //    string uri = string.Concat(
            //        Application.Current.Host.Source.Scheme, "://",
            //        Application.Current.Host.Source.Host, ":",
            //        Application.Current.Host.Source.Port,
            //        "/StoreAppDataService.svc/");

            //    RefundItems.Clear();

            //    Task.Factory.StartNew(() =>
            //    {
            //        StoreDbContext ctx = new StoreDbContext(
            //            new Uri(uri
            //                , UriKind.Absolute));

            //        var receiptDb =
            //            ctx.ExecuteSyncronous(ctx.SaleDocuments.Expand("SaleItems/PriceItem/Gear,SaleItems/PriceItem/UnitOfMeasure,SaleItems/PriceItem/Remainders,SaleItems/PriceItem/Prices")
            //            .Where(s => s.Id == SelectedSaleDocument.SaleDocumentData.Id)).FirstOrDefault();

                    
            //        if (receiptDb != null)
            //        {

            //            IList<StoreAppDataService.RefundItem> refs = default(IList<StoreAppDataService.RefundItem>);

            //            var refundsDb =
            //                ctx.ExecuteSyncronous(ctx.RefundDocuments.Expand("RefundItems").Where(d => d.SaleDocument_Id == receiptDb.Id))
            //                    .ToList();

            //            if (refundsDb != null)
            //            {
            //                refs = refundsDb.SelectMany(s => s.RefundItems).ToList();
            //            }

            //            int number = 1;
            //            receiptDb.SaleItems.ForEach(i =>
            //            {
            //                if (refs == null || !refs.Any(v => v.SaleItem_Id == i.Id) || (refs.Any(v => v.SaleItem_Id == i.Id) && refs.Where(wh => wh.SaleItem_Id == i.Id).Sum(sm => sm.Count) < i.Count))
            //                {
            //                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
            //                    {
            //                        var item = new RefundItem()
            //                        {
            //                            Articul = i.PriceItem.Gear.Articul,
            //                            CatalogNumber = i.PriceItem.Gear.CatalogNumber,
            //                            IsDuplicate = i.PriceItem.Gear.IsDuplicate ? "*" : "",
            //                            Name = i.PriceItem.Gear.Name,
            //                            Number = number++,
            //                            RetailPrice = (int) i.Price,
            //                            SoldCount = (int)i.Count - (int)refs.Where(wh => wh.SaleItem_Id == i.Id).Sum(sm => sm.Count),
            //                            PreviousSoldCount = (int) i.Count,
            //                            Uom = i.PriceItem.UnitOfMeasure.Name,
            //                            WhosalePrice =
            //                                (int) i.PriceItem.Prices.OrderByDescending(g => g.PriceDate).First().Price,
            //                            PriceItemData = i.PriceItem,
            //                            Discount = (int) i.Discount,
            //                            SaleItem_Id = i.Id

            //                        };
            //                        RefundItems.Add(item);
            //                        item.RefundItemChanged += i_RefundItemChanged;
            //                    });
            //                }
            //            });

            //            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            //            {
            //                OnPropertyChanged("TotalRefund");
            //            });
            //        }
            //    });
            //});

            //#endregion
            #region PrintReportCommand

            PrintReportCommand = new UICommand(a =>
            {
                RefundDocumentReportControl control = new RefundDocumentReportControl(SavedDocumentId);
                control.Show();
            });

            #endregion

            #region RemoveRefundItemCommand

            RemoveRefundItemCommand = new UICommand(a =>
            {
                if (SelectedRefundItem != null)
                {
                    SelectedRefundItem.RefundItemChanged -= i_RefundItemChanged;
                    RefundItems.Remove(SelectedRefundItem);
                    OnPropertyChanged("TotalRefund");
                }
            });
            #endregion
        }
        #endregion


        //private void UpdateCustomersList()
        //{

        //    string uri = string.Concat(
        //        Application.Current.Host.Source.Scheme, "://",
        //        Application.Current.Host.Source.Host, ":",
        //        Application.Current.Host.Source.Port,
        //        "/StoreAppDataService.svc/");

        //    Task.Factory.StartNew(() =>
        //    {
        //        StoreDbContext ctx = new StoreDbContext(
        //            new Uri(uri
        //                , UriKind.Absolute));

        //        var salesDb =
        //            ctx.ExecuteSyncronous(ctx.SaleDocuments.Expand("SaleItems,Customer")
        //            .Where(f => !f.IsOrder && f.Creator_Id == App.CurrentUser.UserName)
        //            .OrderByDescending(or => or.SaleDate)).ToList();

        //        DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //        {
        //            SaleDocuments.Clear();

        //            salesDb.ForEach(i =>
        //            {
        //                SaleDocuments.Add(new LookupSalesDocumentModel()
        //                {
        //                    Amount = i.SaleItems.Sum(s => s.Amount),
        //                    Customer = i.Customer.Name,
        //                    Number = i.Number,
        //                    SaleDate = i.SaleDate,
        //                    SaleDocumentData = i
        //                });
        //            });

        //        });
        //    });


        //}

    }
}
