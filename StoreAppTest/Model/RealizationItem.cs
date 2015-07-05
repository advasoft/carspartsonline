namespace StoreAppTest.Model
{
    using System;
    using StoreAppDataService;

    public class RealizationItem
    {
        public string Articul { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string IsDuplicate { get; set; }
        public int Number { get; set; }
        public string Uom { get; set; }
        public string PriceListName { get; set; }
        public string SaledDate { get; set; }
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
            get { return _price - (SaledCount == 0 ? 0 : Discount / SaledCount); }
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
        public int SaledCount { get; set; }
        public int Amount { get; set; }
        public int AmountWithoutDebt { get; set; }
        public int AmountWithoutDebtProfit { get; set; }

        public int Discount { get; set; }

        public string Additional { get; set; }
    }
}
