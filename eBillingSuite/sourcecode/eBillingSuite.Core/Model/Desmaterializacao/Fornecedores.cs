namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fornecedores
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Contribuinte { get; set; }

        [StringLength(50)]
        public string Telefone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(500)]
        public string Morada { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        [StringLength(500)]
        public string WebSite { get; set; }

        public bool WantMainValidations { get; set; }
    }
}
