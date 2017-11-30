namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CamposXML")]
    public partial class CamposXML
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string CaminhoXML { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string NomeCampo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string TipoXML { get; set; }

        public string Regex { get; set; }
    }
}
