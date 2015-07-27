namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public partial class RefundItem
    {
        [Key]
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
}
