namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class DebtDischargeDocument
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        [Required]
        public DateTime DischargeDate { get; set; }

        //[DataMember]
        ////[ForeignKey("SaleDocument")]
        //[Required]
        //public long SaleDocument_Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Customer")]
        public string Debtor_Id { get; set; }

        [DataMember]
        [Required]
        public decimal Amount { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("User")]
        public string Creator_Id { get; set; }

        [DataMember]
        public virtual Customer Debtor { get; set; }

        //[DataMember]
        //public virtual SaleDocument SaleDocument { get; set; }

        [DataMember]
        public bool IsDischarge
        {
            get;
            set; 
        }

        [DataMember]
        public virtual User Creator { get; set; }
    }
}
