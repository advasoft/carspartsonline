
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
            var gearsNews = context.GearNews.Where(w => w.CreatedDate >= dateDate).ToList();
            foreach (var gearNews in gearsNews)
            {
                var priceItem = context.PriceItems.Where(p => p.Gear_Id == gearNews.Gear_Id).FirstOrDefault();
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
        public ProductModel GetProduct(string articul)
        {
            ProductModel model = null;

            var context = new StoreDbContext();
            var findedGear = context.Gears.Where(g => g.Articul == articul).FirstOrDefault();
            if (findedGear != null)
            {
                var priceItem = context.PriceItems.Where(p => p.Gear_Id == findedGear.Id).FirstOrDefault();
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
            
            return model;
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
}
