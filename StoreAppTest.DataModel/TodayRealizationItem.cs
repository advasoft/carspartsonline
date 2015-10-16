namespace StoreAppTest.DataModel
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class TodayRealizationItem
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime Today { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string CatalogNumber { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsDuplicate { get; set; }

        [DataMember]
        public string UnitOfMeasure { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal WholePrice { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public decimal Count { get; set; }

        [DataMember]
        public decimal SoldCount { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal Remainders { get; set; }

        [DataMember]
        public long SaleItem_Id { get; set; }

        [DataMember]
        public long PriceItem_Id { get; set; }

        [DataMember]
        public bool IsInDebt { get; set; }

        [DataMember]
        public string Customer { get; set; }

        [DataMember]
        public string PriceListName { get; set; }

        [DataMember]
        public string SaledTime { get; set; }

        [DataMember]
        public string SalesNumber { get; set; }
    }

}
