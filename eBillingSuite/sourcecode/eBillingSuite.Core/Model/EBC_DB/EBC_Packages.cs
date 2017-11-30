namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_Packages
    {
        [Key]
        public Guid PKID { get; set; }

        public int PackageState { get; set; }

        public int CurrentFollowupRetry { get; set; }

        public bool IntegrationEnvironment { get; set; }

        [Required]
        [StringLength(200)]
        public string ApplicationName { get; set; }

        [StringLength(512)]
        public string MessageID { get; set; }

        public Guid FKSpecificDeliveryOptionsID { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TransitionState { get; set; }

        public bool? SentLinkEmail { get; set; }
    }
}
