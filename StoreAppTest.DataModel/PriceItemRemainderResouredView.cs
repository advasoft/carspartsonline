
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public class PriceItemRemainderResouredView
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
        public decimal WholesalePrice { get; set; }

        [DataMember]
        public string Supplier { get; set; }

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
        public decimal RecommendedRemainder { get; set; }
        [DataMember]
        public decimal LowerLimitRemainder { get; set; }
        [DataMember]
        //[Key]
        //[Column(Order = 6)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Remainder_Id { get; set; }
        [DataMember]
        //[Key]
        //[Column(Order = 7)]
        public string Warehouse { get; set; }
        [DataMember]
        public DateTime RemainderDate { get; set; }
        [DataMember]
        public decimal Remainders { get; set; }

        [DataMember]
        public byte[] Resource { get; set; }

    }
}