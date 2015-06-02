namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Warehouse
    {
        public Warehouse()
        {
            //Remainders = new HashSet<Remainder>();
            //Users = new HashSet<User>();
        }

        [DataMember]
        [Key]
        [StringLength(255)]
        public string Name { get; set; }

        //[DataMember]
        //public virtual ICollection<Remainder> Remainders { get; set; }

        //[DataMember]
        //public virtual ICollection<User> Users { get; set; }
    }
}
