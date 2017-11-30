namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissionType")]
    public partial class PermissionType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PermissionType()
        {
            UsersPermissions = new HashSet<UsersPermissions>();
        }

        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(126)]
        public string Name { get; set; }

        public int Type { get; set; }

        public bool isService { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersPermissions> UsersPermissions { get; set; }
    }
}
