
namespace StoreAppTest.Model
{
    using System;
    using System.Collections.ObjectModel;

    public class AcceptIncomeModel : Notified
    {
        public AcceptIncomeModel()
        {
            IncomeItems = new ObservableCollection<IncomeItem>();
            IsEnabled = true;
        }


        public ObservableCollection<IncomeItem> IncomeItems { get; set; }


        public string IncomeNumber { get; set; }

        public DateTime IncomeDate { get; set; }


        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        private bool _isEnabled;

    }
}
