

namespace StoreAppTest.Model
{
    using System.Collections.ObjectModel;
    using StoreAppDataService;

    public class UserModel : Notified
    {

        public UserModel()
        {
            WarehouseList = new ObservableCollection<Warehouse>();
        }


        private string _UserName;
        private string _DisplayName;
        private string _Warehouse_Id;
        private bool _IsSupplierVisible;

        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                _DisplayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        public string Warehouse_Id
        {
            get { return _Warehouse_Id; }
            set
            {
                _Warehouse_Id = value;
                OnPropertyChanged("Warehouse_Id");
            }
        }

        public bool IsSupplierVisible
        {
            get { return _IsSupplierVisible; }
            set
            {
                _IsSupplierVisible = value;
                OnPropertyChanged("IsSupplierVisible");
            }
        }

        public ObservableCollection<Warehouse> WarehouseList { get; set; }
    }
}
