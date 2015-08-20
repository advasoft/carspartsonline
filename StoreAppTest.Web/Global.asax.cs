using System;

using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace StoreAppTest.Web
{
    extern alias ef;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Controllers;
    using DataModel;
    using StoreAppTest.Utilities;
    using System.Linq;
    using System.Text;
    using ef::System.Data.Entity;

    public class WebApiApplication : System.Web.HttpApplication
    {
        private Thread _timerThread;
        private System.Timers.Timer _timer;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            _timerThread = new Thread(new ThreadStart(TimerThread));
            _timerThread.IsBackground = true;
            _timerThread.Name = "TimerThread";
            _timerThread.Start();
        }


        protected void TimerThread()
        {
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerWorker);
            _timer.Interval = 3600000;
            _timer.Enabled = true;
            _timer.AutoReset = true;
            _timer.Start();
        }

        protected void TimerWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            //if (DateTime.Now.Hour != 13 || DateTime.Now.Minute != 2)
            //    return;

            if (DateTimeHelper.GetNowKz().Hour != 23)
                return;

            var context = new StoreDbContext();
            var users = context.Users.ToList();

            var realizationTasks = new Task[users.Count];
            for (int i = 0; i < users.Count; i++)
            {
                var i1 = i;
                realizationTasks[i] = Task.Run(() =>
                {
                    var user = users[i1];

                    CloseRealiztionPerDay(user);
                });
            }
        }

        protected void CloseRealiztionPerDay(User user)
        {
            var now = DateTimeHelper.GetNowKz();
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            string realizationNumber = "";

            var lastNum = GetLastSaleDocumentsPerDaysNumber();

            if (!string.IsNullOrEmpty(lastNum))
            {
                int lastNumber = 0;
                if (int.TryParse(lastNum, out lastNumber))
                {

                    realizationNumber = (++lastNumber).ToString();
                }
            }
            else
            {
                realizationNumber = "1";
            }

            var strt = startDate;
            var endd = endDate;

            var dischargesSource =
               GetUserDebtDischargeDocumentsByDate(strt, endd, user.UserName).ToList();

            var discharges = (from d in dischargesSource
                          group d by new
                          {
                              Debtor = d.Debtor_Id
                          }
                              into ds
                              select new DebtDischargeDocument()
                              {
                                  Amount = ds.Sum(s => s.Amount),
                                  Debtor_Id = ds.Key.Debtor
                              }).ToList();



            var refundItems = GetTodayRefundItems(user.UserName).ToList();

            var realization = GetRealizationItemsRaw(user).ToList();


            decimal TotalAmount = 0;
            var TotalAmount_totalDisc = discharges.Sum(s => s.Amount);
            var TotalAmount_total = realization.Where(w => !w.SaleItemData.SaleDocument.IsInDebt).Sum(s => s.Amount);
            TotalAmount = TotalAmount_total + TotalAmount_totalDisc;

            decimal TotalRefund = 0;
            TotalRefund = refundItems.Where(w => !w.RefundDocument.SaleDocument.IsInDebt).Sum(s => s.Amount);

            decimal SubTotal = 0;
            SubTotal = TotalAmount - TotalRefund;

            decimal TotalAmountProfit = 0;
            var TotalAmountProfit_totalDisc = discharges.Sum(s => s.Amount);
            decimal TotalAmountProfit_total = realization.Where(w => !w.SaleItemData.SaleDocument.IsInDebt).Sum(
                s => s.SoldCount * s.WholePrice);
            TotalAmountProfit = TotalAmount - (TotalAmountProfit_total + TotalAmountProfit_totalDisc);

            decimal TotalRefundProfit = 0;
            decimal TotalRefundProfit_total = refundItems.Where(w => !w.RefundDocument.SaleDocument.IsInDebt).Sum(
                s => s.Count * s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price);
            TotalRefundProfit = TotalRefund - TotalRefundProfit_total;

            decimal SubTotalProfit = 0;
            SubTotalProfit = TotalAmountProfit - TotalRefundProfit;


            if (realization.Count > 0)
            {
                var salePerDay = new SaleDocumentsPerDay();
                salePerDay.Creator_Id = user.UserName;
                salePerDay.IsClosed = true;
                salePerDay.Number = realizationNumber;
                salePerDay.SaleDocumentsDate = DateTimeHelper.GetNowKz();
                salePerDay.Barcode = GetBarcode(realizationNumber);
                salePerDay.TotalAmount = TotalAmount;
                salePerDay.TotalRefund = TotalRefund;
                salePerDay.SubTotal = SubTotal;
                salePerDay.TotalAmountProfit = TotalAmountProfit;
                salePerDay.TotalRefundProfit = TotalRefundProfit;
                salePerDay.SubTotalProfit = SubTotalProfit;


                foreach (var receiptItem in realization)
                {
                    var item = new SalesPerDayItem()
                    {
                        SaleDocumentsPerDay_Id = salePerDay.Id,
                        SaleItem_Id = receiptItem.SaleItemData.Id

                    };

                    item.Amount = receiptItem.Amount;
                    item.CatalogNumber = receiptItem.CatalogNumber;
                    item.Count = receiptItem.SoldCount;
                    item.Discount = receiptItem.Discount;
                    item.IsDuplicate = receiptItem.IsDuplicate == "*" ? true : false;
                    item.Name = receiptItem.Name;
                    item.Price = receiptItem.Price;
                    item.UnitOfMeasure = receiptItem.Uom;
                    item.Remainders = receiptItem.Remainders;

                    salePerDay.SalesPerDayItems.Add(item);
                }

                AddRealizationPerDay(salePerDay);
            }
        }


        protected IEnumerable<DebtDischargeDocument> GetUserDebtDischargeDocumentsByDate(DateTime from, DateTime to, string userName = "")
        {
            var fromDate = from;
            var toDate = to;

            var context = new StoreDbContext();
            var query = context.DebtDischargeDocuments
                .Include(i => i.Debtor)
                .Where(c => c.Creator_Id == userName && c.DischargeDate >= fromDate && c.DischargeDate <= toDate && c.IsDischarge);

            return query;
        }

        protected string GetLastSaleDocumentsPerDaysNumber()
        {
            string number = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT TOP 1 [Number] ");
            sb.Append("FROM dbo.[SaleDocumentsPerDays] ");
            sb.Append("ORDER BY [Id] DESC ");

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sb.ToString();

                number = (string)command.ExecuteScalar();
            }

            return number;
        }

        protected SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDbContext"].ConnectionString);

            return connection;
        }

        protected IEnumerable<RealizationItem> GetRealizationItemsRaw(User user)
        {
            var context = new StoreDbContext();

            var now = DateTimeHelper.GetNowKz();
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);


            var closedRefundPerDay = context.RefundsPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.RefundItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed &&
                            w.SaleDocumentsPerDay.Creator_Id == user.UserName).Select(s => s.RefundItem.Id).ToList();


            var refundQuery =
                context.RefundItems
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.PriceItem.Gear)
                .Include(i => i.PriceItem.UnitOfMeasure)
                .Include(i => i.PriceItem.Remainders)
                .Include(i => i.PriceItem.Prices)
                .Include(i => i.PriceItem.PriceLists)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= startDate
                            && w.RefundDocument.RefundDate <= endDate
                            && w.RefundDocument.Creator_Id == user.UserName).ToList();

            var refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();

            var closedSalesPerDay =
                context.SalesPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed &&
                            w.SaleDocumentsPerDay.Creator_Id == user.UserName).Select(s => s.SaleItem.Id).ToList();

            var salesQuery =
                context.SaleItems
                    .Include(i => i.SaleDocument)
                    .Include(i => i.PriceItem.PriceLists)
                    .Include(i => i.PriceItem.Gear)
                    .Include(i => i.PriceItem.UnitOfMeasure)
                    .Include(i => i.PriceItem.Remainders)
                    .Include(i => i.PriceItem.Prices)
                    .Include(i => i.PriceItem.PriceLists)
                    .Where(
                        w =>
                            w.SaleDocument.SaleDate >= startDate
                            && w.SaleDocument.SaleDate <= endDate
                            && !w.SaleDocument.IsOrder
                            && w.SaleDocument.Creator_Id == user.UserName).ToList();

            var salesItems = salesQuery.Where(w => !closedSalesPerDay.Contains(w.Id)).ToList();
            var realization = new List<RealizationItem>();
            salesItems.ForEach(s =>
            {

                var remainders = 0;
                var rems =
                    s.PriceItem.Remainders.Where(w => w.Warehouse_Id == user.Warehouse_Id)
                        .FirstOrDefault();
                if (rems != null)
                    remainders = (int)rems.Amount;

                var itemRefunds =
                    refundItems.Where(wh => wh.SaleItem_Id == s.Id && wh.RefundDocument.SaleDocument.IsInDebt == true)
                        .Sum(sum => sum.Count);


                var count = s.Count - itemRefunds;
                if (count != 0)
                {
                    realization.Add(new RealizationItem()
                    {

                        CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                        IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
                        Name = s.PriceItem.Gear.Name,
                        Price = (int)s.Price,
                        Remainders = remainders,
                        SoldCount = (int)count,
                        Uom = s.PriceItem.UnitOfMeasure.Name,
                        WholePrice = (int)s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                        Amount = (int)((s.Price - (s.Discount / s.Count)) * count),
                        Discount = (int)s.Discount,
                        SaleItemData = s,
                        Customer = s.SaleDocument.IsInDebt ? s.SaleDocument.Customer_Name : "",
                        IsInDebt = s.SaleDocument.IsInDebt,
                        PriceListName = s.PriceItem.PriceLists.First().Name,
                        SaledDate = s.SaleDocument.SaleDate.ToString("T"),
                        SaledNumber = s.SaleDocument.Number,
                        SaledCount = (int)s.Count,

                    });
                }
            });

            return realization;
        }

        protected IEnumerable<RealizationItem> GetRealizationItems(User user)
        {
            var context = new StoreDbContext();

            var now = DateTimeHelper.GetNowKz();
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            var closedRefundPerDay = context.RefundsPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.RefundItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed &&
                            w.SaleDocumentsPerDay.Creator_Id == user.UserName).Select(s => s.RefundItem.Id).ToList();


            var refundQuery =
                context.RefundItems
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.PriceItem.Gear)
                .Include(i => i.PriceItem.UnitOfMeasure)
                .Include(i => i.PriceItem.Remainders)
                .Include(i => i.PriceItem.Prices)
                .Include(i => i.PriceItem.PriceLists)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= startDate
                            && w.RefundDocument.RefundDate <= endDate
                            && w.RefundDocument.Creator_Id == user.UserName).ToList();

            var refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();

            var closedSalesPerDay =
                context.SalesPerDayItems
                .Include(i => i.SaleDocumentsPerDay)
                .Include(i => i.SaleItem)
                    .Where(
                        w =>
                            w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                            && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                            && w.SaleDocumentsPerDay.IsClosed
                            && w.SaleDocumentsPerDay.Creator_Id == user.UserName).Select(s => s.SaleItem.Id).ToList();

            var salesQuery =
                context.SaleItems
                    .Include(i => i.SaleDocument)
                    .Include(i => i.PriceItem.PriceLists)
                    .Include(i => i.PriceItem.Gear)
                    .Include(i => i.PriceItem.UnitOfMeasure)
                    .Include(i => i.PriceItem.Remainders)
                    .Include(i => i.PriceItem.Prices)
                    .Include(i => i.PriceItem.PriceLists)
                    .Where(
                        w =>
                            w.SaleDocument.SaleDate >= startDate
                            && w.SaleDocument.SaleDate <= endDate
                            && !w.SaleDocument.IsOrder
                            && w.SaleDocument.Creator_Id == user.UserName).ToList();

            var salesItems = salesQuery.Where(w => !closedSalesPerDay.Contains(w.Id)).ToList();
            var realization = new List<RealizationItem>();
            salesItems.ForEach(s =>
            {

                var remainders = 0;
                var rems =
                    s.PriceItem.Remainders.Where(w => w.Warehouse_Id == user.Warehouse_Id)
                        .FirstOrDefault();
                if (rems != null)
                    remainders = (int)rems.Amount;

                var itemRefunds =
                    refundItems.Where(wh => wh.SaleItem_Id == s.Id && wh.RefundDocument.SaleDocument.IsInDebt == true)
                        .Sum(sum => sum.Count);



                var count = s.Count - itemRefunds;
                if (count != 0)
                {
                    realization.Add(new RealizationItem()
                    {

                        CatalogNumber = s.PriceItem.Gear.CatalogNumber,
                        IsDuplicate = s.PriceItem.Gear.IsDuplicate ? "*" : "",
                        Name = s.PriceItem.Gear.Name,
                        Price = (int)s.Price,
                        Remainders = remainders,
                        SoldCount = (int)count,
                        Uom = s.PriceItem.UnitOfMeasure.Name,
                        WholePrice = (int)s.PriceItem.Prices.OrderByDescending(o => o.PriceDate).First().Price,
                        Amount = (int)((s.Price - (s.Discount / s.Count)) * count),
                        Discount = (int)s.Discount,
                        SaleItemData = s,
                        Customer = s.SaleDocument.IsInDebt ? s.SaleDocument.Customer_Name : "",
                        IsInDebt = s.SaleDocument.IsInDebt,
                        PriceListName = s.PriceItem.PriceLists.First().Name,
                        SaledDate = s.SaleDocument.SaleDate.ToString("T"),
                        SaledNumber = s.SaleDocument.Number,
                        SaledCount = (int)s.Count,

                    });
                }
            });
            int index = 1;
            var query = from r in realization
                        group r by new
                        {
                            CatalogNumber = r.CatalogNumber,
                            IsDuplicate = r.IsDuplicate,
                            Name = r.Name,
                            Price = r.Price,
                            Uom = r.Uom,
                            IsInDebt = r.IsInDebt,
                            PriceListName = r.PriceListName,
                            SaledDate = r.SaledDate,
                            SaledNumber = r.SaledNumber,
                        }
                            into groups
                            select new RealizationItem()
                            {
                                Number = index++,
                                CatalogNumber = groups.Key.CatalogNumber,
                                IsDuplicate = groups.Key.IsDuplicate,
                                Name = groups.Key.Name,
                                Price = (int)groups.Key.Price,
                                Remainders = (int)groups.Average(s => s.Remainders),
                                SoldCount = groups.Sum(s => s.SoldCount),
                                Uom = groups.Key.Uom,
                                Amount = groups.Sum(s => s.Amount),
                                AmountWithoutDebt = groups.Sum(s => s.AmountWithoutDebt),
                                AmountWithoutDebtProfit = groups.Sum(s => s.AmountWithoutDebtProfit),
                                Discount = groups.Sum(s => s.Discount),
                                WholePrice = (int)groups.Average(a => a.WholePrice),
                                DebtDischarge = (int)groups.Average(s => s.DebtDischarge),
                                IsInDebt = groups.Key.IsInDebt,
                                PriceListName = groups.Key.PriceListName,
                                SaledDate = groups.Key.SaledDate,
                                SaledNumber = groups.Key.SaledNumber,
                                SaledCount = groups.Sum(s => s.SaledCount),
                            };
            return query;
        }

        protected IEnumerable<RefundItem> GetTodayRefundItems(string userName)
        {
            var now = DateTimeHelper.GetNowKz();
            var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            var context = new StoreDbContext();

            var closedRefundPerDay = context.RefundsPerDayItems
                .Where(
                    w =>
                        w.SaleDocumentsPerDay.SaleDocumentsDate >= startDate
                        && w.SaleDocumentsPerDay.SaleDocumentsDate <= endDate
                        && w.SaleDocumentsPerDay.IsClosed &&
                        w.SaleDocumentsPerDay.Creator_Id == userName).Select(s => s.RefundItem.Id).ToList();
            var refundQuery =
                context.RefundItems
                .Include(i => i.RefundDocument)
                .Include(i => i.RefundDocument.SaleDocument)
                .Include(i => i.PriceItem.Gear)
                .Include(i => i.PriceItem.UnitOfMeasure)
                .Include(i => i.PriceItem.Remainders)
                .Include(i => i.PriceItem.Prices)
                .Include(i => i.PriceItem.PriceLists)
                .Include(i => i.SaleItem.SaleDocument)
                    .Where(
                        w =>
                            w.RefundDocument.RefundDate >= startDate
                            && w.RefundDocument.RefundDate <= endDate
                            && w.RefundDocument.Creator_Id == userName).ToList();

            var refundItems = refundQuery.Where(w => !closedRefundPerDay.Contains(w.Id)).ToList();
            return refundItems;
        }

        protected string GetBarcode(string realizationNumber)
        {
            string pref = "RE";
            int lenAll = 4;
            int lenNum = realizationNumber.Length;
            int zeroCount = lenAll - lenNum;

            StringBuilder builder = new StringBuilder(pref);
            for (int i = 0; i < zeroCount; i++)
            {
                builder.Append("0");
            }

            builder.Append(realizationNumber);

            return builder.ToString();
        }

        protected void AddRealizationPerDay(SaleDocumentsPerDay saleDocumentsPerDay)
        {

            //clearvalues
            saleDocumentsPerDay.Creator = null;

            var items = saleDocumentsPerDay.SalesPerDayItems;
            saleDocumentsPerDay.SalesPerDayItems = null;


            var context = new StoreDbContext();
            using (var tr = context.Database.BeginTransaction())
            {
                try
                {
                    context.SaleDocumentsPerDays.Add(saleDocumentsPerDay);
                    context.SaveChanges();

                    foreach (var item in items)
                    {
                        item.SaleDocumentsPerDay_Id = saleDocumentsPerDay.Id;

                        item.SaleDocumentsPerDay = null;
                        item.SaleItem = null;

                        context.SalesPerDayItems.Add(item);
                    }
                    context.SaveChanges();
                    tr.Commit();
                }
                catch (Exception exception)
                {
                    tr.Rollback();
                }
            }

        }

    }
}