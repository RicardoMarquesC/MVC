using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
    public class UserData
    {
        public string CodUtilizador { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool allPermission { get; set; }
    }
}