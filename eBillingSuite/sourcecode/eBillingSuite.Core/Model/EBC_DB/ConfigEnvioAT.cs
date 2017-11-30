namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConfigEnvioAT")]
    public partial class ConfigEnvioAT
    {
        [Key]
        public Guid pkid { get; set; }

        public int NumberOfAttempts { get; set; }

        public int UnidadeTempo { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoUnidadeTempo { get; set; }

        [StringLength(250)]
        public string EnderecoEmail { get; set; }

        [StringLength(250)]
        public string fkInstancia { get; set; }
    }
}
