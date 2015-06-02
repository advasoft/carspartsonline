
namespace StoreAppTest.Model
{
    using System;
    using StoreAppDataService;

    public class LookupSalesDocumentModel : Notified
    {
        public SaleDocument SaleDocumentData { get; set; }
        public string Number { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }

        #region Overrides of Object

        /// <summary>
        /// Возвращает строку, представляющую текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Товарный чек № {0} от {1} на сумму {2}", Number, SaleDate, Amount);
        }

        #endregion
    }
}
