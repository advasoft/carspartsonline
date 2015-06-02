
namespace StoreAppTest.Model
{
    using System;
    using System.Collections.Generic;

    public class Refund
    {

        public Refund()
        {
            RefundItems = new List<RefundItem>();
        }


        public DateTime RefundDate { get; set; }

        public string RefundNumber { get; set; }

        public string ReceiptNumber { get; set; }

        public IList<RefundItem> RefundItems { get; set; } 
    }
}
