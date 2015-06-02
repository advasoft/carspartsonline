
namespace StoreAppTest.Model
{
    using System;
    using StoreAppDataService;

    public class DebtItem : Notified
    {

        public int Number { get; set; }
        public DateTime Date { get; set; }
        public LookupSalesDocumentModel SaleDocument { get; set; }
        public DebtDischargeDocument DebtDischargeDocument { get; set; }
        public string Debtor { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }

        public int Res
        {
            get
            {
                return Up - Down;
            }
        }
    }
}
