
namespace StoreAppTest.Event
{
    using Client.Model;
    using Microsoft.Practices.Prism.PubSubEvents;

    public class WarehouseTransferSendedRequestChangedEvent : PubSubEvent<WarehouseTransferRequest>
    {

    }
}
