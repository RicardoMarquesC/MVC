namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("xmlTemplate")]
    public partial class xmlTemplate
    {
        [Key]
        public Guid pkid { get; set; }

        [StringLength(500)]
        public string CaminhoXML { get; set; }

        [StringLength(50)]
        public string NomeCampo { get; set; }

        [StringLength(50)]
        public string Tipo { get; set; }

        [StringLength(50)]
        public string TipoXML { get; set; }

        [StringLength(500)]
        public string Regex { get; set; }

        public bool? isATfield { get; set; }
    }
}
