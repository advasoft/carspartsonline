
namespace StoreAppTest.Web.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class PriceChangeReportItem
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long PriceItem_Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long PreviousPrice_Id { get; set; }

        [DataMember]
        //[ForeignKey("PriceItem")]
        public long NewPrice_Id { get; set; }


        [DataMember]
        //[ForeignKey("Income")]
        public long PriceChangeReport_Id { get; set; }

        [DataMember]
        public int Remainders { get; set; }

        [DataMember]
        public virtual PriceChangeReport PriceChangeReport { get; set; }

        [DataMember]
        public virtual PriceItem PriceItem { get; set; }

        //[DataMember]
        //public virtual RetailPrice PreviousRetailPrice { get; set; }

        //[DataMember]
        //public virtual RetailPrice RetailPrice { get; set; }

        [DataMember]
        public virtual WholesalePrice PreviousPrice { get; set; }

        [DataMember]
        public virtual WholesalePrice NewPrice { get; set; }

    }
}