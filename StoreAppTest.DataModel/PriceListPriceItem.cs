
namespace StoreAppTest.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using Web.DataModel;

    [DataContract]
    public class PriceListPriceItem
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        [DataMember]
        public string PriceList_Name { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        [DataMember]
        public long PriceItem_Id { get; set; }

        [DataMember]
        public virtual PriceList PriceList { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }
    }
}
