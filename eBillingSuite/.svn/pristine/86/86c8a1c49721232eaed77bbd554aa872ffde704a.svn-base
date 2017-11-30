namespace eBillingSuite.Model.Desmaterializacao
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UsersPermissions
    {
        [Key]
        public Guid pkid { get; set; }

        public Guid FKUser { get; set; }

        public Guid FKPermission { get; set; }

        public virtual PermissionType PermissionType { get; set; }

        public virtual Users Users { get; set; }
    }
}
