namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_XMLInbound
    {
        [Required]
        [StringLength(500)]
        public string Fornecedor { get; set; }

        public int NumeroXML { get; set; }

        [Key]
        public Guid pkid { get; set; }
    }
}
