

namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Controls;
    using GalaSoft.MvvmLight.Threading;
    using StoreAppDataService;
    using Utilities;

    public class CashFlowByCashierViewModel : ViewModelBase
    {
        public CashFlowByCashierViewModel()
        {
            CashFlowItems = new ObservableCollection<CashFlowItemModel>();
            InitCommands();
        }

        public PeriodTypes SelectedPeriodType
        {
            get { return _selectedPeriodType; }
            set
            {
                _selectedPeriodType = value;
                OnPropertyChanged("SelectedPeriodType");
                OnPropertyChanged("AtDatePresentation");
                var now = DateTime.Now;
                switch (value)
                {
                    case PeriodTypes.Day:
                        AtFromDate = DateTimeHelper.GetStartDay(now);
                        AtToDate = DateTimeHelper.GetEndDay(now);
                        PreviousAtFromDate = DateTimeHelper.GetPreviousStartDay(now);
                        PreviousAtToDate = DateTimeHelper.GetPreviousEndDay(now);
                        UpdateData();
                        break;
                    case PeriodTypes.Week:
                        AtFromDate = DateTimeHelper.GetStartWeek(now);
                        AtToDate = DateTimeHelper.GetEndWeek(now);
                        PreviousAtFromDate = DateTimeHelper.GetPreviousStartWeek(now);
                        PreviousAtToDate = DateTimeHelper.GetPreviousEndWeek(now);
                        UpdateData();
                        break;
                    case PeriodTypes.Month:
                        AtFromDate = DateTimeHelper.GetStartMonth(now);
                        AtToDate = DateTimeHelper.GetEndMonth(now);
                        PreviousAtFromDate = DateTimeHelper.GetPreviousStartMonth(now);
                        PreviousAtToDate = DateTimeHelper.GetPreviousEndMonth(now);
                        UpdateData();
                        break;
                    case PeriodTypes.HalfYear:
                        AtFromDate = DateTimeHelper.GetStartHalfYear(now);
                        AtToDate = DateTimeHelper.GetEndHalfYear(now);
                        PreviousAtFromDate = DateTimeHelper.GetPreviousStartHalfYear(now);
                        PreviousAtToDate = DateTimeHelper.GetPreviousEndHalfYear(now);
                        UpdateData();
                        break;
                    case PeriodTypes.Year:
                        AtFromDate = DateTimeHelper.GetStartYear(now);
                        AtToDate = DateTimeHelper.GetEndYear(now);
                        PreviousAtFromDate = DateTimeHelper.GetPreviousStartYear(now);
                        PreviousAtToDate = DateTimeHelper.GetPreviousEndYear(now);
                        UpdateData();
                        break;
                    case PeriodTypes.Custom:
                        break;
                }
                
            }
        }
        private PeriodTypes _selectedPeriodType;

        public bool IsCustomPeriod
        {
            get { return _isCustomPeriod; }
            set
            {
                _isCustomPeriod = value;
                OnPropertyChanged("IsCustomPeriod");
                if(value)
                    SelectedPeriodType = PeriodTypes.Custom;
                else
                    SelectedPeriodType = PeriodTypes.Day;
            }
        }
        private bool _isCustomPeriod;


        public DateTime AtFromDate
        {
            get { return _atFromDate; }
            set
            {
                _atFromDate = value;
                OnPropertyChanged("AtFromDate");
            }
        }
        private DateTime _atFromDate;


        public DateTime AtToDate
        {
            get { return _atToDate; }
            set
            {
                _atToDate = value;
                OnPropertyChanged("AtToDate");
            }
        }
        private DateTime _atToDate;


        public DateTime PreviousAtFromDate { get; set; }
        public DateTime PreviousAtToDate { get; set; }

        public string AtDatePresentation
        {
            get
            {
                string result = string.Empty;
                switch (SelectedPeriodType)
                {
                    case PeriodTypes.Day:
                        result = "За день";
                        break;
                    case PeriodTypes.Week:
                        result = "За неделю";
                        break;
                    case PeriodTypes.Month:
                        result = "За месяц";
                        break;
                    case PeriodTypes.HalfYear:
                        result = "За полгода";
                        break;
                    case PeriodTypes.Year:
                        result = "За год";
                        break;
                    case PeriodTypes.Custom:
                        result = "За произвольный период";
                        break;
                }
                return result;
            }
        }


        public ObservableCollection<CashFlowItemModel> CashFlowItems { get; set; }

        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            SelectedPeriodType = PeriodTypes.Day;
        }

        #endregion

        #region Commands

        public ICommand SetDayPeriodCommand { get; set; }
        public ICommand SetWeekPeriodCommand { get; set; }
        public ICommand SetMonthPeriodCommand { get; set; }
        public ICommand SetHalfYearPeriodCommand { get; set; }
        public ICommand SetYearPeriodCommand { get; set; }

        public ICommand RefreshCustomPeriodDataCommand { get; set; }

        private void InitCommands()
        {
            SetDayPeriodCommand = new UICommand(a =>
            {
                SelectedPeriodType = PeriodTypes.Day;    
            });
            SetWeekPeriodCommand = new UICommand(a =>
            {
                SelectedPeriodType = PeriodTypes.Week;
            });
            SetMonthPeriodCommand = new UICommand(a =>
            {
                SelectedPeriodType = PeriodTypes.Month;
            });
            SetHalfYearPeriodCommand = new UICommand(a =>
            {
                SelectedPeriodType = PeriodTypes.HalfYear;
            });
            SetYearPeriodCommand = new UICommand(a =>
            {
                SelectedPeriodType = PeriodTypes.Year;
            });
            RefreshCustomPeriodDataCommand = new UICommand(a =>
            {
                AtFromDate = DateTimeHelper.GetStartDay(AtFromDate);
                AtToDate = DateTimeHelper.GetEndDay(AtToDate);
                var now = AtFromDate;
                var dayDiffrerence = AtToDate - AtFromDate;
                var days = dayDiffrerence.Days + 1;
                var startNewDate = DateTimeHelper.GetStartDay(now.AddDays(-1*days));
                var endNewDate = DateTimeHelper.GetEndDay(startNewDate.AddDays(dayDiffrerence.Days));
                PreviousAtFromDate = startNewDate;
                PreviousAtToDate = endNewDate;

                UpdateData();
            });
        }

        #endregion


        public void UpdateData()
        {
            CashFlowItems.Clear();

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

                    StoreDbContext ctxRefund = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                    StoreDbContext debtCtx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                    var realizations = ctx.ExecuteSyncronous(
                        ctx.SalesPerDayItems
                        .Expand("SaleDocumentsPerDay,SaleItem/SaleDocument/Creator,SaleItem/PriceItem/Prices")
                        .Where(w => w.SaleDocumentsPerDay.SaleDocumentsDate >= AtFromDate 
                            &&  w.SaleDocumentsPerDay.SaleDocumentsDate <= AtToDate
                            &&  w.SaleDocumentsPerDay.Creator_Id == App.CurrentUser.UserName)).ToList();//SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices

                    var previousRealizations = ctx.ExecuteSyncronous(
                        ctx.SalesPerDayItems
                        .Expand("SaleDocumentsPerDay,SaleItem/SaleDocument/Creator,SaleItem/PriceItem/Prices")
                        .Where(w => w.SaleDocumentsPerDay.SaleDocumentsDate >= PreviousAtFromDate 
                            &&  w.SaleDocumentsPerDay.SaleDocumentsDate <= PreviousAtToDate
                            &&  w.SaleDocumentsPerDay.Creator_Id == App.CurrentUser.UserName)).ToList();//SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices

                    var tempGroupedRealzation =
                        from r in realizations
                            select new
                            {
                                Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                                Sales = r.Amount,
                                WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                                ? r.SaleItem.PriceItem.Prices.Where(
                                                    p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                                    .OrderByDescending(or => or.PriceDate)
                                                    .First()
                                                    .Price
                                                  : 0)
                            };

                    var groupedRealization = from r in tempGroupedRealzation
                                             group r by r.Cashier
                                                 into cshrs
                                                 select new
                                                 {
                                                     Cashier = cshrs.Key,
                                                     Sales = cshrs.Sum(s => s.Sales),
                                                     WholesaleSales = cshrs.Sum(s => s.WholesaleSales)
                                                 };


                    var tempGroupedPreviousRealization =
                        from r in previousRealizations
                        select new
                        {
                            Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                            Sales = r.Amount,
                            WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                            ? r.SaleItem.PriceItem.Prices.Where(
                                                p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                                .OrderByDescending(or => or.PriceDate)
                                                .First()
                                                .Price
                                              : 0)
                        };
                    var groupedPreviousRealization = from r in tempGroupedPreviousRealization
                                                     group r by r.Cashier
                                                         into cshrs
                                                         select new
                                                         {
                                                             Cashier = cshrs.Key,
                                                             Sales = cshrs.Sum(s => s.Sales),
                                                             WholesaleSales = cshrs.Sum(s => s.WholesaleSales)
                                                         };

                    var realization = from r in groupedRealization
                                      join pr in groupedPreviousRealization on r.Cashier equals pr.Cashier into prvs
                                      from previous in prvs.DefaultIfEmpty()
                                      select new
                                      {
                                          Cashier = r.Cashier,
                                          Sales = r.Sales,
                                          WholesaleSales = r.WholesaleSales,
                                          PreviousSales = previous != null ? previous.Sales : 0,
                                          PreviousWholesaleSales = previous != null ? previous.WholesaleSales : 0
                                      };


                    var refunds = ctxRefund.ExecuteSyncronous(
                        ctx.RefundItems.Expand("RefundDocument/SaleDocument,SaleItem/SaleDocument/Creator,SaleItem/PriceItem/Prices") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                            .Where(
                                w =>
                                    w.RefundDocument.RefundDate >= AtFromDate 
                                    && w.RefundDocument.RefundDate <= AtToDate
                                    && !w.RefundDocument.SaleDocument.IsInDebt
                                    && w.RefundDocument.Creator_Id == App.CurrentUser.UserName)).ToList();

                    var previousRefunds = ctxRefund.ExecuteSyncronous(
                        ctx.RefundItems.Expand("RefundDocument/SaleDocument,SaleItem/SaleDocument/Creator,SaleItem/PriceItem/Prices") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                            .Where(
                                w =>
                                    w.RefundDocument.RefundDate >= PreviousAtFromDate 
                                    && w.RefundDocument.RefundDate <= PreviousAtToDate
                                    && !w.RefundDocument.SaleDocument.IsInDebt
                                    && w.RefundDocument.Creator_Id == App.CurrentUser.UserName)).ToList();


                     var tempGroupedRefunds =
                         from r in refunds
                         select new
                         {
                             Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                             Refunds = r.Amount,
                             WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                             ? r.SaleItem.PriceItem.Prices.Where(
                                                 p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                                 .OrderByDescending(or => or.PriceDate)
                                                 .First()
                                                 .Price
                                               : 0)
                         };
                     var groupedRefunds = from r in tempGroupedRefunds
                                          group r by r.Cashier
                                              into cshrs
                                              select new
                                              {
                                                  Cashier = cshrs.Key,
                                                  Refunds = cshrs.Sum(s => s.Refunds),
                                                  WholesaleRefunds = cshrs.Sum(s => s.WholesaleSales)

                                              };


                     var tempGroupedPreviousRefunds =
                         from r in previousRefunds
                         select new
                         {
                             Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                             Refunds = r.Amount,
                             WholesaleSales = r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                             ? r.SaleItem.PriceItem.Prices.Where(
                                                 p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                                 .OrderByDescending(or => or.PriceDate)
                                                 .First()
                                                 .Price
                                               : 0)
                         };

                     var groupedPreviousRefunds = from r in tempGroupedPreviousRefunds
                                                  group r by r.Cashier
                                                      into cshrs
                                                      select new
                                                      {
                                                          Cashier = cshrs.Key,
                                                          Refunds = cshrs.Sum(s => s.Refunds),
                                                          WholesaleRefunds = cshrs.Sum(s => s.WholesaleSales)

                                                      };

                     var refund = from r in groupedRefunds
                                  join pr in groupedPreviousRefunds on r.Cashier equals pr.Cashier into prvs
                                  from previous in prvs.DefaultIfEmpty()
                                  select new
                                  {
                                      Cashier = r.Cashier,
                                      Refunds = r.Refunds,
                                      WholesaleRefunds = r.WholesaleRefunds,
                                      PreviousRefunds = previous != null ? previous.Refunds : 0,
                                      PreviousWholesaleRefunds = previous != null ? previous.WholesaleRefunds : 0
                                  };


                     //var debts = debtCtx.ExecuteSyncronous(
                     //   debtCtx.SaleItems.Expand("SaleDocument/Creator,PriceItem/Prices") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                     //       .Where(
                     //           w =>
                     //               w.SaleDocument.SaleDate >= AtFromDate 
                     //               && w.SaleDocument.SaleDate <= AtToDate
                     //               && w.SaleDocument.IsInDebt
                     //               && w.SaleDocument.Creator_Id == App.CurrentUser.UserName)).ToList();
                    var debts = realizations.Where(r => r.SaleItem.SaleDocument.IsInDebt);

                     //var previousDebts = debtCtx.ExecuteSyncronous(
                     //   debtCtx.SaleItems.Expand("SaleDocument/Creator,PriceItem/Prices") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                     //       .Where(
                     //           w =>
                     //               w.SaleDocument.SaleDate >= PreviousAtFromDate 
                     //               && w.SaleDocument.SaleDate <= PreviousAtToDate
                     //               && w.SaleDocument.IsInDebt
                     //               && w.SaleDocument.Creator_Id == App.CurrentUser.UserName)).ToList();

                    var previousDebts = previousRealizations.Where(r => r.SaleItem.SaleDocument.IsInDebt);


                     //var groupedDebts = from r in debts
                     //                   group r by r.SaleDocument.Creator.DisplayName
                     //                       into cshrs
                     //                       select new
                     //                       {
                     //                           Cashier = cshrs.Key,
                     //                           Debts = cshrs.Sum(s => s.Amount),
                     //                           WholesaleDebts =
                     //                               cshrs.Sum(
                     //                                       s =>
                     //                                           (s.Count * s.PriceItem.Prices.Count > 0
                     //                                               ? s.PriceItem.Prices.Where(
                     //                                                   p => p.PriceDate <= s.SaleDocument.SaleDate)
                     //                                                   .OrderByDescending(or => or.PriceDate)
                     //                                                   .First()
                     //                                                   .Price
                     //                                               : 0))
                     //                       };
                    var tempGroupedDebts =
                        from r in debts
                            select new
                            {
                                Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                                Debts = r.Amount,
                                WholesaleDebts =
                                    r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                                    ? r.SaleItem.PriceItem.Prices.Where(
                                                        p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                                        .OrderByDescending(or => or.PriceDate)
                                                        .First()
                                                        .Price
                                                    : 0)
                            };
                    var groupedDebts = from r in tempGroupedDebts
                                       group r by r.Cashier
                                           into cshrs
                                           select new
                                           {
                                               Cashier = cshrs.Key,
                                               Debts = cshrs.Sum(s => s.Debts),
                                               WholesaleDebts = cshrs.Sum(s => s.WholesaleDebts)
                                           };


                     //var groupedPreviousDebts = from r in previousDebts
                     //                           group r by r.SaleDocument.Creator.DisplayName
                     //                               into cshrs
                     //                               select new
                     //                               {
                     //                                   Cashier = cshrs.Key,
                     //                                   Debts = cshrs.Sum(s => s.Amount),
                     //                                   WholesaleDebts =
                     //                                       cshrs.Sum(
                     //                                               s =>
                     //                                                   (s.Count * s.PriceItem.Prices.Count > 0
                     //                                                       ? s.PriceItem.Prices.Where(
                     //                                                           p => p.PriceDate <= s.SaleDocument.SaleDate)
                     //                                                           .OrderByDescending(or => or.PriceDate)
                     //                                                           .First()
                     //                                                           .Price
                     //                                                       : 0))
                     //                               };

                    var tempPreviousGroupedDebts =
                        from r in previousDebts
                            select new
                            {
                                Cashier = r.SaleItem.SaleDocument.Creator.DisplayName,
                                Debts = r.Amount,
                                WholesaleDebts =
                                    r.Count * (r.SaleItem.PriceItem.Prices.Count > 0
                                                    ? r.SaleItem.PriceItem.Prices.Where(
                                                        p => p.PriceDate <= r.SaleItem.SaleDocument.SaleDate)
                                                        .OrderByDescending(or => or.PriceDate)
                                                        .First()
                                                        .Price
                                                    : 0)
                            };

                    var groupedPreviousDebts = from r in tempPreviousGroupedDebts
                                       group r by r.Cashier
                                           into cshrs
                                           select new
                                           {
                                               Cashier = cshrs.Key,
                                               Debts = cshrs.Sum(s => s.Debts),
                                               WholesaleDebts = cshrs.Sum(s => s.WholesaleDebts)
                                           };

                     var debt = (from r in groupedDebts
                                 join pr in groupedPreviousDebts on r.Cashier equals pr.Cashier into prvs
                                 from previous in prvs.DefaultIfEmpty()
                                 select new
                                 {
                                     Cashier = r.Cashier,
                                     Debts = r.Debts,
                                     WholesaleDebts = r.WholesaleDebts,
                                     PreviousDebts = previous != null ? previous.Debts : 0,
                                     PreviousWholesaleDebts = previous != null ? previous.WholesaleDebts : 0
                                 }).ToList();

                     var additionalDebts = debtCtx.ExecuteSyncronous(
                        debtCtx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => !w.IsDischarge
                                        && w.DischargeDate >= AtFromDate
                                        && w.DischargeDate <= AtToDate
                                        && w.Creator_Id == App.CurrentUser.UserName)).ToList();

                     var previousAdditionalDebts = debtCtx.ExecuteSyncronous(
                        debtCtx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => !w.IsDischarge
                                        && w.DischargeDate >= PreviousAtFromDate
                                        && w.DischargeDate <= PreviousAtToDate
                                        && w.Creator_Id == App.CurrentUser.UserName)).ToList();


                    var groupedAdditionaDebts = from r in additionalDebts
                        group r by r.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Debts = cshrs.Sum(s => s.Amount)
                        };


                    var groupedPreviousAdditionaDebts = from r in previousAdditionalDebts
                        group r by r.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Debts = cshrs.Sum(s => s.Amount)
                        };

                    var additionalDebt = from r in groupedAdditionaDebts
                        join pr in groupedPreviousAdditionaDebts on r.Cashier equals pr.Cashier into prvs
                        from previous in prvs.DefaultIfEmpty()
                        select new
                        {
                            Cashier = r.Cashier,
                            Debts = r.Debts,
                            WholesaleDebts = r.Debts,
                            PreviousDebts = previous != null ? previous.Debts : 0,
                            PreviousWholesaleDebts = previous != null ? previous.Debts : 0
                        };


                    debt.AddRange(additionalDebt);

                    var totalDebt = from d in debt
                        group d by d.Cashier
                        into dbt
                        select new
                        {
                            Cashier = dbt.Key,
                            Debts = dbt.Sum(s => s.Debts),
                            WholesaleDebts = dbt.Sum(s => s.WholesaleDebts),
                            PreviousDebts = dbt.Sum(s => s.PreviousDebts),
                            PreviousWholesaleDebts = dbt.Sum(s => s.PreviousWholesaleDebts)
                        };

                    var debtDischarges = ctx.ExecuteSyncronous(
                        ctx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => w.IsDischarge
                                        && w.DischargeDate >= AtFromDate
                                        && w.DischargeDate <= AtToDate
                                        && w.Creator_Id == App.CurrentUser.UserName)).ToList();

                    var previousDebtDischarges = ctx.ExecuteSyncronous(
                        ctx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => w.IsDischarge
                                        && w.DischargeDate >= PreviousAtFromDate
                                        && w.DischargeDate <= PreviousAtToDate
                                        && w.Creator_Id == App.CurrentUser.UserName)).ToList();



                    var groupedDebtDischarges = from r in debtDischarges
                        group r by r.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Discharges = cshrs.Sum(s => s.Amount)
                        };

                    var groupedPreviousDebtDischarges = from r in previousDebtDischarges
                        group r by r.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Discharges = cshrs.Sum(s => s.Amount)
                        };

                    var debtDischarge = from r in groupedDebtDischarges
                        join pr in groupedPreviousDebtDischarges on r.Cashier equals pr.Cashier into prvs
                        from previous in prvs.DefaultIfEmpty()
                        select new
                        {
                            Cashier = r.Cashier,
                            DebDischargests = r.Discharges,
                            PreviousDischarges = previous != null ? previous.Discharges : 0
                        };

                    var totalData = from s in realization
                                    join r in refund on s.Cashier equals r.Cashier into rfnds
                                    join d in totalDebt on s.Cashier equals d.Cashier into dbts
                                    join dd in debtDischarge on s.Cashier equals dd.Cashier into dschrgs
                                    from rfndsItem in rfnds.DefaultIfEmpty()
                                    from dbtsItem in dbts.DefaultIfEmpty()
                                    from dschrgsItem in dschrgs.DefaultIfEmpty()
                                    select new
                                    {
                                        Cashier = s.Cashier,
                                        Sales = s.Sales,
                                        WholesaleSales = s.WholesaleSales,
                                        PreviousSales = s.PreviousSales,
                                        PreviousWholesaleSales = s.PreviousWholesaleSales,
                                        Refunds = rfndsItem != null ? rfndsItem.Refunds : 0,
                                        WholesaleRefunds = rfndsItem != null ? rfndsItem.WholesaleRefunds : 0,
                                        PreviousRefunds = rfndsItem != null ? rfndsItem.PreviousRefunds : 0,
                                        PreviousWholesaleRefunds = rfndsItem != null ? rfndsItem.PreviousWholesaleRefunds : 0,
                                        Debts = dbtsItem != null ? dbtsItem.Debts : 0,
                                        WholesaleDebts = dbtsItem != null ? dbtsItem.WholesaleDebts : 0,
                                        PreviousDebts = dbtsItem != null ? dbtsItem.PreviousDebts : 0,
                                        PreviousWholesaleDebts = dbtsItem != null ? dbtsItem.PreviousWholesaleDebts : 0,
                                        DebtDischarges = dschrgsItem != null ? dschrgsItem.DebDischargests : 0,
                                        PreviousDebtDischarges = dschrgsItem != null ? dschrgsItem.PreviousDischarges : 0
                                    };

                    foreach (var item in totalData)
                    {
                        var item1 = item;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {

                            CashFlowItems.Add(new CashFlowItemModel()
                            {
                                Cashier = item1.Cashier,
                                Sales = (int)item1.Sales,
                                SalesByWholesales = (int)item1.WholesaleSales,
                                PreviousSales = (int)item1.PreviousSales,
                                PreviousSalesByWholesales = (int)item1.PreviousWholesaleSales,
                                Refunds = (int)item1.Refunds,
                                RefundsByWholesales = (int)item1.WholesaleRefunds,
                                PreviousRefunds = (int)item1.PreviousRefunds,
                                PreviousRefundsByWholesales = (int)item1.PreviousWholesaleRefunds,
                                Debds = (int)item1.Debts,
                                DebdsByWholesales = (int)item1.WholesaleDebts,
                                PreviousDebds = (int)item1.PreviousDebts,
                                PreviousDebdsByWholesales = (int)item1.PreviousWholesaleDebts,
                                DebdDischarges = (int)item1.DebtDischarges,
                                PreviousDebdDischarges = (int)item1.PreviousDebtDischarges
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageChildWindow msch = new MessageChildWindow();
                        msch.Title = "Ошибка";
                        msch.Message = ex.Message;
                        msch.Show();
                    });                
                }
            });
        

                    


            //CashFlowItems.Add(new CashFlowItemModel()
            //{
            //    Cashier = "Кассир 1",
            //    Sales = 10500,
            //    PreviousSales = 9700,
            //    Refunds = 1000,
            //    PreviousRefunds = 0,
            //    Debds = 3000,
            //    PreviousDebds = 1000,
            //    DebdDischarges = 1900,
            //    PreviousDebdDischarges = 1000
            //});
            //CashFlowItems.Add(new CashFlowItemModel()
            //{
            //    Cashier = "Кассир 2",
            //    Sales = 22000,
            //    PreviousSales = 25500,
            //    Refunds = 0,
            //    PreviousRefunds = 4000,
            //    Debds = 3000,
            //    PreviousDebds = 0,
            //    DebdDischarges = 1900,
            //    PreviousDebdDischarges = 1000
            //});
            //CashFlowItems.Add(new CashFlowItemModel()
            //{
            //    Cashier = "Кассир 3",
            //    Sales = 10500,
            //    PreviousSales = 9700,
            //    Refunds = 1000,
            //    PreviousRefunds = 0,
            //    Debds = 3000,
            //    PreviousDebds = 1000,
            //    DebdDischarges = 1900,
            //    PreviousDebdDischarges = 1000
            //});
        }
    }

}
