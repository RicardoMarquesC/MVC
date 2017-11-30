namespace eBillingSuite.Model.eBillingConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DigitalConfigurations
    {
        [Key]
        public Guid Pkid { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(20)]
        public string Tipo { get; set; }

        public string Dados { get; set; }
    }
}
