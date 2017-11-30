namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_MercadoCert
    {
        [Key]
        [Column(Order = 0)]
        public Guid fkInstance { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid fkMercado { get; set; }

        [StringLength(50)]
        public string serialnumber { get; set; }

        public string Caminho { get; set; }

        public string PasswordCert { get; set; }

        public string certEmailNotification { get; set; }

        public bool usePDFWS { get; set; }

        public string WS_Url { get; set; }

        public string WS_Username { get; set; }

        public string WS_Password { get; set; }

        public string WS_Domain { get; set; }

    }
}