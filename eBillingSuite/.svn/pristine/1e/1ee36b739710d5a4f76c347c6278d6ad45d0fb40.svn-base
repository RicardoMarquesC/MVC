namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MapeamentoXML")]
    public partial class MapeamentoXML
    {
        [Key]
        [Column(Order = 0)]
        public Guid pkid { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FKCaminhoXML_Forn { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid FKCaminhoXML_Cli { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid FKEmpresa { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string TipoXML { get; set; }

        [StringLength(50)]
        public string TipoCustom { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string NifFornecedor { get; set; }
    }
}
