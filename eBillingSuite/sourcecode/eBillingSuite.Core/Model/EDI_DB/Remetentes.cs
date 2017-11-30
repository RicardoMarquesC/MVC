namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Remetentes
    {
        [Key]
        public Guid PKID { get; set; }

        [Required]
        [StringLength(200)]
        public string URL { get; set; }

        [Required]
        [StringLength(241)]
        public string Nome { get; set; }

        public bool Activo { get; set; }

        [StringLength(128)]
        public string NIF { get; set; }
    }
}
