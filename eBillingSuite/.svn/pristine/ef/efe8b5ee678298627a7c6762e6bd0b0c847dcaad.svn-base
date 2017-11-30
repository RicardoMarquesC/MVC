namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_XMLLines
    {
        public int NumeroXML { get; set; }

        [Required]
        public string Element { get; set; }

        [StringLength(50)]
        public string NomeCampo { get; set; }

        [StringLength(50)]
        public string TipoXML { get; set; }

        public bool? Obrigatorio { get; set; }

        public int? Posicao { get; set; }

        [StringLength(100)]
        public string CampoBD { get; set; }

        public bool? isATfield { get; set; }

        [Key]
        public Guid pkid { get; set; }
    }
}
