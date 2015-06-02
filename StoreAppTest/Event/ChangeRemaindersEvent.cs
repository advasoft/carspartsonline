
namespace StoreAppTest.Event
{
    using System.Collections.Generic;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Model;

    public class ChangeRemaindersEvent : PubSubEvent<IList<PriceItem>>
    {

    }
}
