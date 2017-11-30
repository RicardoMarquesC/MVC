namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutboundLineItems
    {
        [Key]
        public Guid pkIDOUTBOUND_Item { get; set; }

        [StringLength(20)]
        public string Quantidade { get; set; }

        [StringLength(50)]
        public string Preco { get; set; }

        [StringLength(50)]
        public string ValorCIVA { get; set; }

        [StringLength(50)]
        public string ValorSIVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TaxaIVA { get; set; }

        [StringLength(50)]
        public string CentroCusto { get; set; }

        [StringLength(35)]
        public string NotaEncomenda { get; set; }

        public Guid FKOutboundProcessID { get; set; }

        [StringLength(6)]
        public string LineItemID { get; set; }

        [StringLength(50)]
        public string DescontosLinha { get; set; }

        [StringLength(50)]
        public string Referencia { get; set; }

        [StringLength(50)]
        public string ImportExport { get; set; }

        [StringLength(50)]
        public string ViagemPartida { get; set; }

        [StringLength(50)]
        public string Contentor { get; set; }

        [StringLength(50)]
        public string CGA { get; set; }

        [StringLength(50)]
        public string TotalDescontos { get; set; }

        public virtual OutboundProcesses OutboundProcesses { get; set; }
    }
}
