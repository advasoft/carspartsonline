namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class RetailPrice
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime PriceDate { get; set; }

        [DataMember]
        [Required]
        //[ForeignKey("PriceItem")]
        public long PriceItem_Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Author")]
        public string Author_Id { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }

        [DataMember]
        public virtual User Author { get; set; }
    }
}
