
namespace StoreAppTest.Event
{
    using System.Collections.Generic;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Model;
    using StoreAppDataService;
    using IncomeItem = Model.IncomeItem;

    public class NewIncomeAddedEvent : PubSubEvent<IList<IncomeItem>>
    {

    }
}
