﻿
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();    
        }


        [DataMember]
        [Key]
        [StringLength(255)]
        public string RoleName { get; set; }

        [DataMember]
        public virtual ICollection<User> Users { get; set; }
    }
}