namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoginAT")]
    public partial class LoginAT
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(25)]
        public string usrat { get; set; }

        [Required]
        public string pwdat { get; set; }

        public Guid fkEmpresa { get; set; }
    }
}
