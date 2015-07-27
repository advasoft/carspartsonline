namespace StoreAppTest.Model
{
    using System;
    using System.Runtime.Serialization;
    using Client.Model;

    [DataContract]
    public class RealizationItem : Notified
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
        public string Uom { get; set; }
        [DataMember]
        public string PriceListName { get; set; }
        [DataMember]
        public string SaledDate { get; set; }
        [DataMember]
        public string SaledNumber { get; set; }
        [DataMember]
        public bool IsInDebt { get; set; }
        [DataMember]
        public string Customer
        {
            get;
            set; 
        }
        [DataMember]
        public int DebtDischarge
        {
            get;
            set; 
        }
        [DataMember]
        public SaleItem SaleItemData { get; set; }

        private int _price = 0;
        [DataMember]
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
        [DataMember]
        public int WholePrice
        {
            get { return _wholePrice; }
            set
            {
                _wholePrice = value;
            }
        }
        [DataMember]
        public int Remainders { get; set; }
        [DataMember]
        public int SoldCount { get; set; }
        [DataMember]
        public int SaledCount { get; set; }
        [DataMember]
        public int Amount { get; set; }

        [DataMember]
        public int AmountWithoutDebt
        {
            get { return _AmountWithoutDebt; }
            set
            {
                _AmountWithoutDebt = value;
                OnPropertyChanged("AmountWithoutDebt");
            }
        }
        private int _AmountWithoutDebt;

        [DataMember]
        public int AmountWithoutDebtProfit
        {
            get { return _AmountWithoutDebtProfit; }
            set
            {
                _AmountWithoutDebtProfit = value;
                OnPropertyChanged("AmountWithoutDebtProfit");
            }
        }
        private int _AmountWithoutDebtProfit;

        [DataMember]
        public int Discount { get; set; }
        [DataMember]
        public string Additional { get; set; }
    }
}
