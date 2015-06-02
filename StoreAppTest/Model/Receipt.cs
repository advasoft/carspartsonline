
namespace StoreAppTest.Model
{
    using System;
    using System.Collections.Generic;
    using StoreAppDataService;

    public class Receipt
    {
        public Receipt()
        {
            ReceiptItems = new List<ReceiptItem>();
        }


        public DateTime ReceiptDate { get; set; }
        public string ReceiptNumber { get; set; }

        public Customer Customer { get; set; }

        public bool IsReceiptPrinted { get; set; }

        public bool IsOrder { get; set; }

        public bool IsInDebt { get; set; }

        public bool IsInvoice { get; set; }

        public string PriceListName { get; set; }

        public IList<ReceiptItem> ReceiptItems { get; set; }
    }
}
