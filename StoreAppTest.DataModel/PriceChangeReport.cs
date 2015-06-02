
namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class PriceChangeReport
    {
        public PriceChangeReport()
        {
            PriceChangeReportItems = new HashSet<PriceChangeReportItem>();
        }


        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(255)]
        public string ReportNumber { get; set; }

        [DataMember]
        public DateTime ReportDate { get; set; }


        [DataMember]
        [Required]
        [StringLength(255)]
        //[ForeignKey("Creator")]
        public string Creator_Id { get; set; }
        [DataMember]
        public virtual User Creator { get; set; }

        [DataMember]
        public virtual ICollection<PriceChangeReportItem> PriceChangeReportItems { get; set; }

    }
}