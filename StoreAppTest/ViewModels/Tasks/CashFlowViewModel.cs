

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

    public class CashFlowViewModel : ViewModelBase
    {
        public CashFlowViewModel()
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

                    var realizations = ctx.ExecuteSyncronous(
                        ctx.SalesPerDayItems
                        .Expand("SaleDocumentsPerDay,SaleItem/SaleDocument/Creator")
                        .Where(w => w.SaleDocumentsPerDay.SaleDocumentsDate >= AtFromDate 
                            &&  w.SaleDocumentsPerDay.SaleDocumentsDate <= AtToDate)).ToList();//SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices

                    var previousRealizations = ctx.ExecuteSyncronous(
                        ctx.SalesPerDayItems
                        .Expand("SaleDocumentsPerDay,SaleItem/SaleDocument/Creator")
                        .Where(w => w.SaleDocumentsPerDay.SaleDocumentsDate >= PreviousAtFromDate 
                            &&  w.SaleDocumentsPerDay.SaleDocumentsDate <= PreviousAtToDate)).ToList();//SaleDocument,PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices

                    var groupedRealization = from r in realizations
                        group r by r.SaleItem.SaleDocument.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Sales = cshrs.Sum(s => s.SaleItem.Amount)
                        };


                    var groupedPreviousRealization = from r in previousRealizations
                        group r by r.SaleItem.SaleDocument.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Sales = cshrs.Sum(s => s.SaleItem.Amount)
                        };

                    var realization = from r in groupedRealization
                        join pr in groupedPreviousRealization on r.Cashier equals pr.Cashier into prvs
                        from previous in prvs.DefaultIfEmpty()
                        select new
                        {
                            Cashier = r.Cashier,
                            Sales = r.Sales,
                            PreviousSales = previous != null ? previous.Sales : 0
                        };


                     var refunds = ctx.ExecuteSyncronous(
                        ctx.RefundItems.Expand("RefundDocument,SaleItem/SaleDocument/Creator") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                            .Where(
                                w =>
                                    w.RefundDocument.RefundDate >= AtFromDate 
                                    && w.RefundDocument.RefundDate <= AtToDate)).ToList();

                     var previousRefunds = ctx.ExecuteSyncronous(
                        ctx.RefundItems.Expand("RefundDocument,SaleItem/SaleDocument/Creator") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                            .Where(
                                w =>
                                    w.RefundDocument.RefundDate >= PreviousAtFromDate 
                                    && w.RefundDocument.RefundDate <= PreviousAtToDate)).ToList();

                    var groupedRefunds = from r in refunds
                        group r by r.SaleItem.SaleDocument.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Refunds = cshrs.Sum(s => s.Amount)
                        };


                    var groupedPreviousRefunds = from r in previousRefunds
                        group r by r.SaleItem.SaleDocument.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Refunds = cshrs.Sum(s => s.Amount)
                        };

                    var refund = from r in groupedRefunds
                        join pr in groupedPreviousRefunds on r.Cashier equals pr.Cashier into prvs
                        from previous in prvs.DefaultIfEmpty()
                        select new
                        {
                            Cashier = r.Cashier,
                            Refunds = r.Refunds,
                            PreviousRefunds = previous != null ? previous.Refunds : 0
                        };


                     var debts = ctx.ExecuteSyncronous(
                        ctx.SaleItems.Expand("SaleDocument/Creator") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                            .Where(
                                w =>
                                    w.SaleDocument.SaleDate >= AtFromDate 
                                    && w.SaleDocument.SaleDate <= AtToDate
                                    && w.SaleDocument.IsInDebt)).ToList();

                     var previousDebts = ctx.ExecuteSyncronous(
                        ctx.SaleItems.Expand("SaleDocument/Creator") //PriceItem/Gear,PriceItem/UnitOfMeasure,PriceItem/Remainders,PriceItem/Prices
                            .Where(
                                w =>
                                    w.SaleDocument.SaleDate >= PreviousAtFromDate 
                                    && w.SaleDocument.SaleDate <= PreviousAtToDate
                                    && w.SaleDocument.IsInDebt)).ToList();


                    var groupedDebts = from r in debts
                        group r by r.SaleDocument.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Debts = cshrs.Sum(s => s.Amount)
                        };


                    var groupedPreviousDebts = from r in previousDebts
                        group r by r.SaleDocument.Creator.DisplayName
                        into cshrs
                        select new
                        {
                            Cashier = cshrs.Key,
                            Debts = cshrs.Sum(s => s.Amount)
                        };

                    var debt = (from r in groupedDebts
                        join pr in groupedPreviousDebts on r.Cashier equals pr.Cashier into prvs
                        from previous in prvs.DefaultIfEmpty()
                        select new
                        {
                            Cashier = r.Cashier,
                            Debts = r.Debts,
                            PreviousDebts = previous != null ? previous.Debts : 0
                        }).ToList();

                    var additionalDebts = ctx.ExecuteSyncronous(
                        ctx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => !w.IsDischarge
                                        && w.DischargeDate >= AtFromDate
                                        && w.DischargeDate <= AtToDate)).ToList();

                    var previousAdditionalDebts = ctx.ExecuteSyncronous(
                        ctx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => !w.IsDischarge
                                        && w.DischargeDate >= PreviousAtFromDate
                                        && w.DischargeDate <= PreviousAtToDate)).ToList();


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
                            PreviousDebts = previous != null ? previous.Debts : 0
                        };


                    debt.AddRange(additionalDebt);

                    var totalDebt = from d in debt
                        group d by d.Cashier
                        into dbt
                        select new
                        {
                            Cashier = dbt.Key,
                            Debts = dbt.Sum(s => s.Debts),
                            PreviousDebts = dbt.Sum(s => s.PreviousDebts)
                        };

                    var debtDischarges = ctx.ExecuteSyncronous(
                        ctx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => w.IsDischarge
                                        && w.DischargeDate >= AtFromDate
                                        && w.DischargeDate <= AtToDate)).ToList();

                    var previousDebtDischarges = ctx.ExecuteSyncronous(
                        ctx.DebtDischargeDocuments.Expand("Creator")
                            .Where(w => w.IsDischarge
                                        && w.DischargeDate >= PreviousAtFromDate
                                        && w.DischargeDate <= PreviousAtToDate)).ToList();



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
                            PreviousSales = s.PreviousSales,
                            Refunds = rfndsItem != null ? rfndsItem.Refunds : 0,
                            PreviousRefunds = rfndsItem != null ? rfndsItem.PreviousRefunds : 0,
                            Debts = dbtsItem != null ? dbtsItem.Debts : 0,
                            PreviousDebts = dbtsItem != null ? dbtsItem.PreviousDebts : 0,
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
                                PreviousSales = (int)item1.PreviousSales,
                                Refunds = (int)item1.Refunds,
                                PreviousRefunds = (int)item1.PreviousRefunds,
                                Debds = (int)item1.Debts,
                                PreviousDebds = (int)item1.PreviousDebts,
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

    public class CashFlowItemModel
    {
        public string Cashier { get; set; }
        public int Sales { get; set; }
        public int PreviousSales { get; set; }

        public decimal SalesDifference
        {
            get
            {
                if (PreviousSales == 0)
                    return 100;
                //return (((decimal)PreviousSales) / ((decimal)Sales)) * 100;
                return (((decimal)Sales) / ((decimal)PreviousSales)) * 100;
            }
        }

        public int Refunds { get; set; }
        public int PreviousRefunds { get; set; }

        public decimal RefundsDifference
        {
            get
            {
                if (PreviousRefunds == 0)
                    return 100;
                //return (((decimal)PreviousRefunds) / ((decimal)Refunds)) * 100;
                return (((decimal)Refunds) / ((decimal)PreviousRefunds)) * 100;
            }
        }

        public int Debds { get; set; }
        public int PreviousDebds { get; set; }

        public decimal DebdsDifference
        {
            get
            {
                if (PreviousDebds == 0)
                    return 100;
                //return (((decimal)PreviousDebds) / ((decimal)Debds)) * 100;
                return (((decimal)Debds) / ((decimal)PreviousDebds)) * 100;
            }
        }

        public int DebdDischarges { get; set; }
        public int PreviousDebdDischarges { get; set; }

        public decimal DebdDischargesDifference
        {
            get
            {
                if (PreviousDebdDischarges == 0)
                    return 100;
                //return (((decimal)PreviousDebdDischarges) / ((decimal)DebdDischarges)) * 100;
                return (((decimal)DebdDischarges) / ((decimal)PreviousDebdDischarges)) * 100;
            }
        }

        public int Totals
        {
            get { return (Sales - Refunds) - (Debds + DebdDischarges); }
        }

        public int PreviousTotals
        {
            get { return (PreviousSales - PreviousRefunds) - (PreviousDebds + PreviousDebdDischarges); }
        }
        public decimal TotalsDifference
        {
            get
            {
                if (PreviousTotals == 0)
                    return 100;
                //return (((decimal)PreviousTotals) / ((decimal)Totals)) * 100;
                return (((decimal)Totals) / ((decimal)PreviousTotals)) * 100;
            }
        }
    }

    public enum PeriodTypes
    {
        Day,
        Week,
        Month,
        HalfYear,
        Year,
        Custom
    }
}
