namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_Customers
    {
        [Key]
        public Guid PKID { get; set; }

        [Required]
        [StringLength(600)]
        public string Name { get; set; }

        [Required]
        [StringLength(241)]
        public string Email { get; set; }

        public Guid FKInstanceID { get; set; }

        public Guid FKEmailContentID { get; set; }

        public Guid FKSpecificDeliveryOptionsID { get; set; }

        [Required]
        [StringLength(128)]
        public string NIF { get; set; }

        [StringLength(50)]
        public string Mercado { get; set; }

        public bool? XMLAss { get; set; }

        public bool? XMLNAss { get; set; }

        public bool? PDFAss { get; set; }

        public bool? PDFNAss { get; set; }
    }
}
