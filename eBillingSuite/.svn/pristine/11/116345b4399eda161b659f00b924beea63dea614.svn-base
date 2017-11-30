namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TipoRegexs
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoRegex { get; set; }

        [Required]
        [StringLength(350)]
        public string Regex { get; set; }
    }
}
