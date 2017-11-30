namespace eBillingSuite.Model.eBillingConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class eSuite_Produtos
    {
        [Key]
        public Guid pkid { get; set; }

        [StringLength(500)]
        public string utilizador { get; set; }

        public Guid? produto { get; set; }

        public bool? activo { get; set; }
    }
}
