namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_CertSignatureDetails
    {
        [Key]
        public Guid pkid { get; set; }

        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(100)]
        public string Keywords { get; set; }

        [StringLength(100)]
        public string Creator { get; set; }

        [StringLength(100)]
        public string Producer { get; set; }

        [StringLength(100)]
        public string SigReason { get; set; }

        [StringLength(100)]
        public string SigContact { get; set; }

        [StringLength(100)]
        public string SigLocation { get; set; }

        public bool SigVisible { get; set; }

        public Guid fkInstance { get; set; }

        public Guid fkMercado { get; set; }

        public virtual EBC_Instances EBC_Instances { get; set; }

        public virtual EBC_Mercados EBC_Mercados { get; set; }
    }
}
