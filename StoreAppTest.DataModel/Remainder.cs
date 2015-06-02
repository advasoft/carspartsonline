namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Remainder
    {
        public Remainder()
        {
            RemaindersUserChanges = new HashSet<RemaindersUserChange>();    
        }


        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime RemainderDate { get; set; }

        [DataMember]
        [Required]
        //[ForeignKey("PriceItem")]
        public long PriceItem_Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Warehouse")]
        public string Warehouse_Id { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }

        [DataMember]
        public virtual Warehouse Warehouse { get; set; }

        [DataMember]
        public virtual ICollection<RemaindersUserChange> RemaindersUserChanges { get; set; } 
    }
}
