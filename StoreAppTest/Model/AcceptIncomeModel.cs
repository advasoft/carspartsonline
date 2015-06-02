
namespace StoreAppTest.Model
{
    using System;
    using System.Collections.ObjectModel;

    public class AcceptIncomeModel
    {
        public AcceptIncomeModel()
        {
            IncomeItems = new ObservableCollection<IncomeItem>();
        }


        public ObservableCollection<IncomeItem> IncomeItems { get; set; }


        public string IncomeNumber { get; set; }

        public DateTime IncomeDate { get; set; }
    }
}
