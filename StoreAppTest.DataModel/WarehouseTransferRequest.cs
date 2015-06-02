
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class WarehouseTransferRequest
    {
        public WarehouseTransferRequest()
        {
            WarehouseTransferRequestItemItems = new HashSet<WarehouseTransferRequestItem>();
        }

        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        public string RequestNumber { get; set; }

        [DataMember]
        public DateTime RequestDate { get; set; }

        [DataMember]
        [DefaultValue(false)]
        public bool IsAccept { get; set; }

        [DataMember]
        [DefaultValue(false)]
        public bool IsReserve { get; set; }

        [DataMember]
        public DateTime? StateChangedDate { get; set; }

        [DataMember]
        [StringLength(255)]
        public string Status { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Supplier")]
        public string Customer_Id { get; set; }

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
        public string LastChanged_Id { get; set; }

        [DataMember]
        public virtual ICollection<WarehouseTransferRequestItem> WarehouseTransferRequestItemItems { get; set; }

        [DataMember]
        public virtual Warehouse Supplier { get; set; }

        [DataMember]
        public virtual Warehouse Customer { get; set; }

        [DataMember]
        public virtual User Creator { get; set; }

        [DataMember]
        public virtual User LastChanged { get; set; }

        [DataMember]
        [StringLength(500)]
        public string Description { get; set; }
    }
}