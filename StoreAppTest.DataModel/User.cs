namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class User
    {
        public User()
        {
            //DebtDischargeDocuments = new HashSet<DebtDischargeDocument>();
            //Incomes = new HashSet<Income>();
            //PriceLists = new HashSet<PriceList>();
            //RefundDocuments = new HashSet<RefundDocument>();
            //RefundDocuments1 = new HashSet<RefundDocument>();
            //RetailPrices = new HashSet<RetailPrice>();
            //SaleDocuments = new HashSet<SaleDocument>();
            Roles = new HashSet<Role>();
        }

        [DataMember]
        [Key]
        [StringLength(255)]
        public string UserName { get; set; }

        [DataMember]
        [StringLength(255)]
        public string DisplayName { get; set; }

        [DataMember]
        //[Required]
        [StringLength(255)]
        [ForeignKey("Warehouse")]
        public string Warehouse_Id { get; set; }

        [DataMember]
        public byte[] PasswordHash { get; set; }

        [DataMember]
        public bool IsSupplierVisible
        {
            get;
            set; }

        //[DataMember]
        //public virtual ICollection<DebtDischargeDocument> DebtDischargeDocuments { get; set; }

        //[DataMember]
        //public virtual ICollection<Income> Incomes { get; set; }

        //[DataMember]
        //public virtual ICollection<PriceList> PriceLists { get; set; }

        //[DataMember]
        //public virtual ICollection<RefundDocument> RefundDocuments { get; set; }

        //[DataMember]
        //public virtual ICollection<RefundDocument> RefundDocuments1 { get; set; }

        //[DataMember]
        //public virtual ICollection<RetailPrice> RetailPrices { get; set; }

        //[DataMember]
        //public virtual ICollection<SaleDocument> SaleDocuments { get; set; }

        //[DataMember]
        //public virtual ICollection<SaleDocument> SaleDocuments1 { get; set; }

        [DataMember]
        public virtual Warehouse Warehouse { get; set; }


        [DataMember]
        public virtual ICollection<Role> Roles { get; set; }

    }
}
