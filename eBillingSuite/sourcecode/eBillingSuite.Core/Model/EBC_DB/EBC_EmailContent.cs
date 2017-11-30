namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_EmailContent
    {
        [Key]
        public Guid PKID { get; set; }

        [Column(TypeName = "text")]
        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
