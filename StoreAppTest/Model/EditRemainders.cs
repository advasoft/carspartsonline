
namespace StoreAppTest.Model
{
    using System.Collections.ObjectModel;

    public class EditRemainders : Notified
    {
        public EditRemainders()
        {
            RemaindersItems = new ObservableCollection<EditRemaindersItem>();    
        }


        public string Name { get; set; }
        public ObservableCollection<EditRemaindersItem> RemaindersItems { get; set; }

    }

    public class EditRemaindersItem : Notified
    {
        public string Warehouse { get; set; }

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }
        private int _count;


    }

}
