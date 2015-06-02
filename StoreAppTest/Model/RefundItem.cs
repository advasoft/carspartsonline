namespace StoreAppTest.Model
{
    using System;

    public class RefundItem : Notified
    {
        public string Articul { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string IsDuplicate { get; set; }
        public int Number { get; set; }
        public string Uom { get; set; }
        public long SaleItem_Id { get; set; }

        public int Discount
        {
            get;
            set; }

        public StoreAppDataService.PriceItem PriceItemData { get; set; }

        public int WhosalePrice { get; set; }

        private int _retailPrice;

        public int RetailPrice
        {
            get
            {
                return _retailPrice - (Discount / SoldCount);
            }
            set
            {
                _retailPrice = value;
                OnPropertyChanged("RetailPrice");
                OnPropertyChanged("Amount");
                RefundItemChanged(this, new EventArgs());
            }
        }

        private int _soldItem;
        public int SoldCount
        {
            get { return _soldItem; }
            set
            {
                _soldItem = value;
                OnPropertyChanged("SoldCount");
                OnPropertyChanged("Amount");
                RefundItemChanged(this, new EventArgs());
            }
        }

        public int PreviousSoldCount { get; set; }


        public int Amount
        {
            get
            {
                return (RetailPrice * SoldCount);
            }}

        public event EventHandler RefundItemChanged = (sender, args) => { };

    }
}
