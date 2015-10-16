using System;

namespace StoreAppTest.DataModel
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TodayRealizationsTotal
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
