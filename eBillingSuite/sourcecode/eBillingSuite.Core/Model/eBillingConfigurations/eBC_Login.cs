﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.eBillingConfigurations
{
    [Table("eBC_Login")]
    public partial class eBC_Login
    {
        [Required]
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodUtilizador { get; set; }


        [Required]
        [StringLength(20)]
        public string username { get; set; }

        [Required]
        [MaxLength]
        public string password { get; set; }

        public DateTime? LastLogin { get; set; }

        [MaxLength]
        public string Instances { get; set; }
    }
}
