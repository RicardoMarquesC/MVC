namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TipoFacturas
    {
        [Key]
        public Guid pkid { get; set; }

        [StringLength(50)]
        public string nome { get; set; }

        [Required]
        [StringLength(250)]
        public string RecognitionTags { get; set; }

        [StringLength(50)]
        public string FinalXmlDescription { get; set; }

        public bool? IsGenericDocument { get; set; }
    }
}
