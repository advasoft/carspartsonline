
namespace StoreAppTest.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using DevExpress.Data.Access;
    using DevExpress.Data.Browsing;
    using DevExpress.Xpf.Collections;
    using StoreAppDataService;
    using Utilities;
    using PropertyDescriptor = DevExpress.Data.Browsing.PropertyDescriptor;

    public class RemaindersItem : CustomTypeHelper<RemaindersItem>
        //PropertyDescriptor, ICustomTypeProvider //CustomTypeHelper<RemaindersItem>
    {
        //private string propertyName;
        //private Type propertyType;
        //private bool isReadOnly = false;


        //public RemaindersItem(string name, Attribute[] attributes) : base(name, attributes)
        //{
        //}

        //public RemaindersItem(PropertyDescriptor descr) : base(descr)
        //{
        //}

        private StoreAppDataService.PriceItem _priceItemData;

        public RemaindersItem(StoreAppDataService.PriceItem priceItemData)
        {
            _priceItemData = priceItemData;
        }


        public int Number { get; set; }

        private string _Articul;

        public string Articul
        {
            get { return _Articul; }
            set
            {
                _Articul = value;
                NotifyPropertyChanged("Articul");
            }
        }


        private string _CatalogNumber;
        public string CatalogNumber
        {
            get { return _CatalogNumber; }
            set
            {
                _CatalogNumber = value;
                NotifyPropertyChanged("CatalogNumber");
            }
        }


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }


        private string _IsDuplicate;
        public string IsDuplicate
        {
            get { return _IsDuplicate; }
            set
            {
                _IsDuplicate = value;
                NotifyPropertyChanged("IsDuplicate");
            }
        }


        private string _Uom;
        public string Uom
        {
            get { return _Uom; }
            set
            {
                _Uom = value;
                NotifyPropertyChanged("Uom");
            }
        }
        

        public string Supplier
        {
            get
            {
                return _Supplier;
            }
            set
            {
                _Supplier = value;
                NotifyPropertyChanged("Supplier");
            }
        }
        private string _Supplier;


        private int _WholesalePrice;
        public int WholesalePrice
        {
            get { return _WholesalePrice; }
            set
            {
                _WholesalePrice = value;
                NotifyPropertyChanged("WholesalePrice");
            }
        }


        private int _BuyPriceRur;
        public int BuyPriceRur
        {
            get { return _BuyPriceRur; }
            set
            {
                _BuyPriceRur = value;
                NotifyPropertyChanged("BuyPriceRur");
            }
        }


        private int _BuyPriceTng;
        public int BuyPriceTng
        {
            get { return _BuyPriceTng; }
            set
            {
                _BuyPriceTng = value;
                NotifyPropertyChanged("WholesalePrice");
            }
        }


        private bool _Selected;
        [Display(Order = 0, Name = "Выделен")]
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                NotifyPropertyChanged("Selected");
            }
        }


        public long Gear_Id { get; set; }
        public long PriceItem_Id { get; set; }

        public StoreAppDataService.PriceItem GetPriceItemData()
        {
            return _priceItemData;
        }

        //CustomTypeHelper<RemaindersItem> helper = new CustomTypeHelper<RemaindersItem>();



        //public static void AddProperty(String name)
        //{
        //    CustomTypeHelper<RemaindersItem>.AddProperty(name);
        //}

        //public static void AddProperty(String name, Type propertyType)
        //{
        //    CustomTypeHelper<RemaindersItem>.AddProperty(name, propertyType);
        //}

        //public static void AddProperty(String name, Type propertyType, List<Attribute> attributes)
        //{
        //    CustomTypeHelper<RemaindersItem>.AddProperty(name, propertyType, attributes);
        //}


        //public void SetPropertyValue(string propertyName, object value)
        //{
        //    helper.SetPropertyValue(propertyName, value);
        //}

        //public object GetPropertyValue(string propertyName)
        //{
        //    return helper.GetPropertyValue(propertyName);
        //}

        //public PropertyInfo[] GetProperties()
        //{
        //    return helper.GetProperties();
        //}

        //public Type GetCustomType()
        //{
        //    return helper.GetCustomType();
        //}


        //    #region Overrides of PropertyDescriptor

        //    public RemaindersItem(string propertyName, Type propertyType)
        //        : base(propertyName, null) {
        //        this.propertyName = propertyName;
        //        this.propertyType = propertyType;
        //    }


        //    public override bool CanResetValue(object component) {
        //        return false;
        //    }
        //    public override object GetValue(object component)
        //    {
        //        return null;
        //    }
        //    public override void SetValue(object component, object val) 
        //    {

        //    }
        //    public override bool IsReadOnly { get { return isReadOnly; } }
        //    public override string Name { get { return propertyName; } }
        //    public override Type ComponentType { get { return typeof(RemaindersItem); } }
        //    public override Type PropertyType { get { return propertyType; } }
        //    public override void ResetValue(object component) {
        //    }
        //    public override bool ShouldSerializeValue(object component) { return true; }
        //    #endregion
        //}
    }

    public class RemaindersCollection : ObservableCollection<RemaindersItem>, ITypedList
    {
        #region Implementation of ITypedList

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            ArrayList descriptors = new ArrayList();
            descriptors.Add(new RemaindersPropertyDescriptor("Number", true));
            //descriptors.Add(new RemaindersPropertyDescriptor("Articul", true));
            descriptors.Add(new RemaindersPropertyDescriptor("CatalogNumber", true));
            descriptors.Add(new RemaindersPropertyDescriptor("Name", true));
            descriptors.Add(new RemaindersPropertyDescriptor("IsDuplicate", true));
            descriptors.Add(new RemaindersPropertyDescriptor("Uom", true));
            descriptors.Add(new RemaindersPropertyDescriptor("Supplier", true));
            descriptors.Add(new RemaindersPropertyDescriptor("WholesalePrice", true));
            descriptors.Add(new RemaindersPropertyDescriptor("BuyPriceRur", true));
            descriptors.Add(new RemaindersPropertyDescriptor("BuyPriceTng", true));
            descriptors.Add(new RemaindersPropertyDescriptor("Selected", true));
            

            RemaindersItem remaindersItem = new RemaindersItem(null);
            var props = remaindersItem.GetProperties();
            foreach (var propertyInfo in props)
            {
                descriptors.Add(new RemaindersPropertyDescriptor(propertyInfo.Name, false));
            }        

            DevExpress.Data.Browsing.PropertyDescriptor[] propertyDescriptors = new DevExpress.Data.Browsing.PropertyDescriptor[descriptors.Count];
            descriptors.CopyTo(propertyDescriptors, 0);
            return new PropertyDescriptorCollection(propertyDescriptors); 
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return "";
        }

        #endregion
    }

    public class RemaindersPropertyDescriptor : DevExpress.Data.Browsing.PropertyDescriptor
    {
        private bool _isReadonly = false;
        public RemaindersPropertyDescriptor(string name, Attribute[] attributes) : base(name, attributes)
        {
        }

        public RemaindersPropertyDescriptor(string name, bool isReadonly)
            : base(name, getPropertyDescriptorAttributes(name))
        {
            _isReadonly = isReadonly;
        }

        private static System.Attribute[] getPropertyDescriptorAttributes(string name)
        {
            DevExpress.Data.Browsing.PropertyDescriptor desc = getPropertyDescriptor(name);
            if (desc == null)
                return null;

            System.Attribute[] attributes = new System.Attribute[desc.Attributes.Count];
            desc.Attributes.CopyTo(attributes, 0);

            return attributes;
        }

        private static DevExpress.Data.Browsing.PropertyDescriptor getPropertyDescriptor(string name)
        {
            return TypeDescriptor.GetProperties(typeof(RemaindersItem)).Find(name, false);
        }

        #region Overrides of PropertyDescriptor

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            RemaindersItem remaindersItem = component as RemaindersItem;
            if (remaindersItem != null)
            {
                switch (this.Name)
                {
                    case "Number":
                        return remaindersItem.Number;
                    //case "Articul":
                    //    return remaindersItem.Articul;
                    case "CatalogNumber":
                        return remaindersItem.CatalogNumber;
                    case "Name":
                        return remaindersItem.Name;
                    case "IsDuplicate":
                        return remaindersItem.IsDuplicate;
                    case "Uom":
                        return remaindersItem.Uom;
                    case "WholesalePrice":
                        return remaindersItem.WholesalePrice;
                    case "Supplier":
                        return remaindersItem.Supplier;
                    case "BuyPriceRur":
                        return remaindersItem.BuyPriceRur;
                    case "BuyPriceTng":
                        return remaindersItem.BuyPriceTng;
                    case "Selected":
                        return remaindersItem.Selected;

                    default:
                        var props = remaindersItem.GetProperties();
                        if (props.Where(p => p.Name == this.Name).Any())
                        {
                            return remaindersItem.GetPropertyValue(this.Name);
                        }
                        break;                     
                }
            }
            return null;
        }

        public override void SetValue(object component, object value)
        {
            RemaindersItem remaindersItem = component as RemaindersItem;
            if (!IsReadOnly && remaindersItem != null)
            {

                switch (this.Name)
                {
                    case "Selected":
                        remaindersItem.Selected = (bool)value;
                        break;
                    default:
                        var props = remaindersItem.GetProperties();
                        if (props.Where(p => p.Name == this.Name).Any())
                        {
                            remaindersItem.SetPropertyValue(this.Name, value);
                        }
                        break;
                }
                //switch (this.Name)
                //{
                //    case "Address_City":
                //        person.Address.City = (string)value;
                //        break;
                //}
            }
        }

        public override bool ShouldSerializeValue(object obj)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
            
        }

        public override Type PropertyType
        {
            get
            {
                RemaindersItem remaindersItem = new RemaindersItem(null);

                switch (this.Name)
                {
                    case "Number":
                        return typeof(string);
                    //case "Articul":
                    //    return typeof(string);
                    case "CatalogNumber":
                        return typeof(string);
                    case "Name":
                        return typeof(string);
                    case "IsDuplicate":
                        return typeof(string);
                    case "Uom":
                        return typeof(string);
                    case "WholesalePrice":
                        return typeof(int);
                    case "Supplier":
                        return typeof(string);
                    case "BuyPriceRur":
                        return typeof(int);
                    case "BuyPriceTng":
                        return typeof(int);
                    case "Selected":
                        return typeof (bool);

                    default:
                        var props = remaindersItem.GetProperties();
                        var prop = props.Where(p => p.Name == this.Name).FirstOrDefault();
                        return prop.PropertyType;
                }
                return null;
            }
        }

        public override bool IsReadOnly
        {
            get { return false; } //_isReadonly; }
            }

        public override Type ComponentType
        {
            get { return typeof(RemaindersItem); }
        }

        #endregion
    }


    //public class PropertyDescriptor : System.ComponentModel.PropertyDescriptor {
    //    public PropertyDescriptor(string name)
    //        : base(name, getPropertyDescriptorAttributes(name)) {
    //    }
    //    public override void SetValue(object component, object value) {
    //        Person person = component as Person;
    //        if(!IsReadOnly && person != null) {    
    //            switch (this.Name) {
    //                case "Name":
    //                    person.Name = (string)value;
    //                    break;
    //                case "Address":
    //                    person.Address = (Address)value;
    //                    break;
    //                case "Address_City":
    //                    person.Address.City = (string)value;
    //                    break;
    //            }
    //        }
    //    }

    //    public override object GetValue(object component) {
    //        Person person = component as Person;
    //        if(person != null) {    
    //            switch (this.Name) {
    //                case "Name":
    //                    return person.Name;
    //                case "Address":
    //                    return person.Address;
    //                case "Address_City":
    //                    if(person.Address != null)
    //                        return person.Address.City;
    //                    break;
    //            }
    //        }
    //        return null;
    //    }

    //    public override bool CanResetValue(object component) {
    //        return false;
    //    }
    //    public override void ResetValue(object component) {
    //    }
    //    public override System.Type PropertyType {
    //        get {
    //            switch (this.Name) {
    //                case "Name":
    //                    return typeof(string);
    //                case "Address":
    //                    return typeof(Address);
    //                case "Address_City":
    //                    return typeof(string);
    //            }
    //            return null;
    //        }
    //    }
    //    public override bool IsReadOnly {
    //        get {
    //            return false;
    //        }
    //    }
    //    public override System.Type ComponentType {
    //        get {
    //            return typeof(Person);
    //        }
    //    }
    //    public override bool ShouldSerializeValue(object component) {
    //        return false;
    //    }    
    //    private static System.ComponentModel.PropertyDescriptor getPropertyDescriptor(string name) {
    //        return System.ComponentModel.TypeDescriptor.GetProperties(typeof(Person)).Find(name, false);
    //    }

    //    private static System.Attribute[] getPropertyDescriptorAttributes(string name) {
    //        System.ComponentModel.PropertyDescriptor desc = getPropertyDescriptor(name);
    //        if(desc == null)
    //            return null;

    //        System.Attribute[] attributes = new System.Attribute[desc.Attributes.Count];
    //        desc.Attributes.CopyTo(attributes, 0);

    //        return attributes;
    //    }

    //    public override System.ComponentModel.TypeConverter Converter {
    //        get {
    //            if (this.Name == "Address")
    //                return new AddressTypeConverter();
    //            else
    //                return base.Converter;
    //        }
    //    }
    //}

    //public class RemaindersPropertyDescriptor : System.ComponentModel.PropertyDescriptor
    //{
    //    protected string _name;

    //    public RemaindersPropertyDescriptor(string name)
    //    {
    //        _name = name;
    //    }
    //    public override void SetValue(object component, object value)
    //    {
    //        Person person = component as Person;
    //        if (!IsReadOnly && person != null)
    //        {
    //            switch (this.Name)
    //            {
    //                case "Name":
    //                    person.Name = (string)value;
    //                    break;
    //                case "Address":
    //                    person.Address = (Address)value;
    //                    break;
    //                case "Address_City":
    //                    person.Address.City = (string)value;
    //                    break;
    //            }
    //        }
    //    }

    //    public override object GetValue(object component)
    //    {
    //        Person person = component as Person;
    //        if (person != null)
    //        {
    //            switch (this.Name)
    //            {
    //                case "Name":
    //                    return person.Name;
    //                case "Address":
    //                    return person.Address;
    //                case "Address_City":
    //                    if (person.Address != null)
    //                        return person.Address.City;
    //                    break;
    //            }
    //        }
    //        return null;
    //    }

    //    public override bool CanResetValue(object component)
    //    {
    //        return false;
    //    }
    //    public override void ResetValue(object component)
    //    {
    //    }
    //    public override System.Type PropertyType
    //    {
    //        get
    //        {
    //            switch (this.Name)
    //            {
    //                case "Name":
    //                    return typeof(string);
    //                case "Address":
    //                    return typeof(Address);
    //                case "Address_City":
    //                    return typeof(string);
    //            }
    //            return null;
    //        }
    //    }
    //    public override bool IsReadOnly
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }
    //    public override System.Type ComponentType
    //    {
    //        get
    //        {
    //            return typeof(Person);
    //        }
    //    }
    //    public override bool ShouldSerializeValue(object component)
    //    {
    //        return false;
    //    }
    //    private static System.ComponentModel.PropertyDescriptor getPropertyDescriptor(string name)
    //    {
    //        return System.ComponentModel.TypeDescriptor.GetProperties(typeof(Person)).Find(name, false);
    //    }

    //    private static System.Attribute[] getPropertyDescriptorAttributes(string name)
    //    {
    //        System.ComponentModel.PropertyDescriptor desc = getPropertyDescriptor(name);
    //        if (desc == null)
    //            return null;

    //        System.Attribute[] attributes = new System.Attribute[desc.Attributes.Count];
    //        desc.Attributes.CopyTo(attributes, 0);

    //        return attributes;
    //    }

    //    public override System.ComponentModel.TypeConverter Converter
    //    {
    //        get
    //        {
    //            if (this.Name == "Address")
    //                return new AddressTypeConverter();
    //            else
    //                return base.Converter;
    //        }
    //    }
    //}

}
