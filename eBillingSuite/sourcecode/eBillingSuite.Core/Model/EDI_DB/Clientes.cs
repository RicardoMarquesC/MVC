namespace eBillingSuite.Model.EDI_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clientes
    {
        [Key]
        public Guid PKID { get; set; }

        [Required]
        [StringLength(600)]
        public string Nome { get; set; }

        public Guid FKInstanciaID { get; set; }

        [Required]
        [StringLength(128)]
        public string NIF { get; set; }

        [StringLength(150)]
        public string URL { get; set; }

        public int TempoEspera { get; set; }

        public int TempoEsperaUnidade { get; set; }

        public int Tentativas { get; set; }

        public int Intervalo { get; set; }

        public int IntervaloUnidade { get; set; }

        public virtual Instancias Instancias { get; set; }
    }
}
