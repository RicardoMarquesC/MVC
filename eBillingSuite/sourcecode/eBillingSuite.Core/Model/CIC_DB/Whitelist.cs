namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Whitelist")]
    public partial class Whitelist
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Whitelist()
        {
            InboundPacket = new HashSet<InboundPacket>();
        }

        [Key]
        public Guid PKWhitelistID { get; set; }

        [Required]
        [StringLength(127)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(241)]
        public string EmailName { get; set; }

        public Guid FKIntegrationFilterID { get; set; }

        public bool Enabled { get; set; }

        public bool HaveXML { get; set; }

        public bool ConcatAnexos { get; set; }

        [StringLength(50)]
        public string Mercado { get; set; }

        public bool? XMLAss { get; set; }

        public bool? XMLNAss { get; set; }

        public bool? PDFAss { get; set; }

        public bool? DoYouWantForwardEmail { get; set; }

        public bool? DoYouWantForwardFTP { get; set; }

        public bool? PDFNAss { get; set; }
        public bool? UsesPluginSystem { get; set; }

        [StringLength(50)]
        public string NIF { get; set; }

        public bool? PdfLink { get; set; }

        public string PdfLinkBaseURL { get; set; }

        public string ftpServer { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string port { get; set; }

        public string listEmails { get; set; }

        public string ReplyToAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InboundPacket> InboundPacket { get; set; }

        public virtual IntegrationFilters IntegrationFilters { get; set; }
    }
}