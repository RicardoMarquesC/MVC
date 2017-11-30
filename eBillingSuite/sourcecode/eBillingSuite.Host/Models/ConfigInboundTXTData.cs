using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	public class ConfigInboundTXTData
	{
		public System.Guid pkid { get; set; }
		public System.Guid fkInstanceId { get; set; }
		public string InboundPacketPropertyName { get; set; }
		public string tipo { get; set; }
		public int posicaoTxt { get; set; }
	}
}