namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_InstanceDeniedSenders
    {
        [Key]
        public Guid pkid { get; set; }

        public Guid fkInstance { get; set; }

        [Required]
        [StringLength(50)]
        public string senderNIF { get; set; }
    }
}
