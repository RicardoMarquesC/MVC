namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaxasIva")]
    public partial class TaxasIva
    {
        [Key]
        [Column(Order = 0)]
        public Guid Pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Taxa { get; set; }
    }
}
