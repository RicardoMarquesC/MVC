namespace eBillingSuite.Model.CIC_DB
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
        public Guid PKEBCPackageID { get; set; }

        [StringLength(25)]
        public string DMSID { get; set; }

        [StringLength(600)]
        public string NomeReceptor { get; set; }

        [StringLength(241)]
        public string EmailReceptor { get; set; }

        [StringLength(15)]
        public string NIFReceptor { get; set; }

        [StringLength(50)]
        public string NumFactura { get; set; }

        [StringLength(10)]
        public string DataFactura { get; set; }

        [StringLength(50)]
        public string QuantiaComIVA { get; set; }

        [StringLength(100)]
        public string DigitalInfoFileName { get; set; }

        public Guid? FKOutboundProcessID { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? GeneratedFromLine { get; set; }

        [StringLength(50)]
        public string CurrentEBCState { get; set; }

        public DateTime? LastUpdate { get; set; }

        public DateTime? LastReportGenerated { get; set; }

        [StringLength(200)]
        public string Source { get; set; }

        [StringLength(4000)]
        public string DocOriginal { get; set; }

        [StringLength(50)]
        public string QuantiaSemIVA { get; set; }

        [StringLength(50)]
        public string Moeda { get; set; }

        [StringLength(50)]
        public string QuantiaComIVAMoedaInterna { get; set; }

        [StringLength(50)]
        public string QuantiaSemIVAMoedaInterna { get; set; }

        [StringLength(50)]
        public string TaxaCambio { get; set; }

        [StringLength(50)]
        public string CondicaoPagamento { get; set; }

        [StringLength(15)]
        public string SWIFT { get; set; }

        [StringLength(34)]
        public string IBAN { get; set; }

        [StringLength(50)]
        public string TipoDocumento { get; set; }

        [StringLength(600)]
        public string NomeEmissor { get; set; }

        [StringLength(15)]
        public string NIFEmissor { get; set; }

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
        public string NumEncomenda { get; set; }

        [StringLength(50)]
        public string DigitalOrPaper { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identificador { get; set; }

        public bool? Reprocessado { get; set; }

        public virtual OutboundProcesses OutboundProcesses { get; set; }
    }
}
