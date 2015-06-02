namespace StoreAppTest.Model
{
    using System;

    public class IncomeItem : Notified
    {
        public string Articul { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string IsDuplicate { get; set; }
        public int Number { get; set; }
        public long Gear_Id { get; set; }
        public string Uom { get; set; }
        public long PriceItem_Id { get; set; }
        public decimal RecommendedRemainder { get; set; }
        public decimal LowerLimitRemainder { get; set; }
        public int WholesalePrice { get; set; }

        public int BuyPriceRur
        {
            get
            {
                return _BuyPriceRur;
            }
            set
            {
                _BuyPriceRur = value;
                OnPropertyChanged("BuyPriceRur");
            }
        }

        private int _BuyPriceRur;



        public int BuyPriceTng
        {
            get
            {
                return _BuyPriceTng;
            }
            set
            {
                _BuyPriceTng = value;
                OnPropertyChanged("BuyPriceTng");
            }
        }

        private int _BuyPriceTng;

        public int Remainders { get; set; }
        public long Income_ID { get; set; }
        public string Income { get; set; }

        private int _Incomes;
        public int Incomes
        {
            get { return _Incomes; }
            set
            {
                _Incomes = value;
                OnPropertyChanged("Incomes");
                IncomeItemItemChanged(this, new EventArgs());
            }
        }

        private int _NewPrice;


        private bool _IsAccepted;

        public bool IsAccepted
        {
            get { return _IsAccepted; }
            set
            {
                _IsAccepted = value;
                OnPropertyChanged("IsAccepted");
            }
        }

        public int NewPrice
        {
            get { return _NewPrice; }
            set
            {
                _NewPrice = value;
                OnPropertyChanged("NewPrice");
                IncomeItemItemChanged(this, new EventArgs());
            }
        }
        public event EventHandler IncomeItemItemChanged = (sender, args) => { };

    }
}
