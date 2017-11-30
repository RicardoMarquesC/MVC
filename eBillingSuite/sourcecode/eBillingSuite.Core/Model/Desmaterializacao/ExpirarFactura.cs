namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExpirarFactura")]
    public partial class ExpirarFactura
    {
        [Key]
        public Guid pkid { get; set; }

        public int TempoExpirarFactura { get; set; }

        [StringLength(100)]
        public string Fase { get; set; }
    }
}
