
using System.Web.Http;

namespace StoreAppTest.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;
    using DataModel;

    public class IntegrationController : ApiController
    {
        [HttpPost]
        public IHttpActionResult PutOrder(OrderItemModel[] orderItems)
        {
            var context = new StoreDbContext();


            SaleDocument sd = new SaleDocument();

            var lastOrder = context.SaleDocuments.OrderByDescending(o => o.Id).FirstOrDefault();
            if (lastOrder != null)
            {
                int lastNumber = 0;
                if (int.TryParse(lastOrder.Number, out lastNumber))
                {
                    sd.Number = (++lastNumber).ToString();
                }
            }
            else
            {
                sd.Number = "1";
            }
            sd.SaleDate = DateTime.Now;
            sd.Creator_Id = "ecommerce";
            sd.Customer_Name = "Клиент интернет-магазина";

            for (int i = 0; i < orderItems.Length; i++)
            {
                var orderItem = orderItems[i];
                var findedGear = context.Gears.Where(g => g.Articul == orderItem.Articul).FirstOrDefault();
                if (findedGear == null)
                    return
                        new ExceptionResult(
                            new Exception(string.Format("Запчасть с кодом: {0} не найдена", orderItem.Articul)), this);

                var priceItem = context.PriceItems.Where(pr => pr.Gear_Id == findedGear.Id).FirstOrDefault();
                if (priceItem == null)
                    return
                        new ExceptionResult(
                            new Exception(string.Format("Запчасть с кодом: {0} не найдена", orderItem.Articul)), this);


                SaleItem item = new SaleItem();
                item.Articul = findedGear.Articul;
                item.CatalogNumber = findedGear.CatalogNumber;
                item.IsDuplicate = findedGear.IsDuplicate;
                item.Name = findedGear.Name;
                item.Price = orderItem.Price;
                item.Count = orderItem.Count;
                item.Discount = orderItem.Discount;
                item.Amount = (item.Price*item.Count) - item.Discount;
                item.PriceItem_Id = priceItem.Id;

                sd.SaleItems.Add(item);

            }

            context.SaleDocuments.Add(sd);
            context.SaveChanges();

            return new OkResult(this);
        }

        [HttpGet]
        public IEnumerable<ProductModel> GetNewProducts(string date)
        {
            IList<ProductModel> result = new List<ProductModel>();
            var context = new StoreDbContext();
            var dateDate = DateTime.Parse(date);
            var gearsNews = context.GearNews
                .Include("Gear")
                .Where(w => w.CreatedDate >= dateDate).ToList();
            foreach (var gearNews in gearsNews)
            {
                var priceItem = context.PriceItems
                    .Include("Prices")
                    .Include("Remainders")
                    .Where(p => p.Gear_Id == gearNews.Gear_Id).FirstOrDefault();
                ProductModel product = new ProductModel();
                product.Articul = gearNews.Gear.Articul;
                product.CatalogNumber = gearNews.Gear.CatalogNumber;
                product.Name = gearNews.Gear.Name;
                product.UnitOfMeasure = priceItem.Uom_Id;
                product.Price = priceItem.Prices.OrderByDescending(o => o.PriceDate)
                    .First().Price*(decimal) 1.2;

                foreach (var remainder in priceItem.Remainders)
                {
                    product.Remainders.Add(new RemaindersModel()
                    {
                        SalePoint = remainder.Warehouse_Id,
                        Count = (int)remainder.Amount
                    });
                }

                result.Add(product);
            }

            return result;
        }

        [HttpGet]
        public JsonResult<ProductModel> GetProduct(string articul)
        {
            ProductModel model = null;

            var context = new StoreDbContext();
            var findedGear = context.Gears.Where(g => g.Articul == articul).FirstOrDefault();
            if (findedGear != null)
            {
                var priceItem = context.PriceItems
                    .Include("Prices")
                    .Include("Remainders")
                    .Where(p => p.Gear_Id == findedGear.Id).FirstOrDefault();
                model = new ProductModel();
                model.Articul = findedGear.Articul;
                model.CatalogNumber = findedGear.CatalogNumber;
                model.Name = findedGear.Name;
                model.UnitOfMeasure = priceItem.Uom_Id;
                model.Price = priceItem.Prices.OrderByDescending(o => o.PriceDate)
                    .First().Price * (decimal)1.2;

                foreach (var remainder in priceItem.Remainders)
                {
                    model.Remainders.Add(new RemaindersModel()
                    {
                        SalePoint = remainder.Warehouse_Id,
                        Count = (int)remainder.Amount
                    });
                }
            }
            
            return Json(model);
        }

        [HttpGet]
        public JsonResult<IList<ProductModel>> GetAllProducts(string priceListName = "")
        {
            IList<ProductModel> result = new List<ProductModel>();
            var context = new StoreDbContext();

            IQueryable<PriceItem> priceItems = context.PriceItems
                .Include("Gear")
                .Include("Prices")
                .Include("Remainders");
            if (!string.IsNullOrEmpty(priceListName))
                priceItems = priceItems
                    .Where(w => w.PriceLists.Any(p => p.Name.ToLower() == priceListName.ToLower()));

            foreach (var priceItem in priceItems)
            {
                ProductModel product = new ProductModel();
                product.Articul = priceItem.Gear.Articul;
                product.CatalogNumber = priceItem.Gear.CatalogNumber;
                product.Name = priceItem.Gear.Name;
                product.UnitOfMeasure = priceItem.Uom_Id;
                product.Price = priceItem.Prices.OrderByDescending(o => o.PriceDate)
                    .First().Price * (decimal)1.2;

                foreach (var remainder in priceItem.Remainders)
                {
                    product.Remainders.Add(new RemaindersModel()
                    {
                        SalePoint = remainder.Warehouse_Id,
                        Count = (int)remainder.Amount
                    });
                }

                result.Add(product);
            }

            return Json(result);
        }

        [HttpGet]
        public JsonResult<IList<ProductModel>> ProductChanges(string from, string to)
        {
            var fromDate = DateTime.Parse(from);
            var toDate = DateTime.Parse(to);

            IList<ProductModel> result = new List<ProductModel>();

            var context = new StoreDbContext();
            var changes = context.GearsChanges
                .Include("Gear")
                .Where(g => g.ChangesDate >= fromDate && g.ChangesDate <= toDate).ToList();

            var lastPrdcts = (from ch in changes
                group ch by ch.Gear_Id
                into grs
                select new GearChanges()
                {
                    Gear_Id = grs.Key,
                    ChangesDate = grs.Max(m => m.ChangesDate)
                }).ToList();

            var things = from ch in changes
                join lst in lastPrdcts on new {Id = ch.Gear_Id, Date = ch.ChangesDate} equals
                    new {Id = lst.Gear_Id, Date = lst.ChangesDate}
                select ch;


            foreach (var change in things)
            {
                ProductModel product = new ProductModel();
                product.Articul = change.Gear.Articul;
                product.CatalogNumber = change.Gear.CatalogNumber;
                product.Name = change.Gear.Name;

                result.Add(product);
            }

            return Json(result);
        }

        [HttpGet]
        public JsonResult<IList<ProductModel>> ProductsPriceChanged(string from, string to)
        {
            var fromDate = DateTime.Parse(from);
            var toDate = DateTime.Parse(to);

            IList<ProductModel> result = new List<ProductModel>();

            var context = new StoreDbContext();

            var repChnages = context.PriceChangeReportItems
                .Include("PriceChangeReport")
                .Include("PriceItem")
                .Include("PriceItem.Gear")
                .Include("NewPrice")
                .Where(p => p.PriceChangeReport.ReportDate >= fromDate && p.PriceChangeReport.ReportDate <= toDate).ToList();

            var lastPrdcts = (from ch in repChnages
                             group ch by ch.PriceItem_Id
                                 into grs
                                 select new PriceChangeReportItemTEMP()
                                 {
                                     PriceItem_Id = grs.Key,
                                     LastDate = grs.Max(m => m.PriceChangeReport.ReportDate)
                                 }).ToList();

            var things = (from ch in repChnages
                         join lst in lastPrdcts on new { Id = ch.PriceItem_Id, Date = ch.PriceChangeReport.ReportDate } equals
                             new { Id = lst.PriceItem_Id, Date = lst.LastDate }
                         select ch).ToList();

            foreach (var repChange in things)
            {
                ProductModel product = new ProductModel();
                product.Articul = repChange.PriceItem.Gear.Articul;
                product.CatalogNumber = repChange.PriceItem.Gear.CatalogNumber;
                product.Name = repChange.PriceItem.Gear.Name;
                product.UnitOfMeasure = repChange.PriceItem.Uom_Id;
                product.Price = repChange.NewPrice.Price * (decimal)1.2;
                result.Add(product);
            }
            return Json(result);
        }


        [HttpGet]
        public JsonResult<IList<ProductRemainderModel>> ProductRemaindersChanged(string from, string to)
        {
            var fromDate = DateTime.Parse(from);
            var toDate = DateTime.Parse(to);

            IList<ProductRemainderModel> result = new List<ProductRemainderModel>();

            var context = new StoreDbContext();
            var changes = context.RemaindersChanges
                .Include("Remainder.PriceItem.Gear")
                .Where(g => g.ChangesDate >= fromDate && g.ChangesDate <= toDate).ToList();


            var lastPrdcts = (from ch in changes
                             group ch by ch.Remainder_Id
                                 into grs
                                 select new RemainderChanges()
                                 {
                                     Remainder_Id = grs.Key,
                                     ChangesDate = grs.Max(m => m.ChangesDate)
                                 }).ToList();

            var things = (from ch in changes
                         join lst in lastPrdcts on new { Id = ch.Remainder_Id, Date = ch.ChangesDate } equals
                             new { Id = lst.Remainder_Id, Date = lst.ChangesDate }
                         select ch).ToList();

            foreach (var change in things)
            {
                
                ProductRemainderModel product = new ProductRemainderModel();
                product.Articul = change.Remainder.PriceItem.Gear.Articul;
                product.CatalogNumber = change.Remainder.PriceItem.Gear.CatalogNumber;
                product.Name = change.Remainder.PriceItem.Gear.Name;
                product.SalePoint = change.Remainder.Warehouse_Id;
                product.Count = (int)change.Remainder.Amount;

                result.Add(product);
            }

            return Json(result);
        }
    }


    public class OrderItemModel
    {
        public string Articul { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Discount { get; set; }
    }

    public class ProductModel
    {
        public ProductModel()
        {
            Remainders = new List<RemaindersModel>();
        }


        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string Articul { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Price { get; set; }
        public IList<RemaindersModel> Remainders { get; set; }
    }

    public class RemaindersModel
    {
        public string SalePoint { get; set; }
        public int Count { get; set; }
    }

    public class ProductRemainderModel
    {

        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string Articul { get; set; }
        public string SalePoint { get; set; }
        public int Count { get; set; }
    }


    public class PriceChangeReportItemTEMP
    {
        public long PriceItem_Id { get; set; }
        public DateTime LastDate { get; set; }
    }
}
