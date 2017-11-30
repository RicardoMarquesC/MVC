namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_ConfigTXT
    {
        [StringLength(50)]
        public string NomeCampo { get; set; }

        [StringLength(50)]
        public string Posicao { get; set; }

        public string Regex { get; set; }

        [StringLength(50)]
        public string Tipo { get; set; }

        public Guid? FKInstanceID { get; set; }

        [Key]
        public Guid pkid { get; set; }
    }
}
