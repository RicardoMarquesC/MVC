namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_SpecificDeliveryOptions
    {
        [Key]
        public Guid PKID { get; set; }

        public int resendAfterCount { get; set; }

        public int resendAfterPeriodUnitType { get; set; }

        public int resendAfterPeriodUnit { get; set; }

        public int WaitForEfectiveResponseUnit { get; set; }

        public int WaitForEffectiveResponseUnitType { get; set; }

        [StringLength(200)]
        public string NotificationEmailTecnical { get; set; }

        [StringLength(200)]
        public string NotificationEmailFunctional { get; set; }

        [StringLength(200)]
        public string NotificationEmailMonitoring { get; set; }
    }
}
