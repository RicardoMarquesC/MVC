namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TiposNomenclaturaPDF")]
    public partial class TiposNomenclaturaPDF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TiposNomenclaturaPDF()
        {
            TipoNomenclaturaSender = new HashSet<TipoNomenclaturaSender>();
        }

        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(250)]
        public string tiponomenclatura { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipoNomenclaturaSender> TipoNomenclaturaSender { get; set; }
    }
}
