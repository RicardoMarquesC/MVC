namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlertasSite")]
    public partial class AlertasSite
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(350)]
        public string Alerta { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string LocalizacaoSite { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Lingua { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Tipo { get; set; }
    }
}
