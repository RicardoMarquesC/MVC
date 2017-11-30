using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	public class PacketConfigsData
	{
		public System.Guid PKID { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string XML { get; set; }
	}
}