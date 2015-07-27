namespace StoreAppTest.Model
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class IncomeItem : Notified
    {
        [DataMember]
        public string Articul { get; set; }
        [DataMember]
        public string CatalogNumber { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string IsDuplicate { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public long Gear_Id { get; set; }
        [DataMember]
        public string Uom { get; set; }
        [DataMember]
        public long PriceItem_Id { get; set; }
        [DataMember]
        public decimal RecommendedRemainder { get; set; }
        [DataMember]
        public decimal LowerLimitRemainder { get; set; }
        [DataMember]
        public int WholesalePrice { get; set; }
        [DataMember]
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
        [DataMember]
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

        [DataMember]
        public int Remainders { get; set; }
        [DataMember]
        public long Income_ID { get; set; }
        [DataMember]
        public string Income { get; set; }

        private int _Incomes;
        [DataMember]
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
        [DataMember]
        public bool IsAccepted
        {
            get { return _IsAccepted; }
            set
            {
                _IsAccepted = value;
                OnPropertyChanged("IsAccepted");
            }
        }
        [DataMember]
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

        [DataMember]
        public DateTime IncomeDate { get; set; }

        public event EventHandler IncomeItemItemChanged = (sender, args) => { };

    }
}
