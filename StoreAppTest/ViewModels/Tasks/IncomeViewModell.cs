

namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Controls;
    using DevExpress.Xpf.Editors.Helpers;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Print;
    using StoreAppDataService;
    using Utilities;
    using Views;
    using IncomeItem = Model.IncomeItem;
    using PriceItem = StoreAppDataService.PriceItem;

    public class IncomeViewModell : ViewModelBase
    {

        private string _priceListName;
        private IncomPage _view;

        private IEventAggregator _agregator;
        private NewIncomeAddedEvent _newIncomeAddedEvent;
        private CloseViewNeedEvent _closeViewNeedEvent;

        public IncomeViewModell(string priceListName)
        {
            _incomeItems = new ObservableCollection<IncomeItem>();

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _newIncomeAddedEvent = _agregator.GetEvent<NewIncomeAddedEvent>();
            _closeViewNeedEvent = _agregator.GetEvent<CloseViewNeedEvent>();

            _priceListName = priceListName;
            SupplierList = new ObservableCollection<Supplier>();
            InitCommands();
        }

        public Guid ViewId { get; set; }

        public IncomPage View
        {
            get { return _view; }
            set { _view = value; }
        }

        public ObservableCollection<IncomeItem> IncomeItems
        {
            get { return _incomeItems; }
            set
            {
                _incomeItems = value;
                OnPropertyChanged("IncomeItems");
            }
        }
        private ObservableCollection<IncomeItem> _incomeItems;



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


        public IncomeItem SelectedIncome
        {
            get { return _selecteIncome; }
            set
            {
                _selecteIncome = value;
                OnPropertyChanged("SelectedIncome");
            }
        }
        private IncomeItem _selecteIncome;


        public ObservableCollection<Supplier> SupplierList { get; set; }

        public Supplier SelectedSupplier
        {
            get { return _selecteSupplier; }
            set
            {
                _selecteSupplier = value;
                OnPropertyChanged("SelectedSupplier");
            }
        }
        private Supplier _selecteSupplier;

        public string IncomeNumber
        {
            get { return _IncomeNumber; }
            set
            {
                _IncomeNumber = value;
                OnPropertyChanged("IncomeNumber");
            }
        }
        private string _IncomeNumber;



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


        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            //UpdateIncomeList();
            UpdateSuppliersList();
        }

        #endregion



        #region Commands

        public ICommand SelectPriceItemCommand { get; set; }
        public ICommand AddPriceItemCommand { get; set; }
        public ICommand EditPriceItemCommand { get; set; }
        public ICommand AddNewSupplierCommand { get; set; }
        public ICommand SaveIncomeCommand { get; set; }

        public ICommand RemoveIncomeItemCommand { get; set; }
        public ICommand PrintReportCommand { get; set; }

        private void InitCommands()
        {
            #region SelectPriceItemCommand
            SelectPriceItemCommand = new UICommand(o =>
            {

                SelectPriceControl ctrl = new SelectPriceControl();
                ctrl.PriceListComboBoxEdit.IsEnabled = false;
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
                                IncomeItem model = new IncomeItem()
                                {
                                    Number = IncomeItems.Select(s => s.Number).LastOrDefault() + 1,
                                    Articul = source.Articul,
                                    BuyPriceRur = (int)source.BuyPriceRur,
                                    BuyPriceTng = (int)source.BuyPriceTng,
                                    CatalogNumber = source.CatalogNumber,
                                    Incomes = 0,
                                    IsDuplicate = source.IsDuplicate ? "*" : "",
                                    Gear_Id = source.Gear_Id,
                                    Name = source.Gear_Name,
                                    LowerLimitRemainder = source.LowerLimitRemainder,
                                    RecommendedRemainder = source.RecommendedRemainder,
                                    NewPrice = source.WholesalePrice,
                                    PriceItem_Id = source.PriceItem_Id,
                                    Uom = source.Uom,
                                    WholesalePrice = source.WholesalePrice,
                                    Remainders = source.Remainders
                                };
                                IncomeItems.Add(model);
                            }
                            SelectedIncome = IncomeItems.First();
                        }
                        else
                        {
                            IncomeItem model = new IncomeItem()
                            {
                                Number = IncomeItems.Select(s => s.Number).LastOrDefault() + 1,
                                Articul = vm.SelectedPriceItem.Articul,
                                BuyPriceRur = (int)vm.SelectedPriceItem.BuyPriceRur,
                                BuyPriceTng = (int)vm.SelectedPriceItem.BuyPriceTng,
                                CatalogNumber = vm.SelectedPriceItem.CatalogNumber,
                                Incomes = 0,
                                IsDuplicate = vm.SelectedPriceItem.IsDuplicate ? "*" : "",
                                Gear_Id = vm.SelectedPriceItem.Gear_Id,
                                Name = vm.SelectedPriceItem.Gear_Name,
                                LowerLimitRemainder = vm.SelectedPriceItem.LowerLimitRemainder,
                                RecommendedRemainder = vm.SelectedPriceItem.RecommendedRemainder,
                                NewPrice = 0,
                                PriceItem_Id = vm.SelectedPriceItem.PriceItem_Id,
                                Uom = vm.SelectedPriceItem.Uom,
                                WholesalePrice = vm.SelectedPriceItem.WholesalePrice,
                                Remainders = vm.SelectedPriceItem.Remainders
                            };


                            IncomeItems.Add(model);
                            SelectedIncome = model;
                        }
                    }
                };

                ctrl.Show();
                vm.LoadView();

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

                var findedPriceList =
                    ctx.ExecuteSyncronous(ctx.PriceLists.Expand("PriceItems").Where(p => p.Name == _priceListName)).FirstOrDefault();

                editor.Closed += (sender, args) =>
                {
                    if (editor.DialogResult == true)
                    {
                        var newGear = new Gear();
                        newGear.Articul = editModel.Articul;
                        newGear.CatalogNumber = editModel.CatalogNumber;
                        newGear.Category_Id = "Обычные";
                        newGear.IsDuplicate = editModel.IsDuplicate;
                        newGear.LowerLimitRemainder = editModel.LowerLimitRemainder;
                        newGear.Name = editModel.Name;
                        newGear.RecommendedRemainder = editModel.RecommendedRemainder;

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

                        ctx.AddToPriceItems(newPriceItem);
                        ctx.SaveChangesSynchronous();

                        newPriceItem.PriceLists.Add(findedPriceList);
                        //findedPriceList.PriceItems.Add(newPriceItem);
                        ctx.AddLink(newPriceItem, "PriceLists", findedPriceList);
                        ctx.SaveChangesSynchronous();

                        var newRemainder = new Remainder();
                        newRemainder.PriceItem_Id = newPriceItem.Id;
                        newRemainder.PriceItem = newPriceItem;
                        newRemainder.RemainderDate = DateTimeHelper.GetNowKz();
                        newRemainder.Warehouse_Id = App.CurrentUser.Warehouse_Id;

                        ctx.AddToRemainders(newRemainder);


                        WholesalePrice rp = new WholesalePrice();
                        rp.Price = editModel.WholesalePrice;
                        rp.PriceDate = DateTimeHelper.GetNowKz();
                        rp.PriceItem_Id = newPriceItem.Id;
                        ctx.AddToWholesalePrices(rp);
                        //ctx.SaveChangesSynchronous();


                        newPriceItem.Remainders.Add(newRemainder);
                        newPriceItem.Prices.Add(rp);

                        ctx.SaveChangesSynchronous();

                        //_newPriceItemAddEvent.Publish(newPriceItem);


                        IncomeItem model = new IncomeItem()
                        {
                            Number = IncomeItems.Count == 0 ? 1 : IncomeItems.Select(s => s.Number).LastOrDefault() + 1,
                            Articul = newGear.Articul,
                            BuyPriceRur = (int) newPriceItem.BuyPriceRur,
                            BuyPriceTng = (int) newPriceItem.BuyPriceTng,
                            CatalogNumber = newGear.CatalogNumber,
                            Incomes = 0,
                            IsDuplicate = newGear.IsDuplicate ? "*" : "",
                            Gear_Id = newGear.Id,
                            Name = newGear.Name,
                            LowerLimitRemainder = newGear.LowerLimitRemainder,
                            RecommendedRemainder = newGear.RecommendedRemainder,
                            NewPrice = 0,
                            PriceItem_Id = newPriceItem.Id,
                            Uom = newPriceItem.Uom_Id,
                            WholesalePrice = 0,
                            //(int)newPriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                            Remainders = 0
                        };


                        IncomeItems.Add(model);
                        SelectedIncome = model;
                    }
                };
                editor.Show();


            });

            #endregion

            #region EditPriceItemCommand

            EditPriceItemCommand = new UICommand(a =>
            {
                if (SelectedIncome == null) return;

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
                            .Where(w => w.Id == SelectedIncome.PriceItem_Id)).FirstOrDefault();

                var editModel = new PriceItemEditModel();

                editModel.UomList = new ObservableCollection<UnitOfMeasure>(uoms);
                editModel.Uom = findedPriceItem.UnitOfMeasure;
                editModel.Articul = findedPriceItem.Gear.Articul;
                editModel.BuyPriceRur = (int)findedPriceItem.BuyPriceRur;
                editModel.BuyPriceTng = (int)findedPriceItem.BuyPriceTng;
                editModel.CatalogNumber = findedPriceItem.Gear.CatalogNumber;
                editModel.IsDuplicate = findedPriceItem.Gear.IsDuplicate;
                editModel.LowerLimitRemainder = (int)findedPriceItem.Gear.LowerLimitRemainder;
                editModel.Name = findedPriceItem.Gear.Name;
                editModel.RecommendedRemainder = (int)findedPriceItem.Gear.RecommendedRemainder;
                editModel.WholesalePrice = (int)findedPriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price;
                editModel.PreviousWholesalePrice = editModel.WholesalePrice;
                editModel.IsAdmin = App.CurrentUser.UserName.ToLower() == "admin";

                var editor = new PriceItemEditControl();
                editor.DataContext = editModel;
                editor.Closed += (sender, args) =>
                {
                    findedPriceItem.Gear.Articul = editModel.Articul;
                    findedPriceItem.Gear.CatalogNumber = editModel.CatalogNumber;
                    findedPriceItem.Gear.Category_Id = "Обычные";
                    findedPriceItem.Gear.IsDuplicate = editModel.IsDuplicate;
                    findedPriceItem.Gear.LowerLimitRemainder = editModel.LowerLimitRemainder;
                    findedPriceItem.Gear.Name = editModel.Name;
                    findedPriceItem.Gear.RecommendedRemainder = editModel.RecommendedRemainder;

                    ctx.ChangeState(findedPriceItem.Gear, EntityStates.Modified);

                    findedPriceItem.Uom_Id = editModel.Uom.Name;
                    //findedPriceItem.WholesalePrice = editModel.WholesalePrice;
                    findedPriceItem.BuyPriceRur = editModel.BuyPriceRur;
                    findedPriceItem.BuyPriceTng = editModel.BuyPriceTng;

                    ctx.ChangeState(findedPriceItem, EntityStates.Modified);


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

                    SelectedIncome.Articul = findedPriceItem.Gear.Articul;
                    SelectedIncome.BuyPriceRur = (int)findedPriceItem.BuyPriceRur;
                    SelectedIncome.BuyPriceTng = (int)findedPriceItem.BuyPriceTng;
                    SelectedIncome.CatalogNumber = findedPriceItem.Gear.CatalogNumber;
                    SelectedIncome.IsDuplicate = findedPriceItem.Gear.IsDuplicate ? "*" : "";
                    SelectedIncome.Name = findedPriceItem.Gear.Name;
                    SelectedIncome.LowerLimitRemainder = findedPriceItem.Gear.LowerLimitRemainder;
                    SelectedIncome.RecommendedRemainder = findedPriceItem.Gear.RecommendedRemainder;
                    SelectedIncome.PriceItem_Id = findedPriceItem.Id;
                    SelectedIncome.Uom = findedPriceItem.Uom_Id;
                    SelectedIncome.WholesalePrice = (int)
                        findedPriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price;
                    if (editModel.PreviousWholesalePrice != editModel.WholesalePrice)
                    {
                        CreatePriceChangeReport(findedPriceItem);
                    }

                };
                editor.Show();
            });

            #endregion

            #region AddNewSupplierCommand

            AddNewSupplierCommand = new UICommand(o =>
            {
                ChildWindow p = new ChildWindow();
                Supplier s = new Supplier();
                SupplierDetails dt = new SupplierDetails();
                dt.DataContext = s;
                p.Content = dt;

                dt.OkButtonClicked += (sender, args) =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        p.Close();
                        UpdateSuppliersList();
                    });

                };
                dt.CancelButtonClicked += (sender, args) => { DispatcherHelper.CheckBeginInvokeOnUI(() => p.Close()); };

                p.Show();
            });

            #endregion

            #region SaveIncomeCommand

            SaveIncomeCommand = new UICommand(a =>
            {
                _view.SuppliersLookUpEdit.CausesValidation = true;
                if (!_view.SuppliersLookUpEdit.DoValidate())
                {
                    _view.SuppliersLookUpEdit.CausesValidation = false;
                    return;
                }
                _view.SuppliersLookUpEdit.CausesValidation = false;



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

                        var now = DateTimeHelper.GetNowKz();
                        Income inc = new Income();
                        inc.Creator_Id = App.CurrentUser.UserName;
                        inc.IncomeDate = now;
                        inc.IncomeNumber = IncomeNumber;
                        inc.Supplier_Id = SelectedSupplier.Name;
                        inc.IsAccept = false;
                        
                        ctx.AddToIncomes(inc);
                        ctx.SaveChangesSynchronous();


                        foreach (var incomeItem in IncomeItems)
                        {
                            var inci = new StoreAppDataService.IncomeItem();
                            inci.Amount = incomeItem.Incomes;
                            inci.Income_Id = inc.Id;
                            inci.PriceItem_Id = incomeItem.PriceItem_Id;
                            inci.NewPrice = incomeItem.NewPrice;
                            inci.BuyPriceRur = incomeItem.BuyPriceRur;
                            inci.BuyPriceTng = incomeItem.BuyPriceTng;

                            ctx.AddToIncomeItems(inci);
                            inc.IncomeItems.Add(inci);

                            incomeItem.Income_ID = inc.Id;

                            //WholesalePrice rp = new WholesalePrice();
                            //rp.Price = incomeItem.NewPrice;
                            //rp.PriceDate = now;
                            //rp.PriceItem_Id = incomeItem.PriceItem_Id;
                            //ctx.AddToWholesalePrices(rp);

                            //RetailPrice rp = new RetailPrice();
                            //rp.Author_Id = App.CurrentUser.UserName;
                            //rp.Price = incomeItem.NewPrice;
                            //rp.PriceDate = now;
                            //rp.PriceItem_Id = incomeItem.PriceItem_Id;
                            //ctx.AddToRetailPrices(rp);

                        }
                        ctx.SaveChangesSynchronous();

                        IncomeItems.ForEach(f =>
                        {
                            f.Income_ID = inc.Id;
                            f.Income = string.Format("Оприходование № {0} от {1:dd.MM.yyyy}",
                                inc.IncomeNumber, inc.IncomeDate);
                        });

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _newIncomeAddedEvent.Publish(IncomeItems);
                            //_closeViewNeedEvent.Publish(ViewId);
                            SavedDocumentId = inc.Id;
                            Saved = true;

                        });
                    }
                    catch (Exception exception)
                    {
                        throw;
                    }
                });

            });

            #endregion

            #region RemoveIncomeItemCommand

            RemoveIncomeItemCommand = new UICommand(a =>
            {
                if (SelectedIncome != null)
                {
                    IncomeItems.Remove(SelectedIncome);

                }
            });
            #endregion

            #region PrintReportCommand

            PrintReportCommand = new UICommand(a =>
            {
                IncomeDocumentReportControl control = new IncomeDocumentReportControl(SavedDocumentId);
                control.Show();
            });

            #endregion

        }

        #endregion



        //private void UpdateIncomeList()
        //{
        //    IsLoading = true;
        //    ObservableCollection<IncomeItem> result = new ObservableCollection<IncomeItem>();


        //    string uri = string.Concat(
        //        Application.Current.Host.Source.Scheme, "://",
        //        Application.Current.Host.Source.Host, ":",
        //        Application.Current.Host.Source.Port,
        //        "/StoreAppDataService.svc/");


        //    Task<IList<RemaindersItem>>.Factory.StartNew(() =>
        //    {
        //        try
        //        {

        //            StoreDbContext ctx = new StoreDbContext(
        //                new Uri(uri
        //                    , UriKind.Absolute));

        //            IList<PriceIncomeItemView> pricelistitems = new List<PriceIncomeItemView>();

        //            if (NewsChecked)
        //            {
        //                var qr = ctx.ExecuteSyncronous<PriceItemRemainderView>(
        //                    new Uri(string.Format("{0}GetNewsPriceItems?priceListName='{1}'", uri, _priceListName),
        //                        UriKind.Absolute));
        //                pricelistitems = qr.ToList();
        //            }
        //            else if (SucksChecked)
        //            {
        //                var qr = ctx.ExecuteSyncronous<PriceItemRemainderView>(
        //                    new Uri(string.Format("{0}GetSucksPriceItems?priceListName='{1}'", uri, _priceListName),
        //                        UriKind.Absolute));
        //                pricelistitems = qr.ToList();

        //            }
        //            else if (LowerLimitChecked)
        //            {

        //                var qr = ctx.ExecuteSyncronous<PriceItemRemainderView>(
        //                    new Uri(string.Format("{0}GetLowerLimitPriceItems?priceListName='{1}'", uri, _priceListName),
        //                        UriKind.Absolute));
        //                pricelistitems = qr.ToList();
        //            }
        //            else if (IncomesChecked)
        //            {
        //                var qr = ctx.ExecuteSyncronous<PriceItemRemainderView>(
        //                    new Uri(string.Format("{0}GetIncomesPriceItems?priceListName='{1}'", uri, _priceListName),
        //                        UriKind.Absolute));

        //                pricelistitems = qr.ToList();
        //            }
        //            else
        //            {
        //                var qr = ctx.ExecuteSyncronous<PriceItemRemainderView>(
        //                    new Uri(string.Format("{0}GetAllPriceItems?priceListName='{1}'", uri, _priceListName),
        //                        UriKind.Absolute));
        //                pricelistitems = qr.ToList();

        //            }
        //            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //            //{
        //            //    _view.PricesGridControl.Columns.BeginUpdate();
        //            //});

        //            RemaindersItem check = new RemaindersItem(null);

        //            var columnsRemainders =
        //                from p in pricelistitems
        //                group p by p.Warehouse into grp
        //                select grp.Key;


        //            var columns = columnsRemainders;
        //            foreach (var column in columns)
        //            {
        //                if (!check.GetProperties().Any(p => p.Name == column))
        //                {
        //                    RemaindersItem.AddProperty(column, typeof(decimal));
        //                    //DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //                    //{
        //                    //    _view.PricesGridControl.Columns.Add(new GridColumn() { FieldName = column });
        //                    //});

        //                }
        //            }

        //            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //            //{
        //            //    _view.PricesGridControl.Columns.EndUpdate();
        //            //});


        //            int number = 1;
        //            var pricelistitemsQuery = from p in pricelistitems
        //                                      group p by
        //                                          new
        //                                          {
        //                                              Articul = p.Articul,
        //                                              p.BuyPriceRur,
        //                                              p.BuyPriceTng,
        //                                              p.CatalogNumber,
        //                                              p.Gear_Id,
        //                                              p.Gear_Name,
        //                                              p.IsDuplicate,
        //                                              p.LowerLimitRemainder,
        //                                              p.PriceItem_Id,
        //                                              p.RecommendedRemainder,
        //                                              p.Uom,
        //                                              p.WholesalePrice
        //                                          }
        //                                          into grp
        //                                          select grp.Key;


        //            var remsQuery = from r in pricelistitems
        //                            group r by new { r.PriceItem_Id, r.Warehouse, r.Remainder_Id }
        //                                into rgrp
        //                                select new
        //                                {
        //                                    PriceItem = rgrp.Key.PriceItem_Id,
        //                                    Warehouse = rgrp.Key.Warehouse,
        //                                    RemainderId = rgrp.Key.Remainder_Id,
        //                                    Amount = rgrp.Sum(m => m.Remainders)
        //                                };


        //            foreach (var item in pricelistitemsQuery)
        //            {
        //                var fakePriceItem = new PriceItem()
        //                {
        //                    BuyPriceRur = item.BuyPriceRur,
        //                    BuyPriceTng = item.BuyPriceTng,
        //                    Gear_Id = item.Gear_Id,
        //                    Id = item.PriceItem_Id,
        //                    Uom_Id = item.Uom,
        //                    WholesalePrice = item.WholesalePrice
        //                };
        //                foreach (var source in remsQuery.Where(r => r.PriceItem == item.PriceItem_Id))
        //                {
        //                    fakePriceItem.Remainders.Add(new Remainder()
        //                    {
        //                        Amount = source.Amount,
        //                        Id = source.RemainderId,
        //                        PriceItem_Id = item.PriceItem_Id,
        //                        Warehouse_Id = source.Warehouse
        //                    });
        //                }


        //                var rems = remsQuery.Where(p => p.PriceItem == item.PriceItem_Id);
        //                RemaindersItem exp = new RemaindersItem(fakePriceItem)
        //                {
        //                    Number = number++,
        //                    Articul = item.Articul,
        //                    CatalogNumber = item.CatalogNumber,
        //                    IsDuplicate = item.IsDuplicate ? "*" : "",
        //                    Name = item.Gear_Name,
        //                    Uom = item.Uom,
        //                    WholesalePrice = item.WholesalePrice,
        //                };

        //                foreach (var rem in rems)
        //                {
        //                    exp.SetPropertyValue(rem.Warehouse, rem.Amount);
        //                }
        //                result.Add(exp);

        //            }

        //            DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //            {
        //                RemainderItems = result;
        //            });

        //            //_view.PricesGridControl.ItemsSource = RemainderItems;

        //        }
        //        catch (Exception exception)
        //        {
        //            MessageBox.Show(exception.Message);
        //        }


        //        //DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //        //{

        //        //_view.RemaindersGrid.ItemsSource = RemainderItems;
        //        //try
        //        //{
        //        //IsLoading = false;
        //        //}
        //        //    catch (Exception)
        //        //    {

        //        //        throw;
        //        //    }
        //        //});

        //        return result;
        //    }).ContinueWith(r =>
        //    {
        //        DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //        {
        //            IsLoading = false;
        //        });

        //    });
        //}


        private void UpdateSuppliersList()
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
                    ctx.ExecuteSyncronous(ctx.Suppliers).ToList();

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    SupplierList.Clear();

                    customersDb.ForEach(i =>
                    {
                        SupplierList.Add(i);
                    });

                });
            });


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
