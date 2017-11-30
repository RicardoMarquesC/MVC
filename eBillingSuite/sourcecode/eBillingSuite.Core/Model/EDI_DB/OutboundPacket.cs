namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OutboundPacket")]
    public partial class OutboundPacket
    {
        [Key]
        public Guid PKEDIPacketID { get; set; }

        public Guid FKOutboundProcessID { get; set; }

        [Required]
        [StringLength(25)]
        public string DMSID { get; set; }

        [Required]
        [StringLength(40)]
        public string NomeReceptor { get; set; }

        [StringLength(241)]
        public string URLReceptor { get; set; }

        [Required]
        [StringLength(15)]
        public string NIFReceptor { get; set; }

        [Required]
        [StringLength(40)]
        public string NomeEmissor { get; set; }

        [Required]
        [StringLength(15)]
        public string NIFEmissor { get; set; }

        [Required]
        [StringLength(15)]
        public string NumFactura { get; set; }

        [Required]
        [StringLength(10)]
        public string DataFactura { get; set; }

        [StringLength(100)]
        public string Ficheiro { get; set; }

        [StringLength(15)]
        public string DocOriginal { get; set; }

        [Required]
        [StringLength(16)]
        public string QuantiaComIVA { get; set; }

        [Required]
        [StringLength(16)]
        public string QuantiaSemIVA { get; set; }

        [Required]
        [StringLength(3)]
        public string Moeda { get; set; }

        [Required]
        [StringLength(16)]
        public string QuantiaComIVAMoedaInterna { get; set; }

        [Required]
        [StringLength(16)]
        public string QuantiaSemIVAMoedaInterna { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TaxaCambio { get; set; }

        [StringLength(4)]
        public string CondicaoPagamento { get; set; }

        [StringLength(15)]
        public string SWIFT { get; set; }

        [StringLength(34)]
        public string IBAN { get; set; }

        [StringLength(4)]
        public string TipoDocumento { get; set; }

        public DateTime DataCriacao { get; set; }

        public int GeradoDaLinha { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public DateTime UltimaActualizacao { get; set; }

        public DateTime UltimoRelatorioGerado { get; set; }

        [StringLength(200)]
        public string Pasta { get; set; }

        [StringLength(40)]
        public string UUID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TaxaIVA { get; set; }

        [Required]
        [StringLength(16)]
        public string MontanteIVA { get; set; }

        public DateTime DataCheckpoint { get; set; }

        public int TentativaFollowup { get; set; }
    }
}
