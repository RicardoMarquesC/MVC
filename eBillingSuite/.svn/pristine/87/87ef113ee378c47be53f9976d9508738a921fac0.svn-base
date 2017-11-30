namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InboundResumoIVA")]
    public partial class InboundResumoIVA
    {
        [Key]
        [Column(Order = 0)]
        public Guid PKInboundResumoIvaID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InternalProcessID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(16)]
        public string BaseResumo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(16)]
        public string ValorImposto { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(16)]
        public string TaxaIVA { get; set; }
    }
}
