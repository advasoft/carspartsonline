
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

    public class DebtorListViewModel : ViewModelBase
    {
        private IEventAggregator _agregator;
        private Customer _customer;

        public DebtorListViewModel(Customer customer)
        {
            _customer = customer;

            DebtorItems = new ObservableCollection<DebtRemainderItem>();

            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _agregator.GetEvent<DebtorSelectedCustomerEvent>().Subscribe(DebtorSelectedCustomerEventHandler);

            Init();
        }

        public ObservableCollection<DebtRemainderItem> DebtorItems { get; set; }

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

        #region Commands

        private void Init()
        {

        }

        #endregion

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            UpdateDebtList();
        }

        #endregion

        private void UpdateDebtList()
        {
            IsLoading = true;
            DebtorItems.Clear();

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

                List<SaleDocument> salesDb = null;
                var query = ctx.SaleDocuments.Expand("SaleItems,Customer").Where(f => f.IsInDebt && f.Creator_Id == App.CurrentUser.UserName);
                if (_customer != null)
                    query = query.Where(c => c.Customer_Name == _customer.Name);

                salesDb =
                    ctx.ExecuteSyncronous(query).ToList();

                IList<StoreAppDataService.RefundItem> refs = default(IList<StoreAppDataService.RefundItem>);

                var refundsDb =
                    ctx.ExecuteSyncronous(ctx.RefundDocuments.Expand("RefundItems").Where(f => f.SaleDocument.IsInDebt && f.Creator_Id == App.CurrentUser.UserName))
                        .ToList();

                if (refundsDb != null)
                {
                    refs = refundsDb.SelectMany(s => s.RefundItems).ToList();
                }

                List<DebtItem> salesDebtItems = new List<DebtItem>();
                salesDb.ForEach(i =>
                {

                    if (refs == null || !refs.Any(v => v.SaleItem_Id == i.Id) ||
                        (refs.Any(v => v.SaleItem_Id == i.Id) &&
                         refs.Where(wh => wh.SaleItem_Id == i.Id).Sum(sm => sm.Count) < i.SaleItems.Sum(sm => sm.Count)))
                    {
                        var @return =
                            (int) refs.Where(wh => wh.SaleItem_Id == i.Id).Sum(sm => (sm.Count*sm.Price) - sm.Discount);
                        salesDebtItems.Add(new DebtItem()
                        {
                            Date = i.SaleDate,
                            Debtor = i.Customer_Name,
                            Up = (int)i.SaleItems.Sum(s => s.Amount) - @return,
                            SaleDocument =
                                new LookupSalesDocumentModel()
                                {
                                    Number = i.Number,
                                    SaleDate = i.SaleDate,
                                    Amount = i.SaleItems.Sum(s => s.Amount) - @return,
                                    Customer = i.Customer.Name,
                                    SaleDocumentData = i
                                }
                        });
                    }
                });


                List<DebtDischargeDocument> dischDb = null;
                IQueryable<DebtDischargeDocument> queryDisch = ctx.DebtDischargeDocuments;

                if (_customer != null)
                    queryDisch = queryDisch.Where(c => c.Debtor_Id == _customer.Name);

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
                salesDebtItems.OrderBy(o => o.Date).GroupBy(gp => new
                {
                    Customer = gp.Debtor

                }).Select(s => new DebtRemainderItem()
                {
                    Amount = s.Sum(a => a.Res),
                    Customer = s.Key.Customer
                })
                        
                .ForEach(f =>
                {
                    f.Number = number++;
                    if (f.Amount > 0)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            DebtorItems.Add(f);
                        });
                    }
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
            UpdateDebtList();
        }
    }
}
