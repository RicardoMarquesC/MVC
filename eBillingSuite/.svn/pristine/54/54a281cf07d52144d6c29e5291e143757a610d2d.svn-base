namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_XmlToTxtTransform
    {
        [Key]
        public Guid pkid { get; set; }

        public Guid fkInstanceId { get; set; }

        [Required]
        [StringLength(50)]
        public string InboundPacketPropertyName { get; set; }

        [Required]
        [StringLength(50)]
        public string tipo { get; set; }

        public int posicaoTxt { get; set; }
    }
}
