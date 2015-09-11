
namespace StoreAppTest.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Customer.
    /// </summary>
    /// <KeyProperties>
    /// Name
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Customers")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Name")]
    [Serializable]
    public partial class Customer : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Customer.
        /// </summary>
        /// <param name="name">Начальное значение Name.</param>
        /// <param name="creator_Id">Начальное значение Creator_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Customer CreateCustomer(string name, string creator_Id)
        {
            Customer customer = new Customer();
            customer.Name = name;
            customer.Creator_Id = creator_Id;
            return customer;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Details.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Details
        {
            get
            {
                return this._Details;
            }
            set
            {
                this.OnDetailsChanging(value);
                this._Details = value;
                this.OnDetailsChanged();
                this.OnPropertyChanged("Details");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Details;
        partial void OnDetailsChanging(string value);
        partial void OnDetailsChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BankDetails.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string BankDetails
        {
            get
            {
                return this._BankDetails;
            }
            set
            {
                this.OnBankDetailsChanging(value);
                this._BankDetails = value;
                this.OnBankDetailsChanged();
                this.OnPropertyChanged("BankDetails");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _BankDetails;
        partial void OnBankDetailsChanging(string value);
        partial void OnBankDetailsChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства ShipmentAddress.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string ShipmentAddress
        {
            get
            {
                return this._ShipmentAddress;
            }
            set
            {
                this.OnShipmentAddressChanging(value);
                this._ShipmentAddress = value;
                this.OnShipmentAddressChanged();
                this.OnPropertyChanged("ShipmentAddress");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _ShipmentAddress;
        partial void OnShipmentAddressChanging(string value);
        partial void OnShipmentAddressChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Creator_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Creator_Id
        {
            get
            {
                return this._Creator_Id;
            }
            set
            {
                this.OnCreator_IdChanging(value);
                this._Creator_Id = value;
                this.OnCreator_IdChanged();
                this.OnPropertyChanged("Creator_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Creator_Id;
        partial void OnCreator_IdChanging(string value);
        partial void OnCreator_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Creator.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
                this.OnPropertyChanged("Creator");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Creator;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.User.
    /// </summary>
    /// <KeyProperties>
    /// UserName
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Users")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("UserName")]
    [Serializable]
    [DataContract]
    public partial class User : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект User.
        /// </summary>
        /// <param name="userName">Начальное значение UserName.</param>
        /// <param name="isSupplierVisible">Начальное значение IsSupplierVisible.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static User CreateUser(string userName, bool isSupplierVisible)
        {
            User user = new User();
            user.UserName = userName;
            user.IsSupplierVisible = isSupplierVisible;
            return user;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства UserName.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                this.OnUserNameChanging(value);
                this._UserName = value;
                this.OnUserNameChanged();
                this.OnPropertyChanged("UserName");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _UserName;
        partial void OnUserNameChanging(string value);
        partial void OnUserNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства DisplayName.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string DisplayName
        {
            get
            {
                return this._DisplayName;
            }
            set
            {
                this.OnDisplayNameChanging(value);
                this._DisplayName = value;
                this.OnDisplayNameChanged();
                this.OnPropertyChanged("DisplayName");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _DisplayName;
        partial void OnDisplayNameChanging(string value);
        partial void OnDisplayNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Warehouse_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Warehouse_Id
        {
            get
            {
                return this._Warehouse_Id;
            }
            set
            {
                this.OnWarehouse_IdChanging(value);
                this._Warehouse_Id = value;
                this.OnWarehouse_IdChanged();
                this.OnPropertyChanged("Warehouse_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Warehouse_Id;
        partial void OnWarehouse_IdChanging(string value);
        partial void OnWarehouse_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PasswordHash.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public byte[] PasswordHash
        {
            get
            {
                if ((this._PasswordHash != null))
                {
                    return ((byte[])(this._PasswordHash.Clone()));
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.OnPasswordHashChanging(value);
                this._PasswordHash = value;
                this.OnPasswordHashChanged();
                this.OnPropertyChanged("PasswordHash");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private byte[] _PasswordHash;
        partial void OnPasswordHashChanging(byte[] value);
        partial void OnPasswordHashChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsSupplierVisible.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public bool IsSupplierVisible
        {
            get
            {
                return this._IsSupplierVisible;
            }
            set
            {
                this.OnIsSupplierVisibleChanging(value);
                this._IsSupplierVisible = value;
                this.OnIsSupplierVisibleChanged();
                this.OnPropertyChanged("IsSupplierVisible");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsSupplierVisible;
        partial void OnIsSupplierVisibleChanging(bool value);
        partial void OnIsSupplierVisibleChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceLists.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<PriceList> PriceLists
        {
            get
            {
                return this._PriceLists;
            }
            set
            {
                this._PriceLists = value;
                this.OnPropertyChanged("PriceLists");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<PriceList> _PriceLists = new global::System.Data.Services.Client.DataServiceCollection<PriceList>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для Roles.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<Role> Roles
        {
            get
            {
                return this._Roles;
            }
            set
            {
                this._Roles = value;
                this.OnPropertyChanged("Roles");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Role> _Roles = new global::System.Data.Services.Client.DataServiceCollection<Role>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для Warehouse.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public Warehouse Warehouse
        {
            get
            {
                return this._Warehouse;
            }
            set
            {
                this._Warehouse = value;
                this.OnPropertyChanged("Warehouse");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Warehouse _Warehouse;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceList.
    /// </summary>
    /// <KeyProperties>
    /// Name
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceLists")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Name")]
    [Serializable]
    public partial class PriceList : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceList.
        /// </summary>
        /// <param name="name">Начальное значение Name.</param>
        /// <param name="uploadUser_Id">Начальное значение UploadUser_Id.</param>
        /// <param name="uploadDate">Начальное значение UploadDate.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceList CreatePriceList(string name, string uploadUser_Id, global::System.DateTime uploadDate)
        {
            PriceList priceList = new PriceList();
            priceList.Name = name;
            priceList.UploadUser_Id = uploadUser_Id;
            priceList.UploadDate = uploadDate;
            return priceList;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства UploadUser_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string UploadUser_Id
        {
            get
            {
                return this._UploadUser_Id;
            }
            set
            {
                this.OnUploadUser_IdChanging(value);
                this._UploadUser_Id = value;
                this.OnUploadUser_IdChanged();
                this.OnPropertyChanged("UploadUser_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _UploadUser_Id;
        partial void OnUploadUser_IdChanging(string value);
        partial void OnUploadUser_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства UploadDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime UploadDate
        {
            get
            {
                return this._UploadDate;
            }
            set
            {
                this.OnUploadDateChanging(value);
                this._UploadDate = value;
                this.OnUploadDateChanged();
                this.OnPropertyChanged("UploadDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _UploadDate;
        partial void OnUploadDateChanging(global::System.DateTime value);
        partial void OnUploadDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceItems.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<PriceItem> PriceItems
        {
            get
            {
                return this._PriceItems;
            }
            set
            {
                this._PriceItems = value;
                this.OnPropertyChanged("PriceItems");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<PriceItem> _PriceItems = new global::System.Data.Services.Client.DataServiceCollection<PriceItem>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для UploadUser.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User UploadUser
        {
            get
            {
                return this._UploadUser;
            }
            set
            {
                this._UploadUser = value;
                this.OnPropertyChanged("UploadUser");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _UploadUser;
        /// <summary>
        /// В схеме отсутствуют комментарии для Users.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<User> Users
        {
            get
            {
                return this._Users;
            }
            set
            {
                this._Users = value;
                this.OnPropertyChanged("Users");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<User> _Users = new global::System.Data.Services.Client.DataServiceCollection<User>(null, global::System.Data.Services.Client.TrackingMode.None);
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceItems")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class PriceItem : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceItem.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="gear_Id">Начальное значение Gear_Id.</param>
        /// <param name="buyPriceRur">Начальное значение BuyPriceRur.</param>
        /// <param name="buyPriceTng">Начальное значение BuyPriceTng.</param>
        /// <param name="uom_Id">Начальное значение Uom_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceItem CreatePriceItem(long ID, long gear_Id, decimal buyPriceRur, decimal buyPriceTng, string uom_Id)
        {
            PriceItem priceItem = new PriceItem();
            priceItem.Id = ID;
            priceItem.Gear_Id = gear_Id;
            priceItem.BuyPriceRur = buyPriceRur;
            priceItem.BuyPriceTng = buyPriceTng;
            priceItem.Uom_Id = uom_Id;
            return priceItem;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Gear_Id
        {
            get
            {
                return this._Gear_Id;
            }
            set
            {
                this.OnGear_IdChanging(value);
                this._Gear_Id = value;
                this.OnGear_IdChanged();
                this.OnPropertyChanged("Gear_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Gear_Id;
        partial void OnGear_IdChanging(long value);
        partial void OnGear_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceRur.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal BuyPriceRur
        {
            get
            {
                return this._BuyPriceRur;
            }
            set
            {
                this.OnBuyPriceRurChanging(value);
                this._BuyPriceRur = value;
                this.OnBuyPriceRurChanged();
                this.OnPropertyChanged("BuyPriceRur");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceRur;
        partial void OnBuyPriceRurChanging(decimal value);
        partial void OnBuyPriceRurChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceTng.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal BuyPriceTng
        {
            get
            {
                return this._BuyPriceTng;
            }
            set
            {
                this.OnBuyPriceTngChanging(value);
                this._BuyPriceTng = value;
                this.OnBuyPriceTngChanged();
                this.OnPropertyChanged("BuyPriceTng");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceTng;
        partial void OnBuyPriceTngChanging(decimal value);
        partial void OnBuyPriceTngChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Uom_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Uom_Id
        {
            get
            {
                return this._Uom_Id;
            }
            set
            {
                this.OnUom_IdChanging(value);
                this._Uom_Id = value;
                this.OnUom_IdChanged();
                this.OnPropertyChanged("Uom_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Uom_Id;
        partial void OnUom_IdChanging(string value);
        partial void OnUom_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Barcode1.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Barcode1
        {
            get
            {
                return this._Barcode1;
            }
            set
            {
                this.OnBarcode1Changing(value);
                this._Barcode1 = value;
                this.OnBarcode1Changed();
                this.OnPropertyChanged("Barcode1");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Barcode1;
        partial void OnBarcode1Changing(string value);
        partial void OnBarcode1Changed();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Barcode2.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Barcode2
        {
            get
            {
                return this._Barcode2;
            }
            set
            {
                this.OnBarcode2Changing(value);
                this._Barcode2 = value;
                this.OnBarcode2Changed();
                this.OnPropertyChanged("Barcode2");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Barcode2;
        partial void OnBarcode2Changing(string value);
        partial void OnBarcode2Changed();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Barcode3.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Barcode3
        {
            get
            {
                return this._Barcode3;
            }
            set
            {
                this.OnBarcode3Changing(value);
                this._Barcode3 = value;
                this.OnBarcode3Changed();
                this.OnPropertyChanged("Barcode3");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Barcode3;
        partial void OnBarcode3Changing(string value);
        partial void OnBarcode3Changed();
        /// <summary>
        /// В схеме отсутствуют комментарии для Gear.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public Gear Gear
        {
            get
            {
                return this._Gear;
            }
            set
            {
                this._Gear = value;
                this.OnPropertyChanged("Gear");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Gear _Gear;
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceLists.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<PriceList> PriceLists
        {
            get
            {
                return this._PriceLists;
            }
            set
            {
                this._PriceLists = value;
                this.OnPropertyChanged("PriceLists");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<PriceList> _PriceLists = new global::System.Data.Services.Client.DataServiceCollection<PriceList>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для Prices.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<WholesalePrice> Prices
        {
            get
            {
                return this._Prices;
            }
            set
            {
                this._Prices = value;
                this.OnPropertyChanged("Prices");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<WholesalePrice> _Prices = new global::System.Data.Services.Client.DataServiceCollection<WholesalePrice>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для Remainders.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<Remainder> Remainders
        {
            get
            {
                return this._Remainders;
            }
            set
            {
                this._Remainders = value;
                this.OnPropertyChanged("Remainders");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Remainder> _Remainders = new global::System.Data.Services.Client.DataServiceCollection<Remainder>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для UnitOfMeasure.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public UnitOfMeasure UnitOfMeasure
        {
            get
            {
                return this._UnitOfMeasure;
            }
            set
            {
                this._UnitOfMeasure = value;
                this.OnPropertyChanged("UnitOfMeasure");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private UnitOfMeasure _UnitOfMeasure;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Gear.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Gears")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class Gear : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Gear.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="name">Начальное значение Name.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="category_Id">Начальное значение Category_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Gear CreateGear(long ID, string name, bool isDuplicate, string category_Id)
        {
            Gear gear = new Gear();
            gear.Id = ID;
            gear.Name = name;
            gear.IsDuplicate = isDuplicate;
            gear.Category_Id = category_Id;
            return gear;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Articul.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Articul
        {
            get
            {
                return this._Articul;
            }
            set
            {
                this.OnArticulChanging(value);
                this._Articul = value;
                this.OnArticulChanged();
                this.OnPropertyChanged("Articul");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Articul;
        partial void OnArticulChanging(string value);
        partial void OnArticulChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Category_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Category_Id
        {
            get
            {
                return this._Category_Id;
            }
            set
            {
                this.OnCategory_IdChanging(value);
                this._Category_Id = value;
                this.OnCategory_IdChanged();
                this.OnPropertyChanged("Category_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Category_Id;
        partial void OnCategory_IdChanging(string value);
        partial void OnCategory_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Image_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Nullable<long> Image_Id
        {
            get
            {
                return this._Image_Id;
            }
            set
            {
                this.OnImage_IdChanging(value);
                this._Image_Id = value;
                this.OnImage_IdChanged();
                this.OnPropertyChanged("Image_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Nullable<long> _Image_Id;
        partial void OnImage_IdChanging(global::System.Nullable<long> value);
        partial void OnImage_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для GearCategory.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public GearCategory GearCategory
        {
            get
            {
                return this._GearCategory;
            }
            set
            {
                this._GearCategory = value;
                this.OnPropertyChanged("GearCategory");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private GearCategory _GearCategory;
        /// <summary>
        /// В схеме отсутствуют комментарии для Image.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public Resource Image
        {
            get
            {
                return this._Image;
            }
            set
            {
                this._Image = value;
                this.OnPropertyChanged("Image");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Resource _Image;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.GearCategory.
    /// </summary>
    /// <KeyProperties>
    /// Name
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("GearCategories")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Name")]
    [Serializable]
    [DataContract]
    public partial class GearCategory : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект GearCategory.
        /// </summary>
        /// <param name="name">Начальное значение Name.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static GearCategory CreateGearCategory(string name)
        {
            GearCategory gearCategory = new GearCategory();
            gearCategory.Name = name;
            return gearCategory;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Gears.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<Gear> Gears
        {
            get
            {
                return this._Gears;
            }
            set
            {
                this._Gears = value;
                this.OnPropertyChanged("Gears");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Gear> _Gears = new global::System.Data.Services.Client.DataServiceCollection<Gear>(null, global::System.Data.Services.Client.TrackingMode.None);
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Resource.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Resources")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class Resource : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Resource.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Resource CreateResource(long ID)
        {
            Resource resource = new Resource();
            resource.Id = ID;
            return resource;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства ResourceData.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public byte[] ResourceData
        {
            get
            {
                if ((this._ResourceData != null))
                {
                    return ((byte[])(this._ResourceData.Clone()));
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.OnResourceDataChanging(value);
                this._ResourceData = value;
                this.OnResourceDataChanged();
                this.OnPropertyChanged("ResourceData");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private byte[] _ResourceData;
        partial void OnResourceDataChanging(byte[] value);
        partial void OnResourceDataChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.WholesalePrice.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("WholesalePrices")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class WholesalePrice : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект WholesalePrice.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="priceDate">Начальное значение PriceDate.</param>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="price">Начальное значение Price.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static WholesalePrice CreateWholesalePrice(long ID, global::System.DateTime priceDate, long priceItem_Id, decimal price)
        {
            WholesalePrice wholesalePrice = new WholesalePrice();
            wholesalePrice.Id = ID;
            wholesalePrice.PriceDate = priceDate;
            wholesalePrice.PriceItem_Id = priceItem_Id;
            wholesalePrice.Price = price;
            return wholesalePrice;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.DateTime PriceDate
        {
            get
            {
                return this._PriceDate;
            }
            set
            {
                this.OnPriceDateChanging(value);
                this._PriceDate = value;
                this.OnPriceDateChanged();
                this.OnPropertyChanged("PriceDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _PriceDate;
        partial void OnPriceDateChanging(global::System.DateTime value);
        partial void OnPriceDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Price.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Price
        {
            get
            {
                return this._Price;
            }
            set
            {
                this.OnPriceChanging(value);
                this._Price = value;
                this.OnPriceChanged();
                this.OnPropertyChanged("Price");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Price;
        partial void OnPriceChanging(decimal value);
        partial void OnPriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public PriceItem PriceItem
        {
            get
            {
                return this._PriceItem;
            }
            set
            {
                this._PriceItem = value;
                this.OnPropertyChanged("PriceItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private PriceItem _PriceItem;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Remainder.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Remainders")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class Remainder : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Remainder.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="remainderDate">Начальное значение RemainderDate.</param>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="warehouse_Id">Начальное значение Warehouse_Id.</param>
        /// <param name="amount">Начальное значение Amount.</param>
        /// <param name="recommendedRemainder">Начальное значение RecommendedRemainder.</param>
        /// <param name="lowerLimitRemainder">Начальное значение LowerLimitRemainder.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Remainder CreateRemainder(long ID, global::System.DateTime remainderDate, long priceItem_Id, string warehouse_Id, decimal amount, decimal recommendedRemainder, decimal lowerLimitRemainder)
        {
            Remainder remainder = new Remainder();
            remainder.Id = ID;
            remainder.RemainderDate = remainderDate;
            remainder.PriceItem_Id = priceItem_Id;
            remainder.Warehouse_Id = warehouse_Id;
            remainder.Amount = amount;
            remainder.RecommendedRemainder = recommendedRemainder;
            remainder.LowerLimitRemainder = lowerLimitRemainder;
            return remainder;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RemainderDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.DateTime RemainderDate
        {
            get
            {
                return this._RemainderDate;
            }
            set
            {
                this.OnRemainderDateChanging(value);
                this._RemainderDate = value;
                this.OnRemainderDateChanged();
                this.OnPropertyChanged("RemainderDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _RemainderDate;
        partial void OnRemainderDateChanging(global::System.DateTime value);
        partial void OnRemainderDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Warehouse_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Warehouse_Id
        {
            get
            {
                return this._Warehouse_Id;
            }
            set
            {
                this.OnWarehouse_IdChanging(value);
                this._Warehouse_Id = value;
                this.OnWarehouse_IdChanged();
                this.OnPropertyChanged("Warehouse_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Warehouse_Id;
        partial void OnWarehouse_IdChanging(string value);
        partial void OnWarehouse_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Amount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Amount
        {
            get
            {
                return this._Amount;
            }
            set
            {
                this.OnAmountChanging(value);
                this._Amount = value;
                this.OnAmountChanged();
                this.OnPropertyChanged("Amount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Amount;
        partial void OnAmountChanging(decimal value);
        partial void OnAmountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RecommendedRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal RecommendedRemainder
        {
            get
            {
                return this._RecommendedRemainder;
            }
            set
            {
                this.OnRecommendedRemainderChanging(value);
                this._RecommendedRemainder = value;
                this.OnRecommendedRemainderChanged();
                this.OnPropertyChanged("RecommendedRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _RecommendedRemainder;
        partial void OnRecommendedRemainderChanging(decimal value);
        partial void OnRecommendedRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства LowerLimitRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal LowerLimitRemainder
        {
            get
            {
                return this._LowerLimitRemainder;
            }
            set
            {
                this.OnLowerLimitRemainderChanging(value);
                this._LowerLimitRemainder = value;
                this.OnLowerLimitRemainderChanged();
                this.OnPropertyChanged("LowerLimitRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _LowerLimitRemainder;
        partial void OnLowerLimitRemainderChanging(decimal value);
        partial void OnLowerLimitRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public PriceItem PriceItem
        {
            get
            {
                return this._PriceItem;
            }
            set
            {
                this._PriceItem = value;
                this.OnPropertyChanged("PriceItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private PriceItem _PriceItem;
        /// <summary>
        /// В схеме отсутствуют комментарии для RemaindersUserChanges.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<RemaindersUserChange> RemaindersUserChanges
        {
            get
            {
                return this._RemaindersUserChanges;
            }
            set
            {
                this._RemaindersUserChanges = value;
                this.OnPropertyChanged("RemaindersUserChanges");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<RemaindersUserChange> _RemaindersUserChanges = new global::System.Data.Services.Client.DataServiceCollection<RemaindersUserChange>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для Warehouse.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public Warehouse Warehouse
        {
            get
            {
                return this._Warehouse;
            }
            set
            {
                this._Warehouse = value;
                this.OnPropertyChanged("Warehouse");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Warehouse _Warehouse;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.RemaindersUserChange.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("RemaindersUserChanges")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class RemaindersUserChange : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект RemaindersUserChange.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="date">Начальное значение Date.</param>
        /// <param name="user_Id">Начальное значение User_Id.</param>
        /// <param name="remainder_Id">Начальное значение Remainder_Id.</param>
        /// <param name="amound">Начальное значение Amound.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static RemaindersUserChange CreateRemaindersUserChange(long ID, global::System.DateTime date, string user_Id, long remainder_Id, decimal amound)
        {
            RemaindersUserChange remaindersUserChange = new RemaindersUserChange();
            remaindersUserChange.Id = ID;
            remaindersUserChange.Date = date;
            remaindersUserChange.User_Id = user_Id;
            remaindersUserChange.Remainder_Id = remainder_Id;
            remaindersUserChange.Amound = amound;
            return remaindersUserChange;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Date.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.DateTime Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                this.OnDateChanging(value);
                this._Date = value;
                this.OnDateChanged();
                this.OnPropertyChanged("Date");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _Date;
        partial void OnDateChanging(global::System.DateTime value);
        partial void OnDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства User_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string User_Id
        {
            get
            {
                return this._User_Id;
            }
            set
            {
                this.OnUser_IdChanging(value);
                this._User_Id = value;
                this.OnUser_IdChanged();
                this.OnPropertyChanged("User_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _User_Id;
        partial void OnUser_IdChanging(string value);
        partial void OnUser_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainder_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Remainder_Id
        {
            get
            {
                return this._Remainder_Id;
            }
            set
            {
                this.OnRemainder_IdChanging(value);
                this._Remainder_Id = value;
                this.OnRemainder_IdChanged();
                this.OnPropertyChanged("Remainder_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Remainder_Id;
        partial void OnRemainder_IdChanging(long value);
        partial void OnRemainder_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Amound.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Amound
        {
            get
            {
                return this._Amound;
            }
            set
            {
                this.OnAmoundChanging(value);
                this._Amound = value;
                this.OnAmoundChanged();
                this.OnPropertyChanged("Amound");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Amound;
        partial void OnAmoundChanging(decimal value);
        partial void OnAmoundChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Remainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public Remainder Remainder
        {
            get
            {
                return this._Remainder;
            }
            set
            {
                this._Remainder = value;
                this.OnPropertyChanged("Remainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Remainder _Remainder;
        /// <summary>
        /// В схеме отсутствуют комментарии для User.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public User User
        {
            get
            {
                return this._User;
            }
            set
            {
                this._User = value;
                this.OnPropertyChanged("User");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _User;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Warehouse.
    /// </summary>
    /// <KeyProperties>
    /// Name
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Warehouses")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Name")]
    [Serializable]
    [DataContract]
    public partial class Warehouse : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Warehouse.
        /// </summary>
        /// <param name="name">Начальное значение Name.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Warehouse CreateWarehouse(string name)
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Name = name;
            return warehouse;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.UnitOfMeasure.
    /// </summary>
    /// <KeyProperties>
    /// Name
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("UnitOfMeasures")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Name")]
    [Serializable]
    [DataContract]
    public partial class UnitOfMeasure : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект UnitOfMeasure.
        /// </summary>
        /// <param name="name">Начальное значение Name.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static UnitOfMeasure CreateUnitOfMeasure(string name)
        {
            UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
            unitOfMeasure.Name = name;
            return unitOfMeasure;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }

        #region Overrides of Object

        /// <summary>
        /// Определяет, равен ли заданный объект <see cref="T:System.Object"/> текущему объекту <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// Значение true, если заданный объект <see cref="T:System.Object"/> равен текущему объекту <see cref="T:System.Object"/>; в противном случае — значение false.
        /// </returns>
        /// <param name="obj">Элемент <see cref="T:System.Object"/>, который требуется сравнить с текущим элементом <see cref="T:System.Object"/>. </param>
        public override bool Equals(object obj)
        {
            var obje = obj as UnitOfMeasure;
            if (obje != null)
                return obje.Name == Name;
            return false;
        }

        #endregion
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Role.
    /// </summary>
    /// <KeyProperties>
    /// RoleName
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Roles")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("RoleName")]
    [Serializable]
    public partial class Role : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Role.
        /// </summary>
        /// <param name="roleName">Начальное значение RoleName.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Role CreateRole(string roleName)
        {
            Role role = new Role();
            role.RoleName = roleName;
            return role;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RoleName.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string RoleName
        {
            get
            {
                return this._RoleName;
            }
            set
            {
                this.OnRoleNameChanging(value);
                this._RoleName = value;
                this.OnRoleNameChanged();
                this.OnPropertyChanged("RoleName");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _RoleName;
        partial void OnRoleNameChanging(string value);
        partial void OnRoleNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Users.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<User> Users
        {
            get
            {
                return this._Users;
            }
            set
            {
                this._Users = value;
                this.OnPropertyChanged("Users");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<User> _Users = new global::System.Data.Services.Client.DataServiceCollection<User>(null, global::System.Data.Services.Client.TrackingMode.None);
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.DebtDischargeDocument.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("DebtDischargeDocuments")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class DebtDischargeDocument : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект DebtDischargeDocument.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="dischargeDate">Начальное значение DischargeDate.</param>
        /// <param name="debtor_Id">Начальное значение Debtor_Id.</param>
        /// <param name="amount">Начальное значение Amount.</param>
        /// <param name="creator_Id">Начальное значение Creator_Id.</param>
        /// <param name="isDischarge">Начальное значение IsDischarge.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static DebtDischargeDocument CreateDebtDischargeDocument(long ID, global::System.DateTime dischargeDate, string debtor_Id, decimal amount, string creator_Id, bool isDischarge)
        {
            DebtDischargeDocument debtDischargeDocument = new DebtDischargeDocument();
            debtDischargeDocument.Id = ID;
            debtDischargeDocument.DischargeDate = dischargeDate;
            debtDischargeDocument.Debtor_Id = debtor_Id;
            debtDischargeDocument.Amount = amount;
            debtDischargeDocument.Creator_Id = creator_Id;
            debtDischargeDocument.IsDischarge = isDischarge;
            return debtDischargeDocument;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства DischargeDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime DischargeDate
        {
            get
            {
                return this._DischargeDate;
            }
            set
            {
                this.OnDischargeDateChanging(value);
                this._DischargeDate = value;
                this.OnDischargeDateChanged();
                this.OnPropertyChanged("DischargeDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _DischargeDate;
        partial void OnDischargeDateChanging(global::System.DateTime value);
        partial void OnDischargeDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Debtor_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Debtor_Id
        {
            get
            {
                return this._Debtor_Id;
            }
            set
            {
                this.OnDebtor_IdChanging(value);
                this._Debtor_Id = value;
                this.OnDebtor_IdChanged();
                this.OnPropertyChanged("Debtor_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Debtor_Id;
        partial void OnDebtor_IdChanging(string value);
        partial void OnDebtor_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Amount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Amount
        {
            get
            {
                return this._Amount;
            }
            set
            {
                this.OnAmountChanging(value);
                this._Amount = value;
                this.OnAmountChanged();
                this.OnPropertyChanged("Amount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Amount;
        partial void OnAmountChanging(decimal value);
        partial void OnAmountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Creator_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Creator_Id
        {
            get
            {
                return this._Creator_Id;
            }
            set
            {
                this.OnCreator_IdChanging(value);
                this._Creator_Id = value;
                this.OnCreator_IdChanged();
                this.OnPropertyChanged("Creator_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Creator_Id;
        partial void OnCreator_IdChanging(string value);
        partial void OnCreator_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDischarge.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsDischarge
        {
            get
            {
                return this._IsDischarge;
            }
            set
            {
                this.OnIsDischargeChanging(value);
                this._IsDischarge = value;
                this.OnIsDischargeChanged();
                this.OnPropertyChanged("IsDischarge");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDischarge;
        partial void OnIsDischargeChanging(bool value);
        partial void OnIsDischargeChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Creator.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
                this.OnPropertyChanged("Creator");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Creator;
        /// <summary>
        /// В схеме отсутствуют комментарии для Debtor.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Customer Debtor
        {
            get
            {
                return this._Debtor;
            }
            set
            {
                this._Debtor = value;
                this.OnPropertyChanged("Debtor");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Customer _Debtor;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.GearNew.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("GearNews")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class GearNew : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект GearNew.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="createdDate">Начальное значение CreatedDate.</param>
        /// <param name="gear_Id">Начальное значение Gear_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static GearNew CreateGearNew(long ID, global::System.DateTime createdDate, long gear_Id)
        {
            GearNew gearNew = new GearNew();
            gearNew.Id = ID;
            gearNew.CreatedDate = createdDate;
            gearNew.Gear_Id = gear_Id;
            return gearNew;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CreatedDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime CreatedDate
        {
            get
            {
                return this._CreatedDate;
            }
            set
            {
                this.OnCreatedDateChanging(value);
                this._CreatedDate = value;
                this.OnCreatedDateChanged();
                this.OnPropertyChanged("CreatedDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _CreatedDate;
        partial void OnCreatedDateChanging(global::System.DateTime value);
        partial void OnCreatedDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Gear_Id
        {
            get
            {
                return this._Gear_Id;
            }
            set
            {
                this.OnGear_IdChanging(value);
                this._Gear_Id = value;
                this.OnGear_IdChanged();
                this.OnPropertyChanged("Gear_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Gear_Id;
        partial void OnGear_IdChanging(long value);
        partial void OnGear_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Gear.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Gear Gear
        {
            get
            {
                return this._Gear;
            }
            set
            {
                this._Gear = value;
                this.OnPropertyChanged("Gear");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Gear _Gear;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.IncomeItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("IncomeItems")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class IncomeItem : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект IncomeItem.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="amount">Начальное значение Amount.</param>
        /// <param name="newPrice">Начальное значение NewPrice.</param>
        /// <param name="buyPriceRur">Начальное значение BuyPriceRur.</param>
        /// <param name="buyPriceTng">Начальное значение BuyPriceTng.</param>
        /// <param name="income_Id">Начальное значение Income_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static IncomeItem CreateIncomeItem(long ID, long priceItem_Id, decimal amount, decimal newPrice, decimal buyPriceRur, decimal buyPriceTng, long income_Id)
        {
            IncomeItem incomeItem = new IncomeItem();
            incomeItem.Id = ID;
            incomeItem.PriceItem_Id = priceItem_Id;
            incomeItem.Amount = amount;
            incomeItem.NewPrice = newPrice;
            incomeItem.BuyPriceRur = buyPriceRur;
            incomeItem.BuyPriceTng = buyPriceTng;
            incomeItem.Income_Id = income_Id;
            return incomeItem;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Amount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Amount
        {
            get
            {
                return this._Amount;
            }
            set
            {
                this.OnAmountChanging(value);
                this._Amount = value;
                this.OnAmountChanged();
                this.OnPropertyChanged("Amount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Amount;
        partial void OnAmountChanging(decimal value);
        partial void OnAmountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства NewPrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal NewPrice
        {
            get
            {
                return this._NewPrice;
            }
            set
            {
                this.OnNewPriceChanging(value);
                this._NewPrice = value;
                this.OnNewPriceChanged();
                this.OnPropertyChanged("NewPrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _NewPrice;
        partial void OnNewPriceChanging(decimal value);
        partial void OnNewPriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceRur.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceRur
        {
            get
            {
                return this._BuyPriceRur;
            }
            set
            {
                this.OnBuyPriceRurChanging(value);
                this._BuyPriceRur = value;
                this.OnBuyPriceRurChanged();
                this.OnPropertyChanged("BuyPriceRur");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceRur;
        partial void OnBuyPriceRurChanging(decimal value);
        partial void OnBuyPriceRurChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceTng.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceTng
        {
            get
            {
                return this._BuyPriceTng;
            }
            set
            {
                this.OnBuyPriceTngChanging(value);
                this._BuyPriceTng = value;
                this.OnBuyPriceTngChanged();
                this.OnPropertyChanged("BuyPriceTng");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceTng;
        partial void OnBuyPriceTngChanging(decimal value);
        partial void OnBuyPriceTngChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Income_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Income_Id
        {
            get
            {
                return this._Income_Id;
            }
            set
            {
                this.OnIncome_IdChanging(value);
                this._Income_Id = value;
                this.OnIncome_IdChanged();
                this.OnPropertyChanged("Income_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Income_Id;
        partial void OnIncome_IdChanging(long value);
        partial void OnIncome_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Income.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Income Income
        {
            get
            {
                return this._Income;
            }
            set
            {
                this._Income = value;
                this.OnPropertyChanged("Income");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Income _Income;
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public PriceItem PriceItem
        {
            get
            {
                return this._PriceItem;
            }
            set
            {
                this._PriceItem = value;
                this.OnPropertyChanged("PriceItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private PriceItem _PriceItem;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Income.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Incomes")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class Income : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Income.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="incomeNumber">Начальное значение IncomeNumber.</param>
        /// <param name="incomeDate">Начальное значение IncomeDate.</param>
        /// <param name="isAccept">Начальное значение IsAccept.</param>
        /// <param name="supplier_Id">Начальное значение Supplier_Id.</param>
        /// <param name="creator_Id">Начальное значение Creator_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Income CreateIncome(long ID, string incomeNumber, global::System.DateTime incomeDate, bool isAccept, string supplier_Id, string creator_Id)
        {
            Income income = new Income();
            income.Id = ID;
            income.IncomeNumber = incomeNumber;
            income.IncomeDate = incomeDate;
            income.IsAccept = isAccept;
            income.Supplier_Id = supplier_Id;
            income.Creator_Id = creator_Id;
            return income;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IncomeNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string IncomeNumber
        {
            get
            {
                return this._IncomeNumber;
            }
            set
            {
                this.OnIncomeNumberChanging(value);
                this._IncomeNumber = value;
                this.OnIncomeNumberChanged();
                this.OnPropertyChanged("IncomeNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _IncomeNumber;
        partial void OnIncomeNumberChanging(string value);
        partial void OnIncomeNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IncomeDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime IncomeDate
        {
            get
            {
                return this._IncomeDate;
            }
            set
            {
                this.OnIncomeDateChanging(value);
                this._IncomeDate = value;
                this.OnIncomeDateChanged();
                this.OnPropertyChanged("IncomeDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _IncomeDate;
        partial void OnIncomeDateChanging(global::System.DateTime value);
        partial void OnIncomeDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsAccept.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsAccept
        {
            get
            {
                return this._IsAccept;
            }
            set
            {
                this.OnIsAcceptChanging(value);
                this._IsAccept = value;
                this.OnIsAcceptChanged();
                this.OnPropertyChanged("IsAccept");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsAccept;
        partial void OnIsAcceptChanging(bool value);
        partial void OnIsAcceptChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства AcceptedDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Nullable<global::System.DateTime> AcceptedDate
        {
            get
            {
                return this._AcceptedDate;
            }
            set
            {
                this.OnAcceptedDateChanging(value);
                this._AcceptedDate = value;
                this.OnAcceptedDateChanged();
                this.OnPropertyChanged("AcceptedDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Nullable<global::System.DateTime> _AcceptedDate;
        partial void OnAcceptedDateChanging(global::System.Nullable<global::System.DateTime> value);
        partial void OnAcceptedDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Supplier_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Supplier_Id
        {
            get
            {
                return this._Supplier_Id;
            }
            set
            {
                this.OnSupplier_IdChanging(value);
                this._Supplier_Id = value;
                this.OnSupplier_IdChanged();
                this.OnPropertyChanged("Supplier_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Supplier_Id;
        partial void OnSupplier_IdChanging(string value);
        partial void OnSupplier_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Creator_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Creator_Id
        {
            get
            {
                return this._Creator_Id;
            }
            set
            {
                this.OnCreator_IdChanging(value);
                this._Creator_Id = value;
                this.OnCreator_IdChanged();
                this.OnPropertyChanged("Creator_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Creator_Id;
        partial void OnCreator_IdChanging(string value);
        partial void OnCreator_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Accepter_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Accepter_Id
        {
            get
            {
                return this._Accepter_Id;
            }
            set
            {
                this.OnAccepter_IdChanging(value);
                this._Accepter_Id = value;
                this.OnAccepter_IdChanged();
                this.OnPropertyChanged("Accepter_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Accepter_Id;
        partial void OnAccepter_IdChanging(string value);
        partial void OnAccepter_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Accepter.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User Accepter
        {
            get
            {
                return this._Accepter;
            }
            set
            {
                this._Accepter = value;
                this.OnPropertyChanged("Accepter");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Accepter;
        /// <summary>
        /// В схеме отсутствуют комментарии для Creator.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
                this.OnPropertyChanged("Creator");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Creator;
        /// <summary>
        /// В схеме отсутствуют комментарии для IncomeItems.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<IncomeItem> IncomeItems
        {
            get
            {
                return this._IncomeItems;
            }
            set
            {
                this._IncomeItems = value;
                this.OnPropertyChanged("IncomeItems");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<IncomeItem> _IncomeItems = new global::System.Data.Services.Client.DataServiceCollection<IncomeItem>(null, global::System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// В схеме отсутствуют комментарии для Supplier.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Supplier Supplier
        {
            get
            {
                return this._Supplier;
            }
            set
            {
                this._Supplier = value;
                this.OnPropertyChanged("Supplier");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Supplier _Supplier;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.Supplier.
    /// </summary>
    /// <KeyProperties>
    /// Name
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Suppliers")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Name")]
    [Serializable]
    public partial class Supplier : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект Supplier.
        /// </summary>
        /// <param name="name">Начальное значение Name.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Supplier CreateSupplier(string name)
        {
            Supplier supplier = new Supplier();
            supplier.Name = name;
            return supplier;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства City.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string City
        {
            get
            {
                return this._City;
            }
            set
            {
                this.OnCityChanging(value);
                this._City = value;
                this.OnCityChanged();
                this.OnPropertyChanged("City");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _City;
        partial void OnCityChanging(string value);
        partial void OnCityChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Phone.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Phone
        {
            get
            {
                return this._Phone;
            }
            set
            {
                this.OnPhoneChanging(value);
                this._Phone = value;
                this.OnPhoneChanged();
                this.OnPropertyChanged("Phone");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Phone;
        partial void OnPhoneChanging(string value);
        partial void OnPhoneChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Email.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this.OnEmailChanging(value);
                this._Email = value;
                this.OnEmailChanged();
                this.OnPropertyChanged("Email");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Email;
        partial void OnEmailChanging(string value);
        partial void OnEmailChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Manager.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Manager
        {
            get
            {
                return this._Manager;
            }
            set
            {
                this.OnManagerChanging(value);
                this._Manager = value;
                this.OnManagerChanged();
                this.OnPropertyChanged("Manager");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Manager;
        partial void OnManagerChanging(string value);
        partial void OnManagerChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceChangeReportItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceChangeReportItems")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class PriceChangeReportItem : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceChangeReportItem.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="previousPrice_Id">Начальное значение PreviousPrice_Id.</param>
        /// <param name="newPrice_Id">Начальное значение NewPrice_Id.</param>
        /// <param name="priceChangeReport_Id">Начальное значение PriceChangeReport_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceChangeReportItem CreatePriceChangeReportItem(long ID, long priceItem_Id, long previousPrice_Id, long newPrice_Id, long priceChangeReport_Id)
        {
            PriceChangeReportItem priceChangeReportItem = new PriceChangeReportItem();
            priceChangeReportItem.Id = ID;
            priceChangeReportItem.PriceItem_Id = priceItem_Id;
            priceChangeReportItem.PreviousPrice_Id = previousPrice_Id;
            priceChangeReportItem.NewPrice_Id = newPrice_Id;
            priceChangeReportItem.PriceChangeReport_Id = priceChangeReport_Id;
            return priceChangeReportItem;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PreviousPrice_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long PreviousPrice_Id
        {
            get
            {
                return this._PreviousPrice_Id;
            }
            set
            {
                this.OnPreviousPrice_IdChanging(value);
                this._PreviousPrice_Id = value;
                this.OnPreviousPrice_IdChanged();
                this.OnPropertyChanged("PreviousPrice_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PreviousPrice_Id;
        partial void OnPreviousPrice_IdChanging(long value);
        partial void OnPreviousPrice_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства NewPrice_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long NewPrice_Id
        {
            get
            {
                return this._NewPrice_Id;
            }
            set
            {
                this.OnNewPrice_IdChanging(value);
                this._NewPrice_Id = value;
                this.OnNewPrice_IdChanged();
                this.OnPropertyChanged("NewPrice_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _NewPrice_Id;
        partial void OnNewPrice_IdChanging(long value);
        partial void OnNewPrice_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceChangeReport_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long PriceChangeReport_Id
        {
            get
            {
                return this._PriceChangeReport_Id;
            }
            set
            {
                this.OnPriceChangeReport_IdChanging(value);
                this._PriceChangeReport_Id = value;
                this.OnPriceChangeReport_IdChanged();
                this.OnPropertyChanged("PriceChangeReport_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceChangeReport_Id;
        partial void OnPriceChangeReport_IdChanging(long value);
        partial void OnPriceChangeReport_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для NewPrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public WholesalePrice NewPrice
        {
            get
            {
                return this._NewPrice;
            }
            set
            {
                this._NewPrice = value;
                this.OnPropertyChanged("NewPrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private WholesalePrice _NewPrice;
        /// <summary>
        /// В схеме отсутствуют комментарии для PreviousPrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public WholesalePrice PreviousPrice
        {
            get
            {
                return this._PreviousPrice;
            }
            set
            {
                this._PreviousPrice = value;
                this.OnPropertyChanged("PreviousPrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private WholesalePrice _PreviousPrice;
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceChangeReport.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public PriceChangeReport PriceChangeReport
        {
            get
            {
                return this._PriceChangeReport;
            }
            set
            {
                this._PriceChangeReport = value;
                this.OnPropertyChanged("PriceChangeReport");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private PriceChangeReport _PriceChangeReport;
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public PriceItem PriceItem
        {
            get
            {
                return this._PriceItem;
            }
            set
            {
                this._PriceItem = value;
                this.OnPropertyChanged("PriceItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private PriceItem _PriceItem;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceChangeReport.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceChangeReports")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class PriceChangeReport : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceChangeReport.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="reportNumber">Начальное значение ReportNumber.</param>
        /// <param name="reportDate">Начальное значение ReportDate.</param>
        /// <param name="creator_Id">Начальное значение Creator_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceChangeReport CreatePriceChangeReport(long ID, string reportNumber, global::System.DateTime reportDate, string creator_Id)
        {
            PriceChangeReport priceChangeReport = new PriceChangeReport();
            priceChangeReport.Id = ID;
            priceChangeReport.ReportNumber = reportNumber;
            priceChangeReport.ReportDate = reportDate;
            priceChangeReport.Creator_Id = creator_Id;
            return priceChangeReport;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства ReportNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string ReportNumber
        {
            get
            {
                return this._ReportNumber;
            }
            set
            {
                this.OnReportNumberChanging(value);
                this._ReportNumber = value;
                this.OnReportNumberChanged();
                this.OnPropertyChanged("ReportNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _ReportNumber;
        partial void OnReportNumberChanging(string value);
        partial void OnReportNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства ReportDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.DateTime ReportDate
        {
            get
            {
                return this._ReportDate;
            }
            set
            {
                this.OnReportDateChanging(value);
                this._ReportDate = value;
                this.OnReportDateChanged();
                this.OnPropertyChanged("ReportDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _ReportDate;
        partial void OnReportDateChanging(global::System.DateTime value);
        partial void OnReportDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Creator_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Creator_Id
        {
            get
            {
                return this._Creator_Id;
            }
            set
            {
                this.OnCreator_IdChanging(value);
                this._Creator_Id = value;
                this.OnCreator_IdChanged();
                this.OnPropertyChanged("Creator_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Creator_Id;
        partial void OnCreator_IdChanging(string value);
        partial void OnCreator_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Creator.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public User Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
                this.OnPropertyChanged("Creator");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Creator;
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceChangeReportItems.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<PriceChangeReportItem> PriceChangeReportItems
        {
            get
            {
                return this._PriceChangeReportItems;
            }
            set
            {
                this._PriceChangeReportItems = value;
                this.OnPropertyChanged("PriceChangeReportItems");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<PriceChangeReportItem> _PriceChangeReportItems = new global::System.Data.Services.Client.DataServiceCollection<PriceChangeReportItem>(null, global::System.Data.Services.Client.TrackingMode.None);
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.RefundDocument.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [DataContract]
    [Serializable]
    public partial class RefundDocument
    {
        public RefundDocument()
        {
            RefundItems = new List<RefundItem>();
        }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string RefundNumber { get; set; }

        [DataMember]
        public DateTime RefundDate { get; set; }

        [DataMember]
        //[ForeignKey("SaleDocument")]
        public long SaleDocument_Id { get; set; }

        [DataMember]
        //[ForeignKey("Creator")]
        public string Creator_Id { get; set; }

        [DataMember]
        //[ForeignKey("LastChanger")]
        public string LastChanger_Id { get; set; }

        [DataMember]
        public virtual SaleDocument SaleDocument { get; set; }

        [DataMember]
        public virtual User Creator { get; set; }

        [DataMember]
        public virtual User LastChanger { get; set; }

        [DataMember]
        public virtual List<RefundItem> RefundItems { get; set; }
    }    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.RefundItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [DataContract]
    [Serializable]
    public partial class RefundItem
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long PriceItem_Id { get; set; }

        [DataMember]
        public long SaleItem_Id { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public decimal Count { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        //[ForeignKey("RefundDocument")]
        public long RefundDocument_Id { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }

        [DataMember]
        public virtual SaleItem SaleItem { get; set; }

        [DataMember]
        public virtual RefundDocument RefundDocument { get; set; }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.SaleItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("SaleItems")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class SaleItem : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект SaleItem.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="price">Начальное значение Price.</param>
        /// <param name="discount">Начальное значение Discount.</param>
        /// <param name="count">Начальное значение Count.</param>
        /// <param name="amount">Начальное значение Amount.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="saleDocument_Id">Начальное значение SaleDocument_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static SaleItem CreateSaleItem(long ID, long priceItem_Id, decimal price, decimal discount, decimal count, decimal amount, bool isDuplicate, long saleDocument_Id)
        {
            SaleItem saleItem = new SaleItem();
            saleItem.Id = ID;
            saleItem.PriceItem_Id = priceItem_Id;
            saleItem.Price = price;
            saleItem.Discount = discount;
            saleItem.Count = count;
            saleItem.Amount = amount;
            saleItem.IsDuplicate = isDuplicate;
            saleItem.SaleDocument_Id = saleDocument_Id;
            return saleItem;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Price.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Price
        {
            get
            {
                return this._Price;
            }
            set
            {
                this.OnPriceChanging(value);
                this._Price = value;
                this.OnPriceChanged();
                this.OnPropertyChanged("Price");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Price;
        partial void OnPriceChanging(decimal value);
        partial void OnPriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Discount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Discount
        {
            get
            {
                return this._Discount;
            }
            set
            {
                this.OnDiscountChanging(value);
                this._Discount = value;
                this.OnDiscountChanged();
                this.OnPropertyChanged("Discount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Discount;
        partial void OnDiscountChanging(decimal value);
        partial void OnDiscountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Count.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Count
        {
            get
            {
                return this._Count;
            }
            set
            {
                this.OnCountChanging(value);
                this._Count = value;
                this.OnCountChanged();
                this.OnPropertyChanged("Count");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Count;
        partial void OnCountChanging(decimal value);
        partial void OnCountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Amount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Amount
        {
            get
            {
                return this._Amount;
            }
            set
            {
                this.OnAmountChanging(value);
                this._Amount = value;
                this.OnAmountChanged();
                this.OnPropertyChanged("Amount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Amount;
        partial void OnAmountChanging(decimal value);
        partial void OnAmountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Articul.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Articul
        {
            get
            {
                return this._Articul;
            }
            set
            {
                this.OnArticulChanging(value);
                this._Articul = value;
                this.OnArticulChanged();
                this.OnPropertyChanged("Articul");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Articul;
        partial void OnArticulChanging(string value);
        partial void OnArticulChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SaleDocument_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long SaleDocument_Id
        {
            get
            {
                return this._SaleDocument_Id;
            }
            set
            {
                this.OnSaleDocument_IdChanging(value);
                this._SaleDocument_Id = value;
                this.OnSaleDocument_IdChanged();
                this.OnPropertyChanged("SaleDocument_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _SaleDocument_Id;
        partial void OnSaleDocument_IdChanging(long value);
        partial void OnSaleDocument_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public PriceItem PriceItem
        {
            get
            {
                return this._PriceItem;
            }
            set
            {
                this._PriceItem = value;
                this.OnPropertyChanged("PriceItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private PriceItem _PriceItem;
        /// <summary>
        /// В схеме отсутствуют комментарии для SaleDocument.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public SaleDocument SaleDocument
        {
            get
            {
                return this._SaleDocument;
            }
            set
            {
                this._SaleDocument = value;
                this.OnPropertyChanged("SaleDocument");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private SaleDocument _SaleDocument;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.SaleDocument.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("SaleDocuments")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class SaleDocument : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект SaleDocument.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="saleDate">Начальное значение SaleDate.</param>
        /// <param name="customer_Name">Начальное значение Customer_Name.</param>
        /// <param name="number">Начальное значение Number.</param>
        /// <param name="creator_Id">Начальное значение Creator_Id.</param>
        /// <param name="lastChanger_Id">Начальное значение LastChanger_Id.</param>
        /// <param name="isReceiptPrinted">Начальное значение IsReceiptPrinted.</param>
        /// <param name="isOrder">Начальное значение IsOrder.</param>
        /// <param name="isInDebt">Начальное значение IsInDebt.</param>
        /// <param name="isInvoice">Начальное значение IsInvoice.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static SaleDocument CreateSaleDocument(long ID, global::System.DateTime saleDate, string customer_Name, string number, string creator_Id, string lastChanger_Id, bool isReceiptPrinted, bool isOrder, bool isInDebt, bool isInvoice)
        {
            SaleDocument saleDocument = new SaleDocument();
            saleDocument.Id = ID;
            saleDocument.SaleDate = saleDate;
            saleDocument.Customer_Name = customer_Name;
            saleDocument.Number = number;
            saleDocument.Creator_Id = creator_Id;
            saleDocument.LastChanger_Id = lastChanger_Id;
            saleDocument.IsReceiptPrinted = isReceiptPrinted;
            saleDocument.IsOrder = isOrder;
            saleDocument.IsInDebt = isInDebt;
            saleDocument.IsInvoice = isInvoice;
            return saleDocument;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SaleDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime SaleDate
        {
            get
            {
                return this._SaleDate;
            }
            set
            {
                this.OnSaleDateChanging(value);
                this._SaleDate = value;
                this.OnSaleDateChanged();
                this.OnPropertyChanged("SaleDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _SaleDate;
        partial void OnSaleDateChanging(global::System.DateTime value);
        partial void OnSaleDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Customer_Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Customer_Name
        {
            get
            {
                return this._Customer_Name;
            }
            set
            {
                this.OnCustomer_NameChanging(value);
                this._Customer_Name = value;
                this.OnCustomer_NameChanged();
                this.OnPropertyChanged("Customer_Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Customer_Name;
        partial void OnCustomer_NameChanging(string value);
        partial void OnCustomer_NameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Number.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                this.OnNumberChanging(value);
                this._Number = value;
                this.OnNumberChanged();
                this.OnPropertyChanged("Number");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Number;
        partial void OnNumberChanging(string value);
        partial void OnNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Creator_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Creator_Id
        {
            get
            {
                return this._Creator_Id;
            }
            set
            {
                this.OnCreator_IdChanging(value);
                this._Creator_Id = value;
                this.OnCreator_IdChanged();
                this.OnPropertyChanged("Creator_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Creator_Id;
        partial void OnCreator_IdChanging(string value);
        partial void OnCreator_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства LastChanger_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string LastChanger_Id
        {
            get
            {
                return this._LastChanger_Id;
            }
            set
            {
                this.OnLastChanger_IdChanging(value);
                this._LastChanger_Id = value;
                this.OnLastChanger_IdChanged();
                this.OnPropertyChanged("LastChanger_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _LastChanger_Id;
        partial void OnLastChanger_IdChanging(string value);
        partial void OnLastChanger_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsReceiptPrinted.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsReceiptPrinted
        {
            get
            {
                return this._IsReceiptPrinted;
            }
            set
            {
                this.OnIsReceiptPrintedChanging(value);
                this._IsReceiptPrinted = value;
                this.OnIsReceiptPrintedChanged();
                this.OnPropertyChanged("IsReceiptPrinted");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsReceiptPrinted;
        partial void OnIsReceiptPrintedChanging(bool value);
        partial void OnIsReceiptPrintedChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsOrder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsOrder
        {
            get
            {
                return this._IsOrder;
            }
            set
            {
                this.OnIsOrderChanging(value);
                this._IsOrder = value;
                this.OnIsOrderChanged();
                this.OnPropertyChanged("IsOrder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsOrder;
        partial void OnIsOrderChanging(bool value);
        partial void OnIsOrderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsInDebt.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsInDebt
        {
            get
            {
                return this._IsInDebt;
            }
            set
            {
                this.OnIsInDebtChanging(value);
                this._IsInDebt = value;
                this.OnIsInDebtChanged();
                this.OnPropertyChanged("IsInDebt");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsInDebt;
        partial void OnIsInDebtChanging(bool value);
        partial void OnIsInDebtChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsInvoice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsInvoice
        {
            get
            {
                return this._IsInvoice;
            }
            set
            {
                this.OnIsInvoiceChanging(value);
                this._IsInvoice = value;
                this.OnIsInvoiceChanged();
                this.OnPropertyChanged("IsInvoice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsInvoice;
        partial void OnIsInvoiceChanging(bool value);
        partial void OnIsInvoiceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Barcode.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Barcode
        {
            get
            {
                return this._Barcode;
            }
            set
            {
                this.OnBarcodeChanging(value);
                this._Barcode = value;
                this.OnBarcodeChanged();
                this.OnPropertyChanged("Barcode");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Barcode;
        partial void OnBarcodeChanging(string value);
        partial void OnBarcodeChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Creator.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
                this.OnPropertyChanged("Creator");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Creator;
        /// <summary>
        /// В схеме отсутствуют комментарии для Customer.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Customer Customer
        {
            get
            {
                return this._Customer;
            }
            set
            {
                this._Customer = value;
                this.OnPropertyChanged("Customer");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Customer _Customer;
        /// <summary>
        /// В схеме отсутствуют комментарии для LastChanger.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User LastChanger
        {
            get
            {
                return this._LastChanger;
            }
            set
            {
                this._LastChanger = value;
                this.OnPropertyChanged("LastChanger");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _LastChanger;
        /// <summary>
        /// В схеме отсутствуют комментарии для SaleItems.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<SaleItem> SaleItems
        {
            get
            {
                return this._SaleItems;
            }
            set
            {
                this._SaleItems = value;
                this.OnPropertyChanged("SaleItems");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<SaleItem> _SaleItems = new global::System.Data.Services.Client.DataServiceCollection<SaleItem>(null, global::System.Data.Services.Client.TrackingMode.None);
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.RefundsPerDayItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("RefundsPerDayItems")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class RefundsPerDayItem : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект RefundsPerDayItem.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="refundItem_Id">Начальное значение RefundItem_Id.</param>
        /// <param name="saleDocumentsPerDay_Id">Начальное значение SaleDocumentsPerDay_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static RefundsPerDayItem CreateRefundsPerDayItem(long ID, long refundItem_Id, long saleDocumentsPerDay_Id)
        {
            RefundsPerDayItem refundsPerDayItem = new RefundsPerDayItem();
            refundsPerDayItem.Id = ID;
            refundsPerDayItem.RefundItem_Id = refundItem_Id;
            refundsPerDayItem.SaleDocumentsPerDay_Id = saleDocumentsPerDay_Id;
            return refundsPerDayItem;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RefundItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long RefundItem_Id
        {
            get
            {
                return this._RefundItem_Id;
            }
            set
            {
                this.OnRefundItem_IdChanging(value);
                this._RefundItem_Id = value;
                this.OnRefundItem_IdChanged();
                this.OnPropertyChanged("RefundItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _RefundItem_Id;
        partial void OnRefundItem_IdChanging(long value);
        partial void OnRefundItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SaleDocumentsPerDay_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long SaleDocumentsPerDay_Id
        {
            get
            {
                return this._SaleDocumentsPerDay_Id;
            }
            set
            {
                this.OnSaleDocumentsPerDay_IdChanging(value);
                this._SaleDocumentsPerDay_Id = value;
                this.OnSaleDocumentsPerDay_IdChanged();
                this.OnPropertyChanged("SaleDocumentsPerDay_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _SaleDocumentsPerDay_Id;
        partial void OnSaleDocumentsPerDay_IdChanging(long value);
        partial void OnSaleDocumentsPerDay_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для RefundItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public RefundItem RefundItem
        {
            get
            {
                return this._RefundItem;
            }
            set
            {
                this._RefundItem = value;
                this.OnPropertyChanged("RefundItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private RefundItem _RefundItem;
        /// <summary>
        /// В схеме отсутствуют комментарии для SaleDocumentsPerDay.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public SaleDocumentsPerDay SaleDocumentsPerDay
        {
            get
            {
                return this._SaleDocumentsPerDay;
            }
            set
            {
                this._SaleDocumentsPerDay = value;
                this.OnPropertyChanged("SaleDocumentsPerDay");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private SaleDocumentsPerDay _SaleDocumentsPerDay;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.SaleDocumentsPerDay.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("SaleDocumentsPerDays")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class SaleDocumentsPerDay : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект SaleDocumentsPerDay.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="saleDocumentsDate">Начальное значение SaleDocumentsDate.</param>
        /// <param name="number">Начальное значение Number.</param>
        /// <param name="totalAmount">Начальное значение TotalAmount.</param>
        /// <param name="totalRefund">Начальное значение TotalRefund.</param>
        /// <param name="subTotal">Начальное значение SubTotal.</param>
        /// <param name="totalAmountProfit">Начальное значение TotalAmountProfit.</param>
        /// <param name="totalRefundProfit">Начальное значение TotalRefundProfit.</param>
        /// <param name="subTotalProfit">Начальное значение SubTotalProfit.</param>
        /// <param name="creator_Id">Начальное значение Creator_Id.</param>
        /// <param name="isClosed">Начальное значение IsClosed.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static SaleDocumentsPerDay CreateSaleDocumentsPerDay(long ID, global::System.DateTime saleDocumentsDate, string number, decimal totalAmount, decimal totalRefund, decimal subTotal, decimal totalAmountProfit, decimal totalRefundProfit, decimal subTotalProfit, string creator_Id, bool isClosed)
        {
            SaleDocumentsPerDay saleDocumentsPerDay = new SaleDocumentsPerDay();
            saleDocumentsPerDay.Id = ID;
            saleDocumentsPerDay.SaleDocumentsDate = saleDocumentsDate;
            saleDocumentsPerDay.Number = number;
            saleDocumentsPerDay.TotalAmount = totalAmount;
            saleDocumentsPerDay.TotalRefund = totalRefund;
            saleDocumentsPerDay.SubTotal = subTotal;
            saleDocumentsPerDay.TotalAmountProfit = totalAmountProfit;
            saleDocumentsPerDay.TotalRefundProfit = totalRefundProfit;
            saleDocumentsPerDay.SubTotalProfit = subTotalProfit;
            saleDocumentsPerDay.Creator_Id = creator_Id;
            saleDocumentsPerDay.IsClosed = isClosed;
            return saleDocumentsPerDay;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SaleDocumentsDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.DateTime SaleDocumentsDate
        {
            get
            {
                return this._SaleDocumentsDate;
            }
            set
            {
                this.OnSaleDocumentsDateChanging(value);
                this._SaleDocumentsDate = value;
                this.OnSaleDocumentsDateChanged();
                this.OnPropertyChanged("SaleDocumentsDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _SaleDocumentsDate;
        partial void OnSaleDocumentsDateChanging(global::System.DateTime value);
        partial void OnSaleDocumentsDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Barcode.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Barcode
        {
            get
            {
                return this._Barcode;
            }
            set
            {
                this.OnBarcodeChanging(value);
                this._Barcode = value;
                this.OnBarcodeChanged();
                this.OnPropertyChanged("Barcode");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Barcode;
        partial void OnBarcodeChanging(string value);
        partial void OnBarcodeChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Number.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                this.OnNumberChanging(value);
                this._Number = value;
                this.OnNumberChanged();
                this.OnPropertyChanged("Number");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Number;
        partial void OnNumberChanging(string value);
        partial void OnNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства TotalAmount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal TotalAmount
        {
            get
            {
                return this._TotalAmount;
            }
            set
            {
                this.OnTotalAmountChanging(value);
                this._TotalAmount = value;
                this.OnTotalAmountChanged();
                this.OnPropertyChanged("TotalAmount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _TotalAmount;
        partial void OnTotalAmountChanging(decimal value);
        partial void OnTotalAmountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства TotalRefund.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal TotalRefund
        {
            get
            {
                return this._TotalRefund;
            }
            set
            {
                this.OnTotalRefundChanging(value);
                this._TotalRefund = value;
                this.OnTotalRefundChanged();
                this.OnPropertyChanged("TotalRefund");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _TotalRefund;
        partial void OnTotalRefundChanging(decimal value);
        partial void OnTotalRefundChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SubTotal.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal SubTotal
        {
            get
            {
                return this._SubTotal;
            }
            set
            {
                this.OnSubTotalChanging(value);
                this._SubTotal = value;
                this.OnSubTotalChanged();
                this.OnPropertyChanged("SubTotal");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _SubTotal;
        partial void OnSubTotalChanging(decimal value);
        partial void OnSubTotalChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства TotalAmountProfit.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal TotalAmountProfit
        {
            get
            {
                return this._TotalAmountProfit;
            }
            set
            {
                this.OnTotalAmountProfitChanging(value);
                this._TotalAmountProfit = value;
                this.OnTotalAmountProfitChanged();
                this.OnPropertyChanged("TotalAmountProfit");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _TotalAmountProfit;
        partial void OnTotalAmountProfitChanging(decimal value);
        partial void OnTotalAmountProfitChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства TotalRefundProfit.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal TotalRefundProfit
        {
            get
            {
                return this._TotalRefundProfit;
            }
            set
            {
                this.OnTotalRefundProfitChanging(value);
                this._TotalRefundProfit = value;
                this.OnTotalRefundProfitChanged();
                this.OnPropertyChanged("TotalRefundProfit");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _TotalRefundProfit;
        partial void OnTotalRefundProfitChanging(decimal value);
        partial void OnTotalRefundProfitChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SubTotalProfit.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal SubTotalProfit
        {
            get
            {
                return this._SubTotalProfit;
            }
            set
            {
                this.OnSubTotalProfitChanging(value);
                this._SubTotalProfit = value;
                this.OnSubTotalProfitChanged();
                this.OnPropertyChanged("SubTotalProfit");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _SubTotalProfit;
        partial void OnSubTotalProfitChanging(decimal value);
        partial void OnSubTotalProfitChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Creator_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Creator_Id
        {
            get
            {
                return this._Creator_Id;
            }
            set
            {
                this.OnCreator_IdChanging(value);
                this._Creator_Id = value;
                this.OnCreator_IdChanged();
                this.OnPropertyChanged("Creator_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Creator_Id;
        partial void OnCreator_IdChanging(string value);
        partial void OnCreator_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsClosed.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public bool IsClosed
        {
            get
            {
                return this._IsClosed;
            }
            set
            {
                this.OnIsClosedChanging(value);
                this._IsClosed = value;
                this.OnIsClosedChanged();
                this.OnPropertyChanged("IsClosed");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsClosed;
        partial void OnIsClosedChanging(bool value);
        partial void OnIsClosedChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Creator.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public User Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
                this.OnPropertyChanged("Creator");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Creator;
        /// <summary>
        /// В схеме отсутствуют комментарии для SalesPerDayItems.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public global::System.Data.Services.Client.DataServiceCollection<SalesPerDayItem> SalesPerDayItems
        {
            get
            {
                return this._SalesPerDayItems;
            }
            set
            {
                this._SalesPerDayItems = value;
                this.OnPropertyChanged("SalesPerDayItems");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<SalesPerDayItem> _SalesPerDayItems = new global::System.Data.Services.Client.DataServiceCollection<SalesPerDayItem>(null, global::System.Data.Services.Client.TrackingMode.None);
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.SalesPerDayItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("SalesPerDayItems")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    [DataContract]
    public partial class SalesPerDayItem : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект SalesPerDayItem.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="name">Начальное значение Name.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="price">Начальное значение Price.</param>
        /// <param name="discount">Начальное значение Discount.</param>
        /// <param name="count">Начальное значение Count.</param>
        /// <param name="amount">Начальное значение Amount.</param>
        /// <param name="remainders">Начальное значение Remainders.</param>
        /// <param name="saleItem_Id">Начальное значение SaleItem_Id.</param>
        /// <param name="saleDocumentsPerDay_Id">Начальное значение SaleDocumentsPerDay_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static SalesPerDayItem CreateSalesPerDayItem(long ID, string name, bool isDuplicate, decimal price, decimal discount, decimal count, decimal amount, decimal remainders, long saleItem_Id, long saleDocumentsPerDay_Id)
        {
            SalesPerDayItem salesPerDayItem = new SalesPerDayItem();
            salesPerDayItem.Id = ID;
            salesPerDayItem.Name = name;
            salesPerDayItem.IsDuplicate = isDuplicate;
            salesPerDayItem.Price = price;
            salesPerDayItem.Discount = discount;
            salesPerDayItem.Count = count;
            salesPerDayItem.Amount = amount;
            salesPerDayItem.Remainders = remainders;
            salesPerDayItem.SaleItem_Id = saleItem_Id;
            salesPerDayItem.SaleDocumentsPerDay_Id = saleDocumentsPerDay_Id;
            return salesPerDayItem;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства UnitOfMeasure.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public string UnitOfMeasure
        {
            get
            {
                return this._UnitOfMeasure;
            }
            set
            {
                this.OnUnitOfMeasureChanging(value);
                this._UnitOfMeasure = value;
                this.OnUnitOfMeasureChanged();
                this.OnPropertyChanged("UnitOfMeasure");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _UnitOfMeasure;
        partial void OnUnitOfMeasureChanging(string value);
        partial void OnUnitOfMeasureChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Price.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Price
        {
            get
            {
                return this._Price;
            }
            set
            {
                this.OnPriceChanging(value);
                this._Price = value;
                this.OnPriceChanged();
                this.OnPropertyChanged("Price");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Price;
        partial void OnPriceChanging(decimal value);
        partial void OnPriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Discount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Discount
        {
            get
            {
                return this._Discount;
            }
            set
            {
                this.OnDiscountChanging(value);
                this._Discount = value;
                this.OnDiscountChanged();
                this.OnPropertyChanged("Discount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Discount;
        partial void OnDiscountChanging(decimal value);
        partial void OnDiscountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Count.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Count
        {
            get
            {
                return this._Count;
            }
            set
            {
                this.OnCountChanging(value);
                this._Count = value;
                this.OnCountChanged();
                this.OnPropertyChanged("Count");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Count;
        partial void OnCountChanging(decimal value);
        partial void OnCountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Amount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Amount
        {
            get
            {
                return this._Amount;
            }
            set
            {
                this.OnAmountChanging(value);
                this._Amount = value;
                this.OnAmountChanged();
                this.OnPropertyChanged("Amount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Amount;
        partial void OnAmountChanging(decimal value);
        partial void OnAmountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainders.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public decimal Remainders
        {
            get
            {
                return this._Remainders;
            }
            set
            {
                this.OnRemaindersChanging(value);
                this._Remainders = value;
                this.OnRemaindersChanged();
                this.OnPropertyChanged("Remainders");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Remainders;
        partial void OnRemaindersChanging(decimal value);
        partial void OnRemaindersChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SaleItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long SaleItem_Id
        {
            get
            {
                return this._SaleItem_Id;
            }
            set
            {
                this.OnSaleItem_IdChanging(value);
                this._SaleItem_Id = value;
                this.OnSaleItem_IdChanged();
                this.OnPropertyChanged("SaleItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _SaleItem_Id;
        partial void OnSaleItem_IdChanging(long value);
        partial void OnSaleItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства SaleDocumentsPerDay_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public long SaleDocumentsPerDay_Id
        {
            get
            {
                return this._SaleDocumentsPerDay_Id;
            }
            set
            {
                this.OnSaleDocumentsPerDay_IdChanging(value);
                this._SaleDocumentsPerDay_Id = value;
                this.OnSaleDocumentsPerDay_IdChanged();
                this.OnPropertyChanged("SaleDocumentsPerDay_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _SaleDocumentsPerDay_Id;
        partial void OnSaleDocumentsPerDay_IdChanging(long value);
        partial void OnSaleDocumentsPerDay_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для SaleDocumentsPerDay.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public SaleDocumentsPerDay SaleDocumentsPerDay
        {
            get
            {
                return this._SaleDocumentsPerDay;
            }
            set
            {
                this._SaleDocumentsPerDay = value;
                this.OnPropertyChanged("SaleDocumentsPerDay");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private SaleDocumentsPerDay _SaleDocumentsPerDay;
        /// <summary>
        /// В схеме отсутствуют комментарии для SaleItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        [DataMember]
        public SaleItem SaleItem
        {
            get
            {
                return this._SaleItem;
            }
            set
            {
                this._SaleItem = value;
                this.OnPropertyChanged("SaleItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private SaleItem _SaleItem;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.WarehouseTransferRequestItem.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("WarehouseTransferRequestItems")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class WarehouseTransferRequestItem : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект WarehouseTransferRequestItem.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="count">Начальное значение Count.</param>
        /// <param name="countAccepted">Начальное значение CountAccepted.</param>
        /// <param name="wholesalePrice">Начальное значение WholesalePrice.</param>
        /// <param name="acceptedAmount">Начальное значение AcceptedAmount.</param>
        /// <param name="warehouseTransferRequest_Id">Начальное значение WarehouseTransferRequest_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static WarehouseTransferRequestItem CreateWarehouseTransferRequestItem(long ID, long priceItem_Id, decimal count, decimal countAccepted, decimal wholesalePrice, decimal acceptedAmount, long warehouseTransferRequest_Id)
        {
            WarehouseTransferRequestItem warehouseTransferRequestItem = new WarehouseTransferRequestItem();
            warehouseTransferRequestItem.Id = ID;
            warehouseTransferRequestItem.PriceItem_Id = priceItem_Id;
            warehouseTransferRequestItem.Count = count;
            warehouseTransferRequestItem.CountAccepted = countAccepted;
            warehouseTransferRequestItem.WholesalePrice = wholesalePrice;
            warehouseTransferRequestItem.AcceptedAmount = acceptedAmount;
            warehouseTransferRequestItem.WarehouseTransferRequest_Id = warehouseTransferRequest_Id;
            return warehouseTransferRequestItem;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Count.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Count
        {
            get
            {
                return this._Count;
            }
            set
            {
                this.OnCountChanging(value);
                this._Count = value;
                this.OnCountChanged();
                this.OnPropertyChanged("Count");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Count;
        partial void OnCountChanging(decimal value);
        partial void OnCountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CountAccepted.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal CountAccepted
        {
            get
            {
                return this._CountAccepted;
            }
            set
            {
                this.OnCountAcceptedChanging(value);
                this._CountAccepted = value;
                this.OnCountAcceptedChanged();
                this.OnPropertyChanged("CountAccepted");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _CountAccepted;
        partial void OnCountAcceptedChanging(decimal value);
        partial void OnCountAcceptedChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WholesalePrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal WholesalePrice
        {
            get
            {
                return this._WholesalePrice;
            }
            set
            {
                this.OnWholesalePriceChanging(value);
                this._WholesalePrice = value;
                this.OnWholesalePriceChanged();
                this.OnPropertyChanged("WholesalePrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _WholesalePrice;
        partial void OnWholesalePriceChanging(decimal value);
        partial void OnWholesalePriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства AcceptedAmount.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal AcceptedAmount
        {
            get
            {
                return this._AcceptedAmount;
            }
            set
            {
                this.OnAcceptedAmountChanging(value);
                this._AcceptedAmount = value;
                this.OnAcceptedAmountChanged();
                this.OnPropertyChanged("AcceptedAmount");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _AcceptedAmount;
        partial void OnAcceptedAmountChanging(decimal value);
        partial void OnAcceptedAmountChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WarehouseTransferRequest_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long WarehouseTransferRequest_Id
        {
            get
            {
                return this._WarehouseTransferRequest_Id;
            }
            set
            {
                this.OnWarehouseTransferRequest_IdChanging(value);
                this._WarehouseTransferRequest_Id = value;
                this.OnWarehouseTransferRequest_IdChanged();
                this.OnPropertyChanged("WarehouseTransferRequest_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _WarehouseTransferRequest_Id;
        partial void OnWarehouseTransferRequest_IdChanging(long value);
        partial void OnWarehouseTransferRequest_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для PriceItem.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public PriceItem PriceItem
        {
            get
            {
                return this._PriceItem;
            }
            set
            {
                this._PriceItem = value;
                this.OnPropertyChanged("PriceItem");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private PriceItem _PriceItem;
        /// <summary>
        /// В схеме отсутствуют комментарии для WarehouseTransferRequest.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public WarehouseTransferRequest WarehouseTransferRequest
        {
            get
            {
                return this._WarehouseTransferRequest;
            }
            set
            {
                this._WarehouseTransferRequest = value;
                this.OnPropertyChanged("WarehouseTransferRequest");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private WarehouseTransferRequest _WarehouseTransferRequest;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.WarehouseTransferRequest.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("WarehouseTransferRequests")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    [Serializable]
    public partial class WarehouseTransferRequest : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект WarehouseTransferRequest.
        /// </summary>
        /// <param name="ID">Начальное значение Id.</param>
        /// <param name="requestNumber">Начальное значение RequestNumber.</param>
        /// <param name="requestDate">Начальное значение RequestDate.</param>
        /// <param name="isAccept">Начальное значение IsAccept.</param>
        /// <param name="isReserve">Начальное значение IsReserve.</param>
        /// <param name="customer_Id">Начальное значение Customer_Id.</param>
        /// <param name="supplier_Id">Начальное значение Supplier_Id.</param>
        /// <param name="creator_Id">Начальное значение Creator_Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static WarehouseTransferRequest CreateWarehouseTransferRequest(long ID, string requestNumber, global::System.DateTime requestDate, bool isAccept, bool isReserve, string customer_Id, string supplier_Id, string creator_Id)
        {
            WarehouseTransferRequest warehouseTransferRequest = new WarehouseTransferRequest();
            warehouseTransferRequest.Id = ID;
            warehouseTransferRequest.RequestNumber = requestNumber;
            warehouseTransferRequest.RequestDate = requestDate;
            warehouseTransferRequest.IsAccept = isAccept;
            warehouseTransferRequest.IsReserve = isReserve;
            warehouseTransferRequest.Customer_Id = customer_Id;
            warehouseTransferRequest.Supplier_Id = supplier_Id;
            warehouseTransferRequest.Creator_Id = creator_Id;
            return warehouseTransferRequest;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RequestNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string RequestNumber
        {
            get
            {
                return this._RequestNumber;
            }
            set
            {
                this.OnRequestNumberChanging(value);
                this._RequestNumber = value;
                this.OnRequestNumberChanged();
                this.OnPropertyChanged("RequestNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _RequestNumber;
        partial void OnRequestNumberChanging(string value);
        partial void OnRequestNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RequestDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime RequestDate
        {
            get
            {
                return this._RequestDate;
            }
            set
            {
                this.OnRequestDateChanging(value);
                this._RequestDate = value;
                this.OnRequestDateChanged();
                this.OnPropertyChanged("RequestDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _RequestDate;
        partial void OnRequestDateChanging(global::System.DateTime value);
        partial void OnRequestDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsAccept.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsAccept
        {
            get
            {
                return this._IsAccept;
            }
            set
            {
                this.OnIsAcceptChanging(value);
                this._IsAccept = value;
                this.OnIsAcceptChanged();
                this.OnPropertyChanged("IsAccept");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsAccept;
        partial void OnIsAcceptChanging(bool value);
        partial void OnIsAcceptChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsReserve.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsReserve
        {
            get
            {
                return this._IsReserve;
            }
            set
            {
                this.OnIsReserveChanging(value);
                this._IsReserve = value;
                this.OnIsReserveChanged();
                this.OnPropertyChanged("IsReserve");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsReserve;
        partial void OnIsReserveChanging(bool value);
        partial void OnIsReserveChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства StateChangedDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Nullable<global::System.DateTime> StateChangedDate
        {
            get
            {
                return this._StateChangedDate;
            }
            set
            {
                this.OnStateChangedDateChanging(value);
                this._StateChangedDate = value;
                this.OnStateChangedDateChanged();
                this.OnPropertyChanged("StateChangedDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Nullable<global::System.DateTime> _StateChangedDate;
        partial void OnStateChangedDateChanging(global::System.Nullable<global::System.DateTime> value);
        partial void OnStateChangedDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Status.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this.OnStatusChanging(value);
                this._Status = value;
                this.OnStatusChanged();
                this.OnPropertyChanged("Status");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Status;
        partial void OnStatusChanging(string value);
        partial void OnStatusChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Customer_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Customer_Id
        {
            get
            {
                return this._Customer_Id;
            }
            set
            {
                this.OnCustomer_IdChanging(value);
                this._Customer_Id = value;
                this.OnCustomer_IdChanged();
                this.OnPropertyChanged("Customer_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Customer_Id;
        partial void OnCustomer_IdChanging(string value);
        partial void OnCustomer_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Supplier_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Supplier_Id
        {
            get
            {
                return this._Supplier_Id;
            }
            set
            {
                this.OnSupplier_IdChanging(value);
                this._Supplier_Id = value;
                this.OnSupplier_IdChanged();
                this.OnPropertyChanged("Supplier_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Supplier_Id;
        partial void OnSupplier_IdChanging(string value);
        partial void OnSupplier_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Creator_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Creator_Id
        {
            get
            {
                return this._Creator_Id;
            }
            set
            {
                this.OnCreator_IdChanging(value);
                this._Creator_Id = value;
                this.OnCreator_IdChanged();
                this.OnPropertyChanged("Creator_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Creator_Id;
        partial void OnCreator_IdChanging(string value);
        partial void OnCreator_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства LastChanged_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string LastChanged_Id
        {
            get
            {
                return this._LastChanged_Id;
            }
            set
            {
                this.OnLastChanged_IdChanging(value);
                this._LastChanged_Id = value;
                this.OnLastChanged_IdChanged();
                this.OnPropertyChanged("LastChanged_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _LastChanged_Id;
        partial void OnLastChanged_IdChanging(string value);
        partial void OnLastChanged_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Description.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this.OnDescriptionChanging(value);
                this._Description = value;
                this.OnDescriptionChanged();
                this.OnPropertyChanged("Description");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Description;
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для Creator.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
                this.OnPropertyChanged("Creator");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _Creator;
        /// <summary>
        /// В схеме отсутствуют комментарии для Customer.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Warehouse Customer
        {
            get
            {
                return this._Customer;
            }
            set
            {
                this._Customer = value;
                this.OnPropertyChanged("Customer");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Warehouse _Customer;
        /// <summary>
        /// В схеме отсутствуют комментарии для LastChanged.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public User LastChanged
        {
            get
            {
                return this._LastChanged;
            }
            set
            {
                this._LastChanged = value;
                this.OnPropertyChanged("LastChanged");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private User _LastChanged;
        /// <summary>
        /// В схеме отсутствуют комментарии для Supplier.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Warehouse Supplier
        {
            get
            {
                return this._Supplier;
            }
            set
            {
                this._Supplier = value;
                this.OnPropertyChanged("Supplier");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private Warehouse _Supplier;
        /// <summary>
        /// В схеме отсутствуют комментарии для WarehouseTransferRequestItemItems.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<WarehouseTransferRequestItem> WarehouseTransferRequestItemItems
        {
            get
            {
                return this._WarehouseTransferRequestItemItems;
            }
            set
            {
                this._WarehouseTransferRequestItemItems = value;
                this.OnPropertyChanged("WarehouseTransferRequestItemItems");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<WarehouseTransferRequestItem> _WarehouseTransferRequestItemItems = new global::System.Data.Services.Client.DataServiceCollection<WarehouseTransferRequestItem>(null, global::System.Data.Services.Client.TrackingMode.None);
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceItemRemainderView.
    /// </summary>
    /// <KeyProperties>
    /// PriceItem_Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceItemRemainderViews")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("PriceItem_Id")]
    [Serializable]
    public partial class PriceItemRemainderView : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceItemRemainderView.
        /// </summary>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="buyPriceRur">Начальное значение BuyPriceRur.</param>
        /// <param name="buyPriceTng">Начальное значение BuyPriceTng.</param>
        /// <param name="wholesalePrice">Начальное значение WholesalePrice.</param>
        /// <param name="gear_Id">Начальное значение Gear_Id.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="recommendedRemainder">Начальное значение RecommendedRemainder.</param>
        /// <param name="lowerLimitRemainder">Начальное значение LowerLimitRemainder.</param>
        /// <param name="remainder_Id">Начальное значение Remainder_Id.</param>
        /// <param name="remainderDate">Начальное значение RemainderDate.</param>
        /// <param name="remainders">Начальное значение Remainders.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceItemRemainderView CreatePriceItemRemainderView(long priceItem_Id, decimal buyPriceRur, decimal buyPriceTng, decimal wholesalePrice, long gear_Id, bool isDuplicate, decimal recommendedRemainder, decimal lowerLimitRemainder, long remainder_Id, global::System.DateTime remainderDate, decimal remainders)
        {
            PriceItemRemainderView priceItemRemainderView = new PriceItemRemainderView();
            priceItemRemainderView.PriceItem_Id = priceItem_Id;
            priceItemRemainderView.BuyPriceRur = buyPriceRur;
            priceItemRemainderView.BuyPriceTng = buyPriceTng;
            priceItemRemainderView.WholesalePrice = wholesalePrice;
            priceItemRemainderView.Gear_Id = gear_Id;
            priceItemRemainderView.IsDuplicate = isDuplicate;
            priceItemRemainderView.RecommendedRemainder = recommendedRemainder;
            priceItemRemainderView.LowerLimitRemainder = lowerLimitRemainder;
            priceItemRemainderView.Remainder_Id = remainder_Id;
            priceItemRemainderView.RemainderDate = remainderDate;
            priceItemRemainderView.Remainders = remainders;
            return priceItemRemainderView;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceRur.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceRur
        {
            get
            {
                return this._BuyPriceRur;
            }
            set
            {
                this.OnBuyPriceRurChanging(value);
                this._BuyPriceRur = value;
                this.OnBuyPriceRurChanged();
                this.OnPropertyChanged("BuyPriceRur");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceRur;
        partial void OnBuyPriceRurChanging(decimal value);
        partial void OnBuyPriceRurChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceTng.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceTng
        {
            get
            {
                return this._BuyPriceTng;
            }
            set
            {
                this.OnBuyPriceTngChanging(value);
                this._BuyPriceTng = value;
                this.OnBuyPriceTngChanged();
                this.OnPropertyChanged("BuyPriceTng");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceTng;
        partial void OnBuyPriceTngChanging(decimal value);
        partial void OnBuyPriceTngChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WholesalePrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal WholesalePrice
        {
            get
            {
                return this._WholesalePrice;
            }
            set
            {
                this.OnWholesalePriceChanging(value);
                this._WholesalePrice = value;
                this.OnWholesalePriceChanged();
                this.OnPropertyChanged("WholesalePrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _WholesalePrice;
        partial void OnWholesalePriceChanging(decimal value);
        partial void OnWholesalePriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Supplier.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Supplier
        {
            get
            {
                return this._Supplier;
            }
            set
            {
                this.OnSupplierChanging(value);
                this._Supplier = value;
                this.OnSupplierChanged();
                this.OnPropertyChanged("Supplier");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Supplier;
        partial void OnSupplierChanging(string value);
        partial void OnSupplierChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Uom.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Uom
        {
            get
            {
                return this._Uom;
            }
            set
            {
                this.OnUomChanging(value);
                this._Uom = value;
                this.OnUomChanged();
                this.OnPropertyChanged("Uom");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Uom;
        partial void OnUomChanging(string value);
        partial void OnUomChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Gear_Id
        {
            get
            {
                return this._Gear_Id;
            }
            set
            {
                this.OnGear_IdChanging(value);
                this._Gear_Id = value;
                this.OnGear_IdChanged();
                this.OnPropertyChanged("Gear_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Gear_Id;
        partial void OnGear_IdChanging(long value);
        partial void OnGear_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Articul.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Articul
        {
            get
            {
                return this._Articul;
            }
            set
            {
                this.OnArticulChanging(value);
                this._Articul = value;
                this.OnArticulChanged();
                this.OnPropertyChanged("Articul");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Articul;
        partial void OnArticulChanging(string value);
        partial void OnArticulChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Gear_Name
        {
            get
            {
                return this._Gear_Name;
            }
            set
            {
                this.OnGear_NameChanging(value);
                this._Gear_Name = value;
                this.OnGear_NameChanged();
                this.OnPropertyChanged("Gear_Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Gear_Name;
        partial void OnGear_NameChanging(string value);
        partial void OnGear_NameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RecommendedRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal RecommendedRemainder
        {
            get
            {
                return this._RecommendedRemainder;
            }
            set
            {
                this.OnRecommendedRemainderChanging(value);
                this._RecommendedRemainder = value;
                this.OnRecommendedRemainderChanged();
                this.OnPropertyChanged("RecommendedRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _RecommendedRemainder;
        partial void OnRecommendedRemainderChanging(decimal value);
        partial void OnRecommendedRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства LowerLimitRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal LowerLimitRemainder
        {
            get
            {
                return this._LowerLimitRemainder;
            }
            set
            {
                this.OnLowerLimitRemainderChanging(value);
                this._LowerLimitRemainder = value;
                this.OnLowerLimitRemainderChanged();
                this.OnPropertyChanged("LowerLimitRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _LowerLimitRemainder;
        partial void OnLowerLimitRemainderChanging(decimal value);
        partial void OnLowerLimitRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainder_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Remainder_Id
        {
            get
            {
                return this._Remainder_Id;
            }
            set
            {
                this.OnRemainder_IdChanging(value);
                this._Remainder_Id = value;
                this.OnRemainder_IdChanged();
                this.OnPropertyChanged("Remainder_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Remainder_Id;
        partial void OnRemainder_IdChanging(long value);
        partial void OnRemainder_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Warehouse.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Warehouse
        {
            get
            {
                return this._Warehouse;
            }
            set
            {
                this.OnWarehouseChanging(value);
                this._Warehouse = value;
                this.OnWarehouseChanged();
                this.OnPropertyChanged("Warehouse");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Warehouse;
        partial void OnWarehouseChanging(string value);
        partial void OnWarehouseChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RemainderDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime RemainderDate
        {
            get
            {
                return this._RemainderDate;
            }
            set
            {
                this.OnRemainderDateChanging(value);
                this._RemainderDate = value;
                this.OnRemainderDateChanged();
                this.OnPropertyChanged("RemainderDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _RemainderDate;
        partial void OnRemainderDateChanging(global::System.DateTime value);
        partial void OnRemainderDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainders.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Remainders
        {
            get
            {
                return this._Remainders;
            }
            set
            {
                this.OnRemaindersChanging(value);
                this._Remainders = value;
                this.OnRemaindersChanged();
                this.OnPropertyChanged("Remainders");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Remainders;
        partial void OnRemaindersChanging(decimal value);
        partial void OnRemaindersChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceIncomeItemView.
    /// </summary>
    /// <KeyProperties>
    /// PriceItem_Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceIncomeItemViews")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("PriceItem_Id")]
    [Serializable]
    public partial class PriceIncomeItemView : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceIncomeItemView.
        /// </summary>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="buyPriceRur">Начальное значение BuyPriceRur.</param>
        /// <param name="buyPriceTng">Начальное значение BuyPriceTng.</param>
        /// <param name="wholesalePrice">Начальное значение WholesalePrice.</param>
        /// <param name="gear_Id">Начальное значение Gear_Id.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="recommendedRemainder">Начальное значение RecommendedRemainder.</param>
        /// <param name="lowerLimitRemainder">Начальное значение LowerLimitRemainder.</param>
        /// <param name="remainders">Начальное значение Remainders.</param>
        /// <param name="incomes">Начальное значение Incomes.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceIncomeItemView CreatePriceIncomeItemView(long priceItem_Id, decimal buyPriceRur, decimal buyPriceTng, decimal wholesalePrice, long gear_Id, bool isDuplicate, decimal recommendedRemainder, decimal lowerLimitRemainder, decimal remainders, decimal incomes)
        {
            PriceIncomeItemView priceIncomeItemView = new PriceIncomeItemView();
            priceIncomeItemView.PriceItem_Id = priceItem_Id;
            priceIncomeItemView.BuyPriceRur = buyPriceRur;
            priceIncomeItemView.BuyPriceTng = buyPriceTng;
            priceIncomeItemView.WholesalePrice = wholesalePrice;
            priceIncomeItemView.Gear_Id = gear_Id;
            priceIncomeItemView.IsDuplicate = isDuplicate;
            priceIncomeItemView.RecommendedRemainder = recommendedRemainder;
            priceIncomeItemView.LowerLimitRemainder = lowerLimitRemainder;
            priceIncomeItemView.Remainders = remainders;
            priceIncomeItemView.Incomes = incomes;
            return priceIncomeItemView;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceRur.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceRur
        {
            get
            {
                return this._BuyPriceRur;
            }
            set
            {
                this.OnBuyPriceRurChanging(value);
                this._BuyPriceRur = value;
                this.OnBuyPriceRurChanged();
                this.OnPropertyChanged("BuyPriceRur");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceRur;
        partial void OnBuyPriceRurChanging(decimal value);
        partial void OnBuyPriceRurChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceTng.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceTng
        {
            get
            {
                return this._BuyPriceTng;
            }
            set
            {
                this.OnBuyPriceTngChanging(value);
                this._BuyPriceTng = value;
                this.OnBuyPriceTngChanged();
                this.OnPropertyChanged("BuyPriceTng");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceTng;
        partial void OnBuyPriceTngChanging(decimal value);
        partial void OnBuyPriceTngChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WholesalePrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal WholesalePrice
        {
            get
            {
                return this._WholesalePrice;
            }
            set
            {
                this.OnWholesalePriceChanging(value);
                this._WholesalePrice = value;
                this.OnWholesalePriceChanged();
                this.OnPropertyChanged("WholesalePrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _WholesalePrice;
        partial void OnWholesalePriceChanging(decimal value);
        partial void OnWholesalePriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Uom.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Uom
        {
            get
            {
                return this._Uom;
            }
            set
            {
                this.OnUomChanging(value);
                this._Uom = value;
                this.OnUomChanged();
                this.OnPropertyChanged("Uom");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Uom;
        partial void OnUomChanging(string value);
        partial void OnUomChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Gear_Id
        {
            get
            {
                return this._Gear_Id;
            }
            set
            {
                this.OnGear_IdChanging(value);
                this._Gear_Id = value;
                this.OnGear_IdChanged();
                this.OnPropertyChanged("Gear_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Gear_Id;
        partial void OnGear_IdChanging(long value);
        partial void OnGear_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Articul.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Articul
        {
            get
            {
                return this._Articul;
            }
            set
            {
                this.OnArticulChanging(value);
                this._Articul = value;
                this.OnArticulChanged();
                this.OnPropertyChanged("Articul");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Articul;
        partial void OnArticulChanging(string value);
        partial void OnArticulChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Gear_Name
        {
            get
            {
                return this._Gear_Name;
            }
            set
            {
                this.OnGear_NameChanging(value);
                this._Gear_Name = value;
                this.OnGear_NameChanged();
                this.OnPropertyChanged("Gear_Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Gear_Name;
        partial void OnGear_NameChanging(string value);
        partial void OnGear_NameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RecommendedRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal RecommendedRemainder
        {
            get
            {
                return this._RecommendedRemainder;
            }
            set
            {
                this.OnRecommendedRemainderChanging(value);
                this._RecommendedRemainder = value;
                this.OnRecommendedRemainderChanged();
                this.OnPropertyChanged("RecommendedRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _RecommendedRemainder;
        partial void OnRecommendedRemainderChanging(decimal value);
        partial void OnRecommendedRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства LowerLimitRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal LowerLimitRemainder
        {
            get
            {
                return this._LowerLimitRemainder;
            }
            set
            {
                this.OnLowerLimitRemainderChanging(value);
                this._LowerLimitRemainder = value;
                this.OnLowerLimitRemainderChanged();
                this.OnPropertyChanged("LowerLimitRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _LowerLimitRemainder;
        partial void OnLowerLimitRemainderChanging(decimal value);
        partial void OnLowerLimitRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainders.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Remainders
        {
            get
            {
                return this._Remainders;
            }
            set
            {
                this.OnRemaindersChanging(value);
                this._Remainders = value;
                this.OnRemaindersChanged();
                this.OnPropertyChanged("Remainders");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Remainders;
        partial void OnRemaindersChanging(decimal value);
        partial void OnRemaindersChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Incomes.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Incomes
        {
            get
            {
                return this._Incomes;
            }
            set
            {
                this.OnIncomesChanging(value);
                this._Incomes = value;
                this.OnIncomesChanged();
                this.OnPropertyChanged("Incomes");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Incomes;
        partial void OnIncomesChanging(decimal value);
        partial void OnIncomesChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceIncomeTotalItemView.
    /// </summary>
    /// <KeyProperties>
    /// PriceItem_Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceIncomeTotalItemViews")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("PriceItem_Id")]
    [Serializable]
    public partial class PriceIncomeTotalItemView : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceIncomeTotalItemView.
        /// </summary>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="buyPriceRur">Начальное значение BuyPriceRur.</param>
        /// <param name="buyPriceTng">Начальное значение BuyPriceTng.</param>
        /// <param name="wholesalePrice">Начальное значение WholesalePrice.</param>
        /// <param name="gear_Id">Начальное значение Gear_Id.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="recommendedRemainder">Начальное значение RecommendedRemainder.</param>
        /// <param name="lowerLimitRemainder">Начальное значение LowerLimitRemainder.</param>
        /// <param name="remainders">Начальное значение Remainders.</param>
        /// <param name="incomes">Начальное значение Incomes.</param>
        /// <param name="income_Id">Начальное значение Income_Id.</param>
        /// <param name="isAccept">Начальное значение IsAccept.</param>
        /// <param name="newPrice">Начальное значение NewPrice.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceIncomeTotalItemView CreatePriceIncomeTotalItemView(long priceItem_Id, decimal buyPriceRur, decimal buyPriceTng, decimal wholesalePrice, long gear_Id, bool isDuplicate, decimal recommendedRemainder, decimal lowerLimitRemainder, decimal remainders, decimal incomes, long income_Id, bool isAccept, decimal newPrice)
        {
            PriceIncomeTotalItemView priceIncomeTotalItemView = new PriceIncomeTotalItemView();
            priceIncomeTotalItemView.PriceItem_Id = priceItem_Id;
            priceIncomeTotalItemView.BuyPriceRur = buyPriceRur;
            priceIncomeTotalItemView.BuyPriceTng = buyPriceTng;
            priceIncomeTotalItemView.WholesalePrice = wholesalePrice;
            priceIncomeTotalItemView.Gear_Id = gear_Id;
            priceIncomeTotalItemView.IsDuplicate = isDuplicate;
            priceIncomeTotalItemView.RecommendedRemainder = recommendedRemainder;
            priceIncomeTotalItemView.LowerLimitRemainder = lowerLimitRemainder;
            priceIncomeTotalItemView.Remainders = remainders;
            priceIncomeTotalItemView.Incomes = incomes;
            priceIncomeTotalItemView.Income_Id = income_Id;
            priceIncomeTotalItemView.IsAccept = isAccept;
            priceIncomeTotalItemView.NewPrice = newPrice;
            return priceIncomeTotalItemView;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceRur.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceRur
        {
            get
            {
                return this._BuyPriceRur;
            }
            set
            {
                this.OnBuyPriceRurChanging(value);
                this._BuyPriceRur = value;
                this.OnBuyPriceRurChanged();
                this.OnPropertyChanged("BuyPriceRur");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceRur;
        partial void OnBuyPriceRurChanging(decimal value);
        partial void OnBuyPriceRurChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceTng.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceTng
        {
            get
            {
                return this._BuyPriceTng;
            }
            set
            {
                this.OnBuyPriceTngChanging(value);
                this._BuyPriceTng = value;
                this.OnBuyPriceTngChanged();
                this.OnPropertyChanged("BuyPriceTng");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceTng;
        partial void OnBuyPriceTngChanging(decimal value);
        partial void OnBuyPriceTngChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WholesalePrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal WholesalePrice
        {
            get
            {
                return this._WholesalePrice;
            }
            set
            {
                this.OnWholesalePriceChanging(value);
                this._WholesalePrice = value;
                this.OnWholesalePriceChanged();
                this.OnPropertyChanged("WholesalePrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _WholesalePrice;
        partial void OnWholesalePriceChanging(decimal value);
        partial void OnWholesalePriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Uom.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Uom
        {
            get
            {
                return this._Uom;
            }
            set
            {
                this.OnUomChanging(value);
                this._Uom = value;
                this.OnUomChanged();
                this.OnPropertyChanged("Uom");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Uom;
        partial void OnUomChanging(string value);
        partial void OnUomChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Gear_Id
        {
            get
            {
                return this._Gear_Id;
            }
            set
            {
                this.OnGear_IdChanging(value);
                this._Gear_Id = value;
                this.OnGear_IdChanged();
                this.OnPropertyChanged("Gear_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Gear_Id;
        partial void OnGear_IdChanging(long value);
        partial void OnGear_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Articul.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Articul
        {
            get
            {
                return this._Articul;
            }
            set
            {
                this.OnArticulChanging(value);
                this._Articul = value;
                this.OnArticulChanged();
                this.OnPropertyChanged("Articul");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Articul;
        partial void OnArticulChanging(string value);
        partial void OnArticulChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Gear_Name
        {
            get
            {
                return this._Gear_Name;
            }
            set
            {
                this.OnGear_NameChanging(value);
                this._Gear_Name = value;
                this.OnGear_NameChanged();
                this.OnPropertyChanged("Gear_Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Gear_Name;
        partial void OnGear_NameChanging(string value);
        partial void OnGear_NameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RecommendedRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal RecommendedRemainder
        {
            get
            {
                return this._RecommendedRemainder;
            }
            set
            {
                this.OnRecommendedRemainderChanging(value);
                this._RecommendedRemainder = value;
                this.OnRecommendedRemainderChanged();
                this.OnPropertyChanged("RecommendedRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _RecommendedRemainder;
        partial void OnRecommendedRemainderChanging(decimal value);
        partial void OnRecommendedRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства LowerLimitRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal LowerLimitRemainder
        {
            get
            {
                return this._LowerLimitRemainder;
            }
            set
            {
                this.OnLowerLimitRemainderChanging(value);
                this._LowerLimitRemainder = value;
                this.OnLowerLimitRemainderChanged();
                this.OnPropertyChanged("LowerLimitRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _LowerLimitRemainder;
        partial void OnLowerLimitRemainderChanging(decimal value);
        partial void OnLowerLimitRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainders.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Remainders
        {
            get
            {
                return this._Remainders;
            }
            set
            {
                this.OnRemaindersChanging(value);
                this._Remainders = value;
                this.OnRemaindersChanged();
                this.OnPropertyChanged("Remainders");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Remainders;
        partial void OnRemaindersChanging(decimal value);
        partial void OnRemaindersChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Incomes.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Incomes
        {
            get
            {
                return this._Incomes;
            }
            set
            {
                this.OnIncomesChanging(value);
                this._Incomes = value;
                this.OnIncomesChanged();
                this.OnPropertyChanged("Incomes");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Incomes;
        partial void OnIncomesChanging(decimal value);
        partial void OnIncomesChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Income_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Income_Id
        {
            get
            {
                return this._Income_Id;
            }
            set
            {
                this.OnIncome_IdChanging(value);
                this._Income_Id = value;
                this.OnIncome_IdChanged();
                this.OnPropertyChanged("Income_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Income_Id;
        partial void OnIncome_IdChanging(long value);
        partial void OnIncome_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsAccept.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsAccept
        {
            get
            {
                return this._IsAccept;
            }
            set
            {
                this.OnIsAcceptChanging(value);
                this._IsAccept = value;
                this.OnIsAcceptChanged();
                this.OnPropertyChanged("IsAccept");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsAccept;
        partial void OnIsAcceptChanging(bool value);
        partial void OnIsAcceptChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства NewPrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal NewPrice
        {
            get
            {
                return this._NewPrice;
            }
            set
            {
                this.OnNewPriceChanging(value);
                this._NewPrice = value;
                this.OnNewPriceChanged();
                this.OnPropertyChanged("NewPrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _NewPrice;
        partial void OnNewPriceChanging(decimal value);
        partial void OnNewPriceChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceItemView.
    /// </summary>
    /// <KeyProperties>
    /// PriceItem_Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceItemViews")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("PriceItem_Id")]
    [Serializable]
    public partial class PriceItemView : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceItemView.
        /// </summary>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="buyPriceRur">Начальное значение BuyPriceRur.</param>
        /// <param name="buyPriceTng">Начальное значение BuyPriceTng.</param>
        /// <param name="gear_Id">Начальное значение Gear_Id.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="wholesalePrice">Начальное значение WholesalePrice.</param>
        /// <param name="wholesalePriceDate">Начальное значение WholesalePriceDate.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceItemView CreatePriceItemView(long priceItem_Id, decimal buyPriceRur, decimal buyPriceTng, long gear_Id, bool isDuplicate, decimal wholesalePrice, global::System.DateTime wholesalePriceDate)
        {
            PriceItemView priceItemView = new PriceItemView();
            priceItemView.PriceItem_Id = priceItem_Id;
            priceItemView.BuyPriceRur = buyPriceRur;
            priceItemView.BuyPriceTng = buyPriceTng;
            priceItemView.Gear_Id = gear_Id;
            priceItemView.IsDuplicate = isDuplicate;
            priceItemView.WholesalePrice = wholesalePrice;
            priceItemView.WholesalePriceDate = wholesalePriceDate;
            return priceItemView;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceRur.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceRur
        {
            get
            {
                return this._BuyPriceRur;
            }
            set
            {
                this.OnBuyPriceRurChanging(value);
                this._BuyPriceRur = value;
                this.OnBuyPriceRurChanged();
                this.OnPropertyChanged("BuyPriceRur");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceRur;
        partial void OnBuyPriceRurChanging(decimal value);
        partial void OnBuyPriceRurChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceTng.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceTng
        {
            get
            {
                return this._BuyPriceTng;
            }
            set
            {
                this.OnBuyPriceTngChanging(value);
                this._BuyPriceTng = value;
                this.OnBuyPriceTngChanged();
                this.OnPropertyChanged("BuyPriceTng");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceTng;
        partial void OnBuyPriceTngChanging(decimal value);
        partial void OnBuyPriceTngChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Uom.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Uom
        {
            get
            {
                return this._Uom;
            }
            set
            {
                this.OnUomChanging(value);
                this._Uom = value;
                this.OnUomChanged();
                this.OnPropertyChanged("Uom");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Uom;
        partial void OnUomChanging(string value);
        partial void OnUomChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Gear_Id
        {
            get
            {
                return this._Gear_Id;
            }
            set
            {
                this.OnGear_IdChanging(value);
                this._Gear_Id = value;
                this.OnGear_IdChanged();
                this.OnPropertyChanged("Gear_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Gear_Id;
        partial void OnGear_IdChanging(long value);
        partial void OnGear_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Articul.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Articul
        {
            get
            {
                return this._Articul;
            }
            set
            {
                this.OnArticulChanging(value);
                this._Articul = value;
                this.OnArticulChanged();
                this.OnPropertyChanged("Articul");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Articul;
        partial void OnArticulChanging(string value);
        partial void OnArticulChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Gear_Name
        {
            get
            {
                return this._Gear_Name;
            }
            set
            {
                this.OnGear_NameChanging(value);
                this._Gear_Name = value;
                this.OnGear_NameChanged();
                this.OnPropertyChanged("Gear_Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Gear_Name;
        partial void OnGear_NameChanging(string value);
        partial void OnGear_NameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WholesalePrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal WholesalePrice
        {
            get
            {
                return this._WholesalePrice;
            }
            set
            {
                this.OnWholesalePriceChanging(value);
                this._WholesalePrice = value;
                this.OnWholesalePriceChanged();
                this.OnPropertyChanged("WholesalePrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _WholesalePrice;
        partial void OnWholesalePriceChanging(decimal value);
        partial void OnWholesalePriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WholesalePriceDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime WholesalePriceDate
        {
            get
            {
                return this._WholesalePriceDate;
            }
            set
            {
                this.OnWholesalePriceDateChanging(value);
                this._WholesalePriceDate = value;
                this.OnWholesalePriceDateChanged();
                this.OnPropertyChanged("WholesalePriceDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _WholesalePriceDate;
        partial void OnWholesalePriceDateChanging(global::System.DateTime value);
        partial void OnWholesalePriceDateChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// В схеме отсутствуют комментарии для StoreAppTest.Web.DataModel.PriceListItemRemainderView.
    /// </summary>
    /// <KeyProperties>
    /// PriceItem_Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("PriceListItemRemainderViews")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("PriceItem_Id")]
    [Serializable]
    public partial class PriceListItemRemainderView : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Создайте новый объект PriceListItemRemainderView.
        /// </summary>
        /// <param name="priceItem_Id">Начальное значение PriceItem_Id.</param>
        /// <param name="buyPriceRur">Начальное значение BuyPriceRur.</param>
        /// <param name="buyPriceTng">Начальное значение BuyPriceTng.</param>
        /// <param name="wholesalePrice">Начальное значение WholesalePrice.</param>
        /// <param name="gear_Id">Начальное значение Gear_Id.</param>
        /// <param name="isDuplicate">Начальное значение IsDuplicate.</param>
        /// <param name="recommendedRemainder">Начальное значение RecommendedRemainder.</param>
        /// <param name="lowerLimitRemainder">Начальное значение LowerLimitRemainder.</param>
        /// <param name="remainder_Id">Начальное значение Remainder_Id.</param>
        /// <param name="remainderDate">Начальное значение RemainderDate.</param>
        /// <param name="remainders">Начальное значение Remainders.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PriceListItemRemainderView CreatePriceListItemRemainderView(long priceItem_Id, decimal buyPriceRur, decimal buyPriceTng, decimal wholesalePrice, long gear_Id, bool isDuplicate, decimal recommendedRemainder, decimal lowerLimitRemainder, long remainder_Id, global::System.DateTime remainderDate, decimal remainders)
        {
            PriceListItemRemainderView priceListItemRemainderView = new PriceListItemRemainderView();
            priceListItemRemainderView.PriceItem_Id = priceItem_Id;
            priceListItemRemainderView.BuyPriceRur = buyPriceRur;
            priceListItemRemainderView.BuyPriceTng = buyPriceTng;
            priceListItemRemainderView.WholesalePrice = wholesalePrice;
            priceListItemRemainderView.Gear_Id = gear_Id;
            priceListItemRemainderView.IsDuplicate = isDuplicate;
            priceListItemRemainderView.RecommendedRemainder = recommendedRemainder;
            priceListItemRemainderView.LowerLimitRemainder = lowerLimitRemainder;
            priceListItemRemainderView.Remainder_Id = remainder_Id;
            priceListItemRemainderView.RemainderDate = remainderDate;
            priceListItemRemainderView.Remainders = remainders;
            return priceListItemRemainderView;
        }
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceItem_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long PriceItem_Id
        {
            get
            {
                return this._PriceItem_Id;
            }
            set
            {
                this.OnPriceItem_IdChanging(value);
                this._PriceItem_Id = value;
                this.OnPriceItem_IdChanged();
                this.OnPropertyChanged("PriceItem_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _PriceItem_Id;
        partial void OnPriceItem_IdChanging(long value);
        partial void OnPriceItem_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceRur.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceRur
        {
            get
            {
                return this._BuyPriceRur;
            }
            set
            {
                this.OnBuyPriceRurChanging(value);
                this._BuyPriceRur = value;
                this.OnBuyPriceRurChanged();
                this.OnPropertyChanged("BuyPriceRur");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceRur;
        partial void OnBuyPriceRurChanging(decimal value);
        partial void OnBuyPriceRurChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства BuyPriceTng.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal BuyPriceTng
        {
            get
            {
                return this._BuyPriceTng;
            }
            set
            {
                this.OnBuyPriceTngChanging(value);
                this._BuyPriceTng = value;
                this.OnBuyPriceTngChanged();
                this.OnPropertyChanged("BuyPriceTng");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _BuyPriceTng;
        partial void OnBuyPriceTngChanging(decimal value);
        partial void OnBuyPriceTngChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства WholesalePrice.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal WholesalePrice
        {
            get
            {
                return this._WholesalePrice;
            }
            set
            {
                this.OnWholesalePriceChanging(value);
                this._WholesalePrice = value;
                this.OnWholesalePriceChanged();
                this.OnPropertyChanged("WholesalePrice");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _WholesalePrice;
        partial void OnWholesalePriceChanging(decimal value);
        partial void OnWholesalePriceChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства PriceList_Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string PriceList_Name
        {
            get
            {
                return this._PriceList_Name;
            }
            set
            {
                this.OnPriceList_NameChanging(value);
                this._PriceList_Name = value;
                this.OnPriceList_NameChanged();
                this.OnPropertyChanged("PriceList_Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _PriceList_Name;
        partial void OnPriceList_NameChanging(string value);
        partial void OnPriceList_NameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Supplier.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Supplier
        {
            get
            {
                return this._Supplier;
            }
            set
            {
                this.OnSupplierChanging(value);
                this._Supplier = value;
                this.OnSupplierChanged();
                this.OnPropertyChanged("Supplier");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Supplier;
        partial void OnSupplierChanging(string value);
        partial void OnSupplierChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Uom.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Uom
        {
            get
            {
                return this._Uom;
            }
            set
            {
                this.OnUomChanging(value);
                this._Uom = value;
                this.OnUomChanged();
                this.OnPropertyChanged("Uom");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Uom;
        partial void OnUomChanging(string value);
        partial void OnUomChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Gear_Id
        {
            get
            {
                return this._Gear_Id;
            }
            set
            {
                this.OnGear_IdChanging(value);
                this._Gear_Id = value;
                this.OnGear_IdChanged();
                this.OnPropertyChanged("Gear_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Gear_Id;
        partial void OnGear_IdChanging(long value);
        partial void OnGear_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства CatalogNumber.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string CatalogNumber
        {
            get
            {
                return this._CatalogNumber;
            }
            set
            {
                this.OnCatalogNumberChanging(value);
                this._CatalogNumber = value;
                this.OnCatalogNumberChanged();
                this.OnPropertyChanged("CatalogNumber");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _CatalogNumber;
        partial void OnCatalogNumberChanging(string value);
        partial void OnCatalogNumberChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Articul.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Articul
        {
            get
            {
                return this._Articul;
            }
            set
            {
                this.OnArticulChanging(value);
                this._Articul = value;
                this.OnArticulChanged();
                this.OnPropertyChanged("Articul");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Articul;
        partial void OnArticulChanging(string value);
        partial void OnArticulChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства IsDuplicate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public bool IsDuplicate
        {
            get
            {
                return this._IsDuplicate;
            }
            set
            {
                this.OnIsDuplicateChanging(value);
                this._IsDuplicate = value;
                this.OnIsDuplicateChanged();
                this.OnPropertyChanged("IsDuplicate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private bool _IsDuplicate;
        partial void OnIsDuplicateChanging(bool value);
        partial void OnIsDuplicateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Gear_Name.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Gear_Name
        {
            get
            {
                return this._Gear_Name;
            }
            set
            {
                this.OnGear_NameChanging(value);
                this._Gear_Name = value;
                this.OnGear_NameChanged();
                this.OnPropertyChanged("Gear_Name");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Gear_Name;
        partial void OnGear_NameChanging(string value);
        partial void OnGear_NameChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RecommendedRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal RecommendedRemainder
        {
            get
            {
                return this._RecommendedRemainder;
            }
            set
            {
                this.OnRecommendedRemainderChanging(value);
                this._RecommendedRemainder = value;
                this.OnRecommendedRemainderChanged();
                this.OnPropertyChanged("RecommendedRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _RecommendedRemainder;
        partial void OnRecommendedRemainderChanging(decimal value);
        partial void OnRecommendedRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства LowerLimitRemainder.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal LowerLimitRemainder
        {
            get
            {
                return this._LowerLimitRemainder;
            }
            set
            {
                this.OnLowerLimitRemainderChanging(value);
                this._LowerLimitRemainder = value;
                this.OnLowerLimitRemainderChanged();
                this.OnPropertyChanged("LowerLimitRemainder");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _LowerLimitRemainder;
        partial void OnLowerLimitRemainderChanging(decimal value);
        partial void OnLowerLimitRemainderChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainder_Id.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public long Remainder_Id
        {
            get
            {
                return this._Remainder_Id;
            }
            set
            {
                this.OnRemainder_IdChanging(value);
                this._Remainder_Id = value;
                this.OnRemainder_IdChanged();
                this.OnPropertyChanged("Remainder_Id");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private long _Remainder_Id;
        partial void OnRemainder_IdChanging(long value);
        partial void OnRemainder_IdChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Warehouse.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Warehouse
        {
            get
            {
                return this._Warehouse;
            }
            set
            {
                this.OnWarehouseChanging(value);
                this._Warehouse = value;
                this.OnWarehouseChanged();
                this.OnPropertyChanged("Warehouse");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Warehouse;
        partial void OnWarehouseChanging(string value);
        partial void OnWarehouseChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства RemainderDate.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime RemainderDate
        {
            get
            {
                return this._RemainderDate;
            }
            set
            {
                this.OnRemainderDateChanging(value);
                this._RemainderDate = value;
                this.OnRemainderDateChanged();
                this.OnPropertyChanged("RemainderDate");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _RemainderDate;
        partial void OnRemainderDateChanging(global::System.DateTime value);
        partial void OnRemainderDateChanged();
        /// <summary>
        /// В схеме отсутствуют комментарии для свойства Remainders.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public decimal Remainders
        {
            get
            {
                return this._Remainders;
            }
            set
            {
                this.OnRemaindersChanging(value);
                this._Remainders = value;
                this.OnRemaindersChanged();
                this.OnPropertyChanged("Remainders");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private decimal _Remainders;
        partial void OnRemaindersChanging(decimal value);
        partial void OnRemaindersChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
}
