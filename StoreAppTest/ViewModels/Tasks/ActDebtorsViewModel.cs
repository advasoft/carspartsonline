

namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
    using StoreAppDataService;
    using Utilities;

    public class ActDebtorsViewModel : ViewModelBase
    {
        private IEventAggregator _agregator;
        //private DebtorSelectedCustomerEvent _newRealizationEvent;
        private Customer _customer;

        public ActDebtorsViewModel(Customer customer)
        {
            _customer = customer;

            SaleDocuments = new ObservableCollection<LookupSalesDocumentModel>();
            DebtorItems = new ObservableCollection<DebtItem>();

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _agregator.GetEvent<DebtorSelectedCustomerEvent>().Subscribe(DebtorSelectedCustomerEventHandler);

            Init();
        }


        public ObservableCollection<LookupSalesDocumentModel> SaleDocuments { get; set; }
        public ObservableCollection<DebtItem> DebtorItems { get; set; }

        //private bool _SaleDocumentSelected;
        //public bool SaleDocumentSelected
        //{
        //    get
        //    {
        //        return SelectedSaleDocument != null;
        //    }
        //}


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
        //        OnPropertyChanged("SaleDocumentSelected");
        //        UpdateDebtList();
        //    }
        //}


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

        private decimal _DebtChangeAmount;

        public decimal DebtChangeAmount
        {
            get
            {
                return _DebtChangeAmount;
            }
            set
            {
                _DebtChangeAmount = value;
                OnPropertyChanged("DebtChangeAmount");
            }
        }
        #region Commands

        public ICommand UpDebtCommand { get; set; }
        public ICommand DownDebtCommand { get; set; }
        public ICommand ClearSaleDocumentCommand { get; set; }


        private void Init()
        {
            UpDebtCommand = new UICommand(o =>
            {
                #region check
                if (_customer == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Важно";
                        msch.Message =
                            string.Format(
                                "Не выбран должник");
                        msch.Show();
                    });
                    return;
                }
                if (DebtChangeAmount <= 0)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Важно";
                        msch.Message =
                            string.Format(
                                "Сумма увеличения долга не может быть 0");
                        msch.Show();
                    });
                    return;
                }
                #endregion

                InputDebtDischarge w = new InputDebtDischarge();
                w.DebtDirectionTextBlock.Text = "Увеличение";
                w.CustomerTextBlock.Text = _customer.Name;
                //w.SaleDocumentTextBlock.Text = SelectedSaleDocument.ToString();
                w.DebtAmountEdit.EditValue = DebtChangeAmount;

                w.Closed += (a, e) =>
                {
                    if (w.DialogResult == true)
                    {
                        DebtDischargeDocument doc = new DebtDischargeDocument();
                        doc.IsDischarge = false;
                        decimal val = 0;
                        decimal.TryParse(w.DebtAmountEdit.EditValue.ToString(), out val);
                        doc.Amount = val;
                        doc.Creator_Id = App.CurrentUser.UserName;
                        doc.Debtor_Id = _customer.Name;//SelectedSaleDocument.Customer;
                        doc.DischargeDate = DateTimeHelper.GetNowKz();
                        //doc.SaleDocument_Id = SelectedSaleDocument.SaleDocumentData.Id;
                        
                        string uri = string.Concat(
                            Application.Current.Host.Source.Scheme, "://",
                            Application.Current.Host.Source.Host, ":",
                            Application.Current.Host.Source.Port,
                            "/StoreAppDataService.svc/");


                        //Task.Factory.StartNew(() =>
                        //{
                            StoreDbContext ctx = new StoreDbContext(
                                new Uri(uri
                                    , UriKind.Absolute));

                            ctx.AddToDebtDischargeDocuments(doc);
                            ctx.SaveChangesSynchronous();

                            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            //{
                                DebtChangeAmount = 0;
                        //    });

                            
                        //}).ContinueWith(c =>
                        //{
                        //    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        //    {
                                UpdateDebtList();
                        //    });
                        //});

                    }
                };
                w.Show();
            });

            DownDebtCommand = new UICommand(o =>
            {
                #region check
                if (_customer == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Важно";
                        msch.Message =
                            string.Format(
                                "Не выбран должник");
                        msch.Show();
                    });
                    return;
                }
                if (DebtChangeAmount <= 0)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Важно";
                        msch.Message =
                            string.Format(
                                "Сумма уменьшения долга не может быть 0");
                        msch.Show();
                    });
                    return;
                }
                #endregion

                InputDebtDischarge w = new InputDebtDischarge();
                w.DebtDirectionTextBlock.Text = "Уменьшение";
                w.CustomerTextBlock.Text = _customer.Name;//SelectedSaleDocument.Customer;
                //w.SaleDocumentTextBlock.Text = SelectedSaleDocument.ToString();
                w.DebtAmountEdit.EditValue = DebtChangeAmount;

                w.Closed += (a, e) =>
                {
                    if (w.DialogResult == true)
                    {
                        DebtDischargeDocument doc = new DebtDischargeDocument();
                        doc.IsDischarge = true;
                        decimal val = 0;
                        decimal.TryParse(w.DebtAmountEdit.EditValue.ToString(), out val);
                        doc.Amount = val;
                        doc.Creator_Id = App.CurrentUser.UserName;
                        doc.Debtor_Id = _customer.Name;//SelectedSaleDocument.Customer;
                        doc.DischargeDate = DateTimeHelper.GetNowKz();
                        //doc.SaleDocument_Id = SelectedSaleDocument.SaleDocumentData.Id;

                        string uri = string.Concat(
                            Application.Current.Host.Source.Scheme, "://",
                            Application.Current.Host.Source.Host, ":",
                            Application.Current.Host.Source.Port,
                            "/StoreAppDataService.svc/");


                        //Task.Factory.StartNew(() =>
                        //{
                            StoreDbContext ctx = new StoreDbContext(
                                new Uri(uri
                                    , UriKind.Absolute));

                            ctx.AddToDebtDischargeDocuments(doc);
                            ctx.SaveChangesSynchronous();

                            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            //{
                                DebtChangeAmount = 0;
                        //    });

                        //}).ContinueWith(c =>
                        //{
                            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            //{
                                UpdateDebtList();
                        //    });
                        //});

                    }
                };
                w.Show();
            });

            ClearSaleDocumentCommand = new UICommand(o =>
            {
                //SelectedSaleDocument = null;
            });

        }

        #endregion

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            UpdateSalesDocumentsList();
            UpdateDebtList();
        }

        #endregion

        private void UpdateSalesDocumentsList()
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

                var query = ctx.SaleDocuments.Expand("SaleItems,Customer").Where(f => !f.IsOrder && f.IsInDebt && f.Creator_Id == App.CurrentUser.UserName);
                if (_customer != null)
                    query = query.Where(q => q.Customer_Name == _customer.Name);

                var salesDb =
                    ctx.ExecuteSyncronous(query).ToList();

                var returns =
                    ctx.ExecuteSyncronous(ctx.RefundDocuments.Expand("SaleDocument").Where(f => !f.SaleDocument.IsOrder && f.SaleDocument.IsInDebt && f.Creator_Id == App.CurrentUser.UserName))
                        .Select(s => s.SaleDocument_Id)
                        .ToList();


                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    SaleDocuments.Clear();

                    salesDb.Where(s => !returns.Contains(s.Id)).ForEach(i =>
                    {
                        SaleDocuments.Add(new LookupSalesDocumentModel()
                        {
                            Amount = i.SaleItems.Sum(s => s.Amount),
                            Customer = i.Customer.Name,
                            Number = i.Number,
                            SaleDate = i.SaleDate,
                            SaleDocumentData = i
                        });
                    });

                });
            });


        }

        private void UpdateDebtList()
        {
            string uri = "";

            IsLoading = true;
            

            DebtorItems.Clear();

            uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            Task.Factory.StartNew(() =>
            {
                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));

                List<SaleDocument> salesDb = null;
                var query = ctx.SaleDocuments.Expand("SaleItems,Customer").Where(f => f.IsInDebt && f.Creator_Id == App.CurrentUser.UserName);
                if (_customer != null)
                    query = query.Where(c => c.Customer_Name == _customer.Name);
                //if (SelectedSaleDocument != null)
                //    query = query.Where(q => q.Id == SelectedSaleDocument.SaleDocumentData.Id);

                salesDb =
                    ctx.ExecuteSyncronous(query).ToList();

                var returns =
                    ctx.ExecuteSyncronous(ctx.RefundDocuments.Where(f => f.SaleDocument.IsInDebt && f.Creator_Id == App.CurrentUser.UserName))
                        .Select(s => s.SaleDocument_Id)
                        .ToList();

                List<DebtItem> salesDebtItems = new List<DebtItem>();
                salesDb.Where(s => !returns.Contains(s.Id)).ForEach(i =>
                {
                    salesDebtItems.Add(new DebtItem()
                    {
                        Date = i.SaleDate,
                        Debtor = i.Customer_Name,
                        Up = (int)i.SaleItems.Sum(s => s.Amount),
                        SaleDocument = new LookupSalesDocumentModel() { Number = i.Number, SaleDate = i.SaleDate, Amount = i.SaleItems.Sum(s => s.Amount), Customer = i.Customer.Name, SaleDocumentData = i }
                    });

                });


                List<DebtDischargeDocument> dischDb = null;
                IQueryable<DebtDischargeDocument> queryDisch = ctx.DebtDischargeDocuments.Where(f => f.Creator_Id == App.CurrentUser.UserName);

                if (_customer != null)
                    queryDisch = queryDisch.Where(c => c.Debtor_Id == _customer.Name);
                //if (SelectedSaleDocument != null)
                //    queryDisch = queryDisch.Where(q => q.SaleDocument_Id == SelectedSaleDocument.SaleDocumentData.Id);

                dischDb =
                    ctx.ExecuteSyncronous(queryDisch).ToList();

                dischDb.ForEach(i =>
                {
                    salesDebtItems.Add(new DebtItem()
                    {
                        Date = i.DischargeDate,
                        Debtor = i.Debtor_Id,
                        Down = (int)(i.IsDischarge ? i.Amount : 0),
                        Up = (int)(i.IsDischarge ? 0 : i.Amount),
                        //SaleDocument = new LookupSalesDocumentModel() { Number = i.SaleDocument.Number, SaleDate = i.SaleDocument.SaleDate, Amount = i.SaleDocument.SaleItems.Sum(s => s.Amount), Customer = i.SaleDocument.Customer.Name, SaleDocumentData = i.SaleDocument },
                        DebtDischargeDocument = i
                    });

                });

                int number = 1;
                salesDebtItems.OrderBy(o => o.Date).ForEach(f =>
                {
                    f.Number = number++;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        DebtorItems.Add(f);
                    });
                });

            }).ContinueWith(t =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsLoading = false;
                });
            });


        }

        public void DebtorSelectedCustomerEventHandler(Customer cust)
        {
            _customer = cust;
            UpdateSalesDocumentsList();
            UpdateDebtList();
        }
    }
}
