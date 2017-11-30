namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InboundLineItens
    {
        [Key]
        [Column(Order = 0)]
        public Guid PKInboundLineItemID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InternalProcessID { get; set; }

        [StringLength(6)]
        public string LineItemID { get; set; }

        [StringLength(15)]
        public string DocEntrega { get; set; }

        [StringLength(10)]
        public string DataEntrega { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        [StringLength(16)]
        public string Quantidade { get; set; }

        [StringLength(3)]
        public string Unidade { get; set; }

        [StringLength(16)]
        public string Preco { get; set; }

        [StringLength(16)]
        public string ValorIliquido { get; set; }

        [StringLength(16)]
        public string Desconto { get; set; }

        [StringLength(16)]
        public string ValorDesconto { get; set; }

        [StringLength(15)]
        public string TaxaIVA { get; set; }

        [StringLength(16)]
        public string ValorLiquido { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool DescOuEncargo { get; set; }

        [StringLength(4)]
        public string IDdescEncargo { get; set; }
    }
}
