namespace eBillingSuite.Model.CIC_DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoNomenclaturaSender")]
    public partial class TipoNomenclaturaSender
    {
        [Key]
        public Guid pkid { get; set; }

        public Guid FKtiponomenclatura { get; set; }

        public Guid FKRemetente { get; set; }

        public virtual TiposNomenclaturaPDF TiposNomenclaturaPDF { get; set; }
    }
}
