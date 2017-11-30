namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_XMLClient
    {
        [Key]
        public Guid Pkid { get; set; }

        public Guid FkCliente { get; set; }

        public int NumeroXML { get; set; }
    }
}
