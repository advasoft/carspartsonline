
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class WholesalePrice
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime PriceDate { get; set; }

        [DataMember]
        [Required]
        public long PriceItem_Id { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }

    }
}