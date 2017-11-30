namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DocumentoXML")]
    public partial class DocumentoXML
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(200)]
        public string NomeCampo { get; set; }

        [StringLength(200)]
        public string ValorPorDefeito { get; set; }

        public int Posicao1 { get; set; }

        public int Posicao2 { get; set; }

        public int Posicao3 { get; set; }

        public int Posicao4 { get; set; }

        public int Posicao5 { get; set; }

        public int Posicao6 { get; set; }

        public int Posicao7 { get; set; }
    }
}
