
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class GearChanges
    {
        [DataMember]
        [Required]
        [Key]
        public long Id { get; set; }

        [DataMember]
        [Required]
        public DateTime ChangesDate { get; set; }

        [DataMember]
        [Required]
        public long Gear_Id { get; set; }

        [DataMember]
        public virtual Gear Gear { get; set; }

    }
}