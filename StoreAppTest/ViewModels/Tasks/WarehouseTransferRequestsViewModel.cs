
namespace StoreAppTest.ViewModels
{
    using System;
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

    public class WarehouseTransferRequestsViewModel : ViewModelBase
    {
        private IEventAggregator _agregator;
        private NewWarehouseTransferRequestNeedEvent _newWarehouseTransferRequestNeedEvent;

        public WarehouseTransferRequestsViewModel()
        {
            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _newWarehouseTransferRequestNeedEvent = _agregator.GetEvent<NewWarehouseTransferRequestNeedEvent>();
            _agregator.GetEvent<NewWarehouseTransferRequestAdded>().Subscribe(NewWarehouseTransferRequestAddedHandler);
            _agregator.GetEvent<WarehouseTransferReceivedRequestChangedEvent>().Subscribe(WarehouseTransferReceivedRequestChangedEventHandler);
            _agregator.GetEvent<WarehouseTransferSendedRequestChangedEvent>().Subscribe(WarehouseTransferSendedRequestChangedEventHandler);

            ReceivedRequests = new ObservableCollection<WarehouseTransferRequestModel>();
            SendredRequests = new ObservableCollection<WarehouseTransferRequestModel>();
            InitCommands();
        }

        public DateTime ReceivedAtFromDate
        {
            get { return _receivedAtFromDate; }
            set
            {
                _receivedAtFromDate = DateTimeHelper.GetStartDay(value);
                OnPropertyChanged("ReceivedAtFromDate");
            }
        }
        private DateTime _receivedAtFromDate;


        public DateTime ReceivedAtToDate
        {
            get { return _receivedAtToDate; }
            set
            {
                _receivedAtToDate = DateTimeHelper.GetEndDay(value);
                OnPropertyChanged("ReceivedAtToDate");
            }
        }
        private DateTime _receivedAtToDate;


        public DateTime SendedAtFromDate
        {
            get { return _sendedAtFromDate; }
            set
            {
                _sendedAtFromDate = DateTimeHelper.GetStartDay(value);
                OnPropertyChanged("SendedAtFromDate");
            }
        }
        private DateTime _sendedAtFromDate;


        public DateTime SendedAtToDate
        {
            get { return _sendedAtToDate; }
            set
            {
                _sendedAtToDate = DateTimeHelper.GetEndDay(value);
                OnPropertyChanged("SendedAtToDate");
            }
        }
        private DateTime _sendedAtToDate;

        public bool IsReceivedLoading
        {
            get { return _IsReceivedLoading; }
            set
            {
                _IsReceivedLoading = value;
                OnPropertyChanged("IsReceivedLoading");
            }
        }
        private bool _IsReceivedLoading;

        public bool IsSendedLoading
        {
            get { return _IsSendedLoading; }
            set
            {
                _IsSendedLoading = value;
                OnPropertyChanged("IsSendedLoading");
            }
        }
        private bool _IsSendedLoading;


        public ObservableCollection<WarehouseTransferRequestModel> ReceivedRequests
        {
            get { return _ReceivedRequests; }
            set
            {
                _ReceivedRequests = value;
                OnPropertyChanged("ReceivedRequests");
            }
        }
        private ObservableCollection<WarehouseTransferRequestModel> _ReceivedRequests;


        public WarehouseTransferRequestModel SelectedReceivedRequest
        {
            get { return _SelectedReceivedRequest; }
            set
            {
                _SelectedReceivedRequest = value;
                OnPropertyChanged("SelectedReceivedRequest");
            }
        }
        private WarehouseTransferRequestModel _SelectedReceivedRequest;


        public ObservableCollection<WarehouseTransferRequestModel> SendredRequests
        {
            get { return _SendredRequests; }
            set
            {
                _SendredRequests = value;
                OnPropertyChanged("SendredRequests");
            }
        }
        private ObservableCollection<WarehouseTransferRequestModel> _SendredRequests;


        public WarehouseTransferRequestModel SelectedSendredRequest
        {
            get { return _SelectedSendredRequest; }
            set
            {
                _SelectedSendredRequest = value;
                OnPropertyChanged("SelectedSendredRequest");
            }
        }
        private WarehouseTransferRequestModel _SelectedSendredRequest;

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            var now = DateTime.Now;
            ReceivedAtFromDate = DateTimeHelper.GetStartMonth(now);
            ReceivedAtToDate = DateTimeHelper.GetEndMonth(now);
            SendedAtFromDate = DateTimeHelper.GetStartMonth(now);
            SendedAtToDate = DateTimeHelper.GetEndMonth(now);

            RefreshReceivedRequestCommand.Execute(null);
            RefreshSendedRequestCommand.Execute(null);
        }

        #endregion



        #region Commands

        public ICommand ViewWarehouseTransferRequestCommand { get; set; }

        public ICommand ViewWarehouseTransferRequestSendedCommand { get; set; }

        public ICommand CreateRequestCommand { get; set; }

        public ICommand RefreshReceivedRequestCommand { get; set; }

        public ICommand RefreshSendedRequestCommand { get; set; }

        private void InitCommands()
        {

            #region ViewWarehouseTransferRequestCommand

            ViewWarehouseTransferRequestCommand = new UICommand(a =>
            {
                if (SelectedReceivedRequest != null)
                {

                    try
                    {

                        WarehouseTransferRequestViewModel vm =
                            new WarehouseTransferRequestViewModel(SelectedReceivedRequest);
                        //WarehouseList
                        vm.IsEditMode = false;
                        vm.IsNew = false;
                        vm.RequestNumber = SelectedReceivedRequest.RequestNumber;
                        vm.IsAccepted = SelectedReceivedRequest.IsAccept;
                        vm.Saved = true;
                        vm.SavedDocumentId = SelectedReceivedRequest.Id;

                        var uri = GetContextUri();
            
                        var ctx = new StoreDbContext(uri);

                        int number = 1;
                        var items =
                            ctx.ExecuteSyncronous(
                                ctx.WarehouseTransferRequestItems.Expand("PriceItem/Gear")
                                .Where(
                                    w => w.WarehouseTransferRequest_Id == SelectedReceivedRequest.Id))
                                .ToList()
                                .Select(s => new WarehouseTransferRequestModelItem((int)s.AcceptedAmount,(int)s.CountAccepted)
                                {
                                    AcceptedAmount = (int)s.AcceptedAmount,
                                    Articul = s.PriceItem.Gear.Articul,
                                    CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                                    Count = (int)s.Count,
                                    CountAccepted = (int)s.CountAccepted,
                                    Gear_Id = s.PriceItem.Gear_Id,
                                    Id = s.Id,
                                    IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
                                    Name = s.PriceItem.Gear.Name,
                                    Number = number++,
                                    Uom = s.PriceItem.Uom_Id,
                                    PriceItem_Id = s.PriceItem_Id,
                                    WholesalePrice = (int)s.WholesalePrice,
                                    WarehouseTransferRequest_Id = s.WarehouseTransferRequest_Id

                                });
                        foreach (var warehouseTransferRequestModelItem in items)
                        {
                            warehouseTransferRequestModelItem.WarehouseTransferRequestItemChanged +=
                                vm.model_WarehouseTransferRequestItemChanged;

                        }
                        vm.WarehouseTransferRequestItems =
                            new ObservableCollection<WarehouseTransferRequestModelItem>(items);
                        vm.Warehouse_Id = SelectedReceivedRequest.Supplier_Id;

                        _newWarehouseTransferRequestNeedEvent.Publish(vm);
                        vm.LoadView();
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            });

            #endregion

            #region ViewWarehouseTransferRequestSendedCommand


            ViewWarehouseTransferRequestSendedCommand = new UICommand(a =>
            {
                if (SelectedSendredRequest != null)
                {

                    try
                    {

                        WarehouseTransferRequestViewModel vm =
                            new WarehouseTransferRequestViewModel(SelectedSendredRequest);
                        //WarehouseList
                        vm.IsEditMode = true;
                        vm.IsNew = false;
                        vm.RequestNumber = SelectedSendredRequest.RequestNumber;
                        vm.IsAccepted = SelectedSendredRequest.IsAccept;
                        vm.Saved = true;
                        vm.SavedDocumentId = SelectedSendredRequest.Id;

                        var uri = GetContextUri();

                        var ctx = new StoreDbContext(uri);

                        int number = 1;
                        var items =
                            ctx.ExecuteSyncronous(
                                ctx.WarehouseTransferRequestItems.Expand("PriceItem/Gear")
                                .Where(
                                    w => w.WarehouseTransferRequest_Id == SelectedSendredRequest.Id))
                                .ToList()
                                .Select(s => new WarehouseTransferRequestModelItem((int)s.AcceptedAmount, (int)s.CountAccepted)
                                {
                                    AcceptedAmount = (int)s.AcceptedAmount,
                                    Articul = s.PriceItem.Gear.Articul,
                                    CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                                    Count = (int)s.Count,
                                    CountAccepted = (int)s.CountAccepted,
                                    Gear_Id = s.PriceItem.Gear_Id,
                                    Id = s.Id,
                                    IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
                                    Name = s.PriceItem.Gear.Name,
                                    Number = number++,
                                    Uom = s.PriceItem.Uom_Id,
                                    PriceItem_Id = s.PriceItem_Id,
                                    WholesalePrice = (int)s.WholesalePrice,
                                    WarehouseTransferRequest_Id = s.WarehouseTransferRequest_Id

                                });
                        foreach (var warehouseTransferRequestModelItem in items)
                        {
                            warehouseTransferRequestModelItem.WarehouseTransferRequestItemChanged +=
                                vm.model_WarehouseTransferRequestItemChanged;

                        }

                        vm.WarehouseTransferRequestItems =
                            new ObservableCollection<WarehouseTransferRequestModelItem>(items);

                        var warehouses = ctx.ExecuteSyncronous(ctx.Warehouses.Where(w => w.Name != App.CurrentUser.Warehouse_Id)).ToList();
                        vm.WarehouseList = new ObservableCollection<Warehouse>(warehouses);

                        vm.Warehouse_Id = SelectedSendredRequest.Supplier_Id;
                        _newWarehouseTransferRequestNeedEvent.Publish(vm);
                        vm.LoadView();
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            });

            #endregion

            #region CreateRequestCommand

            CreateRequestCommand = new UICommand(o =>
            {
                try
                {
                    var uri = GetContextUri();

                    StoreDbContext ctx = new StoreDbContext(uri);

                    WarehouseTransferRequestViewModel vm = new WarehouseTransferRequestViewModel(new WarehouseTransferRequestModel());
                    //WarehouseList
                    vm.IsEditMode = true;
                    vm.IsNew = true;

                    var warehouses = ctx.ExecuteSyncronous(ctx.Warehouses.Where(w => w.Name != App.CurrentUser.Warehouse_Id)).ToList();
                    vm.WarehouseList = new ObservableCollection<Warehouse>(warehouses);

                    var reqDb =
                        ctx.ExecuteSyncronous(ctx.WarehouseTransferRequests.OrderByDescending(s => s.Id)).FirstOrDefault();
                    if (reqDb != null)
                    {
                        int lastNumber = 0;
                        if (int.TryParse(reqDb.RequestNumber, out lastNumber))
                        {
                            vm.RequestNumber = (++lastNumber).ToString();
                        }
                    }
                    else
                    {
                        vm.RequestNumber = "1";
                    }

                    _newWarehouseTransferRequestNeedEvent.Publish(vm);
                    vm.LoadView();
                }
                catch (Exception exception)
                {
                    MessageChildWindow ms = new MessageChildWindow();
                    ms.Title = "Ошибка";
                    ms.Message = exception.Message;

                    ms.Show();
                }
            });

            #endregion

            #region RefreshReceivedRequestCommand

            RefreshReceivedRequestCommand = new UICommand(o =>
            {
                IsReceivedLoading = true;

                var receivedUri = GetContextUri();

                ReceivedRequests.Clear();

                Task.Factory.StartNew(() =>
                {

                    try
                    {
                        var ctx = new StoreDbContext(receivedUri);

                        var receivedRequests =
                            ctx.ExecuteSyncronous(
                                ctx.WarehouseTransferRequests.Expand("WarehouseTransferRequestItemItems")
                                .Where(w => w.Supplier_Id == App.CurrentUser.Warehouse_Id
                                && w.RequestDate >= ReceivedAtFromDate && w.RequestDate <= ReceivedAtToDate).OrderByDescending(or => or.RequestDate))
                                .ToList();


                        foreach (var warehouseTransferRequest in receivedRequests)
                        {
                            var model = new WarehouseTransferRequestModel();
                            model.ReserveAmount = (int)
                                warehouseTransferRequest.WarehouseTransferRequestItemItems.Sum(s => s.Count * s.WholesalePrice);
                            model.Customer_Id = warehouseTransferRequest.Customer_Id;
                            model.Description = warehouseTransferRequest.Description;
                            model.Id = warehouseTransferRequest.Id;
                            model.IsAccept = warehouseTransferRequest.IsAccept;
                            model.IsReserve = warehouseTransferRequest.IsReserve;
                            model.RequestDate = warehouseTransferRequest.RequestDate;
                            model.RequestNumber = warehouseTransferRequest.RequestNumber;
                            model.CompletedAmount = (int)
                                warehouseTransferRequest.WarehouseTransferRequestItemItems.Sum(s => s.AcceptedAmount);
                            model.StateChangedDate = warehouseTransferRequest.StateChangedDate;
                            model.Status = warehouseTransferRequest.Status;
                            model.Supplier_Id = warehouseTransferRequest.Supplier_Id;

                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                ReceivedRequests.Add(model);
                            });

                        }


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
                        IsReceivedLoading = false;
                    });
                });
            });

            #endregion

            #region RefreshSendedRequestCommand

            RefreshSendedRequestCommand = new UICommand(o =>
            {
                IsSendedLoading = true;

                var sendedUri = GetContextUri();

                SendredRequests.Clear();

                Task.Factory.StartNew(() =>
                {

                    try
                    {
                        var ctx = new StoreDbContext(sendedUri); 
                        var receivedRequests =
                             ctx.ExecuteSyncronous(
                                 ctx.WarehouseTransferRequests.Expand("WarehouseTransferRequestItemItems")
                                 .Where(w => w.Customer_Id == App.CurrentUser.Warehouse_Id
                                 && w.RequestDate >= SendedAtFromDate && w.RequestDate <= SendedAtToDate)
                                 .OrderByDescending(or => or.RequestDate))
                                 .ToList();


                        foreach (var warehouseTransferRequest in receivedRequests)
                        {
                            var model = new WarehouseTransferRequestModel();
                            model.ReserveAmount = (int)
                                warehouseTransferRequest.WarehouseTransferRequestItemItems.Sum(s => s.Count * s.WholesalePrice);
                            model.Customer_Id = warehouseTransferRequest.Customer_Id;
                            model.Description = warehouseTransferRequest.Description;
                            model.Id = warehouseTransferRequest.Id;
                            model.IsAccept = warehouseTransferRequest.IsAccept;
                            model.IsReserve = warehouseTransferRequest.IsReserve;
                            model.RequestDate = warehouseTransferRequest.RequestDate;
                            model.RequestNumber = warehouseTransferRequest.RequestNumber;
                            model.CompletedAmount = (int)
                                warehouseTransferRequest.WarehouseTransferRequestItemItems.Sum(s => s.AcceptedAmount);
                            model.StateChangedDate = warehouseTransferRequest.StateChangedDate;
                            model.Status = warehouseTransferRequest.Status;
                            model.Supplier_Id = warehouseTransferRequest.Supplier_Id;

                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                SendredRequests.Add(model);
                            });

                        }

                    }
                    catch (Exception exception)
                    {
                        MessageChildWindow ms = new MessageChildWindow();
                        ms.Title = "Ошибка";
                        ms.Message = exception.Message;

                        ms.Show();
                    }
                }).ContinueWith(c =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        IsSendedLoading = false;
                    });
                });
            });
            #endregion
        }

        #endregion



        private Uri GetContextUri()
        {
            return new Uri(string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/"));

        }



        public void NewWarehouseTransferRequestAddedHandler(WarehouseTransferRequest request)
        {
            var model = new WarehouseTransferRequestModel();
            model.ReserveAmount = (int)
                request.WarehouseTransferRequestItemItems.Sum(s => s.Count * s.WholesalePrice);
            model.Customer_Id = request.Customer_Id;
            model.Description = request.Description;
            model.Id = request.Id;
            model.IsAccept = request.IsAccept;
            model.IsReserve = request.IsReserve;
            model.RequestDate = request.RequestDate;
            model.RequestNumber = request.RequestNumber;
            model.CompletedAmount = (int)
                request.WarehouseTransferRequestItemItems.Sum(s => s.AcceptedAmount);
            model.StateChangedDate = request.StateChangedDate;
            model.Status = request.Status;
            model.Supplier_Id = request.Supplier_Id;

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                SendredRequests.Add(model);
            });
        }


        public void WarehouseTransferSendedRequestChangedEventHandler(WarehouseTransferRequest obj)
        {
            var model = SendredRequests.Where(m => m.Id == obj.Id).FirstOrDefault();
            model.ReserveAmount = (int)
                            obj.WarehouseTransferRequestItemItems.Sum(s => s.Count * s.WholesalePrice);
            model.Customer_Id = obj.Customer_Id;
            model.Description = obj.Description;
            model.IsAccept = obj.IsAccept;
            model.IsReserve = obj.IsReserve;
            model.RequestDate = obj.RequestDate;
            model.RequestNumber = obj.RequestNumber;
            model.CompletedAmount = (int)
                obj.WarehouseTransferRequestItemItems.Sum(s => s.AcceptedAmount);
            model.StateChangedDate = obj.StateChangedDate;
            model.Status = obj.Status;
            model.Supplier_Id = obj.Supplier_Id;
        }

        public void WarehouseTransferReceivedRequestChangedEventHandler(WarehouseTransferRequest obj)
        {
            var model = ReceivedRequests.Where(m => m.Id == obj.Id).FirstOrDefault();
            model.ReserveAmount = (int)
                            obj.WarehouseTransferRequestItemItems.Sum(s => s.Count * s.WholesalePrice);
            model.Customer_Id = obj.Customer_Id;
            model.Description = obj.Description;
            model.IsAccept = obj.IsAccept;
            model.IsReserve = obj.IsReserve;
            model.RequestDate = obj.RequestDate;
            model.RequestNumber = obj.RequestNumber;
            model.CompletedAmount = (int)
                obj.WarehouseTransferRequestItemItems.Sum(s => s.AcceptedAmount);
            model.StateChangedDate = obj.StateChangedDate;
            model.Status = obj.Status;
            model.Supplier_Id = obj.Supplier_Id;
        }

    }


    public class WarehouseTransferRequestModel : Notified
    {
        public long Id { get; set; }

        public string RequestNumber { get; set; }

        public DateTime RequestDate { get; set; }

        public bool IsAccept{
            get
            {
                return _IsAccept;
            }
            set
            {
                _IsAccept = value;
                OnPropertyChanged("IsAccept");
            }
        }
        private bool _IsAccept;

        public bool IsReserve
        {
            get
            {
                return _IsReserve;
            }
            set
            {
                _IsReserve = value;
                OnPropertyChanged("IsReserve");
            }
        }
        private bool _IsReserve;

        public DateTime? StateChangedDate { get; set; }

        private string _Status;
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }


        private string _Customer_Id;
        public string Customer_Id
        {
            get
            {
                return _Customer_Id;
            }
            set
            {
                _Customer_Id = value;
                OnPropertyChanged("Customer_Id");
            }
        }

        public string Supplier_Id { get; set; }



        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }



        private int _ReserveAmount;
        public int ReserveAmount
        {
            get
            {
                return _ReserveAmount;
            }
            set
            {
                _ReserveAmount = value;
                OnPropertyChanged("ReserveAmount");
            }
        }



        private int _CompletedAmount;
        public int CompletedAmount
        {
            get
            {
                return _CompletedAmount;
            }
            set
            {
                _CompletedAmount = value;
                OnPropertyChanged("CompletedAmount");
            }
        }


    }

    public class WarehouseTransferRequestModelItem : Notified
    {
        private WarehouseTransferRequestItemChangedEvent _requestItemChangedEvent;
        private IEventAggregator _aggregator;

        public WarehouseTransferRequestModelItem()
        {
            StartedAcceptedCount = 0;
            StartedAcceptedAmmount = 0;

            _aggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _requestItemChangedEvent = _aggregator.GetEvent<WarehouseTransferRequestItemChangedEvent>();
        }

        public WarehouseTransferRequestModelItem(int acceptedAmound, int acceptedCount)
        {
            StartedAcceptedCount = acceptedCount;
            StartedAcceptedAmmount = acceptedAmound;

            _aggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _requestItemChangedEvent = _aggregator.GetEvent<WarehouseTransferRequestItemChangedEvent>();
        }


        public int StartedAcceptedCount { get; private set; }
        public int StartedAcceptedAmmount { get; private set; }

        public long Id { get; set; }

        public int Number { get; set; }

        public string Articul { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string IsDuplicate { get; set; }
        public long Gear_Id { get; set; }
        public string Uom { get; set; }

        public long PriceItem_Id { get; set; }


        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
                OnPropertyChanged("Count");
            }
        }
        private int _Count;

        public int Amount
        {
            get
            {
                return Count * WholesalePrice;
            }
        }


        public int CountAccepted
        {
            get
            {
                return _CountAccepted;
            }
            set
            {
                _CountAccepted = value;
                AcceptedAmount = _CountAccepted*WholesalePrice;
                OnPropertyChanged("CountAccepted");
                WarehouseTransferRequestItemChanged(this, new EventArgs());
                _requestItemChangedEvent.Publish(this);
            }
        }
        private int _CountAccepted;


        public int WholesalePrice
        {
            get
            {
                return _WholesalePrice;
            }
            set
            {
                _WholesalePrice = value;
                OnPropertyChanged("WholesalePrice");
            }
        }
        private int _WholesalePrice;

        public int AcceptedAmount
        {
            get
            {
                return _AcceptedAmount;
            }
            set
            {
                _AcceptedAmount = value;
                OnPropertyChanged("AcceptedAmount");
                WarehouseTransferRequestItemChanged(this, new EventArgs());
                _requestItemChangedEvent.Publish(this);
            }
        }
        private int _AcceptedAmount;

        public long WarehouseTransferRequest_Id { get; set; }

        public event EventHandler<EventArgs> WarehouseTransferRequestItemChanged = (sender, args) => { };

    }

}
