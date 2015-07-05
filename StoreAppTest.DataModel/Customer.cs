namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Customer
    {
        public Customer()
        {
            //DebtDischargeDocuments = new HashSet<DebtDischargeDocument>();
            //SaleDocuments = new HashSet<SaleDocument>();
        }

        [Key]
        [StringLength(255)]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Details { get; set; }

        [DataMember]
        public string BankDetails { get; set; }

        [DataMember]
        public string ShipmentAddress { get; set; }


        [DataMember]
        [Required]
        [StringLength(255)]
        public string Creator_Id { get; set; }

        [DataMember]
        public virtual User Creator { get; set; }

        //[DataMember]
        //public virtual ICollection<DebtDischargeDocument> DebtDischargeDocuments { get; set; }

        //[DataMember]
        //public virtual ICollection<SaleDocument> SaleDocuments { get; set; }
    }
}
