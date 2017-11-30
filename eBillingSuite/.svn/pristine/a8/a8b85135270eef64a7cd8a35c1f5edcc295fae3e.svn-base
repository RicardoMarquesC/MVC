namespace eBillingSuite.Model.EBC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBC_CustomerPackages
    {
        [Key]
        public Guid PKID { get; set; }

        public Guid FKPackageID { get; set; }

        public Guid FKCustomerID { get; set; }
    }
}
