namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class PriceList
    {
        public PriceList()
        {
            PriceItems = new HashSet<PriceItem>();
        }

        [Key]
        [StringLength(255)]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [DataMember]
        //[ForeignKey("UploadUser")]
        public string UploadUser_Id { get; set; }

        [DataMember]
        [Required]
        public DateTime UploadDate { get; set; }

        [DataMember]
        public virtual ICollection<PriceItem> PriceItems { get; set; }

        [DataMember]
        public virtual User UploadUser { get; set; }
    }
}
