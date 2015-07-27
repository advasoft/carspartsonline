
namespace StoreAppTest.Model
{
    using System;

    public class PriceItem : Notified
    {
        public string Articul { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string IsDuplicate { get; set; }
        public int Number { get; set; }
        public int BuyPriceRur { get; set; }
        public int BuyPriceTng { get; set; }
        public string Uom { get; set; }

        public Client.Model.PriceItem PriceItemData { get; set; }

        private int _wholesalePrice = 0;
        public int WholesalePrice
        {
            get { return _wholesalePrice; }
            set
            {
                _wholesalePrice = value;
                OnPropertyChanged("WholesalePrice");
                //OnPropertyChanged("TwonyPercent");
            }
        }

        private int _remainders;
        public int Remainders
        {
            get { return _remainders; }
            set
            {
                _remainders = value;
                OnPropertyChanged("Remainders");
            }
        }

        private int _soldCount;
        public int SoldCount
        {
            get { return _soldCount; }
            set
            {
                _soldCount = value;
                OnPropertyChanged("SoldCount");
                if (PriceItemData != null)
                    CountChanges(this, new CountChangesEventArgs(value, PriceItemData.Id));

            }
        }
        public int TwonyPercent { get; set; //{ return (int)(_wholesalePrice * 1.2); }
        }


        public event EventHandler<CountChangesEventArgs> CountChanges = (sender, args) => { };
    }

    public class CountChangesEventArgs : EventArgs
    {

        public CountChangesEventArgs(int count, long priceItem_Id)
        {
            Count = count;
            PriceItem_Id = priceItem_Id;
        }
        public int Count { get; private set; }
        public long PriceItem_Id { get; private set; }
    }
}
