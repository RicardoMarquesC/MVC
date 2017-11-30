namespace eBillingSuite.Model.eBillingConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class eSuitePermissions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public eSuitePermissions()
        {
            eSuiteUserPermissions = new HashSet<eSuiteUserPermissions>();
        }

        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(250)]
        public string Nome { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eSuiteUserPermissions> eSuiteUserPermissions { get; set; }
    }
}
