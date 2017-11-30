namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_PackageEvents
    {
        [Key]
        public Guid PKID { get; set; }

        public Guid FKPackageID { get; set; }

        public int PackageState { get; set; }

        public DateTime EventDate { get; set; }

        public int EventType { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string EventMessage { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ordinal { get; set; }

        [StringLength(255)]
        public string Obs { get; set; }
    }
}
