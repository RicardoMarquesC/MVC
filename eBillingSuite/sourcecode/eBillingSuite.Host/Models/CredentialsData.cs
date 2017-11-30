using eBillingSuite.Globalization.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	[DoNotGenerateDictionaryEntry]
	public class CredentialsData
	{
		public System.Guid pkid { get; set; }
		public string usrat { get; set; }
		public string pwdat { get; set; }
		public string confirmpwdat { get; set; }
		public System.Guid fkEmpresa { get; set; }
	}
}