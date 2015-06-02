namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class SaleDocumentsPerDay
    {
        public SaleDocumentsPerDay()
        {
            SalesPerDayItems = new HashSet<SalesPerDayItem>();
            RefundsPerDayItems = new HashSet<RefundsPerDayItem>();
        }

        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime SaleDocumentsDate { get; set; }

        [DataMember]
        public string Barcode { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        public string Number { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Creator")]
        public string Creator_Id { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public virtual User Creator { get; set; }

        [DataMember]
        public virtual ICollection<SalesPerDayItem> SalesPerDayItems { get; set; }
        [DataMember]
        public virtual ICollection<RefundsPerDayItem> RefundsPerDayItems { get; set; }

    }
}
