namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ManutencaoSistema")]
    public partial class ManutencaoSistema
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        public string TextoManutencao { get; set; }

        [Required]
        public string LastManDate { get; set; }
    }
}
