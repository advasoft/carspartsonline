namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class GearCategory
    {
        public GearCategory()
        {
            Gears = new HashSet<Gear>();
        }

        [Key]
        [StringLength(255)]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual ICollection<Gear> Gears { get; set; }
    }
}
