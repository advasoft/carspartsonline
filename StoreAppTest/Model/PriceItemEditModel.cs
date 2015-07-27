

namespace StoreAppTest.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Client.Model;
    using Utilities;

    public class PriceItemEditModel : Notified
    {

        public PriceItemEditModel()
        {
            UomList = new ObservableCollection<UnitOfMeasure>();
            InitCommands();
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


        private string _Barcode1;

        public string Barcode1
        {
            get { return _Barcode1; }
            set
            {
                _Barcode1 = value;
                OnPropertyChanged("Barcode1");
            }
        }

        private string _Barcode2;

        public string Barcode2
        {
            get { return _Barcode2; }
            set
            {
                _Barcode2 = value;
                OnPropertyChanged("Barcode2");
            }
        }

        private string _Barcode3;

        public string Barcode3
        {
            get { return _Barcode3; }
            set
            {
                _Barcode3 = value;
                OnPropertyChanged("Barcode3");
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
            get
            {
                return _Uom;
            }
            set
            {
                _Uom = value;
                OnPropertyChanged("Uom");
            }
        }

        public ObservableCollection<UnitOfMeasure> UomList { get; set; }


        #region Commands

        public ICommand GenerateBarcode1Command { get; set; }
        public ICommand GenerateBarcode2Command { get; set; }
        public ICommand GenerateBarcode3Command { get; set; }

        private void InitCommands()
        {
            #region GenerateBarcode1Command

            GenerateBarcode1Command = new UICommand(c =>
            {
                Barcode1 = GenerateBarcode();
            });
            GenerateBarcode2Command = new UICommand(c =>
            {
                Barcode2 = GenerateBarcode();
            });
            GenerateBarcode3Command = new UICommand(c =>
            {
                Barcode3 = GenerateBarcode();
            });
            #endregion
        }

        #endregion

        string GenerateBarcode()
        {
            var randDigit = Next(ulong.MinValue, ulong.MaxValue, new Random());
            var randDigitString = randDigit.ToString();
            int replaceCount = 5;
            var charRandomizer = new Random();
            var charPositionRandomizer = new Random();
            for (int i = 0; i < replaceCount; i++)
            {var position = charPositionRandomizer.Next(0, 17);
                var @char = Convert.ToChar(charRandomizer.Next(65, 90));

                randDigitString = randDigitString.Insert(position, @char.ToString());
            }
            return randDigitString;
        }

        //ulong LongRandom(ulong min, ulong max, Random rand)
        //{
        //    ulong result = (ulong)rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
        //    result = (result << 32);
        //    result = result | (ulong)rand.Next((Int32)min, (Int32)max);
        //    return result;
        //}

        ulong Next(ulong minValue, ulong maxValue, Random rand)
        {
            if (maxValue < minValue)
                throw new ArgumentException();

            return (ulong)(rand.NextDouble() * (maxValue - minValue)) + minValue;
        }

    }
}
