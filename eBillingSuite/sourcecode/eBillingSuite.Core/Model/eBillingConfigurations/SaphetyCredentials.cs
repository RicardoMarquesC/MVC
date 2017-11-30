namespace eBillingSuite.Model.eBillingConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SaphetyCredentials
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(150)]
        public string password { get; set; }

        [Required]
        [StringLength(50)]
        public string instance { get; set; }
    }
}
