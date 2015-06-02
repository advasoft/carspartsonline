
namespace StoreAppTest.Model
{
    using System.Collections.Generic;

    public class Price
    {
        public Price()
        {
            PriceItems = new List<PriceItem>();
        }

        public string PriceName { get; set; }

        public IList<PriceItem> PriceItems { get; set; } 
    }
}
