namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InboundAllowanceCharge")]
    public partial class InboundAllowanceCharge
    {
        [Key]
        [Column(Order = 0)]
        public Guid PKID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKLineItemID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Descricao { get; set; }

        [StringLength(16)]
        public string Desconto { get; set; }

        [StringLength(16)]
        public string ValorLiquido { get; set; }

        [StringLength(16)]
        public string ValorIliquido { get; set; }

        [StringLength(16)]
        public string ValorDesconto { get; set; }

        [StringLength(16)]
        public string Preco { get; set; }

        [StringLength(16)]
        public string Quantidade { get; set; }

        [StringLength(16)]
        public string Capacidade { get; set; }

        [StringLength(3)]
        public string Unidade { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Encargo { get; set; }
    }
}
