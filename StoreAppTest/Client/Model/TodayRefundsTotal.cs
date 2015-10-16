
namespace StoreAppTest.Model
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class TodayRefundsTotal
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime Today { get; set; }

        [DataMember]
        public Decimal Total { get; set; }

        [DataMember]
        public Decimal TotalByBuy { get; set; }

        [DataMember]
        public string PriceListName { get; set; }
    }
}
