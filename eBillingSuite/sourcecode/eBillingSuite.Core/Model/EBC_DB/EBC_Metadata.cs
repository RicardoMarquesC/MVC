namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_Metadata
    {
        [Key]
        public Guid PKID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public bool System { get; set; }

        public bool Mandatory { get; set; }

        [Required]
        [StringLength(640)]
        public string DefaultValue { get; set; }
    }
}
