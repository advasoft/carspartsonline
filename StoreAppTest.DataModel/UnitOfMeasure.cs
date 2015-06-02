namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class UnitOfMeasure
    {
        public UnitOfMeasure()
        {
            //PriceItems = new HashSet<PriceItem>();
        }

        [Key]
        [StringLength(255)]
        [DataMember]
        public string Name { get; set; }

        //[DataMember]
        //public virtual ICollection<PriceItem> PriceItems { get; set; }
    }
}
