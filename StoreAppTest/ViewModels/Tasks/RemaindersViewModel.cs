
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Controls;
    using DevExpress.Xpf.Grid;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;
    using StoreAppDataService;
    using Utilities;
    using Views;
    using PriceItem = StoreAppDataService.PriceItem;

    public class RemaindersViewModel : ViewModelBase
    {
        private string _priceListName;
        private Remainders _view;
        private IEventAggregator _agregator;
        private NewPriceItemAddEvent _newPriceItemAddEvent;
        private EditPriceItemEvent _editPriceItemEvent;
        private RemovePriceItemEvent _removePriceItemEvent;
        private NewWarehouseTransferRequestNeedEvent _newWarehouseTransferRequestNeedEvent;

        //private IList<PriceItem> _items; 

       public RemaindersViewModel(string priceListName, Remainders view)
       // public RemaindersViewModel(Remainders view)
        {
            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _newPriceItemAddEvent = _agregator.GetEvent<NewPriceItemAddEvent>();
            _editPriceItemEvent = _agregator.GetEvent<EditPriceItemEvent>();
            _removePriceItemEvent = _agregator.GetEvent<RemovePriceItemEvent>();
            _newWarehouseTransferRequestNeedEvent = _agregator.GetEvent<NewWarehouseTransferRequestNeedEvent>();

            //RemainderItems = new RemaindersCollection();
            _remainderItems = new RemaindersCollection();

            _priceListName = priceListName;
            _view = view;

            InitCommands();
        }

        public RemaindersCollection RemainderItems
        {
            get { return _remainderItems; }
            set
            {
                _remainderItems = value;
                OnPropertyChanged("RemainderItems");
            }
        }
        private RemaindersCollection _remainderItems;



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


        public RemaindersItem SelectedRemainders
        {
            get { return _selecteRemainders; }
            set
            {
                _selecteRemainders = value;
                OnPropertyChanged("SelectedRemainders");
            }
        }
        private RemaindersItem _selecteRemainders;


        private bool _IncomesChecked;
        private bool _NewsChecked;
        private bool _SucksChecked;
        private bool _LowerLimitChecked;


        public bool IncomesChecked
        {
            get { return _IncomesChecked; }
            set
            {
                _IncomesChecked = value;
                OnPropertyChanged("IncomesChecked");
                if (value)
                {
                    NewsChecked = false;
                    SucksChecked = false;
                    LowerLimitChecked = false;
                    UpdateRemaindersList();
                }
                else
                {
                    if (!IncomesChecked && !NewsChecked && !SucksChecked && !LowerLimitChecked)
                    {
                        UpdateRemaindersList();
                    }
                }
            }
        }
        public bool NewsChecked
        {
            get { return _NewsChecked; }
            set
            {
                _NewsChecked = value;
                OnPropertyChanged("NewsChecked");
                if (value)
                {
                    IncomesChecked = false;
                    SucksChecked = false;
                    LowerLimitChecked = false;
                    UpdateRemaindersList();
                }
                else
                {
                    if (!IncomesChecked && !NewsChecked && !SucksChecked && !LowerLimitChecked)
                    {
                        UpdateRemaindersList();
                    }
                }
            }
        }
        public bool SucksChecked
        {
            get { return _SucksChecked; }
            set
            {
                _SucksChecked = value;
                OnPropertyChanged("SucksChecked");
                if (value)
                {
                    IncomesChecked = false;
                    NewsChecked = false;
                    LowerLimitChecked = false;
                    UpdateRemaindersList();
                }
                else
                {
                    if (!IncomesChecked && !NewsChecked && !SucksChecked && !LowerLimitChecked)
                    {
                        UpdateRemaindersList();
                    }
                }
            }
        }
        public bool LowerLimitChecked
        {
            get { return _LowerLimitChecked; }
            set
            {
                _LowerLimitChecked = value;
                OnPropertyChanged("LowerLimitChecked");
                if (value)
                {
                    IncomesChecked = false;
                    NewsChecked = false;
                    SucksChecked = false;
                    UpdateRemaindersList();
                }
                else
                {
                    if (!IncomesChecked && !NewsChecked && !SucksChecked && !LowerLimitChecked)
                    {
                        UpdateRemaindersList();
                    }
                }
            }
        }



        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            UpdateRemaindersList();
        }

        #endregion


        #region Commands

        public ICommand EditRemaindersCommand { get; set; }
        public ICommand AddPriceItemCommand { get; set; }
        public ICommand RemovePriceItemCommand { get; set; }
        public ICommand EditPriceItemCommand { get; set; }

        public ICommand CreateRequestCommand { get; set; }

        public ICommand PrintTagsCommand { get; set; }


        private void InitCommands()
        {
            #region EditRemaindersCommand
            EditRemaindersCommand = new UICommand(o =>
            {
                if(App.CurrentUser.UserName != "admin")
                    return;
                
                if (SelectedRemainders != null)
                {
                    EditRemainders rm = new EditRemainders();
                    rm.Name = SelectedRemainders.Name;

                    var props = SelectedRemainders.GetProperties();
                    foreach (var propertyInfo in props)
                    {
                        if(propertyInfo.Name == "Number") continue;
                        else if (propertyInfo.Name == "Articul") continue;
                        else if (propertyInfo.Name == "CatalogNumber") continue;
                        else if (propertyInfo.Name == "Name") continue;
                        else if (propertyInfo.Name == "IsDuplicate") continue;
                        else if (propertyInfo.Name == "Uom") continue;
                        else if (propertyInfo.Name == "WholesalePrice") continue;
                        else if (propertyInfo.Name == "Number") continue;
                        else if (propertyInfo.Name == "Supplier") continue;
                        else if (propertyInfo.Name == "BuyPriceRur") continue;
                        else if (propertyInfo.Name == "BuyPriceTng") continue;
                        else if (propertyInfo.Name == "Selected") continue;
                        else if (propertyInfo.Name == "Gear_Id") continue;
                        else if (propertyInfo.Name == "PriceItem_Id") continue;

                        var value = SelectedRemainders.GetPropertyValue(propertyInfo.Name);

                        rm.RemaindersItems.Add(new EditRemaindersItem()
                        {
                            Warehouse = propertyInfo.Name,
                            Count = (int)(value ?? 0)
                        });
                    }

                    RemaindersEditControl ctrl = new RemaindersEditControl();
                    ctrl.DataContext = rm;
                    ctrl.Height = 250;
                    ctrl.Width = 450;

                    ctrl.Closed += (sender, args) =>
                    {
                        if (ctrl.DialogResult == true)
                        {
                            var propes = SelectedRemainders.GetProperties();




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
                                    //    ctx.ExecuteSyncronous(ctx.SaleDocuments.OrderByDescending(s => s.Number))
                                    //        .FirstOrDefault();

                                    var priceItem = SelectedRemainders.GetPriceItemData();
                                    var rems = priceItem.Remainders;

                                    foreach (var propertyInfo in propes)
                                    {

                                        if (propertyInfo.Name == "Number") continue;
                                        else if (propertyInfo.Name == "Articul") continue;
                                        else if (propertyInfo.Name == "CatalogNumber") continue;
                                        else if (propertyInfo.Name == "Name") continue;
                                        else if (propertyInfo.Name == "IsDuplicate") continue;
                                        else if (propertyInfo.Name == "Uom") continue;
                                        else if (propertyInfo.Name == "WholesalePrice") continue;
                                        else if (propertyInfo.Name == "Number") continue;
                                        else if (propertyInfo.Name == "Supplier") continue;
                                        else if (propertyInfo.Name == "BuyPriceRur") continue;
                                        else if (propertyInfo.Name == "BuyPriceTng") continue;
                                        else if (propertyInfo.Name == "Selected") continue;
                                        else if (propertyInfo.Name == "Gear_Id") continue;
                                        else if (propertyInfo.Name == "PriceItem_Id") continue;

                                        var rem = rems.FirstOrDefault(f => f.Warehouse_Id == propertyInfo.Name);
                                        if (rem == null)
                                        {
                                            rem = new Remainder();
                                            rem.PriceItem_Id = priceItem.Id;
                                            rem.RemainderDate = DateTimeHelper.GetNowKz();
                                            rem.Amount = 0;
                                            rem.Warehouse_Id = propertyInfo.Name;
                                            ctx.AddToRemainders(rem);
                                            ctx.SaveChangesSynchronous();

                                        }
                                        var editRem = rm.RemaindersItems.First(f => f.Warehouse == propertyInfo.Name);

                                        var newRemChange = new RemaindersUserChange();
                                        newRemChange.Amound =
                                            editRem.Count;
                                        newRemChange.Date = DateTimeHelper.GetNowKz();
                                        newRemChange.Remainder_Id = rem.Id;
                                        newRemChange.User_Id = App.CurrentUser.UserName;

                                        ctx.AddToRemaindersUserChanges(newRemChange);




                                        var findedRem = ctx.ExecuteSyncronous(ctx.Remainders.Where(r => r.Id == rem.Id))
                                            .FirstOrDefault();

                                        findedRem.Amount = newRemChange.Amound;
                                        findedRem.RemainderDate = DateTimeHelper.GetNowKz();

                                        //ctx.AttachTo("Remainders", findedRem);
                                        ctx.ChangeState(findedRem, EntityStates.Modified);

                                    }

                                    ctx.SaveChangesSynchronous();


                                }
                                catch (Exception ex)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        MessageBox.Show(ex.Message);
                                    });
                                }
                            })
                            .ContinueWith(c =>
                            {
                                foreach (var propertyInfo in propes)
                                {
                                    if (propertyInfo.Name == "Number") continue;
                                    else if (propertyInfo.Name == "Articul") continue;
                                    else if (propertyInfo.Name == "CatalogNumber") continue;
                                    else if (propertyInfo.Name == "Name") continue;
                                    else if (propertyInfo.Name == "IsDuplicate") continue;
                                    else if (propertyInfo.Name == "Uom") continue;
                                    else if (propertyInfo.Name == "WholesalePrice") continue;
                                    else if (propertyInfo.Name == "Number") continue;
                                    else if (propertyInfo.Name == "Supplier") continue;
                                    else if (propertyInfo.Name == "BuyPriceRur") continue;
                                    else if (propertyInfo.Name == "BuyPriceTng") continue;
                                    else if (propertyInfo.Name == "Selected") continue;
                                    else if (propertyInfo.Name == "Gear_Id") continue;
                                    else if (propertyInfo.Name == "PriceItem_Id") continue;

                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        SelectedRemainders.SetPropertyValue(propertyInfo.Name, rm.RemaindersItems.First(f => f.Warehouse == propertyInfo.Name).Count);
                                    });


                                    //var value = SelectedRemainders.GetPropertyValue(propertyInfo.Name);

                                    //rm.RemaindersItems.Add(new EditRemaindersItem()
                                    //{
                                    //    Warehouse = propertyInfo.Name,
                                    //    Count = (decimal)value
                                    //});
                                }

                            });


                        }
                    };

                    ctrl.Show();
                    
                }
            });

            #endregion

            #region AddPriceItemCommand

            AddPriceItemCommand = new UICommand(a =>
            {
                string uri = string.Concat(
                    Application.Current.Host.Source.Scheme, "://",
                    Application.Current.Host.Source.Host, ":",
                    Application.Current.Host.Source.Port,
                    "/StoreAppDataService.svc/");

                StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                var uoms = ctx.ExecuteSyncronous(ctx.UnitOfMeasures).ToList();
                var editModel = new PriceItemEditModel();

                editModel.UomList = new ObservableCollection<UnitOfMeasure>(uoms);
                editModel.Uom = editModel.UomList.First();
                editModel.IsAdmin = App.CurrentUser.UserName.ToLower() == "admin";

                var editor = new PriceItemEditControl();
                editor.DataContext = editModel;

                editor.Closed += (sender, args) =>
                {
                    var newGear = new Gear();
                    newGear.Articul = editModel.Articul;
                    newGear.CatalogNumber = editModel.CatalogNumber;
                    newGear.Category_Id = "Обычные";
                    newGear.IsDuplicate = editModel.IsDuplicate;
                    //newGear.LowerLimitRemainder = editModel.LowerLimitRemainder;
                    newGear.Name = editModel.Name;
                    //newGear.RecommendedRemainder = editModel.RecommendedRemainder;
                    
                    ctx.AddToGears(newGear);
                    ctx.SaveChangesSynchronous();

                    var gearNews = new GearNew();
                    gearNews.CreatedDate = DateTimeHelper.GetNowKz();
                    gearNews.Gear_Id = newGear.Id;
                    ctx.AddToGearNews(gearNews);

                    var newPriceItem = new PriceItem();
                    newPriceItem.Gear_Id = newGear.Id;
                    newPriceItem.Gear = newGear;
                    //newPriceItem.PriceList_Id = _priceListName;
                    newPriceItem.Uom_Id = editModel.Uom.Name;
                    //newPriceItem.WholesalePrice = editModel.WholesalePrice;
                    newPriceItem.BuyPriceRur = editModel.BuyPriceRur;
                    newPriceItem.BuyPriceTng = editModel.BuyPriceTng;
                    newPriceItem.Barcode1 = editModel.Barcode1;
                    newPriceItem.Barcode2 = editModel.Barcode2;
                    newPriceItem.Barcode3 = editModel.Barcode3;

                    ctx.AddToPriceItems(newPriceItem);
                    ctx.SaveChangesSynchronous();

                    var newRemainder = new Remainder();
                    newRemainder.PriceItem_Id = newPriceItem.Id;
                    newRemainder.PriceItem = newPriceItem;
                    newRemainder.RemainderDate = DateTimeHelper.GetNowKz();
                    newRemainder.Warehouse_Id = App.CurrentUser.Warehouse_Id;
                    newRemainder.LowerLimitRemainder = editModel.LowerLimitRemainder;
                    newRemainder.RecommendedRemainder = editModel.RecommendedRemainder;

                    ctx.AddToRemainders(newRemainder);

                    newPriceItem.Remainders.Add(newRemainder);

                    ctx.SaveChangesSynchronous();

                    //_newPriceItemAddEvent.Publish(newPriceItem);

                    RemaindersItem model = new RemaindersItem(newPriceItem)
                    {
                        Number = RemainderItems.Select(s => s.Number).LastOrDefault() + 1,
                        Articul = newGear.Articul,
                        CatalogNumber = newGear.CatalogNumber,
                        IsDuplicate = newGear.IsDuplicate ? "*" : "",
                        Name = newGear.Name,
                        Uom = newPriceItem.Uom_Id,
                        //WholesalePrice = newPriceItem.WholesalePrice,
                    };

                    //foreach (var rem in rems)
                    //{
                    model.SetPropertyValue(newRemainder.Warehouse_Id, newRemainder.Amount);
                    //}

                    RemainderItems.Add(model);

                };
                editor.Show();


            });

            #endregion

            #region EditPriceItemCommand

            EditPriceItemCommand = new UICommand(a =>
            {
                if(SelectedRemainders == null) return;
                
                string uri = string.Concat(
                    Application.Current.Host.Source.Scheme, "://",
                    Application.Current.Host.Source.Host, ":",
                    Application.Current.Host.Source.Port,
                    "/StoreAppDataService.svc/");

                StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                var uoms = ctx.ExecuteSyncronous(ctx.UnitOfMeasures).ToList();
                var findedPriceItem =
                    ctx.ExecuteSyncronous(
                        ctx.PriceItems.Expand("Gear,Remainders,UnitOfMeasure,Prices")
                            .Where(w => w.Id == SelectedRemainders.GetPriceItemData().Id)).FirstOrDefault();

                var findedRemainder =
                    findedPriceItem.Remainders.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id)
                        .FirstOrDefault();

                var editModel = new PriceItemEditModel();

                editModel.UomList = new ObservableCollection<UnitOfMeasure>(uoms);
                editModel.Uom = findedPriceItem.UnitOfMeasure;
                editModel.Articul = findedPriceItem.Gear.Articul;
                editModel.BuyPriceRur = (int)findedPriceItem.BuyPriceRur;
                editModel.BuyPriceTng = (int)findedPriceItem.BuyPriceTng;
                editModel.CatalogNumber = findedPriceItem.Gear.CatalogNumber;
                editModel.IsDuplicate = findedPriceItem.Gear.IsDuplicate;
                editModel.LowerLimitRemainder = findedRemainder == null ? 0 : (int)findedRemainder.LowerLimitRemainder;
                editModel.Name = findedPriceItem.Gear.Name;
                editModel.RecommendedRemainder = findedRemainder == null ? 0 : (int)findedRemainder.RecommendedRemainder;
                editModel.WholesalePrice = (int)findedPriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price;
                editModel.PreviousWholesalePrice = editModel.WholesalePrice;
                editModel.IsAdmin = App.CurrentUser.UserName.ToLower() == "admin";
                editModel.Barcode1 = findedPriceItem.Barcode1;
                editModel.Barcode2 = findedPriceItem.Barcode2;
                editModel.Barcode3 = findedPriceItem.Barcode3;

                var editor = new PriceItemEditControl();
                editor.DataContext = editModel;
                editor.Closed += (sender, args) =>
                {
                    findedPriceItem.Gear.Articul = editModel.Articul;
                    findedPriceItem.Gear.CatalogNumber = editModel.CatalogNumber;
                    findedPriceItem.Gear.Category_Id = "Обычные";
                    findedPriceItem.Gear.IsDuplicate = editModel.IsDuplicate;
                    //findedPriceItem.Gear.LowerLimitRemainder = editModel.LowerLimitRemainder;
                    findedPriceItem.Gear.Name = editModel.Name;
                    //findedPriceItem.Gear.RecommendedRemainder = editModel.RecommendedRemainder;

                    ctx.ChangeState(findedPriceItem.Gear, EntityStates.Modified);

                    findedPriceItem.Uom_Id = editModel.Uom.Name;
                    //findedPriceItem.WholesalePrice = editModel.WholesalePrice;
                    findedPriceItem.BuyPriceRur = editModel.BuyPriceRur;
                    findedPriceItem.BuyPriceTng = editModel.BuyPriceTng;
                    findedPriceItem.Barcode1 = editModel.Barcode1;
                    findedPriceItem.Barcode2 = editModel.Barcode2;
                    findedPriceItem.Barcode3 = editModel.Barcode3;

                    ctx.ChangeState(findedPriceItem, EntityStates.Modified);

                    if (findedRemainder != null)
                    {
                        findedRemainder.RecommendedRemainder = editModel.RecommendedRemainder;
                        findedRemainder.LowerLimitRemainder = editModel.LowerLimitRemainder;
                        ctx.ChangeState(findedRemainder, EntityStates.Modified);
                    }
                    else
                    {
                        var newRemainder = new Remainder();
                        newRemainder.PriceItem_Id = findedPriceItem.Id;
                        newRemainder.PriceItem = findedPriceItem;
                        newRemainder.RemainderDate = DateTimeHelper.GetNowKz();
                        newRemainder.Warehouse_Id = App.CurrentUser.Warehouse_Id;
                        newRemainder.LowerLimitRemainder = editModel.LowerLimitRemainder;
                        newRemainder.RecommendedRemainder = editModel.RecommendedRemainder;
                        ctx.AddToRemainders(newRemainder);

                    }


                    if (editModel.PreviousWholesalePrice != editModel.WholesalePrice)
                    {
                        WholesalePrice price = new WholesalePrice();
                        price.PriceDate = DateTimeHelper.GetNowKz();
                        price.PriceItem_Id = findedPriceItem.Id;
                        price.Price = editModel.WholesalePrice;
                        ctx.AddToWholesalePrices(price);

                        findedPriceItem.Prices.Add(price);
                    }


                    ctx.SaveChangesSynchronous();

                    //_newPriceItemAddEvent.Publish(newPriceItem);

                    SelectedRemainders.Articul = findedPriceItem.Gear.Articul;
                    SelectedRemainders.CatalogNumber = findedPriceItem.Gear.CatalogNumber;
                    SelectedRemainders.IsDuplicate = findedPriceItem.Gear.IsDuplicate ? "*" : "";
                    SelectedRemainders.Name = findedPriceItem.Gear.Name;
                    SelectedRemainders.Uom = findedPriceItem.Uom_Id;
                    SelectedRemainders.WholesalePrice = (int)
                        findedPriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price;

                    if (editModel.PreviousWholesalePrice != editModel.WholesalePrice)
                    {
                        CreatePriceChangeReport(findedPriceItem);
                    }
                };
                editor.Show();
            });

            #endregion

            #region PrintTagsCommand

            PrintTagsCommand = new UICommand(o =>
            {
                ProductTagReportControl control =
                    new ProductTagReportControl(
                        RemainderItems.Where(wh => wh.Selected).Select(s => s.GetPriceItemData().Id).ToArray());
                control.Show();
            });
            #endregion

            #region RemovePriceItemCommand

            RemovePriceItemCommand = new UICommand(a =>
            {
                if (SelectedRemainders == null) return;

                var askWindows = new AskChildWindow();
                askWindows.Title = "Удаление";
                askWindows.Message = "Вы уверены, что хотите удалить запчасть?";

                askWindows.Closed += (sender, args) =>
                {
                    if (askWindows.DialogResult == true)
                    {
                        string uri = string.Concat(
                            Application.Current.Host.Source.Scheme, "://",
                            Application.Current.Host.Source.Host, ":",
                            Application.Current.Host.Source.Port,
                            "/StoreAppDataService.svc/");

                        StoreDbContext ctx = new StoreDbContext(
                            new Uri(uri
                                , UriKind.Absolute));

                        var findedPriceItem =
                            ctx.ExecuteSyncronous(
                                ctx.PriceItems.Expand("Gear,Remainders,Remainders/RemaindersUserChanges,UnitOfMeasure")
                                    .Where(w => w.Id == SelectedRemainders.GetPriceItemData().Id)).FirstOrDefault();

                        try
                        {
                            var gear = findedPriceItem.Gear;
                            foreach (var remainder in findedPriceItem.Remainders)
                            {
                                foreach (var remaindersUserChange in remainder.RemaindersUserChanges)
                                {
                                    ctx.DeleteObject(remaindersUserChange);
                                }
                                ctx.DeleteObject(remainder);
                            }
                            //ctx.SaveChangesSynchronous();
                            ctx.DeleteObject(findedPriceItem);
                            //ctx.SaveChangesSynchronous();
                            ctx.DeleteObject(gear);
                            ctx.SaveChangesSynchronous();

                            RemainderItems.Remove(SelectedRemainders);
                            SelectedRemainders = RemainderItems.FirstOrDefault();
                        }
                        catch (Exception exception)
                        {
                            var messageWindows = new MessageChildWindow();
                            messageWindows.Title = "Ошибка";
                            messageWindows.Message = exception.Message;
                            messageWindows.Show();
                            
                        }
                    }
                };
                askWindows.Show();

            });

            #endregion

            #region CreateRequestCommand

            CreateRequestCommand = new UICommand(o =>
            {
                if(!RemainderItems.Any(r=>r.Selected)) return;

                try
                {
                    var uri = GetContextUri();

                    StoreDbContext ctx = new StoreDbContext(uri);

                    WarehouseTransferRequestViewModel vm = new WarehouseTransferRequestViewModel(new WarehouseTransferRequestModel());
                    //WarehouseList
                    vm.IsEditMode = true;
                    vm.IsNew = true;


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

                    int number = 1;
                    foreach (var item in RemainderItems.Where(w => w.Selected))
                    {
                           vm.WarehouseTransferRequestItems.Add(new WarehouseTransferRequestModelItem()
                           {
                               Number = number++,
                               Articul = item.Articul,
                               CatalogNumber = item.CatalogNumber,
                               IsDuplicate = item.IsDuplicate,
                               Gear_Id = item.GetPriceItemData().Gear_Id,
                               Name = item.Name,
                               PriceItem_Id = item.GetPriceItemData().Id,
                               Uom = item.Uom,
                               WholesalePrice = item.WholesalePrice
                           }); 
                    }
                    var warehouses = ctx.ExecuteSyncronous(ctx.Warehouses.Where(w => w.Name != App.CurrentUser.Warehouse_Id)).ToList();
                    vm.WarehouseList = new ObservableCollection<Warehouse>(warehouses);

                    _newWarehouseTransferRequestNeedEvent.Publish(vm);
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
        }

        #endregion


        private void UpdateRemaindersList()
        {
            IsLoading = true;
            RemaindersCollection result = new RemaindersCollection();


            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");


            Task<IList<RemaindersItem>>.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                    IList<PriceItemRemainderView> pricelistitems = new List<PriceItemRemainderView>();
                    RemaindersClient client = new RemaindersClient(_priceListName);

                    if (NewsChecked)
                    {
                        pricelistitems = client.GetNewsRemainders().ToList();
                    }
                    else if (SucksChecked)
                    {
                        pricelistitems = client.GetSucksRemainders().ToList();

                    }
                    else if (LowerLimitChecked)
                    {

                        pricelistitems = client.GetLowLimitRemainders().ToList();
                    }
                    else if (IncomesChecked)
                    {
                        pricelistitems = client.GetIncomesRemainders().ToList();
                    }
                    else
                    {                  
                        pricelistitems = client.GetAllRemainders().ToList();
                    }
                    //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    //{
                    //    _view.PricesGridControl.Columns.BeginUpdate();
                    //});

                    RemaindersItem check = new RemaindersItem(null);

                    var columnsRemainders = 
                        from p in pricelistitems
                        group p by p.Warehouse into grp
                        select grp.Key;
                    var columns = columnsRemainders;
                    foreach (var column in columns)
                    {
                        if (!check.GetProperties().Any(p => p.Name == column))
                        {
                            RemaindersItem.AddProperty(column, typeof(int));
                            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            //{
                            //    _view.PricesGridControl.Columns.Add(new GridColumn() { FieldName = column });
                            //});

                        }
                    }

                    //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    //{
                    //    _view.PricesGridControl.Columns.EndUpdate();
                    //});


                    int number = 1;
                    var pricelistitemsQuery = from p in pricelistitems
                        group p by
                            new
                            {
                                Articul = p.Articul,
                                p.BuyPriceRur,
                                p.BuyPriceTng,
                                p.CatalogNumber,
                                p.Gear_Id,
                                p.Gear_Name,
                                p.IsDuplicate,
                                p.LowerLimitRemainder,
                                p.PriceItem_Id,
                                p.RecommendedRemainder,
                                p.Uom,
                                p.WholesalePrice,
                                p.Supplier
                            }
                        into grp
                        select grp.Key;


                    var remsQuery = from r in pricelistitems
                        group r by new
                        {r.PriceItem_Id, r.Warehouse,r.Remainder_Id}
                        into rgrp
                        select new
                        {
                            PriceItem = rgrp.Key.PriceItem_Id,
                            Warehouse = rgrp.Key.Warehouse,
                            RemainderId = rgrp.Key.Remainder_Id,
                            Amount = LowerLimitChecked ? rgrp.Average(m => m.Remainders) : rgrp.Sum(m => m.Remainders)
                        };


                    foreach (var item in pricelistitemsQuery)
                    {
                        var fakePriceItem = new PriceItem()
                        {
                            BuyPriceRur = item.BuyPriceRur,
                            BuyPriceTng = item.BuyPriceTng,
                            Gear_Id = item.Gear_Id,
                            Id = item.PriceItem_Id,
                            Uom_Id = item.Uom//,
                            //WholesalePrice = item.WholesalePrice
                        };
                        foreach (var source in remsQuery.Where(r => r.PriceItem == item.PriceItem_Id))
                        {
                            fakePriceItem.Remainders.Add(new Remainder()
                            {
                                Amount = source.Amount,
                                Id = source.RemainderId,
                                PriceItem_Id = item.PriceItem_Id,
                                Warehouse_Id = source.Warehouse
                            });
                        }


                        var rems = remsQuery.Where(p => p.PriceItem == item.PriceItem_Id);
                        RemaindersItem exp = new RemaindersItem(fakePriceItem)
                        {
                            Number = number++,
                            Articul = item.Articul,
                            CatalogNumber = item.CatalogNumber,
                            IsDuplicate = item.IsDuplicate ? "*" : "",
                            Name = item.Gear_Name,
                            Uom = item.Uom,
                            WholesalePrice = (int)item.WholesalePrice,
                            Supplier = item.Supplier,
                            BuyPriceRur = (int)item.BuyPriceRur,
                            BuyPriceTng = (int)item.BuyPriceTng
                        };

                        foreach (var rem in rems)
                        {
                            exp.SetPropertyValue(rem.Warehouse, (int)rem.Amount);
                        }
                        result.Add(exp);

                    }

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        RemainderItems = result;
                    });

                    //_view.PricesGridControl.ItemsSource = RemainderItems;

                }
                catch (Exception exception)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageBox.Show(exception.Message);
                    });
                }


                return result;
            }).ContinueWith(r =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsLoading = false;
                });

            });
        }


        private Uri GetContextUri()
        {
            return new Uri(string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/"));

        }

        private void CreatePriceChangeReport(PriceItem priceItem)
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

                //var acceptIncomeItems = IncomeItems.Where(i => i.Income_ID == incomeId).ToList();

                var now = DateTimeHelper.GetNowKz();

                StoreAppDataService.PriceChangeReport rep = new StoreAppDataService.PriceChangeReport();
                rep.Creator_Id = App.CurrentUser.UserName;
                rep.ReportDate = now;

                var priceReportDb =
                    ctx.ExecuteSyncronous(ctx.PriceChangeReports.OrderByDescending(s => s.Id)).FirstOrDefault();
                if (priceReportDb != null)
                {
                    int lastNumber = 0;
                    if (int.TryParse(priceReportDb.ReportNumber, out lastNumber))
                    {
                        rep.ReportNumber = (++lastNumber).ToString();
                    }
                }
                else
                {
                    rep.ReportNumber = "1";
                }


                ctx.AddToPriceChangeReports(rep);
                ctx.SaveChangesSynchronous();


                var prices = priceItem.Prices;


                    decimal previousPrice = 0;
                    WholesalePrice currentRetailPrice = prices.OrderByDescending(d => d.PriceDate).First();
                    WholesalePrice previousRetailPrice = null;
                    if (prices.Count > 1)
                    {
                        previousRetailPrice = prices.OrderByDescending(d => d.PriceDate).Skip(1).First();
                        previousPrice = previousRetailPrice.Price;

                    }
                    else
                    {
                        previousRetailPrice = currentRetailPrice;
                    }

                    StoreAppTest.StoreAppDataService.PriceChangeReportItem repItem = new StoreAppTest.StoreAppDataService.PriceChangeReportItem()
                    {
                        PriceItem_Id = priceItem.Id,
                        PreviousPrice_Id = previousRetailPrice.Id,
                        NewPrice_Id = currentRetailPrice.Id,
                        PriceChangeReport_Id = rep.Id
                    };

                    ctx.AddToPriceChangeReportItems(repItem);

                    rep.PriceChangeReportItems.Add(repItem);

                

                ctx.SaveChangesSynchronous();

            });
        }

    }

}
