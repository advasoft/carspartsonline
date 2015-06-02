namespace StoreAppTest.Model
{
    using System;

    public class ReceiptItem : Notified
    {
        public string Articul { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string IsDuplicate { get; set; }
        public int Number { get; set; }
        public string Uom { get; set; }
        public long PriceItem_Id { get; set; }
        public StoreAppDataService.PriceItem PriceItemData { get; set; }

        private int _price = 0;
        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
                OnPropertyChanged("Amount");
                ReceiptItemChanged(this, new EventArgs());
            }
        }

        private int _discount;
        public int Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                OnPropertyChanged("Discount");
                OnPropertyChanged("Amount");
                ReceiptItemChanged(this, new EventArgs());
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
                ReceiptItemChanged(this, new EventArgs());
            }
        }

        public int Amount
        {
            get
            {
                return ((Price * SoldCount) - Discount);
            }}

        public event EventHandler ReceiptItemChanged = (sender, args) => { };

        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                OnPropertyChanged("Selected");
            }
        }

        private bool _Selected;

    }
}
