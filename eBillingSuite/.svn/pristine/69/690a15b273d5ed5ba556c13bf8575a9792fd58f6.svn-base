namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutboundProcesses
    {
        [Key]
        [Column(Order = 0)]
        public Guid PKOutboundProcessID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string FicheiroProcessado { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ordinal { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TotalLinhas { get; set; }

        public DateTime? DataProcessamentoCorrecto { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool ProcessadoCorrectamente { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumeroLinhasProcessadas { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LinhaPrimeiroErroEncontrado { get; set; }

        [StringLength(100)]
        public string FicheiroOriginal { get; set; }

        [Column(TypeName = "ntext")]
        public string Erro { get; set; }
    }
}
