namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoFacturaDadosXML")]
    public partial class TipoFacturaDadosXML
    {
        [Key]
        public Guid pkid { get; set; }

        public Guid fkTipoFactura { get; set; }

        [Required]
        [StringLength(250)]
        public string NomeCampo { get; set; }

        [Required]
        [StringLength(50)]
        public string Localizacao { get; set; }

        public int Posicao { get; set; }

        public int? Formato { get; set; }

        [StringLength(50)]
        public string TipoMetadado { get; set; }

        public bool? Obrigatorio { get; set; }

        public string Regex { get; set; }

        [Required]
        [StringLength(250)]
        public string NomeTemplate { get; set; }

        [StringLength(2)]
        public string TipoExtraccao { get; set; }

        public string Formula { get; set; }

		public bool? IsComboBox { get; set; }

		public bool? IsReadOnly { get; set; }

		public string LabelUI { get; set; }

        public bool PersistValueToNextDoc { get; set; }

        public string DefaultValue { get; set; }
    }
}
