
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class RemaindersUserChange
    {
        [Key]
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string User_Id { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [DataMember]
        public long Remainder_Id { get; set; }
        [DataMember]
        public virtual Remainder Remainder { get; set; }
        [DataMember]
        public decimal Amound { get; set; }
    }
}