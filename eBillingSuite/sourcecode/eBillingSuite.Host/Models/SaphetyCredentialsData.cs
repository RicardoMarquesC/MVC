using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBillingSuite.Models
{
	public class SaphetyCredentialsData
	{
		public System.Guid pkid { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string instance { get; set; }
	}
}
