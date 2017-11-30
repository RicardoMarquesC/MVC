namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OutboundIVA")]
    public partial class OutboundIVA
    {
        [Key]
        public Guid pkid { get; set; }

        [StringLength(50)]
        public string Base { get; set; }

        [StringLength(50)]
        public string TaxaIVA { get; set; }

        [StringLength(50)]
        public string ValorIVAaplicado { get; set; }

        public Guid FKOutboundProcessID { get; set; }

        public virtual OutboundProcesses OutboundProcesses { get; set; }
    }
}
