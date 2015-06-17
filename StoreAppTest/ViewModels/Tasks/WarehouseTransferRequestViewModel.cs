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
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;
    using StoreAppDataService;
    using Utilities;
    using Views;

    public class WarehouseTransferRequestViewModel : ViewModelBase
    {
        private WarehouseTransferRequestModel _requestModel;

        private IEventAggregator _agregator;
        private NewWarehouseTransferRequestAdded _newWarehouseTransferRequestAdded;
        private CloseViewNeedEvent _closeViewNeedEvent;
        private WarehouseTransferRequestItemChangedEvent _requestItemChangedEvent;
        private WarehouseTransferReceivedRequestChangedEvent _receivedRequestChangedEvent;
        private WarehouseTransferSendedRequestChangedEvent _sendedRequestChangedEvent;

        
        public WarehouseTransferRequestViewModel(WarehouseTransferRequestModel requestModel)
        {
            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _newWarehouseTransferRequestAdded = _agregator.GetEvent<NewWarehouseTransferRequestAdded>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();
            _requestItemChangedEvent =
                _agregator.GetEvent<WarehouseTransferRequestItemChangedEvent>();
            _requestItemChangedEvent.Subscribe(WarehouseTransferRequestModelItemChangedEventHandler);_requestModel = requestModel;

            _receivedRequestChangedEvent = _agregator.GetEvent<WarehouseTransferReceivedRequestChangedEvent>();
            _sendedRequestChangedEvent = _agregator.GetEvent<WarehouseTransferSendedRequestChangedEvent>();

            WarehouseTransferRequestItems = new ObservableCollection<WarehouseTransferRequestModelItem>();

            SupplierRemainders= new Dictionary<long, Remainder>();
            InitCommands();
        }

        public Guid ViewId { get; set; }
        public WarehouseTransferRequestPage View { get; set; }


        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        private bool _IsLoading;

        public bool IsEditMode { get; set; }
        public bool IsNew { get; set; }

        public bool IsAccepted
        {
            get
            {
                return _IsAccepted;
            }
            set
            {
                _IsAccepted = value;
                OnPropertyChanged("IsAccepted");
            }
        }

        private bool _IsAccepted;

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
        private string _Description;


        public int TotalAcceptedAmount
        {
            get { return WarehouseTransferRequestItems.Sum(s => s.AcceptedAmount); }

        }


        public ObservableCollection<WarehouseTransferRequestModelItem> WarehouseTransferRequestItems
        {
            get { return _WarehouseTransferRequestItems; }
            set
            {
                _WarehouseTransferRequestItems = value;
                OnPropertyChanged("WarehouseTransferRequestItems");
            }
        }
        private ObservableCollection<WarehouseTransferRequestModelItem> _WarehouseTransferRequestItems;


        public WarehouseTransferRequestModelItem SelectedWarehouseTransferRequestModelItem
        {
            get { return _SelectedWarehouseTransferRequestModelItem; }
            set
            {
                _SelectedWarehouseTransferRequestModelItem = value;
                OnPropertyChanged("SelectedWarehouseTransferRequestModelItem");
            }
        }
        private WarehouseTransferRequestModelItem _SelectedWarehouseTransferRequestModelItem;


        

        private string _requestNumber;
        public string RequestNumber
        {
            get { return _requestNumber; }
            set
            {
                _requestNumber = value;
                OnPropertyChanged("RequestNumber");
            }
        }

        public DateTime RequestDate
        {
            get
            {
                if (_requestModel != null)
                    return _requestModel.RequestDate;

                return DateTime.MinValue;
            }
        }



        public ObservableCollection<Warehouse> WarehouseList { get; set; }


        public string Warehouse_Id
        {
            get { return _Warehouse_Id; }
            set
            {
                _Warehouse_Id = value;
                OnPropertyChanged("Warehouse_Id");
                UpdateSupplierRems();
            }
        }
        private string _Warehouse_Id;


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

        public Dictionary<long,Remainder> SupplierRemainders { get; set; }

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            //UpdateSupplierRems();

        }

        #endregion


        private void UpdateSupplierRems()
        {
            if (!string.IsNullOrEmpty(Warehouse_Id))
            {
                SupplierRemainders.Clear();

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

                        //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        //{

                            foreach (var warehouseTransferRequestModelItem in WarehouseTransferRequestItems)
                            {
                                var rem =
                                    ctx.ExecuteSyncronous(
                                        ctx.Remainders.Where(
                                            w =>
                                                w.PriceItem_Id == warehouseTransferRequestModelItem.PriceItem_Id &&
                                                w.Warehouse_Id == Warehouse_Id)).FirstOrDefault();

                                SupplierRemainders.Add(warehouseTransferRequestModelItem.PriceItem_Id, rem);
                            
                            }
                        //});
                    }
                    catch (Exception exception)
                    {
                        throw;
                    }
                });
            }
        }

        #region Commands


        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DeclineRequestCommand { get; set; }

        public ICommand SelectPriceItemCommand { get; set; }
        public ICommand RemoveRequestItemCommand { get; set; }
        public ICommand SaveRequestCommand { get; set; }

        public ICommand PrintReportCommand { get; set; }


        private void InitCommands()
        {

            #region AcceptRequestCommand

            AcceptRequestCommand = new UICommand(a =>
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

                        var request =
                            ctx.ExecuteSyncronous(
                                ctx.WarehouseTransferRequests.Expand("WarehouseTransferRequestItemItems")
                                .Where(w => w.Id == _requestModel.Id))
                                .FirstOrDefault();

                        if (_WarehouseTransferRequestItems.All(ac => ac.Count == ac.CountAccepted && ac.Amount == ac.AcceptedAmount))
                        {
                            request.IsAccept = true;
                            request.IsReserve = false;
                            request.Status = "Отгружается";
                        }
                        else if (_WarehouseTransferRequestItems.All(ac => ac.CountAccepted <= 0 && ac.AcceptedAmount <= 0))
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {

                                MessageChildWindow msch = new MessageChildWindow();
                                msch.Message = "Невозможно подтвердить перемещение с 0 количеством";
                                msch.Show();
                                IsAccepted = false;
                            });
                            return;
                        }
                        else
                        {
                            request.Status = "Отгружается с изменениями";
                        }
                        request.Description = Description;
                        request.LastChanged_Id = App.CurrentUser.UserName;
                        request.StateChangedDate = DateTimeHelper.GetNowKz();
                        ctx.ChangeState(request, EntityStates.Modified);

                        foreach (var warehouseTransferRequestModelItem in WarehouseTransferRequestItems)
                        {
                            var findedItem =
                                request.WarehouseTransferRequestItemItems.Where(
                                    w => w.Id == warehouseTransferRequestModelItem.Id).FirstOrDefault();

                            findedItem.CountAccepted = warehouseTransferRequestModelItem.CountAccepted;
                            findedItem.AcceptedAmount = warehouseTransferRequestModelItem.AcceptedAmount;
                            ctx.ChangeState(findedItem, EntityStates.Modified);


                            var sourceRem =
                                ctx.ExecuteSyncronous(
                                    ctx.Remainders.Where(
                                        w =>
                                            w.PriceItem_Id == warehouseTransferRequestModelItem.PriceItem_Id &&
                                            w.Warehouse_Id == request.Supplier_Id)).FirstOrDefault();

                            if (sourceRem != null)
                            {
                                if (sourceRem.Amount < warehouseTransferRequestModelItem.CountAccepted)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        MessageChildWindow msch = new MessageChildWindow();
                                        msch.Message =
                                            string.Format(
                                                "Невозможно переместить {0} {1} {2} из {3}, т.к. в наличии всего {4}",
                                                warehouseTransferRequestModelItem.Count,
                                                warehouseTransferRequestModelItem.Uom,
                                                warehouseTransferRequestModelItem.Name,
                                                Warehouse_Id, sourceRem.Amount);
                                        msch.Show();
                                        IsAccepted = false;
                                    });           
                                    return;
                                    
                                }
                                var targetRem =
                                    ctx.ExecuteSyncronous(
                                        ctx.Remainders.Where(
                                            w =>
                                                w.PriceItem_Id == warehouseTransferRequestModelItem.PriceItem_Id &&
                                                w.Warehouse_Id == request.Customer_Id)).FirstOrDefault();

                                if (targetRem == null)
                                {
                                    targetRem = new Remainder();
                                    targetRem.Amount = warehouseTransferRequestModelItem.CountAccepted;
                                    targetRem.PriceItem_Id = warehouseTransferRequestModelItem.PriceItem_Id;
                                    targetRem.RemainderDate = DateTimeHelper.GetNowKz();
                                    targetRem.Warehouse_Id = request.Customer_Id;
                                    ctx.AddToRemainders(targetRem);
                                }
                                else
                                {
                                    targetRem.Amount += warehouseTransferRequestModelItem.CountAccepted;
                                    targetRem.RemainderDate = DateTimeHelper.GetNowKz();
                                    ctx.ChangeState(targetRem, EntityStates.Modified);
                                }

                                sourceRem.RemainderDate = DateTimeHelper.GetNowKz();
                                sourceRem.Amount -= warehouseTransferRequestModelItem.CountAccepted;
                                ctx.ChangeState(sourceRem, EntityStates.Modified);
                            }
                            else
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {

                                    MessageChildWindow msch = new MessageChildWindow();
                                    msch.Message =
                                        string.Format("Невозможно переместить {0} {1} {2} из {3}, т.к. нет в наличии",
                                            warehouseTransferRequestModelItem.Count,
                                            warehouseTransferRequestModelItem.Uom,
                                            warehouseTransferRequestModelItem.Name,
                                            Warehouse_Id);
                                    msch.Show();
                                });
                                return;
                            }
                        }

                        ctx.SaveChangesSynchronous();
                        _receivedRequestChangedEvent.Publish(request);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //if (_requestModel != null)
                            //{
                            //    _requestModel.IsAccept = request.IsAccept;
                            //    _requestModel.IsReserve = request.IsReserve;
                            //    _requestModel.Description = request.Description;
                            //    _requestModel.Status = request.Status;

                            //    _requestModel.ReserveAmount = (int)
                            //        request.WarehouseTransferRequestItemItems.Sum(s => s.Count * s.WholesalePrice);
                            //    _requestModel.Customer_Id = request.Customer_Id;
                            //    _requestModel.CompletedAmount = (int)
                            //        request.WarehouseTransferRequestItemItems.Sum(s => s.AcceptedAmount);
                            //}

                            IsAccepted = true;
                        });

                        
                    }
                    catch (Exception exception)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            MessageChildWindow msch = new MessageChildWindow();
                            msch.Message = exception.Message;
                            msch.Show();
                        });
                    }
                });
            });
            #endregion

            #region DeclineRequestCommand

            DeclineRequestCommand = new UICommand(a =>
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

                        var request =
                            ctx.ExecuteSyncronous(
                                ctx.WarehouseTransferRequests.Expand("WarehouseTransferRequestItemItems")
                                .Where(w => w.Id == _requestModel.Id))
                                .FirstOrDefault();

                        request.IsAccept = false;
                        request.IsReserve = false;
                        request.Description = Description;
                        request.LastChanged_Id = App.CurrentUser.UserName;
                        request.StateChangedDate = DateTimeHelper.GetNowKz();
                        request.Status = "Отклоненно";
                        ctx.ChangeState(request, EntityStates.Modified);

                        foreach (var warehouseTransferRequestModelItem in WarehouseTransferRequestItems)
                        {
                            var findedItem =
                                request.WarehouseTransferRequestItemItems.Where(
                                    w => w.Id == warehouseTransferRequestModelItem.Id).FirstOrDefault();

                            findedItem.CountAccepted = warehouseTransferRequestModelItem.CountAccepted;
                            findedItem.AcceptedAmount = warehouseTransferRequestModelItem.AcceptedAmount;

                            ctx.ChangeState(findedItem, EntityStates.Modified);
                        }

                        ctx.SaveChangesSynchronous();
                        _receivedRequestChangedEvent.Publish(request);


                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //if (_requestModel != null)
                            //{
                            //    _requestModel.IsAccept = request.IsAccept;
                            //    _requestModel.IsReserve = request.IsReserve;
                            //    _requestModel.Description = request.Description;
                            //    _requestModel.Status = request.Status;

                            //    _requestModel.ReserveAmount = (int)
                            //        request.WarehouseTransferRequestItemItems.Sum(s => s.Count*s.WholesalePrice);
                            //    _requestModel.Customer_Id = request.Customer_Id;
                            //    _requestModel.CompletedAmount = (int)
                            //        request.WarehouseTransferRequestItemItems.Sum(s => s.AcceptedAmount);
                            //}

                            IsAccepted = true;
                        });
                    }
                    catch (Exception exception)
                    {

                    }
                });
            });
            #endregion

            #region SelectPriceItemCommand

            SelectPriceItemCommand = new UICommand(a =>
            {
                SelectPriceControl ctrl = new SelectPriceControl();
                SelectPriceControlViewModel vm = new SelectPriceControlViewModel();
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
                                WarehouseTransferRequestModelItem model = new WarehouseTransferRequestModelItem()
                                {
                                    Number = WarehouseTransferRequestItems.Select(s => s.Number).LastOrDefault() + 1,
                                    Articul = source.Articul,
                                    CatalogNumber = source.CatalogNumber,
                                    IsDuplicate = source.IsDuplicate ? "*" : "",
                                    Gear_Id = source.Gear_Id,
                                    Name = source.Gear_Name,
                                    PriceItem_Id = source.PriceItem_Id,
                                    Uom = source.Uom,
                                    WholesalePrice = source.WholesalePrice
                                };
                                model.WarehouseTransferRequestItemChanged += model_WarehouseTransferRequestItemChanged;
                                WarehouseTransferRequestItems.Add(model);
                            }
                            SelectedWarehouseTransferRequestModelItem = WarehouseTransferRequestItems.First();
                        }
                        else
                        {
                            WarehouseTransferRequestModelItem model = new WarehouseTransferRequestModelItem()
                            {
                                Number = WarehouseTransferRequestItems.Select(s => s.Number).LastOrDefault() + 1,
                                Articul = vm.SelectedPriceItem.Articul,
                                CatalogNumber = vm.SelectedPriceItem.CatalogNumber,
                                IsDuplicate = vm.SelectedPriceItem.IsDuplicate ? "*" : "",
                                Gear_Id = vm.SelectedPriceItem.Gear_Id,
                                Name = vm.SelectedPriceItem.Gear_Name,
                                PriceItem_Id = vm.SelectedPriceItem.PriceItem_Id,
                                Uom = vm.SelectedPriceItem.Uom,
                                WholesalePrice = vm.SelectedPriceItem.WholesalePrice
                            };
                            model.WarehouseTransferRequestItemChanged += model_WarehouseTransferRequestItemChanged;


                            WarehouseTransferRequestItems.Add(model);
                            SelectedWarehouseTransferRequestModelItem = model;
                        }
                    }
                };

                ctrl.Show();
                vm.LoadView();
            });

            #endregion

            #region RemoveRequestItemCommand

            RemoveRequestItemCommand = new UICommand(a =>
            {
                if (SelectedWarehouseTransferRequestModelItem != null)
                {
                    SelectedWarehouseTransferRequestModelItem.WarehouseTransferRequestItemChanged -=
                        model_WarehouseTransferRequestItemChanged;
                    WarehouseTransferRequestItems.Remove(SelectedWarehouseTransferRequestModelItem);

                }
            });

            #endregion

            #region SaveRequestCommand


            SaveRequestCommand = new UICommand(a =>
            {
                View.WarehouseLookUpEdit.CausesValidation = true;
                if (!View.WarehouseLookUpEdit.DoValidate())
                {
                    View.WarehouseLookUpEdit.CausesValidation = false;
                    return;
                }
                View.WarehouseLookUpEdit.CausesValidation = false;
                if (WarehouseTransferRequestItems.Any(b => b.Count == 0))
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
                        WarehouseTransferRequest request = null;

                        if (IsNew)
                        {
                            //if(IsNew)
                            var now = DateTimeHelper.GetNowKz();
                            request = new WarehouseTransferRequest();
                            request.Creator_Id = App.CurrentUser.UserName;
                            request.Customer_Id = App.CurrentUser.Warehouse_Id;
                            request.IsAccept = false;
                            request.IsReserve = true;
                            request.RequestDate = now;
                            request.StateChangedDate = now;
                            request.Status = "Резерв";
                            request.Supplier_Id = Warehouse_Id;
                            request.RequestNumber = RequestNumber;

                            ctx.AddToWarehouseTransferRequests(request);
                            ctx.SaveChangesSynchronous();

                            foreach (var warehouseTransferRequestModelItem in WarehouseTransferRequestItems)
                            {
                                var requstItem = new WarehouseTransferRequestItem();
                                requstItem.AcceptedAmount = warehouseTransferRequestModelItem.AcceptedAmount;
                                requstItem.Count = warehouseTransferRequestModelItem.Count;
                                requstItem.CountAccepted = warehouseTransferRequestModelItem.CountAccepted;
                                requstItem.PriceItem_Id = warehouseTransferRequestModelItem.PriceItem_Id;
                                requstItem.WarehouseTransferRequest_Id = request.Id;
                                requstItem.WholesalePrice = warehouseTransferRequestModelItem.WholesalePrice;

                                ctx.AddToWarehouseTransferRequestItems(requstItem);
                                request.WarehouseTransferRequestItemItems.Add(requstItem);
                            }
                            ctx.SaveChangesSynchronous();
                            
                            _newWarehouseTransferRequestAdded.Publish(request);
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                MessageChildWindow msch = new MessageChildWindow();
                                msch.Title = "Создание";
                                msch.Message =
                                    string.Format(
                                        "Запрос на перемещение создан");
                                msch.Show();

                                IsNew = false;
                                SavedDocumentId = request.Id;
                                Saved = true;
                            });

                            _requestModel = new WarehouseTransferRequestModel();
                            _requestModel.Id = request.Id;

                            foreach (var warehouseTransferRequestItemItem in request.WarehouseTransferRequestItemItems)
                            {
                                var findedItem = WarehouseTransferRequestItems.Where(i =>
                                    i.AcceptedAmount == warehouseTransferRequestItemItem.AcceptedAmount
                                    && i.Count == warehouseTransferRequestItemItem.Count
                                    && i.CountAccepted == warehouseTransferRequestItemItem.CountAccepted
                                    && i.PriceItem_Id == warehouseTransferRequestItemItem.PriceItem_Id
                                    && i.WholesalePrice == warehouseTransferRequestItemItem.WholesalePrice)
                                    .FirstOrDefault();
                                findedItem.Id = warehouseTransferRequestItemItem.Id;
                            }
                        }
                        else
                        {
                            request =
                                ctx.ExecuteSyncronous(
                                    ctx.WarehouseTransferRequests.Expand("WarehouseTransferRequestItemItems")
                                    .Where(w => w.Id == _requestModel.Id))
                                    .FirstOrDefault();

                            request.Description = Description;
                            request.Supplier_Id = Warehouse_Id;

                            ctx.ChangeState(request, EntityStates.Modified);

                            foreach (var warehouseTransferRequestModelItem in WarehouseTransferRequestItems)
                            {
                                var findedItem =
                                    request.WarehouseTransferRequestItemItems.Where(
                                        w => w.Id == warehouseTransferRequestModelItem.Id).FirstOrDefault();

                                findedItem.Count = warehouseTransferRequestModelItem.Count;
                                ctx.ChangeState(findedItem, EntityStates.Modified);


                            }

                            ctx.SaveChangesSynchronous();
                            _sendedRequestChangedEvent.Publish(request);

                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                MessageChildWindow msch = new MessageChildWindow();
                                msch.Title = "Сохранение";
                                msch.Message =
                                    string.Format(
                                        "Запрос на перемещение сохранен");
                                msch.Show();

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
                });

            });

            #endregion

            #region PrintReportCommand

            PrintReportCommand = new UICommand(a =>
            {
                WarehouseRequestReportControl control = new WarehouseRequestReportControl(SavedDocumentId);
                control.Show();
                
            });

            #endregion
        }

        public void model_WarehouseTransferRequestItemChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("TotalAcceptedAmount");
        }

        public void WarehouseTransferRequestModelItemChangedEventHandler(WarehouseTransferRequestModelItem item)
        {
            OnPropertyChanged("TotalAcceptedAmount");
        }
        #endregion
    }
}
