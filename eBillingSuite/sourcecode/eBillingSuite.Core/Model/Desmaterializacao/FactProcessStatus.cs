namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FactProcessStatus
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string NomeFicheiro { get; set; }

        public bool? Status { get; set; }
    }
}
