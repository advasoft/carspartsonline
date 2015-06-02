
namespace StoreAppTest.Event
{
    using System;
    using Microsoft.Practices.Prism.PubSubEvents;

    public class CloseViewNeedEvent : PubSubEvent<Guid>
    {
    }
}
