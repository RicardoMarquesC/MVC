namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OutboundResumoIVA")]
    public partial class OutboundResumoIVA
    {
        [Key]
        [Column(Order = 0)]
        public Guid PKOutboundResumoIvaID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKOutboundProcessID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(16)]
        public string BaseResumo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(16)]
        public string ValorImposto { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "numeric")]
        public decimal TaxaIVA { get; set; }
    }
}
