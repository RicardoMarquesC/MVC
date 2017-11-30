namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TextosSite")]
    public partial class TextosSite
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string isTexto { get; set; }

        [StringLength(500)]
        public string Texto { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string LocalizacaoSite { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Lingua { get; set; }

        [StringLength(50)]
        public string Tipo { get; set; }

        [StringLength(50)]
        public string Origem { get; set; }

        public int? Posicao { get; set; }
    }
}
