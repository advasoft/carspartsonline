namespace StoreAppTest.Web.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using StoreAppTest.DataModel;

    [DataContract]
    public partial class Gear
    {

        public Gear()
        {
            //PriceItems = new HashSet<PriceItem>();
        }

        [Key]
        [DataMember]
        public long Id { get; set; }


        [StringLength(255)]
        [DataMember]
        //[Required]
        public string CatalogNumber { get; set; }

        [StringLength(500)]
        [DataMember]
        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        [DataMember]
        public string Articul { get; set; }

        [DataMember]
        public bool IsDuplicate { get; set; }

        [Required]
        [StringLength(255)]
        [DataMember]
        //[ForeignKey("GearCategory")]
        public string Category_Id { get; set; }

        [DataMember]
        public decimal RecommendedRemainder { get; set; }

        [DataMember]
        public decimal LowerLimitRemainder { get; set; }


        [DataMember]
        public long? Image_Id
        {
            get;
            set; 
        }

        [DataMember]
        public virtual Resource Image
        {
            get;
            set; }

        [DataMember]
        public virtual GearCategory GearCategory { get; set; }

        //[DataMember]
        //public virtual ICollection<PriceItem> PriceItems { get; set; }

        #region Overrides of Object

        /// <summary>
        /// ќпредел€ет, равен ли заданный объект <see cref="T:System.Object"/> текущему объекту <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true, если заданный объект равен текущему объекту; в противном случае†Ч false.
        /// </returns>
        /// <param name="obj">ќбъект, который требуетс€ сравнить с текущим объектом.</param>
        public override bool Equals(object obj)
        {
            var g = obj as Gear;
            return g.Name == Name && g.Articul == Articul;
        }

        #endregion
    }
}
