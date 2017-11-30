namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cabecalho")]
    public partial class Cabecalho
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKFornecedores { get; set; }

        [StringLength(50)]
        public string NomeCampo { get; set; }

        [StringLength(50)]
        public string Localizacao { get; set; }

        public int? NumPagina { get; set; }

        public bool? Obrigatorio { get; set; }

        public Guid? Ordem { get; set; }

        public bool? isDate { get; set; }

        public Guid? FKTipoFactura { get; set; }

        public bool? Extra { get; set; }

        public bool? isNIF { get; set; }

        [StringLength(50)]
        public string NomeReal { get; set; }
    }
}
