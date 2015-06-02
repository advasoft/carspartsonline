namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Supplier
    {
        public Supplier()
        {
            //Incomes = new HashSet<Income>();
        }

        [DataMember]
        [Key]
        [StringLength(255)]
        public string Name { get; set; }

        [DataMember]
        [StringLength(255)]
        public string City { get; set; }

        [DataMember]
        [StringLength(255)]
        public string Phone { get; set; }

        [DataMember]
        [StringLength(255)]
        public string Email { get; set; }

        [DataMember]
        [StringLength(255)]
        public string Manager { get; set; }

        //[DataMember]
        //public virtual ICollection<Income> Incomes { get; set; }
    }
}
