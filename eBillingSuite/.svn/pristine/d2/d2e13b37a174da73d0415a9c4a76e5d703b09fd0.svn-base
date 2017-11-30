namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InboundIVA")]
    public partial class InboundIVA
    {
        [Key]
        public Guid pkid { get; set; }

        [StringLength(50)]
        public string BaseResumo { get; set; }

        [StringLength(50)]
        public string TaxaIVA { get; set; }

        [StringLength(50)]
        public string ValorImposto { get; set; }

        public Guid InternalProcessID { get; set; }

        public virtual InboundPacket InboundPacket { get; set; }
    }
}
