

namespace StoreAppTest.Model
{
    using System.Collections.ObjectModel;
    using StoreAppDataService;

    public class PriceItemEditModel : Notified
    {

        public PriceItemEditModel()
        {
            UomList = new ObservableCollection<UnitOfMeasure>();
        }

        private string _CatalogNumber;

        public string CatalogNumber
        {
            get { return _CatalogNumber; }
            set
            {
                _CatalogNumber = value;
                OnPropertyChanged("CatalogNumber");
            }
        }

        private string _Name;
        public string Name {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _Articul;

        public string Articul
        {
            get { return _Articul; }
            set
            {
                _Articul = value;
                OnPropertyChanged("Articul");
            }
        }

        private bool _IsDuplicate;

        public bool IsDuplicate
        {
            get { return _IsDuplicate; }
            set
            {
                _IsDuplicate = value;
                OnPropertyChanged("IsDuplicate");
            }
        }

        private int _RecommendedRemainder;

        public int RecommendedRemainder
        {
            get { return _RecommendedRemainder; }
            set
            {
                _RecommendedRemainder = value;
                OnPropertyChanged("RecommendedRemainder");
            }
        }

        private int _LowerLimitRemainder;

        public int LowerLimitRemainder
        {
            get { return _LowerLimitRemainder; }
            set
            {
                _LowerLimitRemainder = value;
                OnPropertyChanged("LowerLimitRemainder");
            }
        }


        private int _BuyPriceRur;

        public int BuyPriceRur
        {
            get { return _BuyPriceRur; }
            set
            {
                _BuyPriceRur = value;
                OnPropertyChanged("BuyPriceRur");
            }
        }

        private int _BuyPriceTng;

        public int BuyPriceTng
        {
            get { return _BuyPriceTng; }
            set
            {
                _BuyPriceTng = value;
                OnPropertyChanged("BuyPriceTng");
            }
        }


        private int _WholesalePrice;

        public int WholesalePrice
        {
            get { return _WholesalePrice; }
            set
            {
                _WholesalePrice = value;
                OnPropertyChanged("WholesalePrice");
            }
        }

        public int PreviousWholesalePrice { get; set; }

        public bool IsAdmin { get; set; }

        private UnitOfMeasure _Uom;

        public UnitOfMeasure Uom
        {
            get { return _Uom; }
            set
            {
                _Uom = value;
                OnPropertyChanged("Uom");
            }
        }

        public ObservableCollection<UnitOfMeasure> UomList { get; set; }

    }
}
