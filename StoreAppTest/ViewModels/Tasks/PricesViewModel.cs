
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using StoreAppDataService;
    using Utilities;
    using Views;
    using PriceItem = Model.PriceItem;
    using Refund = Model.Refund;
    using RefundItem = Model.RefundItem;

    public class PricesViewModel : ViewModelBase
    {
        private IEventAggregator _agregator;
        private NewRealizationNeedEvent _newRealizationEvent;
        private NewReceiptNeedEvent _newReceiptEvent;
        private NewRefundNeedEvent _newRefundNeedEvent;
        private ShowDebtorsEvent _showDebtorsEvent;

        private string _priceListName;

        public PricesViewModel(string priceListName)
        {
            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _newRealizationEvent = _agregator.GetEvent<NewRealizationNeedEvent>();
            _newReceiptEvent = _agregator.GetEvent<NewReceiptNeedEvent>();
            _newRefundNeedEvent = _agregator.GetEvent<NewRefundNeedEvent>();
            _showDebtorsEvent = _agregator.GetEvent<ShowDebtorsEvent>();
            _agregator.GetEvent<ChangeRemaindersEvent>().Subscribe(ChangeRemaindersEventHandler);

            _priceListName = priceListName;
            
            CustomerList = new ObservableCollection<Customer>();
            PriceItems = new ObservableCollection<PriceItem>();

            InitCommands();
        }

        public ObservableCollection<PriceItem> PriceItems { get; set; }

        public ObservableCollection<Customer> CustomerList { get; set; }


        private IDictionary<int, decimal> _oldPrices = new Dictionary<int, decimal>(); 

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private bool _InDebt;
        public bool InDebt
        {
            get { return _InDebt; }
            set
            {
                _InDebt = value;
                OnPropertyChanged("InDebt");
            }
        }

        private bool _IsOrder;
        public bool IsOrder
        {
            get { return _IsOrder; }
            set
            {
                _IsOrder = value;
                OnPropertyChanged("IsOrder");
            }
        }

        private bool _IsInvoice;
        public bool IsInvoice
        {
            get { return _IsInvoice; }
            set
            {
                _IsInvoice = value;
                OnPropertyChanged("IsInvoice");
            }
        }

        private Customer _SelectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }


        private bool _IsECommerceCustomer;

        public bool IsECommerceCustomer
        {
            get { return _IsECommerceCustomer; }
            set
            {
                _IsECommerceCustomer = value;
                if (value)
                {
                    if (_oldPrices.Count == 0)
                    {
                        foreach (var priceItem in PriceItems.Where(p => p.SoldCount > 0))
                        {
                            _oldPrices.Add(priceItem.Number, priceItem.WholesalePrice);
                            priceItem.WholesalePrice = priceItem.TwonyPercent;
                        }
                    }
                    else
                    {
                        foreach (var priceItem in PriceItems.Where(p => p.SoldCount > 0))
                        {
                            priceItem.WholesalePrice = priceItem.TwonyPercent;
                        }
                    }

                    SelectedCustomer = CustomerList.FirstOrDefault(f => f.Name == "Клиент интернет-магазина");
                }
                else
                {
                    foreach (var priceItem in PriceItems)//.Where(p => p.SoldCount > 0))
                    {
                        if (_oldPrices.ContainsKey(priceItem.Number))
                        {
                            priceItem.WholesalePrice = (int) _oldPrices[priceItem.Number];
                            //priceItem.WholesalePrice = (int) (priceItem.WholesalePrice - (priceItem.WholesalePrice*0.2));}
                            SelectedCustomer = CustomerList.FirstOrDefault(f => f.Name == "Розничный покупатель");
                        }
                    }
                    OnPropertyChanged("IsECommerceCustomer");
                }
            }
        }

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            IsLoading = true;

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");



            Task.Factory.StartNew(() =>
            {
                var prices = new List<PriceItem>();

                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));

                //var pricesDb = ctx.ExecuteSyncronous(ctx.PriceItems.Expand("Remainders/Warehouse, Gear, UnitOfMeasure").Where(p => p.PriceList_Id == _priceListName)).ToList();
                var qr = ctx.ExecuteSyncronous<PriceItemRemainderView>(
                    new Uri(string.Format("{0}GetLightPriceItemList?priceListName='{1}'&warehouse='{2}'", uri, _priceListName, App.CurrentUser.Warehouse.Name),
                        UriKind.Absolute));

                int number = 1;

                foreach (var priceItemRemainderView in qr)
                {
                    var item = new PriceItem()
                    {
                        Articul = priceItemRemainderView.Articul,
                        BuyPriceRur = (int) priceItemRemainderView.BuyPriceRur,
                        BuyPriceTng = (int) priceItemRemainderView.BuyPriceTng,
                        CatalogNumber = priceItemRemainderView.CatalogNumber,
                        IsDuplicate = priceItemRemainderView.IsDuplicate ? "*" : "",
                        Name = priceItemRemainderView.Gear_Name,
                        Number = number++,
                        Remainders = (int) priceItemRemainderView.Remainders,
                        SoldCount = 0,
                        Uom = priceItemRemainderView.Uom,
                        WholesalePrice = (int) priceItemRemainderView.WholesalePrice,
                        TwonyPercent = (int) (priceItemRemainderView.WholesalePrice * 1.2m),
                        PriceItemData = new StoreAppDataService.PriceItem() {Id = priceItemRemainderView.PriceItem_Id}

                    };
                
                    prices.Add(item);
                    item.CountChanges +=item_CountChanges;
                }
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    PriceItems = new ObservableCollection<PriceItem>(prices);
                    OnPropertyChanged("PriceItems");

                });
            }).ContinueWith(c =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsLoading = false;
                });
            });

            UpdateCustomersList();

        }

        private void item_CountChanges(object sender, CountChangesEventArgs e)
        {
            var priceItem = PriceItems.Where(p => p.PriceItemData.Id == e.PriceItem_Id).FirstOrDefault();
            if (priceItem.WholesalePrice != priceItem.TwonyPercent && IsECommerceCustomer)
            {
                _oldPrices.Add(priceItem.Number, priceItem.WholesalePrice);
                priceItem.WholesalePrice = priceItem.TwonyPercent;
            }
        }

        #endregion

        #region Commands

        public ICommand OpenNewRealizationCommand { get; set; }
        public ICommand OpenNewReceiptCommand { get; set; }
        public ICommand OpenNewRefundCommand { get; set; }
        public ICommand AddNewCustomerCommand { get; set; }
        public ICommand ShowDebtorsCommand { get; set; }

        private void InitCommands()
        {
            OpenNewRealizationCommand = new UICommand(o =>
            {
                _newRealizationEvent.Publish(null);

            });

            OpenNewReceiptCommand = new UICommand(o =>
            {
                var selectedPriceItems = PriceItems.Where(p => p.SoldCount > 0).ToList();
                var realization = new Model.Receipt();
                realization.Customer = SelectedCustomer;
                realization.IsInDebt = InDebt;
                realization.IsOrder = IsOrder;
                realization.IsInvoice = IsInvoice;
                realization.ReceiptDate = DateTimeHelper.GetNowKz();
                realization.PriceListName = _priceListName;

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));

                var realizationDb =
                    ctx.ExecuteSyncronous(ctx.SaleDocuments.OrderByDescending(s => s.Id)).FirstOrDefault();
                if (realizationDb != null)
                {
                    int lastNumber = 0;
                    if (int.TryParse(realizationDb.Number, out lastNumber))
                    {
                        realization.ReceiptNumber = (++lastNumber).ToString();
                    }
                }
                else
                {
                    realization.ReceiptNumber = "1";
                }



                int index = 1;
                selectedPriceItems.ForEach(i =>
                {
                    int price = i.WholesalePrice;


                    realization.ReceiptItems.Add(new ReceiptItem()
                    {
                        Number = index++,
                        Articul = i.Articul,
                        CatalogNumber = i.CatalogNumber,
                        IsDuplicate = i.IsDuplicate,
                        Name = i.Name,
                        Price = price,
                        SoldCount = i.SoldCount,
                        Uom = i.Uom,
                        PriceItemData = i.PriceItemData,
                        PriceItem_Id = i.PriceItemData.Id
                    });
                });
                _newReceiptEvent.Publish(realization);

                PriceItems.ForEach(i =>
                {
                    i.SoldCount = 0;
                });
            });


            OpenNewRefundCommand = new UICommand(o =>
            {
                var selectedPriceItems = PriceItems.Where(p => p.SoldCount > 0).ToList();


                string uri = string.Concat(
                    Application.Current.Host.Source.Scheme, "://",
                    Application.Current.Host.Source.Host, ":",
                    Application.Current.Host.Source.Port,
                    "/StoreAppDataService.svc/");

                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));

                Refund refund = new Refund();

                var refundDb =
                    ctx.ExecuteSyncronous(ctx.RefundDocuments.OrderByDescending(s => s.Id)).FirstOrDefault();
                if (refundDb != null)
                {
                    int lastNumber = 0;
                    if (int.TryParse(refundDb.RefundNumber, out lastNumber))
                    {
                        refund.RefundNumber = (++lastNumber).ToString();
                    }
                }
                else
                {
                    refund.RefundNumber = "1";
                }

                refund.RefundDate = DateTimeHelper.GetNowKz();

                int index = 1;
                selectedPriceItems.ForEach(i =>
                {
                    refund.RefundItems.Add(new RefundItem()
                    {
                        Number = index++,
                        Articul = i.Articul,
                        CatalogNumber = i.CatalogNumber,
                        IsDuplicate = i.IsDuplicate,
                        Name = i.Name,
                        RetailPrice = i.WholesalePrice,
                        WhosalePrice = i.WholesalePrice,
                        SoldCount = i.SoldCount,
                        Uom = i.Uom,
                        PriceItemData = i.PriceItemData
                        //Discount = i
                    });
                });
                _newRefundNeedEvent.Publish(refund);

                PriceItems.ForEach(i =>
                {
                    i.SoldCount = 0;
                });

            });

            AddNewCustomerCommand = new UICommand(o =>
            {
                ChildWindow p = new ChildWindow();
                Customer c = new Customer();
                CustomerDetails dt = new CustomerDetails();
                dt.DataContext = c;
                p.Content = dt;

                dt.OkButtonClicked += (sender, args) =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        p.Close();
                        UpdateCustomersList();
                    });

                };
                dt.CancelButtonClicked += (sender, args) => { DispatcherHelper.CheckBeginInvokeOnUI(() => p.Close()); };

                p.Show();
            });

            ShowDebtorsCommand = new UICommand(o =>
            {
                _showDebtorsEvent.Publish(null);
            });
        }
        #endregion


        private void UpdateCustomersList()
        {

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            Task.Factory.StartNew(() =>
            {
                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));

                var customersDb =
                    ctx.ExecuteSyncronous(ctx.Customers.Where(
                            c =>
                                c.Creator_Id == App.CurrentUser.UserName ||
                                (c.Name == "Розничный покупатель" || c.Name == "Клиент интернет-магазина"))).ToList();

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        CustomerList.Clear();

                        customersDb.ForEach(i =>
                        {
                            CustomerList.Add(i);
                        });

                        SelectedCustomer = CustomerList.FirstOrDefault(f => f.Name == "Розничный покупатель");

                    });
            });


        }

        public void ChangeRemaindersEventHandler(IList<PriceItem> priceItems)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (IsECommerceCustomer == true)
                    IsECommerceCustomer = false;
                if (IsInvoice == true)
                    IsInvoice = false;
                if (IsOrder == true)
                    IsOrder = false;
                if (InDebt == true)
                    InDebt = false;

                SelectedCustomer = CustomerList.FirstOrDefault(f => f.Name == "Розничный покупатель");
            });


            foreach (var priceItem in priceItems)
            {
                var priceItemModel =
                    PriceItems.Where(p => p.PriceItemData.Id == priceItem.PriceItemData.Id).FirstOrDefault();

                if (priceItemModel != null)
                {
                    var findedRemainder =
                        priceItem.PriceItemData.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                            .FirstOrDefault();
                    if (findedRemainder != null)
                    {
                        priceItemModel.Remainders = (int)findedRemainder.Amount;
                    }
                }
                //PriceItems.Where(p => p.PriceItemData.Id == priceItem.PriceItemData.Id).FirstOrDefault().Remainders =
                //    (int)priceItem.PriceItemData.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                //        .FirstOrDefault()
                //        .Amount;
            }
        }
        
    }
}
