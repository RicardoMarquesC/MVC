namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_Mercados
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EBC_Mercados()
        {
            EBC_CertSignatureDetails = new HashSet<EBC_CertSignatureDetails>();
        }

        [Key]
        public Guid pkid { get; set; }

        [StringLength(50)]
        public string Mercado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EBC_CertSignatureDetails> EBC_CertSignatureDetails { get; set; }
    }
}
