namespace eBillingSuite.Model.eBillingConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIInfo")]
    public partial class PIInfo
    {
        [Required]
        [StringLength(250)]
        public string Nome { get; set; }

        [Key]
        [StringLength(20)]
        public string NIF { get; set; }

        public int Codigo { get; set; }
    }
}
