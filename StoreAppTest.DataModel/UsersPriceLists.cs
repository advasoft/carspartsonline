

namespace StoreAppTest.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using Web.DataModel;

    public class UsersPriceLists
    {

        [DataMember]
        [Key]
        [Required]
        [Column(Order = 0)]
        public string User_UserName { get; set; }


        [Key]
        [Column(Order = 1)]
        [Required]
        [DataMember]
        public string PriceList_Name { get; set; }


        [DataMember]
        public virtual User User { get; set; }

        [DataMember]
        public virtual PriceList PriceList { get; set; }
    }
}
