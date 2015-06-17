
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using advasoft.sltools.menu;
    using Controls;
    using DevExpress.Xpf.Editors.Helpers;
    using Event;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using StoreAppDataService;
    using Tasks;
    using Utilities;
    using Views;
    using Views.Tasks;
    using PriceChangeReport = Views.PriceChangeReport;
    using Receipt = Views.Receipt;
    using Refund = Views.Refund;
    using RefundItem = Model.RefundItem;

    public class TasksViewModel : ViewModelBase, IDisposable
    {
        private TasksView _view;
        private IEventAggregator _agregator;

        public TasksViewModel(TasksView view)
        {
            _view = view;
            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();


            _agregator.GetEvent<NewRealizationNeedEvent>().Unsubscribe(NewRealizationNeedEventHandler);
            _agregator.GetEvent<NewReceiptNeedEvent>().Unsubscribe(NewReceiptNeedEventHandler);
            _agregator.GetEvent<NewRefundNeedEvent>().Unsubscribe(NewRefundNeedEventHandler);
            _agregator.GetEvent<CloseViewNeedEvent>().Unsubscribe(CloseViewNeedEventHandler);
            _agregator.GetEvent<RequestCloseTabEvent>().Unsubscribe(RequestCloseTabEventHandler);
            _agregator.GetEvent<ShowDebtorsEvent>().Unsubscribe(ShowDebtorsEventHandler);
            _agregator.GetEvent<NewPriceListLoadedEvent>().Unsubscribe(NewPriceListLoadedEventHandler);
            _agregator.GetEvent<NewIncomeNeedEvent>().Unsubscribe(NewIncomeNeedEventHandler);
            _agregator.GetEvent<NewWarehouseTransferRequestNeedEvent>().Unsubscribe(NewWarehouseTransferRequestNeedEvent);

            _agregator.GetEvent<NewPriceChangeReportNeedEvent>().Unsubscribe(NewPriceChangeReportNeedEventHandler);





            _agregator.GetEvent<NewRealizationNeedEvent>().Subscribe(NewRealizationNeedEventHandler);
            _agregator.GetEvent<NewReceiptNeedEvent>().Subscribe(NewReceiptNeedEventHandler);
            _agregator.GetEvent<NewRefundNeedEvent>().Subscribe(NewRefundNeedEventHandler);
            _agregator.GetEvent<CloseViewNeedEvent>().Subscribe(CloseViewNeedEventHandler);
            _agregator.GetEvent<RequestCloseTabEvent>().Subscribe(RequestCloseTabEventHandler);
            _agregator.GetEvent<ShowDebtorsEvent>().Subscribe(ShowDebtorsEventHandler);
            _agregator.GetEvent<NewPriceListLoadedEvent>().Subscribe(NewPriceListLoadedEventHandler);
            _agregator.GetEvent<NewIncomeNeedEvent>().Subscribe(NewIncomeNeedEventHandler);
            _agregator.GetEvent<NewWarehouseTransferRequestNeedEvent>().Subscribe(NewWarehouseTransferRequestNeedEvent);
            
            _agregator.GetEvent<NewPriceChangeReportNeedEvent>().Subscribe(NewPriceChangeReportNeedEventHandler);
            _agregator.GetEvent<RealizationPerDayOpenNeedEvent>().Subscribe(RealizationPerDayOpenNeedEventHandler);


            PriceListsTabs = new ObservableCollection<MenuItem>();
            RemaindersTabs = new ObservableCollection<MenuItem>();
            IncomesTabs = new ObservableCollection<MenuItem>();

            InitCommands();
        }

        public ObservableCollection<MenuItem> PriceListsTabs { get; set; }
        public ObservableCollection<MenuItem> RemaindersTabs { get; set; }
        public ObservableCollection<MenuItem> IncomesTabs { get; set; } 

        #region Commands
        public ICommand ShowPriceCommand { get; set; }
        public ICommand ShowRemaindersCommand { get; set; }
        public ICommand ShowIncomesCommand { get; set; }
        public ICommand ShowPriceChangesCommands { get; set; }
        public ICommand ShowWarehouseTransfersCommands { get; set; }

        public ICommand ShowRemaindersAmountCommands { get; set; }

        public ICommand ShowCashFlowCommands { get; set; }
        public ICommand ShowCashByCashierFlowCommands { get; set; }

        public ICommand ShowRealizationPerDayListByCashierCommands { get; set; }
        public ICommand ShowRealizationPerDayListCommands { get; set; }


        private void InitCommands()
        {
            
            ShowPriceCommand = new UICommand((o =>
            {
                var priceName = o as string;

                Prices ps = new Prices();
                PricesViewModel psv = new PricesViewModel(priceName);
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Прайс - " + priceName;
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
                
            }));

            ShowRemaindersCommand = new UICommand((o =>
            {
                var priceName = o as string;

                Remainders ps = new Remainders();
                RemaindersViewModel psv = new RemaindersViewModel(priceName, ps);
                //RemaindersViewModel psv = new RemaindersViewModel(ps);
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Остатки - " + priceName;
                //tb.Header = "Остатки";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            }));

            ShowIncomesCommand = new UICommand((o =>
            {
                var priceName = o as string;

                IncomesPage ps = new IncomesPage();
                IncomesViewModel psv = new IncomesViewModel(priceName, ps);
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Приход - " + priceName;
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            }));

            ShowPriceChangesCommands = new UICommand(a =>
            {
                PriceChangeReportsPage ps = new PriceChangeReportsPage();
                PriceChangeReportsViewModel psv = new PriceChangeReportsViewModel();
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Изменения цен";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            });

            ShowWarehouseTransfersCommands = new UICommand(a =>
            {
                WarehouseTransferRequestsPage ps = new WarehouseTransferRequestsPage();
                WarehouseTransferRequestsViewModel psv = new WarehouseTransferRequestsViewModel();
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Перемещения";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            });

            ShowRemaindersAmountCommands = new UICommand(a =>
            {
                RemaindersWarehouseAmountView ps = new RemaindersWarehouseAmountView();
                RemaindersWarehouseAmountViewModel psv = new RemaindersWarehouseAmountViewModel();
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Стоимость остатков";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            });

            ShowCashFlowCommands = new UICommand(a =>
            {
                CashFlowPage ps = new CashFlowPage();
                CashFlowViewModel psv = new CashFlowViewModel();
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Отчет по денежным средствам";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            });
            ShowCashByCashierFlowCommands = new UICommand(a =>
            {
                CashFlowPage ps = new CashFlowPage();
                CashFlowByCashierViewModel psv = new CashFlowByCashierViewModel();
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Отчет по денежным средствам";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            });
            ShowRealizationPerDayListByCashierCommands = new UICommand(a =>
            {
                RealizationPerDayList ps = new RealizationPerDayList();
                RealizationPerDyaListViewModel psv = new RealizationPerDyaListViewModel(true);
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Реализации за день";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            });
            ShowRealizationPerDayListCommands = new UICommand(a =>
            {
                RealizationPerDayList ps = new RealizationPerDayList();
                RealizationPerDyaListViewModel psv = new RealizationPerDyaListViewModel();
                ps.DataContext = psv;

                ClosableTabItem tb = new ClosableTabItem();
                tb.Header = "Реализации за день";
                tb.Content = ps;

                _view.TasksTabControl.Items.Add(tb);
                _view.TasksTabControl.SelectedItem = tb;
                psv.LoadView();
            });
        }
        #endregion

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            PriceListsTabs.Clear();
            RemaindersTabs.Clear();
            IncomesTabs.Clear();

            string path = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            //var th = new Thread(() =>
            //{
                StoreDbContext ctx = new StoreDbContext(
                    new Uri(path
                        , UriKind.Absolute));





                var priceLists =
                    ctx.ExecuteSyncronous(ctx.PriceLists).ToList();

                IEnumerable<PriceList> query = priceLists;
                //if (App.CurrentUser.UserName != "admin")
                //{
                //    //var users =
                //    //    ctx.ExecuteSyncronous(ctx.Users.Where(w => w.Warehouse_Id == App.CurrentUser.Warehouse_Id))
                //    //        .ToList()
                //    //        .Select(s => s.UserName)
                //    //        .Distinct();

                //    query = priceLists;
                //}
                bool isFisrt = true;
                query.ForEach(p =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MenuItem item = new MenuItem();
                        item.MenuText = p.Name;
                        item.Width = 150;
                        item.Command = ShowPriceCommand;
                        item.CommandParameter = p.Name;
                        item.Background = new SolidColorBrush(Color.FromArgb(255, 95, 95, 95));
                        if(!isFisrt)
                            item.Margin = new Thickness(0,0.5,0,0);
                        //item.Style = (Style)_view.Resources["MainMenuItemsStyle"];
                        PriceListsTabs.Add(item);

                        MenuItem item1 = new MenuItem();
                        item1.MenuText = p.Name;
                        item1.Width = 150;
                        item1.Command = ShowRemaindersCommand;
                        item1.CommandParameter = p.Name;
                        item1.Background = new SolidColorBrush(Color.FromArgb(255, 95, 95, 95));
                        if (!isFisrt)
                            item1.Margin = new Thickness(0, 0.5, 0, 0);
                        //item.Style = (Style)_view.Resources["MainMenuItemsStyle"];
                        RemaindersTabs.Add(item1);

                        MenuItem item2 = new MenuItem();
                        item2.MenuText = p.Name;
                        item2.Width = 150;
                        item2.Command = ShowIncomesCommand;
                        item2.CommandParameter = p.Name;
                        item2.Background = new SolidColorBrush(Color.FromArgb(255, 95, 95, 95));
                        if (!isFisrt)
                            item2.Margin = new Thickness(0, 0.5, 0, 0);
                        //item.Style = (Style)_view.Resources["MainMenuItemsStyle"];
                        IncomesTabs.Add(item2);


                        isFisrt = false;

                    });
                });
            //}) { IsBackground = true };
            //th.Start();
            //th.Join();

        }

        #endregion

        public void CloseViewNeedEventHandler(Guid id)
        {
            var tab = _view.TasksTabControl.Items.Where(t => ((TabItem) t).Name == id.ToString()).FirstOrDefault();
            if (tab != null)
            _view.TasksTabControl.Items.Remove(tab);
        }
        public void NewRealizationNeedEventHandler(object obj)
        {
            Guid viewId = Guid.NewGuid();
            RealizationPerDay ps = new RealizationPerDay();
            RealizationPerDayViewModel vm = new RealizationPerDayViewModel(viewId);
            ps.DataContext = vm;

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Реализация за день № " + vm.RealizationNumber;
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;
            vm.LoadView();

        }

        public void RealizationPerDayOpenNeedEventHandler(RealizationPerDayDocumentModel model)
        {
            Guid viewId = Guid.NewGuid();
            RealizationPerDay ps = new RealizationPerDay();
            ps.CloseRealizationButton.Visibility = Visibility.Collapsed;
            ps.RealizationBarcode.IsEnabled = false;
            ps.RealizationNumberTextBox.IsEnabled = false;

            RealizationPerDayReadOnlyViewModel vm = new RealizationPerDayReadOnlyViewModel(viewId, model.Id);
            vm.RealizationNumber = model.DocumentNumber;
            vm.Barcode = model.Barcode;

            ps.DataContext = vm;

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Реализация за день № " + vm.RealizationNumber;
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;
            vm.LoadView();
        }

        public void NewIncomeNeedEventHandler(IncomeViewModell obj)
        {
            Guid viewId = Guid.NewGuid();
            IncomPage ps = new IncomPage();
            IncomeViewModell vm = obj;
            vm.View = ps;
            vm.ViewId = viewId;
            
            ps.DataContext = vm;

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Оприходование № " + vm.IncomeNumber;
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;
            vm.LoadView();

        }

        public void NewWarehouseTransferRequestNeedEvent(WarehouseTransferRequestViewModel obj)
        {
            Guid viewId = Guid.NewGuid();
            WarehouseTransferRequestPage ps = new WarehouseTransferRequestPage();
            WarehouseTransferRequestViewModel vm = obj;
            vm.View = ps;
            vm.ViewId = viewId;

            ps.DataContext = vm;

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Перемещение № " + vm.RequestNumber;
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;
            vm.LoadView();
        }

        public void NewPriceChangeReportNeedEventHandler(PriceChangeReportViewModel obj)
        {
            Guid viewId = Guid.NewGuid();
            PriceChangeReport ps = new PriceChangeReport();
            PriceChangeReportViewModel vm = obj;
            vm.View = ps;
            vm.ViewId = viewId;

            ps.DataContext = vm;

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Отчет об изменении цен № " + vm.PriceReportNumber;
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;
            vm.LoadView();

        }

        public void NewReceiptNeedEventHandler(Model.Receipt receipt)
        {
            Guid viewId = Guid.NewGuid();

            Receipt ps = new Receipt();
            ReceiptViewModel vm = new ReceiptViewModel(receipt, viewId, receipt.PriceListName);
            ps.DataContext = vm;

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Товарный чек № " + vm.ReceiptNumber;
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;
            vm.LoadView();
        }
        public void NewRefundNeedEventHandler(Model.Refund refundModel)
        {
            Guid viewId = Guid.NewGuid();

            Refund ps = new Refund();
            RefundViewModel vm = new RefundViewModel(refundModel, viewId);
            ps.DataContext = vm;

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Возврат покупателя № ";
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;

            vm.LoadView();
        }
        public void RequestCloseTabEventHandler(ClosableTabItem tab)
        {
            //TODO: Check content
            _view.TasksTabControl.Items.Remove(tab);
        }
        public void ShowDebtorsEventHandler(object data)
        {
            Guid viewId = Guid.NewGuid();

            Debtors ps = new Debtors();

            ClosableTabItem tb = new ClosableTabItem();
            tb.Header = "Должники";
            tb.Content = ps;
            tb.Name = viewId.ToString();

            _view.TasksTabControl.Items.Add(tb);
            _view.TasksTabControl.SelectedItem = tb;

        }
        public void NewPriceListLoadedEventHandler(Price newPriceList)
        {
            MenuItem item = new MenuItem();
            item.MenuText = newPriceList.PriceName;
            item.Width = 150;
            item.Command = ShowPriceCommand;
            item.CommandParameter = newPriceList.PriceName;
            item.Background = new SolidColorBrush(Color.FromArgb(255, 95, 95, 95));
            if (PriceListsTabs.Count > 0)
                item.Margin = new Thickness(0, 0.5, 0, 0);
            //item.Style = (Style)_view.Resources["MainMenuItemsStyle"];
            PriceListsTabs.Add(item);

            MenuItem item1 = new MenuItem();
            item1.MenuText = newPriceList.PriceName;
            item1.Width = 150;
            item1.Command = ShowRemaindersCommand;
            item1.CommandParameter = newPriceList.PriceName;
            item1.Background = new SolidColorBrush(Color.FromArgb(255, 95, 95, 95));
            if (RemaindersTabs.Count > 0)
                item1.Margin = new Thickness(0, 0.5, 0, 0);
            //item.Style = (Style)_view.Resources["MainMenuItemsStyle"];
            RemaindersTabs.Add(item1);

            MenuItem item2 = new MenuItem();
            item2.MenuText = newPriceList.PriceName;
            item2.Width = 150;
            item2.Command = ShowIncomesCommand;
            item2.CommandParameter = newPriceList.PriceName;
            item2.Background = new SolidColorBrush(Color.FromArgb(255, 95, 95, 95));
            if (IncomesTabs.Count > 0)
                item2.Margin = new Thickness(0, 0.5, 0, 0);
            //item.Style = (Style)_view.Resources["MainMenuItemsStyle"];
            IncomesTabs.Add(item2);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            
        }

        #endregion
    }
}
