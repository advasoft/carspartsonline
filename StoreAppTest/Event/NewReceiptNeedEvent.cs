
namespace StoreAppTest.Event
{
    using Microsoft.Practices.Prism.PubSubEvents;
    using Model;

    public class NewReceiptNeedEvent : PubSubEvent<Receipt>
    {
    }
}
