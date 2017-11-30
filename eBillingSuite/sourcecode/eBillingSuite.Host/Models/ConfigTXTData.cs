using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	public class ConfigTXTData
	{
		public string NomeCampo { get; set; }
		public string Posicao { get; set; }
		public string Regex { get; set; }
		public string Tipo { get; set; }
		public Nullable<System.Guid> FKInstanceID { get; set; }
		public System.Guid pkid { get; set; }
	}
}