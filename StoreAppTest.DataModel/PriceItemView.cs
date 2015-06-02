
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public class PriceItemView
    {
        [DataMember]
        [Key]
        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PriceItem_Id { get; set; }
        [DataMember]
        public decimal BuyPriceRur { get; set; }
        [DataMember]
        public decimal BuyPriceTng { get; set; }
        [DataMember]
        //[Key]
        //[Column(Order = 1)]
        public string Uom { get; set; }
        [DataMember]
        //[Key]
        //[Column(Order = 2)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Gear_Id { get; set; }
        [DataMember]
        //[Key]
        //[Column(Order = 3)]
        public string CatalogNumber { get; set; }
        [DataMember]
        //[Key]
        //[Column(Order = 4)]
        public string Articul { get; set; }
        [DataMember]
        public bool IsDuplicate { get; set; }
        [DataMember]
        //[Key]
        //[Column(Order = 5)]
        public string Gear_Name { get; set; }
        [DataMember]
        public decimal WholesalePrice { get; set; }
        [DataMember]
        public DateTime WholesalePriceDate { get; set; }

    }
}