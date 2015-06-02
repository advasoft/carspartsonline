namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class SaleDocument
    {
        public SaleDocument()
        {
            //DebtDischargeDocuments = new HashSet<DebtDischargeDocument>();
            //RefundDocuments = new HashSet<RefundDocument>();
            SaleItems = new HashSet<SaleItem>();
        }

        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime SaleDate { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        public string Customer_Name { get; set; }

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
        [Required]
        [StringLength(255)]
        //[ForeignKey("LastChanger")]
        public string LastChanger_Id { get; set; }

        [DataMember]
        public bool IsReceiptPrinted { get; set; }

        [DataMember]
        public bool IsOrder { get; set; }

        [DataMember]
        public bool IsInDebt { get; set; }

        [DataMember]
        public bool IsInvoice { get; set; }

        [DataMember]
        public string Barcode { get; set; }

        [DataMember]
        public virtual Customer Customer { get; set; }

        //[DataMember]
        //public virtual ICollection<DebtDischargeDocument> DebtDischargeDocuments { get; set; }

        //[DataMember]
        //public virtual ICollection<RefundDocument> RefundDocuments { get; set; }

        [DataMember]
        public virtual User Creator { get; set; }

        [DataMember]
        public virtual User LastChanger { get; set; }

        [DataMember]
        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
}
