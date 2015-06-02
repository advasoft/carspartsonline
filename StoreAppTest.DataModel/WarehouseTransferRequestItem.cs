
namespace StoreAppTest.Web.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class WarehouseTransferRequestItem
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long PriceItem_Id { get; set; }

        [DataMember]
        public decimal Count { get; set; }
        [DataMember]
        public decimal CountAccepted { get; set; }

        [DataMember]
        public decimal WholesalePrice { get; set; }

        [DataMember]
        public decimal AcceptedAmount { get; set; }

        [DataMember]
        //[ForeignKey("Income")]
        public long WarehouseTransferRequest_Id { get; set; }

        [DataMember]
        public virtual WarehouseTransferRequest WarehouseTransferRequest { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }

    }
}