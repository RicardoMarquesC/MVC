namespace eBillingSuite.Model.eBillingConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class eSuiteUserPermissions
    {
        [Key]
        public Guid pkid { get; set; }

        public Guid FKPermissions { get; set; }

        [Required]
        [StringLength(250)]
        public string Username { get; set; }

        public virtual eSuitePermissions eSuitePermissions { get; set; }
    }
}
