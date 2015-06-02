namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class SaleItem
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long PriceItem_Id { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public decimal Count { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        //[ForeignKey("SaleDocument")]
        public long SaleDocument_Id { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }

        [DataMember]
        public virtual SaleDocument SaleDocument { get; set; }
    }
}
