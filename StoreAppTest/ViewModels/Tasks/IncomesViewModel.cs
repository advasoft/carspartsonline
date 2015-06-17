
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
    using PriceChangeReportItem = StoreAppDataService.PriceChangeReportItem;

    public class IncomesViewModel : ViewModelBase
    {
        private IncomesPage _view;
        private string _priceListName;
        private IEventAggregator _agregator;
        private NewIncomeNeedEvent _newIncomeNeedEvent;
        private NewIncomeAddedEvent _newIncomeAddedEvent;
        private NewPriceChangeReportNeedEvent _newPriceChangeReportNeedEvent;

        public IncomesViewModel(string priceListName, IncomesPage view)
        {
            _view = view;
            _priceListName = priceListName;

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _newIncomeNeedEvent = _agregator.GetEvent<NewIncomeNeedEvent>();
            _newIncomeAddedEvent = _agregator.GetEvent<NewIncomeAddedEvent>();
            _newPriceChangeReportNeedEvent = _agregator.GetEvent<NewPriceChangeReportNeedEvent>();
            _newIncomeAddedEvent.Subscribe(NewIncomeAddedEventHandler);

            SupplierList = new ObservableCollection<Supplier>();
            IncomeItems = new ObservableCollection<IncomeItem>();
            SelectedIncomeItems = new ObservableCollection<IncomeItem>();

            InitCommands();
        }


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

        public ObservableCollection<IncomeItem> SelectedIncomeItems
        {
            get { return _selectedIncomeItems; }
            set
            {
                _selectedIncomeItems = value;
                OnPropertyChanged("SelectedIncomeItems");
            }
        }
        private ObservableCollection<IncomeItem> _selectedIncomeItems;


        
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


        public DateTime AtFromDate
        {
            get { return _atFromDate; }
            set
            {
                _atFromDate = DateTimeHelper.GetStartDay(value);
                OnPropertyChanged("AtFromDate");
            }
        }
        private DateTime _atFromDate;


        public DateTime AtToDate
        {
            get { return _atToDate; }
            set
            {
                _atToDate = DateTimeHelper.GetEndDay(value);
                OnPropertyChanged("AtToDate");
            }
        }
        private DateTime _atToDate;

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            UpdateSuppliersList();

            var now = DateTime.Now;
            AtFromDate = DateTimeHelper.GetStartMonth(now);
            AtToDate = DateTimeHelper.GetEndMonth(now);

            UpdateIncomesList();
        }

        #endregion


        #region Commands

        public ICommand OpenNewIncomeCommand { get; set; }
        public ICommand AddNewSupplierCommand { get; set; }
        public ICommand AcceptIncomeCommand { get; set; }
        public ICommand PriceChangeReportCommand { get; set; }
        public ICommand PrintReportCommand { get; set; }
        public ICommand RefreshIncomesCommand { get; set; }

        private void InitCommands()
        {
            #region OpenNewIncomeCommand
            OpenNewIncomeCommand = new UICommand(a =>
            {


                string uri = string.Concat(
                    Application.Current.Host.Source.Scheme, "://",
                    Application.Current.Host.Source.Host, ":",
                    Application.Current.Host.Source.Port,
                    "/StoreAppDataService.svc/");

                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));


                IncomeViewModell vm = new IncomeViewModell(_priceListName);
                vm.SelectedSupplier = SelectedSupplier;

                var incomeDb =
                    ctx.ExecuteSyncronous(ctx.Incomes.OrderByDescending(s => s.Id)).FirstOrDefault();
                if (incomeDb != null)
                {
                    int lastNumber = 0;
                    if (int.TryParse(incomeDb.IncomeNumber, out lastNumber))
                    {
                        vm.IncomeNumber = (++lastNumber).ToString();
                    }
                }
                else
                {
                    vm.IncomeNumber = "1";
                }

                _newIncomeNeedEvent.Publish(vm);

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

            #region AcceptIncomeCommand
            
            AcceptIncomeCommand = new UICommand(o =>
            {
                if(SelectedIncomeItems.Count == 0) return;
                var selected = SelectedIncomeItems.First();
                if(selected.IsAccepted) return;
                
                string uri = string.Concat(
                    Application.Current.Host.Source.Scheme, "://",
                    Application.Current.Host.Source.Host, ":",
                    Application.Current.Host.Source.Port,
                    "/StoreAppDataService.svc/");

                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));

                try
                {

                    var incomeDb =
                        ctx.ExecuteSyncronous(ctx.Incomes.Expand("IncomeItems/PriceItem").Where(i => i.Id == selected.Income_ID)).FirstOrDefault();

                    var acceptIncomeItems = IncomeItems.Where(i => i.Income_ID == selected.Income_ID).ToList();

                    AcceptIncomeModel model = new AcceptIncomeModel();
                    model.IncomeNumber = incomeDb.IncomeNumber;
                    model.IncomeDate = incomeDb.IncomeDate;

                    model.IncomeItems = new ObservableCollection<IncomeItem>(acceptIncomeItems);


                    AcceptIncomeControl ctrl = new AcceptIncomeControl();
                    ctrl.DataContext = model;

                    ctrl.Closed += (sender, args) =>
                    {

                        if (ctrl.DialogResult == true)
                        {
                            incomeDb.AcceptedDate = DateTimeHelper.GetNowKz();
                            incomeDb.IsAccept = true;
                            incomeDb.Accepter_Id = App.CurrentUser.UserName;
                            
                            ctx.ChangeState(incomeDb, EntityStates.Modified);
                            ctx.SaveChangesSynchronous();

                            foreach (var ii in incomeDb.IncomeItems)
                            {
                                ii.PriceItem.BuyPriceRur = ii.BuyPriceRur;
                                ii.PriceItem.BuyPriceTng = ii.BuyPriceTng;

                                ctx.ChangeState(ii.PriceItem, EntityStates.Modified);
                            }
                            ctx.SaveChangesSynchronous();

                            foreach (var acceptIncomeItem in acceptIncomeItems)
                            {
                                var findedIncomeItem = 
                                IncomeItems.Where(i => i.Income_ID == acceptIncomeItem.Income_ID)
                                    .FirstOrDefault();
                                var rem =
                                    ctx.ExecuteSyncronous(
                                        ctx.Remainders.Where(
                                            r =>
                                                r.PriceItem_Id == acceptIncomeItem.PriceItem_Id &&
                                                r.Warehouse_Id == App.CurrentUser.Warehouse_Id)).FirstOrDefault();

                                if (rem == null)
                                {
                                    rem = new Remainder();
                                    rem.Amount = acceptIncomeItem.Incomes;
                                    rem.RemainderDate = DateTimeHelper.GetNowKz();
                                    rem.PriceItem_Id = acceptIncomeItem.PriceItem_Id;
                                    rem.Warehouse_Id = App.CurrentUser.Warehouse_Id;
                                    ctx.AddToRemainders(rem);
                                }
                                else
                                {
                                    rem.Amount += acceptIncomeItem.Incomes;
                                    rem.RemainderDate = DateTimeHelper.GetNowKz();
                                    ctx.ChangeState(rem, EntityStates.Modified);
                                }
                                WholesalePrice rp = new WholesalePrice();
                                rp.Price = acceptIncomeItem.NewPrice;
                                rp.PriceDate = DateTimeHelper.GetNowKz();
                                rp.PriceItem_Id = acceptIncomeItem.PriceItem_Id;
                                ctx.AddToWholesalePrices(rp);
                                ctx.SaveChangesSynchronous();

                                findedIncomeItem.IsAccepted = true;
                                findedIncomeItem.Remainders += acceptIncomeItem.Incomes;
                                findedIncomeItem.WholesalePrice = acceptIncomeItem.NewPrice;
                                acceptIncomeItem.IsAccepted = true;
                            }
                            ctx.SaveChangesSynchronous();
                            CreatePriceChangeReport(incomeDb.Id);
                        }
                    };

                    ctrl.Show();
                }
                catch (Exception exception)
                {

                    throw;
                }

            });

            #endregion

            #region PriceChangeReportCommand

            PriceChangeReportCommand = new UICommand(a =>
            {
                if (SelectedIncomeItems.Count == 0) return;
                var selected = SelectedIncomeItems.First();
                if (!selected.IsAccepted) return;

                string uri = string.Concat(
                    Application.Current.Host.Source.Scheme, "://",
                    Application.Current.Host.Source.Host, ":",
                    Application.Current.Host.Source.Port,
                    "/StoreAppDataService.svc/");

                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));

                var acceptIncomeItems = IncomeItems.Where(i => i.Income_ID == selected.Income_ID).ToList();

                PriceChangeReportViewModel vm = new PriceChangeReportViewModel(_priceListName, acceptIncomeItems);

                var priceReportDb =
                    ctx.ExecuteSyncronous(ctx.PriceChangeReports.OrderByDescending(s => s.Id)).FirstOrDefault();
                if (priceReportDb != null)
                {
                    int lastNumber = 0;
                    if (int.TryParse(priceReportDb.ReportNumber, out lastNumber))
                    {
                        vm.PriceReportNumber = (++lastNumber).ToString();
                    }
                }
                else
                {
                    vm.PriceReportNumber = "1";
                }

                _newPriceChangeReportNeedEvent.Publish(vm);
            });

            #endregion

            #region PrintReportCommand

            PrintReportCommand = new UICommand(a =>
            {
                if (SelectedIncome != null)
                {
                    IncomeDocumentReportControl control = new IncomeDocumentReportControl(SelectedIncome.Income_ID);
                    control.Show();
                }
            });

            #endregion

            #region RefreshIncomesCommand

            RefreshIncomesCommand = new UICommand(a =>
            {
                UpdateIncomesList();
            });
            #endregion

        }
        #endregion

        private void CreatePriceChangeReport(long incomeId)
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

                var acceptIncomeItems = IncomeItems.Where(i => i.Income_ID == incomeId).ToList();

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


                foreach (var acceptIncomeItem in acceptIncomeItems)
                {
                    var item = acceptIncomeItem;
                    var prices =
                        ctx.ExecuteSyncronous(ctx.WholesalePrices.Where(r => r.PriceItem_Id == item.PriceItem_Id))
                            .ToList();

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

                    PriceChangeReportItem repItem = new PriceChangeReportItem()
                    {
                        PriceItem_Id = acceptIncomeItem.PriceItem_Id,
                        PreviousPrice_Id = previousRetailPrice.Id,
                        NewPrice_Id = currentRetailPrice.Id,
                        PriceChangeReport_Id = rep.Id
                    };

                    ctx.AddToPriceChangeReportItems(repItem);

                    rep.PriceChangeReportItems.Add(repItem);

                }

                ctx.SaveChangesSynchronous();

            });
        }

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

        public void NewIncomeAddedEventHandler(IList<IncomeItem> incomeItems)
        {
            foreach (var incomeItem in incomeItems)
            {
                //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                //{
                    IncomeItems.Add(incomeItem);
                //});
            }
        }

        public void UpdateIncomesList()
        {
            IsLoading = true;


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

                    //var qr = ctx.ExecuteSyncronous<PriceIncomeTotalItemView>(
                    //    new Uri(string.Format("{0}GetTotalIncomes?priceListName='{1}'", uri, _priceListName),
                    //        UriKind.Absolute));

                    RemaindersClient client = new RemaindersClient(_priceListName, AtFromDate, AtToDate);
                    var qr = client.GetIncomes();

                    //foreach (var priceItemRemainderView in 
                    var query = from p in qr
                                group p by new
                                {
                                    Articul = p.Articul,
                                    CatalogNumber = p.CatalogNumber,
                                    Income_ID = p.Income_Id,
                                    IsDuplicate = p.IsDuplicate ? "*" : "",
                                    Gear_Id = p.Gear_Id,
                                    Name = p.Gear_Name,
                                    PriceItem_Id = p.PriceItem_Id,
                                    Uom = p.Uom,
                                    IsAccepted = p.IsAccept
                                }
                                    into grp
                                    select new IncomeItem()

                                    //).Select(new IncomeItem()
                                    {
                                        Number = IncomeItems.Select(s => s.Number).LastOrDefault() + 1,
                                        Articul = grp.Key.Articul,
                                        BuyPriceRur = (int)grp.Average(a => a.BuyPriceRur),
                                        BuyPriceTng = (int)grp.Average(a => a.BuyPriceTng),
                                        CatalogNumber = grp.Key.CatalogNumber,
                                        Incomes = (int)grp.Average(s => s.Incomes),
                                        Income_ID = grp.Key.Income_ID,
                                        IsDuplicate = grp.Key.IsDuplicate,
                                        Gear_Id = grp.Key.Gear_Id,
                                        Name = grp.Key.Name,
                                        LowerLimitRemainder = grp.Average(a => a.LowerLimitRemainder),
                                        RecommendedRemainder = grp.Average(a => a.RecommendedRemainder),
                                        NewPrice = (int)grp.Average(a => a.NewPrice),
                                        PriceItem_Id = grp.Key.PriceItem_Id,
                                        Uom = grp.Key.Uom,
                                        WholesalePrice = (int)grp.Average(a => a.WholesalePrice),
                                        Remainders = (int)grp.Average(s => s.Remainders),
                                        IsAccepted = grp.Key.IsAccepted
                                    };

                    var incomeItems = query as IList<IncomeItem> ?? query.ToList();
                    foreach (var grp in incomeItems.GroupBy(g => g.Income_ID))
                    {
                        var grp1 = grp;
                        var inc = ctx.ExecuteSyncronous(ctx.Incomes.Where(w => w.Id == grp1.Key)).FirstOrDefault();
                        foreach (var incomeItem in grp1)
                        {
                            incomeItem.Income = string.Format("Оприходование № {0} от {1:dd.MM.yyyy}",
                                inc.IncomeNumber, inc.IncomeDate);
                        }
                    }
                    foreach (var incomeItem in incomeItems)
                    {
                        var item = incomeItem;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            IncomeItems.Add(item);
                        });
                    }
                    //}
                }
                catch (Exception exception)
                {
                    throw;
                }
            }).ContinueWith(c =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsLoading = false;
                });
            });
            
        }
    }
}
