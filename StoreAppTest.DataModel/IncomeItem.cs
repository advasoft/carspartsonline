namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class IncomeItem
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long PriceItem_Id { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal NewPrice { get; set; }

        [DataMember]
        public decimal BuyPriceRur { get; set; }

        [DataMember]
        public decimal BuyPriceTng { get; set; }

        [DataMember]
        //[ForeignKey("Income")]
        public long Income_Id { get; set; }

        [DataMember]
        public virtual Income Income { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }
    }
}
