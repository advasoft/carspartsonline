namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class RefundDocument
    {
        public RefundDocument()
        {
            RefundItems = new HashSet<RefundItem>();
        }

        [Key]
        [DataMember]
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        [DataMember]
        public string RefundNumber { get; set; }

        [DataMember]
        public DateTime RefundDate { get; set; }

        [DataMember]
        //[ForeignKey("SaleDocument")]
        public long SaleDocument_Id { get; set; }

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
        public virtual SaleDocument SaleDocument { get; set; }

        [DataMember]
        public virtual User Creator { get; set; }

        [DataMember]
        public virtual User LastChanger { get; set; }

        [DataMember]
        public virtual ICollection<RefundItem> RefundItems { get; set; }
    }
}
