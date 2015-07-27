namespace StoreAppTest.Model
{
    using System;

    public class ReceiptItem : Notified
    {

        public string Articul
        {
            get
            {
                return _articul;
            }
            set
            {
                _articul = value;
                OnPropertyChanged("Articul");
            }
        }
        private string _articul;


        public string CatalogNumber
        {
            get
            {
                return _catalogNumbe;
            }
            set
            {
                _catalogNumbe = value;
                OnPropertyChanged("CatalogNumber");
            }
        }
        private string _catalogNumbe;


        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _name;


        public string IsDuplicate
        {
            get
            {
                return _isDuplicate;
            }
            set
            {
                _isDuplicate = value;
                OnPropertyChanged("IsDuplicate");
            }
        }
        private string _isDuplicate;

        public int Number { get; set; }
        public string Uom { get; set; }
        public long PriceItem_Id { get; set; }
        public Client.Model.PriceItem PriceItemData { get; set; }

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

        public int ClearPrice { get; set; }
        public int WholesalePrice { get; set; }

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

        public int Remainders { get; set; }

    }
}
