namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InboundPacket")]
    public partial class InboundPacket
    {
        [Required]
        [StringLength(40)]
        public string PKProcessID { get; set; }

        [Required]
        [StringLength(40)]
        public string NomeEmissor { get; set; }

        [Required]
        [StringLength(15)]
        public string NIFemissor { get; set; }

        [Required]
        [StringLength(40)]
        public string NomeReceptor { get; set; }

        [Required]
        [StringLength(15)]
        public string NIFreceptor { get; set; }

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

        [StringLength(4)]
        public string CondicaoPagamento { get; set; }

        [StringLength(5)]
        public string TipoDocumento { get; set; }

        public Guid FKRemetenteID { get; set; }

        public DateTime DataRecepcao { get; set; }

        public DateTime? DataSubmissaoSAP { get; set; }

        [StringLength(300)]
        public string FicheiroEstadoSAP { get; set; }

        public bool Aprovado { get; set; }

        public DateTime? DataAprovacao { get; set; }

        public bool EnvioResposta { get; set; }

        [StringLength(300)]
        public string FicheiroResposta { get; set; }

        public DateTime? DataResposta { get; set; }

        public bool Valida { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Anotacoes { get; set; }

        [Key]
        public Guid InternalProcessID { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Obs { get; set; }

        public bool Devolvido { get; set; }
    }
}
