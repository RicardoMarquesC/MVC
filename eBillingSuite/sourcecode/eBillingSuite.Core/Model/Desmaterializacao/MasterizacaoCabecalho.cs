namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MasterizacaoCabecalho")]
    public partial class MasterizacaoCabecalho
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKNomeTemplate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string NomeCampo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Topo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Fundo { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string Esquerda { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string Direita { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string RegionId { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string LinhaId { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string WordId { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        public string WordPage { get; set; }

        public string Word { get; set; }
    }
}
