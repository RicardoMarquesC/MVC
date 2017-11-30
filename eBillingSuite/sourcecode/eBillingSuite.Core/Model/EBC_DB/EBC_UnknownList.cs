using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.EBC_DB
{
    public partial class EBC_UnknownList
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(250)]
        public string sender { get; set; }

        [Required]
        public DateTime data { get; set; }

        [Required]
        [StringLength(250)]
        public string emlPath { get; set; }

       
        public string subject { get; set; }
    }
}
