namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutboundLineItens
    {
        [Key]
        [Column(Order = 0)]
        public Guid PKOutboundLineItemID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKOutboundProcessID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(6)]
        public string LineItemID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(16)]
        public string Quantidade { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(16)]
        public string Preco { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(16)]
        public string ValorComIVA { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(16)]
        public string ValorSemIVA { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "numeric")]
        public decimal TaxaIVA { get; set; }

        [StringLength(10)]
        public string CentroCusto { get; set; }

        [StringLength(10)]
        public string NotaEncomenda { get; set; }

        [StringLength(10)]
        public string Contentor { get; set; }

        [StringLength(10)]
        public string ViagemPartida { get; set; }

        [StringLength(10)]
        public string ImportacaoExportacao { get; set; }

        [StringLength(15)]
        public string Referencia { get; set; }

        [StringLength(16)]
        public string CGA { get; set; }
    }
}
