using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.eBillingConfigurations
{
    [Table("UserGraphs")]
    public partial class UserGraphs
    {
        [Required]
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }


        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [MaxLength]
        public string Desmaterializacao { get; set; }

        [Required]
        [MaxLength]
        public string FacElect { get; set; }

        [Required]
        [MaxLength]
        public string ComAT { get; set; }

    }
}
