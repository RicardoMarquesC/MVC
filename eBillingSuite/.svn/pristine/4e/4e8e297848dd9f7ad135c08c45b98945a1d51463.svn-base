namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutboundProcesses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OutboundProcesses()
        {
            OutboundIVA = new HashSet<OutboundIVA>();
            OutboundLineItems = new HashSet<OutboundLineItems>();
            OutboundPacket = new HashSet<OutboundPacket>();
        }

        [Key]
        public Guid PKOutboundProcessID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProcessedFilename { get; set; }

        public DateTime? ProcessCreationDate { get; set; }

        public int Ordinal { get; set; }

        public int? TotalNumberOfLines { get; set; }

        public DateTime? SucceededProcessingDate { get; set; }

        public bool ProcessedCorrectly { get; set; }

        public int NumberOfLinesProcessed { get; set; }

        public int FirstErrorFoundOnLine { get; set; }

        [StringLength(100)]
        public string OriginalFileName { get; set; }

        [StringLength(1000)]
        public string Error { get; set; }

        [StringLength(50)]
        public string NifCliente { get; set; }

        [StringLength(50)]
        public string NifEmissor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutboundIVA> OutboundIVA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutboundLineItems> OutboundLineItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutboundPacket> OutboundPacket { get; set; }
    }
}
