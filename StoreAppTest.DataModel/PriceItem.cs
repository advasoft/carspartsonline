namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public partial class PriceItem
    {
        public PriceItem()
        {
            //IncomeItems = new HashSet<IncomeItem>();
            //RefundItems = new HashSet<RefundItem>();

            Remainders = new HashSet<Remainder>();
            //RetailPrices = new HashSet<RetailPrice>();
            PriceLists = new HashSet<PriceList>();
            Prices = new HashSet<WholesalePrice>();

            //SaleItems = new HashSet<SaleItem>();
        }
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        [Required]
        //[ForeignKey("Gear")]
        public long Gear_Id { get; set; }


        [DataMember]
        [Required]
        [DefaultValue(0)]
        public decimal BuyPriceRur { get; set; }

        [DataMember]
        [Required]
        [DefaultValue(0)]
        public decimal BuyPriceTng { get; set; }

        //[DataMember]
        //[Required]
        //[DefaultValue(0)]
        //public decimal WholesalePrice { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("UnitOfMeasure")]
        public string Uom_Id { get; set; }

        [DataMember]
        [StringLength(50)]
        //[ForeignKey("UnitOfMeasure")]
        public string Barcode1 { get; set; }

        [DataMember]
        [StringLength(50)]
        //[ForeignKey("UnitOfMeasure")]
        public string Barcode2 { get; set; }

        [DataMember]
        [StringLength(50)]
        //[ForeignKey("UnitOfMeasure")]
        public string Barcode3 { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        //[DataMember]
        //[Required]
        //[StringLength(255)]
        ////[ForeignKey("PriceList")]
        //public string PriceList_Id { get; set; }

        [DataMember]
        public virtual Gear Gear { get; set; }

        //[DataMember]
        //public virtual ICollection<IncomeItem> IncomeItems { get; set; }

        [DataMember]
        //public virtual PriceList PriceList { get; set; }
        public virtual ICollection<PriceList> PriceLists { get; set; }
        
        [DataMember]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        //[DataMember]
        //public virtual ICollection<RefundItem> RefundItems { get; set; }

        [DataMember]
        public virtual ICollection<Remainder> Remainders { get; set; }

        [DataMember]
        public virtual ICollection<WholesalePrice> Prices { get; set; }


        //[DataMember]
        //public virtual ICollection<RetailPrice> RetailPrices { get; set; }




        //[DataMember]
        //public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
}
