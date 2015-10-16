
namespace StoreAppTest.Model
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TodayRefundsTotalItem
    {
        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public decimal TotalByBuy { get; set; }
    }
}
