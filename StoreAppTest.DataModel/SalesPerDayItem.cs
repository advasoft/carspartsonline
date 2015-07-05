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

        [StringLength(255)]
        [DataMember]
        //[Required]
        public string CatalogNumber { get; set; }

        [StringLength(500)]
        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public bool IsDuplicate { get; set; }

        [DataMember]
        public string UnitOfMeasure { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public decimal Count { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal Remainders { get; set; }

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
