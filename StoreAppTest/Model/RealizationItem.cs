namespace StoreAppTest.Model
{
    using StoreAppDataService;

    public class RealizationItem
    {
        public string Articul { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string IsDuplicate { get; set; }
        public int Number { get; set; }
        public string Uom { get; set; }

        public bool IsInDebt { get; set; }

        public string Customer
        {
            get;
            set; 
        }

        public int DebtDischarge
        {
            get;
            set; 
        }

        public SaleItem SaleItemData { get; set; }

        private int _price = 0;
        public int Price
        {
            get { return _price; } // - (Discount / SoldCount); }
            set
            {
                _price = value;
            }
        }

        public int PriceWithDiscount
        {
            get { return _price - (SoldCount == 0 ? 0 : Discount / SoldCount); }
        }

        private int _wholePrice = 0;
        public int WholePrice
        {
            get { return _wholePrice; }
            set
            {
                _wholePrice = value;
            }
        }
        public int Remainders { get; set; }
        public int SoldCount { get; set; }
        public int Amount { get; set; }
        public int Discount { get; set; }

    }
}
