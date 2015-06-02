namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Income
    {
        public Income()
        {
            IncomeItems = new HashSet<IncomeItem>();
        }

        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        public string IncomeNumber { get; set; }

        [DataMember]
        public DateTime IncomeDate { get; set; }

        [DataMember]
        [DefaultValue(false)]
        public bool IsAccept { get; set; }

        [DataMember]
        public DateTime? AcceptedDate { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Supplier")]
        public string Supplier_Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Creator")]
        public string Creator_Id { get; set; }

        [DataMember]
        [StringLength(255)]
        //[ForeignKey("Creator")]
        public string Accepter_Id { get; set; }

        [DataMember]
        public virtual ICollection<IncomeItem> IncomeItems { get; set; }

        [DataMember]
        public virtual Supplier Supplier { get; set; }

        [DataMember]
        public virtual User Creator { get; set; }

        [DataMember]
        public virtual User Accepter { get; set; }
    }
}
