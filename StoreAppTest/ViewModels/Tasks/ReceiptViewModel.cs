
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

            PaymentType = "Безналичный расчет";

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


        private bool _IsReserve;
        public bool IsReserve
        {
            get { return _IsReserve; }
            set
            {
                _IsReserve = value;
                OnPropertyChanged("IsReserve");
            }
        }


        private bool _IsRefund;
        public bool IsRefund
        {
            get { return _IsRefund; }
            set
            {
                _IsRefund = value;
                OnPropertyChanged("IsRefund");
            }
        }


        private string _Contract;
        public string Contract
        {
            get { return _Contract; }
            set
            {
                _Contract = value;
                OnPropertyChanged("Contract");
            }
        }

        private string _PaymentType;
        public string PaymentType
        {
            get { return _PaymentType; }
            set
            {
                _PaymentType = value;
                OnPropertyChanged("PaymentType");
            }
        }

        private string _TtnNumber;
        public string TtnNumber
        {
            get { return _TtnNumber; }
            set
            {
                _TtnNumber = value;
                OnPropertyChanged("TtnNumber");
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
                AddPositionToReceipt(value);

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
        public ICommand PrintInvoiceReportCommand { get; set; }

        private void InitCommands()
        {
            #region SaveReceiptCommand
            SaveReceiptCommand = new UICommand(o =>
            {
                Saving = true;

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

                                Saving = false;

                            });           
                            return;
                        }

                        IList<ReceiptItem> lowerPrices = ReceiptItems.Where(rm => rm.Price < rm.WholesalePrice).ToList();
                        if (lowerPrices.Count > 0)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                AskChildWindow window = new AskChildWindow();
                                window.Title = "Подтверждение";
                                StringBuilder builder = new StringBuilder();
                                builder.AppendLine("Для следующих позиций цена реализации ниже оптовой цены:");
                                foreach (var receiptItem in lowerPrices)
                                {
                                    builder.AppendFormat(" {0}\r\n",
                                        receiptItem.Name);
                                }
                                builder.AppendLine("Продолжить сохранение товарного чека?");
                                window.Message = builder.ToString();
                                window.Closed += (sender, args) =>
                                {
                                    if (window.DialogResult == true)
                                    {
                                        Task.Factory.StartNew(() =>
                                        {
                                            SaveReceipt(ctx);
                                        });
                                    }
                                    else
                                    {
                                        Saving = false;
                                    }
                                };
                                window.Show();
                            });
                        }
                        else
                        {
                            SaveReceipt(ctx);
                            #region old
                            //foreach (var receiptItem in ReceiptItems)
                            //{

                            //    if (receiptItem.SoldCount > receiptItem.Remainders)
                            //    {
                            //        var item = receiptItem;
                            //        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            //        {
                            //            MessageChildWindow msch = new MessageChildWindow();
                            //            msch.Title = "Важно";
                            //            msch.Message =
                            //                string.Format(
                            //                    "Продаваемое количество {0} {1}{2} больше количества остатков {3}{2}",
                            //                    item.Name, item.SoldCount, item.Uom, item.Remainders);
                            //            msch.Show();

                            //            Saving = false;

                            //        });
                            //        return;
                            //    }
                            //}

                            //ReceiptItems.ForEach(i =>
                            //{
                            //    i.ReceiptItemChanged -= i_ReceiptItemChanged;
                            //});
                            //var receipt = new SaleDocument();
                            ////receipt.Creator = App.CurrentUser;
                            //receipt.Creator_Id = App.CurrentUser.UserName;
                            //receipt.Customer = Customer;
                            //receipt.Customer_Name = Customer.Name;
                            //receipt.IsInDebt = InDebt;
                            //receipt.IsInvoice = IsInvoice;
                            //receipt.IsOrder = IsOrder;
                            ////receipt.LastChanger = App.CurrentUser;
                            //receipt.LastChanger_Id = App.CurrentUser.UserName;
                            //receipt.Number = ReceiptNumber;
                            //receipt.SaleDate = DateTimeHelper.GetNowKz();
                            //receipt.Barcode = GetBarcode();

                            //ctx.AddToSaleDocuments(receipt);
                            //ctx.SaveChangesSynchronous();

                            //foreach (var receiptItem in ReceiptItems)
                            //{
                            //    var item = new SaleItem()
                            //    {
                            //        Amount = receiptItem.Amount,
                            //        Count = receiptItem.SoldCount,
                            //        Discount = receiptItem.Discount,
                            //        Price = receiptItem.Price,
                            //        PriceItem_Id = receiptItem.PriceItem_Id,
                            //        //PriceItem = receiptItem.PriceItemData,
                            //        SaleDocument = receipt,
                            //        SaleDocument_Id = receipt.Id

                            //    };
                            //    var rem =
                            //        ctx.ExecuteSyncronous(
                            //            ctx.Remainders.Where(
                            //                w =>
                            //                    w.Warehouse_Id == App.CurrentUser.Warehouse_Id &&
                            //                    w.PriceItem_Id == item.PriceItem_Id))
                            //            .FirstOrDefault();

                            //    if (!receipt.IsOrder)
                            //    {
                            //        if (rem == null)
                            //        {
                            //            rem = new Remainder();
                            //            rem.PriceItem_Id = receiptItem.PriceItem_Id;
                            //            rem.RemainderDate = DateTimeHelper.GetNowKz();
                            //            rem.Warehouse_Id = App.CurrentUser.Warehouse_Id;
                            //            rem.Amount -= item.Count;
                            //            ctx.AddToRemainders(rem);
                            //        }
                            //        else
                            //        {
                            //            //var rem =
                            //            //    item.PriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                            //            //        .FirstOrDefault();
                            //            rem.Amount -= item.Count;
                            //            rem.RemainderDate = DateTimeHelper.GetNowKz();
                            //            ctx.ChangeState(rem, EntityStates.Modified);
                            //        }
                            //        //ctx.AttachTo("Remainders", rem);

                            //    }
                            //    if (receiptItem.PriceItemData == null)
                            //    {
                            //        receiptItem.PriceItemData = new StoreAppDataService.PriceItem()
                            //        {
                            //            Id = receiptItem.PriceItem_Id
                            //        };
                            //    }
                            //    receiptItem.PriceItemData.Remainders.Add(rem);

                            //    receipt.SaleItems.Add(item);
                            //    ctx.AddToSaleItems(item);

                            //    if (rem.PriceItem == null)
                            //    {
                            //        rem.PriceItem = receiptItem.PriceItemData;
                            //    }
                            //}

                            //ctx.SaveChangesSynchronous();


                            //var receiptItems = new List<PriceItem>();
                            //foreach (var receiptItem in ReceiptItems)
                            //{
                            //    var rems = receiptItem.PriceItemData.Remainders.FirstOrDefault();
                            //    receiptItems.Add(new PriceItem()
                            //    {
                            //        PriceItemData = rems.PriceItem,
                            //        Remainders = (int) (rems == null ? 0 : rems.Amount)
                            //    });
                            //}

                            //_changeRemaindersEvent.Publish(receiptItems);

                            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            //{
                            //    //_closeViewNeedEvent.Publish(ViewId);
                            //    SavedDocumentId = receipt.Id;
                            //    Barcode = receipt.Barcode;
                            //    Saved = true;

                            //});
                            #endregion
                        }
                    }
                    catch (Exception e)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            MessageChildWindow msch = new MessageChildWindow();
                            msch.Message = e.Message;
                            msch.Show();

                            Saving = false;
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
                                    WholesalePrice = (int)source.WholesalePrice,
                                    SoldCount = 0,
                                    Uom = source.Uom,
                                    PriceItem_Id = source.PriceItem_Id,
                                    Remainders = source.Remainders,
                                    ClearPrice = (int)source.WholesalePrice
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
                                WholesalePrice = (int)vm.SelectedPriceItem.WholesalePrice,
                                SoldCount = 0,
                                Uom = vm.SelectedPriceItem.Uom,
                                PriceItem_Id = vm.SelectedPriceItem.PriceItem_Id,
                                ClearPrice = (int)vm.SelectedPriceItem.WholesalePrice
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
                if (IsInvoice)
                {
                    InvoiceReportControl control = new InvoiceReportControl(SavedDocumentId, IsRefund, Contract, PaymentType, TtnNumber);
                    control.Show();
                }
                else if (IsOrder)
                {
                    OrderReportControl control = new OrderReportControl(SavedDocumentId, IsReserve, IsRefund);
                    control.Show();                   
                }
                else 
                {
                    SalesDocumentReportControl control = new SalesDocumentReportControl(SavedDocumentId);
                    control.Show();
                }
            });

            #endregion

            #region PrintInvoiceReportCommand

            PrintInvoiceReportCommand = new UICommand(a =>
            {
                SalesInvoiceReportControl control = new SalesInvoiceReportControl(SavedDocumentId, IsRefund);
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
                                c.Creator_Id == App.CurrentUser.UserName || c.Creator_Id == "admin" ||
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
            int lenAll = 7;
            var numb = ReceiptNumber;
            if (numb.StartsWith("ТЧ") || numb.StartsWith("СЧ") || numb.StartsWith("СФ"))
                numb = numb.Remove(0, 2);

            int lenNum = numb.Length;
            int zeroCount = lenAll - lenNum;

            StringBuilder builder = new StringBuilder(pref);
            for (int i = 0; i < zeroCount; i++)
            {
                builder.Append("0");
            }

            builder.Append(numb);

            return builder.ToString();
        }

        private void SaveReceipt(StoreDbContext ctx)
        {
            foreach (var receiptItem in ReceiptItems)
            {

                if (receiptItem.SoldCount > receiptItem.Remainders)
                {
                    var item = receiptItem;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Важно";
                        msch.Message =
                            string.Format(
                                "Продаваемое количество {0} {1}{2} больше количества остатков {3}{2}",
                                item.Name, item.SoldCount, item.Uom, item.Remainders);
                        msch.Show();

                        Saving = false;

                    });
                    return;
                }
                if ((receiptItem.Amount / receiptItem.SoldCount) < (receiptItem.ClearPrice))
                {
                    var item = receiptItem;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Важно";
                        msch.Message =
                            string.Format(
                                "Итоговая цена {0} позиции {1} меньше оптовой цены {2}",
                                receiptItem.Amount / receiptItem.SoldCount, item.Name, receiptItem.ClearPrice);
                        msch.Show();

                        Saving = false;

                    });
                    return;
                }
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
                    SaleDocument_Id = receipt.Id,
                    Name = receiptItem.Name,
                    CatalogNumber = receiptItem.CatalogNumber,
                    Articul = receiptItem.Articul,
                    IsDuplicate = receiptItem.IsDuplicate == "*" ? true : false
                    
                };
                var rem =
                    ctx.ExecuteSyncronous(
                        ctx.Remainders.Where(
                            w =>
                                w.Warehouse_Id == App.CurrentUser.Warehouse_Id &&
                                w.PriceItem_Id == item.PriceItem_Id))
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
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                //_closeViewNeedEvent.Publish(ViewId);
                SavedDocumentId = receipt.Id;
                _Barcode = receipt.Barcode;
                Saved = true;

            });
            _changeRemaindersEvent.Publish(receiptItems);
        }
        private void AddPositionToReceipt(string barcode)
        {
            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            if (string.IsNullOrEmpty(barcode))
                return;
            

            Task.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));


                    var findedPosition =
                        ctx.ExecuteSyncronous(ctx.PriceItems.Expand("Gear,Prices").Where(wh => wh.Barcode1 == barcode || wh.Barcode2 == barcode || wh.Barcode3 == barcode))
                            .FirstOrDefault();

                    if (findedPosition != null)
                    {
                        var price = 0;
                        if (findedPosition.Prices.Count > 0)
                        {
                            price = (int)findedPosition.Prices.OrderByDescending(or => or.PriceDate).First().Price;
                        }

                        var model = new ReceiptItem()
                        {
                            Number = ReceiptItems.Select(s => s.Number).LastOrDefault() + 1,
                            Articul = findedPosition.Gear.Articul,
                            CatalogNumber = findedPosition.Gear.CatalogNumber,
                            IsDuplicate = findedPosition.Gear.IsDuplicate ? "*" : "",
                            Name = findedPosition.Gear.Name,
                            Price = price,
                            WholesalePrice = price,
                            SoldCount = 1,
                            Uom = findedPosition.Uom_Id,
                            PriceItem_Id = findedPosition.Id,
                            ClearPrice = price
                            //PriceItemData = source.PriceItemData,

                        };

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            ReceiptItems.Add(model);
                            SelectedReceiptItem = model;

                            model.ReceiptItemChanged += i_ReceiptItemChanged;

                            OnPropertyChanged("TotalAmount");
                            OnPropertyChanged("TotalDiscount");
                            OnPropertyChanged("SubTotal");

                            _Barcode = string.Empty;
                            OnPropertyChanged("Barcode");
                        });

                    }
                }
                catch (Exception exception)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Ошибка";
                        msch.Message = exception.Message;
                        msch.Show();
                    });
                }
            });
        }
    }
}
