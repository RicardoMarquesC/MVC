namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_Config
    {
        [Key]
        public Guid PKID { get; set; }

        public int ConfigurationKey { get; set; }

        [Required]
        [StringLength(200)]
        public string KeyName { get; set; }

        [Required(AllowEmptyStrings =true)]
        [StringLength(500)]
        public string KeyValue { get; set; }

        public Guid FKInstanceID { get; set; }

        [StringLength(5)]
        public string NotificationInfo { get; set; }

        [StringLength(10)]
        public string ConfigSuiteType { get; set; }

        public int? Position { get; set; }
    }
}
