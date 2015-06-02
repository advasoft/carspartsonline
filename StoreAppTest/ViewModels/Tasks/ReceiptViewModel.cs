
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
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
    using Views;
    using IncomeItem = Model.IncomeItem;
    using PriceItem = Model.PriceItem;
    using Receipt = Model.Receipt;

    public class ReceiptViewModel : ViewModelBase
    {
        public Guid ViewId { get; private set; }

        private IEventAggregator _agregator;
        private CloseViewNeedEvent _closeViewNeedEvent;
        private ChangeRemaindersEvent _changeRemaindersEvent;

        private Receipt _durtyReceipt;

        private string _priceListName;

        public ReceiptViewModel(Model.Receipt durtyReceipt, Guid viewId, string priceListName)
        {
            _durtyReceipt = durtyReceipt;
            ReceiptNumber = durtyReceipt.ReceiptNumber;
            ViewId = viewId;

            _priceListName = priceListName;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();
            _changeRemaindersEvent = _agregator.GetEvent<ChangeRemaindersEvent>();

            ReceiptItems = new ObservableCollection<ReceiptItem>(durtyReceipt.ReceiptItems);
            ReceiptItems.ForEach(i =>
            {
                i.ReceiptItemChanged += i_ReceiptItemChanged;    
            });

            CustomerList = new ObservableCollection<Customer>();

            InitCommands();
        }

        void i_ReceiptItemChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged("TotalAmount");
            OnPropertyChanged("TotalDiscount");
            OnPropertyChanged("SubTotal");
        }

        public bool IsOrder
        {
            get { return _durtyReceipt.IsOrder; }
        }

        public bool InDebt
        {
            get { return _durtyReceipt.IsInDebt; }
        }

        public bool IsInvoice
        {
            get { return _durtyReceipt.IsInvoice; }
        }

        public Customer Customer
        {
            get
            {
                return _durtyReceipt.Customer;
            }
            set
            {
                _durtyReceipt.Customer = value;
                OnPropertyChanged("Customer");
            }
        }

        public string ReceiptNumber
        {
            get
            {
                return _receiptNumber;
            }
            set
            {
                _receiptNumber = value;
                OnPropertyChanged("ReceiptNumber");
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return ReceiptItems.Sum(s => s.Price * s.SoldCount);
            }
        }
        public decimal TotalDiscount
        {
            get
            {
                return ReceiptItems.Sum(s => s.Discount); ;
            }
        }
        public decimal SubTotal
        {
            get
            {
                return TotalAmount - TotalDiscount;
            }
        }

        public double AppliedDiscount
        {
            get
            {
                return _appliedDiscount;
            }
            set
            {
                _appliedDiscount = value;
                OnPropertyChanged("AppliedDiscount");
            }
        }

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

        private bool _SelectAll;
        public bool SelectAll
        {
            get { return _SelectAll; }
            set
            {
                _SelectAll = value;
                OnPropertyChanged("SelectAll");
                if (value)
                {
                    ReceiptItems.ForEach(i =>
                    {
                        i.Selected = true;
                    });   
                }
                else
                {
                    ReceiptItems.ForEach(i =>
                    {
                        i.Selected = false;
                    }); 
                }
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



        private string _receiptNumber;
        private double _appliedDiscount;



        public ReceiptItem SelectedReceiptItem
        {
            get { return _SelectedReceiptItem; }
            set
            {
                _SelectedReceiptItem = value;
                OnPropertyChanged("SelectedReceiptItem");
            }
        }
        private ReceiptItem _SelectedReceiptItem;



        public ObservableCollection<ReceiptItem> ReceiptItems { get; set; }

        public ObservableCollection<Customer> CustomerList { get; set; }

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            UpdateCustomersList();
        }

        #endregion

        #region Commands

        public ICommand SaveReceiptCommand { get; set; }
        public ICommand ApplyDiscountCommand { get; set; }
        public ICommand AddNewCustomerCommand { get; set; }

        public ICommand AddPriceItemCommand { get; set; }
        public ICommand RemovePriceItemCommand { get; set; }

        public ICommand PrintReportCommand { get; set; }

        private void InitCommands()
        {
            #region SaveReceiptCommand
            SaveReceiptCommand = new UICommand(o =>
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

                        //var realizationDb =
                        //ctx.ExecuteSyncronous(ctx.SaleDocuments.OrderByDescending(s => s.Number)).FirstOrDefault();
                        //if (realizationDb != null)
                        //{
                        //    int lastNumber = 0;
                        //    if (int.TryParse(realizationDb.Number, out lastNumber))
                        //    {
                        //        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        //        {
                        //            ReceiptNumber = (++lastNumber).ToString();
                        //        });
                        //    }
                        //}

                        if (!ReceiptItems.All(i => i.SoldCount != 0))
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                MessageChildWindow msch = new MessageChildWindow();
                                msch.Message =
                                    string.Format(
                                        "Не у всех позиций указано количество");
                                msch.Show();
                            });           
                            return;
                        }

                        ReceiptItems.ForEach(i =>
                        {
                            i.ReceiptItemChanged -= i_ReceiptItemChanged;
                        });
                        var receipt = new SaleDocument();
                        //receipt.Creator = App.CurrentUser;
                        receipt.Creator_Id = App.CurrentUser.UserName;
                        receipt.Customer = Customer;
                        receipt.Customer_Name = Customer.Name;
                        receipt.IsInDebt = InDebt;
                        receipt.IsInvoice = IsInvoice;
                        receipt.IsOrder = IsOrder;
                        //receipt.LastChanger = App.CurrentUser;
                        receipt.LastChanger_Id = App.CurrentUser.UserName;
                        receipt.Number = ReceiptNumber;
                        receipt.SaleDate = DateTimeHelper.GetNowKz();
                        receipt.Barcode = GetBarcode();

                        ctx.AddToSaleDocuments(receipt);
                        ctx.SaveChangesSynchronous();

                        foreach (var receiptItem in ReceiptItems)
                        {
                            var item = new SaleItem()
                            {
                                Amount = receiptItem.Amount,
                                Count = receiptItem.SoldCount,
                                Discount = receiptItem.Discount,
                                Price = receiptItem.Price,
                                PriceItem_Id = receiptItem.PriceItem_Id,
                                //PriceItem = receiptItem.PriceItemData,
                                SaleDocument = receipt,
                                SaleDocument_Id = receipt.Id

                            };
                            var rem =
                                ctx.ExecuteSyncronous(ctx.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id && w.PriceItem_Id == item.PriceItem_Id))
                                    .FirstOrDefault();

                            if (!receipt.IsOrder)
                            {
                                if (rem == null)
                                {
                                    rem = new Remainder();
                                    rem.PriceItem_Id = receiptItem.PriceItem_Id;
                                    rem.RemainderDate = DateTimeHelper.GetNowKz();
                                    rem.Warehouse_Id = App.CurrentUser.Warehouse_Id;
                                    rem.Amount -= item.Count;
                                    ctx.AddToRemainders(rem);
                                }
                                else
                                {
                                    //var rem =
                                    //    item.PriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                                    //        .FirstOrDefault();
                                    rem.Amount -= item.Count;
                                    rem.RemainderDate = DateTimeHelper.GetNowKz();
                                    ctx.ChangeState(rem, EntityStates.Modified);
                                }
                                //ctx.AttachTo("Remainders", rem);
                                
                            }
                            if (receiptItem.PriceItemData == null)
                            {
                                receiptItem.PriceItemData = new StoreAppDataService.PriceItem()
                                {
                                    Id = receiptItem.PriceItem_Id
                                };
                            }
                            receiptItem.PriceItemData.Remainders.Add(rem);

                            receipt.SaleItems.Add(item);
                            ctx.AddToSaleItems(item);

                            if (rem.PriceItem == null)
                            {
                                rem.PriceItem = receiptItem.PriceItemData;
                            }
                        }

                        ctx.SaveChangesSynchronous();


                        var receiptItems = new List<PriceItem>();
                        foreach (var receiptItem in ReceiptItems)
                        {
                            var rems = receiptItem.PriceItemData.Remainders.FirstOrDefault();
                            receiptItems.Add(new PriceItem()
                            {
                                PriceItemData = rems.PriceItem,
                                Remainders = (int)(rems == null ? 0 : rems.Amount)
                            });
                        }

                        _changeRemaindersEvent.Publish(receiptItems);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //_closeViewNeedEvent.Publish(ViewId);
                            SavedDocumentId = receipt.Id;
                            Barcode = receipt.Barcode;
                            Saved = true;

                        });
                    }
                    catch (Exception e)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            MessageChildWindow msch = new MessageChildWindow();
                            msch.Message = e.Message;
                            msch.Show();
                        });           
                    }


                }).ContinueWith(c =>
                {
                    
                });
            });
            #endregion

            #region ApplyDiscountCommand
            ApplyDiscountCommand = new UICommand(o =>
            {
                ReceiptItems.Where(r => r.Selected).ForEach(i =>
                {
                    i.Discount = (int)((i.Price * i.SoldCount)* AppliedDiscount/100);
                });
            });
            #endregion

            #region AddNewCustomerCommand
            AddNewCustomerCommand = new UICommand(o =>
            {
                ChildWindow p = new ChildWindow();
                Customer c = new Customer();
                CustomerDetails dt = new CustomerDetails();
                dt.DataContext = c;
                p.Content = dt;

                p.Width = 400;
                p.Height = 200;

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
            #endregion

            #region AddPriceItemCommand

            AddPriceItemCommand = new UICommand(o =>
            {
                SelectPriceControl ctrl = new SelectPriceControl();
                //ctrl.PriceListComboBoxEdit.IsEnabled = false;
                SelectPriceControlViewModel vm = new SelectPriceControlViewModel();
                vm.PriceListName = _priceListName;
                vm.Warehouse = App.CurrentUser.Warehouse_Id;

                ctrl.DataContext = vm;

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        if (ctrl.IsMultiselect)
                        {
                            
                            foreach (var source in vm.PriceItems.Where(p => p.Selected))
                            {
                                var model = new ReceiptItem()
                                {
                                    Number = ReceiptItems.Select(s => s.Number).LastOrDefault() + 1,
                                    Articul = source.Articul,
                                    CatalogNumber = source.CatalogNumber,
                                    IsDuplicate = source.IsDuplicate ? "*" : "",
                                    Name = source.Gear_Name,
                                    Price = (int)source.WholesalePrice,
                                    SoldCount = 0,
                                    Uom = source.Uom,
                                    PriceItem_Id = source.PriceItem_Id,
                                    //PriceItemData = source.PriceItemData,

                                };
                                ReceiptItems.Add(model);
                                model.ReceiptItemChanged += i_ReceiptItemChanged; 
                            }
                            SelectedReceiptItem = ReceiptItems.First();
                        }
                        else
                        {
                            var model = new ReceiptItem()
                            {
                                Number = ReceiptItems.Select(s => s.Number).LastOrDefault() + 1,
                                Articul = vm.SelectedPriceItem.Articul,
                                CatalogNumber = vm.SelectedPriceItem.CatalogNumber,
                                IsDuplicate = vm.SelectedPriceItem.IsDuplicate ? "*" : "",
                                Name = vm.SelectedPriceItem.Gear_Name,
                                Price = (int)vm.SelectedPriceItem.WholesalePrice,
                                SoldCount = 0,
                                Uom = vm.SelectedPriceItem.Uom,
                                PriceItem_Id = vm.SelectedPriceItem.PriceItem_Id
                                //PriceItemData = source.PriceItemData,

                            };

                            ReceiptItems.Add(model);
                            model.ReceiptItemChanged += i_ReceiptItemChanged; 
                            SelectedReceiptItem = model;
                        }

                        OnPropertyChanged("TotalAmount");
                        OnPropertyChanged("TotalDiscount");
                        OnPropertyChanged("SubTotal");

                    }
                };

                ctrl.Show();
                vm.LoadView();

            });

            #endregion

            #region RemovePriceItemCommand

            RemovePriceItemCommand = new UICommand(o =>
            {
                if (SelectedReceiptItem != null)
                {
                    SelectedReceiptItem.ReceiptItemChanged -= i_ReceiptItemChanged; 
                    ReceiptItems.Remove(SelectedReceiptItem);
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("TotalDiscount");
                    OnPropertyChanged("SubTotal");

                }
            });

            #endregion

            #region PrintReportCommand

            PrintReportCommand = new UICommand(a =>
            {
                SalesDocumentReportControl control = new SalesDocumentReportControl(SavedDocumentId);
                control.Show();
            });

            #endregion
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
                    ctx.ExecuteSyncronous(
                        ctx.Customers.Where(
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

                });
            });


        }


        private string GetBarcode()
        {
            string pref = "SL";
            int lenAll = 4;
            int lenNum = ReceiptNumber.Length;
            int zeroCount = lenAll - lenNum;

            StringBuilder builder = new StringBuilder(pref);
            for (int i = 0; i < zeroCount; i++)
            {
                builder.Append("0");
            }

            builder.Append(ReceiptNumber);

            return builder.ToString();
        }


    }
}
