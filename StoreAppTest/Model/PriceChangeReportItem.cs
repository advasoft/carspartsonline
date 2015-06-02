
namespace StoreAppTest.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DevExpress.Data.Browsing;
    using DevExpress.Xpf.Collections;
    using Utilities;

    public class PriceChangeReportItem : CustomTypeHelper<PriceChangeReportItem>
    {
        [Display(Name = "№", Order = 1)]
        public int Number { get; set; }
        public long PriceItem_Id { get; set; }
        public int BuyPriceRur { get; set; }
        public int BuyPriceTng { get; set; }
        [Display(Name = "Оптовая цена", Order = 7)]
        public int WholesalePrice { get; set; }
        [Display(Name = "Единица изм.", Order = 6)]
        public string Uom { get; set; }
        public long Gear_Id { get; set; }
        [Display(Name = "Каталожный номер", Order = 3)]
        public string CatalogNumber { get; set; }
        [Display(Name = "Артикул", Order = 2)]
        public string Articul { get; set; }
        [Display(Name = "*", Order = 5)]
        public string IsDuplicate { get; set; }
        [Display(Name = "Название", Order = 4)]
        public string Gear_Name { get; set; }
        public int RecommendedRemainder { get; set; }
        public int LowerLimitRemainder { get; set; }
        public int Incomes { get; set; }
        public long Income_Id { get; set; }
        public long NewPrice_Id { get; set; }
        public long PreviewsPrice_Id { get; set; }
        public bool IsAccept { get; set; }
        [Display(Name = "Новая цена", Order = 99)]
        public int NewPrice { get; set; }
        public int PreviewsPrice { get; set; }
        public int Remainders { get; set; }
    }

    public class PriceChangeReportCollection : ObservableCollection<PriceChangeReportItem>, ITypedList
    {
        #region Implementation of ITypedList

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            ArrayList descriptors = new ArrayList();
            descriptors.Add(new PriceChangeReportDescriptor("Number", true));
            descriptors.Add(new PriceChangeReportDescriptor("Articul", true));
            descriptors.Add(new PriceChangeReportDescriptor("CatalogNumber", true));
            descriptors.Add(new PriceChangeReportDescriptor("Gear_Name", true));
            descriptors.Add(new PriceChangeReportDescriptor("IsDuplicate", true));
            descriptors.Add(new PriceChangeReportDescriptor("Uom", true));
            descriptors.Add(new PriceChangeReportDescriptor("WholesalePrice", true));
            descriptors.Add(new PriceChangeReportDescriptor("IsAccept", true));
            descriptors.Add(new PriceChangeReportDescriptor("NewPrice", true));
            descriptors.Add(new PriceChangeReportDescriptor("PreviewsPrice", true));

            PriceChangeReportItem reportItem = new PriceChangeReportItem();
            var props = reportItem.GetProperties();
            foreach (var propertyInfo in props)
            {
                descriptors.Add(new PriceChangeReportDescriptor(propertyInfo.Name, false));
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

    public class PriceChangeReportDescriptor : DevExpress.Data.Browsing.PropertyDescriptor
    {
        private bool _isReadonly = false;
        public PriceChangeReportDescriptor(string name, Attribute[] attributes)
            : base(name, attributes)
        {
        }

        public PriceChangeReportDescriptor(string name, bool isReadonly)
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
            return TypeDescriptor.GetProperties(typeof(PriceChangeReportItem)).Find(name, false);
        }

        #region Overrides of PropertyDescriptor

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            PriceChangeReportItem reportItem = component as PriceChangeReportItem;
            if (reportItem != null)
            {
                switch (this.Name)
                {
                    case "Number":
                        return reportItem.Number;
                    case "PriceItem_Id":
                        return reportItem.PriceItem_Id;
                    case "Articul":
                        return reportItem.Articul;
                    case "CatalogNumber":
                        return reportItem.CatalogNumber;
                    case "Gear_Name":
                        return reportItem.Gear_Name;
                    case "IsDuplicate":
                        return reportItem.IsDuplicate;
                    case "Uom":
                        return reportItem.Uom;
                    case "WholesalePrice":
                        return reportItem.WholesalePrice;
                    case "BuyPriceRur":
                        return reportItem.BuyPriceRur;
                    case "BuyPriceTng":
                        return reportItem.BuyPriceTng;
                    case "Gear_Id":
                        return reportItem.Gear_Id;
                    case "RecommendedRemainder":
                        return reportItem.RecommendedRemainder;
                    case "LowerLimitRemainder":
                        return reportItem.LowerLimitRemainder;
                    case "Incomes":
                        return reportItem.Incomes;
                    case "NewPrice":
                        return reportItem.NewPrice;
                    case "PreviewsPrice":
                        return reportItem.PreviewsPrice;
                    case "Income_Id":
                        return reportItem.Income_Id;
                    case "IsAccept":
                        return reportItem.IsAccept;

                    default:
                        var props = reportItem.GetProperties();
                        if (props.Where(p => p.Name == this.Name).Any())
                        {
                            return reportItem.GetPropertyValue(this.Name);
                        }
                        break;
                }
            }
            return null;
        }

        public override void SetValue(object component, object value)
        {
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
                PriceChangeReportItem reportItem = new PriceChangeReportItem();

                switch (this.Name)
                {
                    case "Number":
                        return typeof(int);
                    case "PriceItem_Id":
                        return typeof(long);
                    case "Articul":
                        return typeof(string);
                    case "CatalogNumber":
                        return typeof(string);
                    case "Gear_Name":
                        return typeof(string);
                    case "IsDuplicate":
                        return typeof(string);
                    case "Uom":
                        return typeof(string);
                    case "WholesalePrice":
                        return typeof(int);
                    case "BuyPriceRur":
                        return typeof(int);
                    case "BuyPriceTng":
                        return typeof(int);
                    case "Gear_Id":
                        return typeof(long);
                    case "RecommendedRemainder":
                        return typeof(int);
                    case "LowerLimitRemainder":
                        return typeof(int);
                    case "Incomes":
                        return typeof(int);
                    case "PreviewsPrice":
                    case "NewPrice":
                        return typeof(int);
                    case "Income_Id":
                        return typeof(long);
                    case "IsAccept":
                        return typeof(bool);


                    default:
                        var props = reportItem.GetProperties();
                        var prop = props.Where(p => p.Name == this.Name).FirstOrDefault();
                        return prop.PropertyType;
                }
                return null;
            }
        }

        public override bool IsReadOnly
        {
            get { return true; } //_isReadonly; }
        }

        public override Type ComponentType
        {
            get { return typeof(PriceChangeReportItem); }
        }

        #endregion
    }

}
