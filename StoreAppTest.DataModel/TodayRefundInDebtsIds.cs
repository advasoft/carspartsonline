using System;

namespace StoreAppTest.DataModel
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TodayRefundInDebtsIds
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int SaleItemId { get; set; }

        [DataMember]
        public DateTime Today { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public decimal Amount { get; set; }
    }
}
