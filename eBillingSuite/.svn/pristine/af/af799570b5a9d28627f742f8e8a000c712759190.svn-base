namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Linhas
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKfornecedores { get; set; }

        [StringLength(50)]
        public string NomeCampo { get; set; }

        [StringLength(50)]
        public string Localizacao { get; set; }

        public int? NumPagina { get; set; }

        public bool? Obrigatorio { get; set; }

        public Guid? Ordem { get; set; }

        public Guid? FKtipofactura { get; set; }

        [StringLength(50)]
        public string NomeReal { get; set; }
    }
}
