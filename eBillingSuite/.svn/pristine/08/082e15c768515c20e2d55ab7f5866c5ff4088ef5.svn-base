namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ComATPackets_Guias
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(100)]
        public string NumeroDocumento { get; set; }

        [Required]
        [StringLength(50)]
        public string EstadoAT { get; set; }

        [StringLength(10)]
        public string CodRetornoAT { get; set; }

        public string ObsRetornoAT { get; set; }

        public DateTime? LastSentDate { get; set; }

        public int? SendAttempts { get; set; }

        [StringLength(15)]
        public string NIFEmissor { get; set; }

        public string CaminhoXML { get; set; }

        public string CaminhoTXT { get; set; }

        public string ATDocCodeID { get; set; }

        [StringLength(4)]
        public string Origem { get; set; }
    }
}
