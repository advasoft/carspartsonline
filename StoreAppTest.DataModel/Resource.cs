
namespace StoreAppTest.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Resource
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public byte[] ResourceData { get; set; }

    }
}
