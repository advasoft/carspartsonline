namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class RefundsPerDayItem
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long RefundItem_Id { get; set; }

        [DataMember]
        //[ForeignKey("RefundDocument")]
        public long SaleDocumentsPerDay_Id { get; set; }

        [DataMember]
        public virtual RefundItem RefundItem { get; set; }

        [DataMember]
        public virtual SaleDocumentsPerDay SaleDocumentsPerDay { get; set; }
    }
}
