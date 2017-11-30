namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IntegrationFilters
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IntegrationFilters()
        {
            Whitelist = new HashSet<Whitelist>();
        }

        [Key]
        public Guid PKIntegrationFilterID { get; set; }

        [Required]
        [StringLength(127)]
        public string FriendlyName { get; set; }

        [Required]
        [StringLength(512)]
        public string TypeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Whitelist> Whitelist { get; set; }
    }
}
