namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InboundPacket")]
    public partial class InboundPacket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InboundPacket()
        {
            InboundIVA = new HashSet<InboundIVA>();
            InboundLineItems = new HashSet<InboundLineItems>();
        }

        [Required]
        [StringLength(30)]
        public string PKProcessID { get; set; }

        [StringLength(15)]
        public string NIF { get; set; }

        [StringLength(35)]
        public string NumEncomenda { get; set; }

        [StringLength(50)]
        public string NumFactura { get; set; }

        [StringLength(10)]
        public string DataFactura { get; set; }

        [StringLength(15)]
        public string Quantia { get; set; }

        [StringLength(100)]
        public string DigitalInfoFilename { get; set; }

        public DateTime? ReceptionDate { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public Guid? FKWhiteListID { get; set; }

        [StringLength(127)]
        public string SubmissionFile { get; set; }

        public bool? RequiresManualApproval { get; set; }

        public bool? IsApproved { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public bool? AutoResponseSentBack { get; set; }

        [StringLength(127)]
        public string AutoResponseFile { get; set; }

        public DateTime? AutoResponseDate { get; set; }

        public bool? IsValid { get; set; }

        [Column(TypeName = "ntext")]
        public string Annotations { get; set; }

        [Key]
        public Guid InternalProcessID { get; set; }

        [StringLength(15)]
        public string NIFE { get; set; }

        [StringLength(15)]
        public string DocOriginal { get; set; }

        [Column(TypeName = "ntext")]
        public string Obs { get; set; }

        public bool? Devolvido { get; set; }

        [StringLength(15)]
        public string QuantiaSemIVA { get; set; }

        [StringLength(600)]
        public string NomeFornec { get; set; }

        [StringLength(50)]
        public string Moeda { get; set; }

        [StringLength(15)]
        public string QuantIVAMI { get; set; }

        [StringLength(15)]
        public string QuantSIVAMI { get; set; }

        [StringLength(50)]
        public string TCambio { get; set; }

        [StringLength(50)]
        public string CondicaoPag { get; set; }

        [StringLength(15)]
        public string SWIFT { get; set; }

        [StringLength(34)]
        public string IBAN { get; set; }

        [StringLength(50)]
        public string TipoDoc { get; set; }

        [StringLength(600)]
        public string NomeReceptor { get; set; }

        [StringLength(150)]
        public string EmailCliente { get; set; }

        [StringLength(50)]
        public string Referencia { get; set; }

        [StringLength(50)]
        public string ImportExport { get; set; }

        [StringLength(50)]
        public string ViagemPartida { get; set; }

        [StringLength(50)]
        public string CGA { get; set; }

        [StringLength(50)]
        public string Contentor { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identificador { get; set; }

        [StringLength(150)]
        public string IntegracaoERP { get; set; }

        public bool? Reprocessado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InboundIVA> InboundIVA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InboundLineItems> InboundLineItems { get; set; }

        public virtual Whitelist Whitelist { get; set; }
    }
}
