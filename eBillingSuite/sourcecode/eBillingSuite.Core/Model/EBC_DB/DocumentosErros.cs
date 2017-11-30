namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DocumentosErros
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(10)]
        public string TipoErro { get; set; }

        [Required]
        public string Ficheiro { get; set; }

        [Required]
        public string DetalheErro { get; set; }

        [StringLength(100)]
        public string NumDoc { get; set; }

        [StringLength(15)]
        public string NifEmissor { get; set; }

        [StringLength(15)]
        public string NifRecetor { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataCriacao { get; set; }
    }
}
