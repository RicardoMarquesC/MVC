namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FacturasNIdent")]
    public partial class FacturasNIdent
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [StringLength(250)]
        public string NomeFicheiro { get; set; }

        public DateTime? DataCriacao { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKexpirarFactura { get; set; }
    }
}
