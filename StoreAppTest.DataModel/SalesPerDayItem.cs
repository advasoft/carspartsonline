namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class SalesPerDayItem
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long SaleItem_Id { get; set; }

        [DataMember]
        //[ForeignKey("SaleDocument")]
        public long SaleDocumentsPerDay_Id { get; set; }

        [DataMember]
        public virtual SaleItem SaleItem { get; set; }

        [DataMember]
        public virtual SaleDocumentsPerDay SaleDocumentsPerDay { get; set; }
    }
}
