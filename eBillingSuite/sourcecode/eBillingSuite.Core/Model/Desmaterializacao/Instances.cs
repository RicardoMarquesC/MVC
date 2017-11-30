﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.Desmaterializacao
{
    public partial class Instances
    {
        [Key]
        public int id { get; set; }

        public string VatNumber { get; set; }
        public string Name { get; set; }
        public string InternalCode { get; set; }
        public string NextPageCounter { get; set; }
    }
}