namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InboundPacketInfoObjectProperties
    {
        [Key]
        public Guid Pkid { get; set; }

        [Required]
        [StringLength(50)]
        public string PropertyName { get; set; }
    }
}