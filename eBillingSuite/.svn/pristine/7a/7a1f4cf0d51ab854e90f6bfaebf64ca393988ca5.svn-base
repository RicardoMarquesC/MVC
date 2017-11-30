namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tickets")]
    public partial class Tickets
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoPedido { get; set; }

        
        public string Descricao { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        [DefaultValue (false)]
        public bool Tratado { get; set; }
    }
}
