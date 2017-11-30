namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NomeTemplate")]
    public partial class NomeTemplate
    {
        [Key]
        public Guid pkid { get; set; }

        [Column("NomeTemplate")]
        [Required]
        [StringLength(150)]
        public string NomeTemplate1 { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoXML { get; set; }

        public Guid? fkfornecedor { get; set; }

        public Guid? fktipofact { get; set; }

        public bool? Masterizado { get; set; }

        [Required]
        [StringLength(150)]
        public string NomeOriginal { get; set; }
    }
}
