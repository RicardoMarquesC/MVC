namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_Instances
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EBC_Instances()
        {
            EBC_CertSignatureDetails = new HashSet<EBC_CertSignatureDetails>();
        }

        [Key]
        public Guid PKID { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public Guid FKEmailContentID { get; set; }

        public Guid FKSpecificDeliveryOptionsID { get; set; }

        [StringLength(50)]
        public string NIF { get; set; }

        public bool? HasInternalProcess { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EBC_CertSignatureDetails> EBC_CertSignatureDetails { get; set; }
    }
}
