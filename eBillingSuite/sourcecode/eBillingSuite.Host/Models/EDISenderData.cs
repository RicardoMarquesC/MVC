using eBillingSuite.Globalization.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	[DoNotGenerateDictionaryEntry]
	public class EDISenderData
	{
		public System.Guid PKID { get; set; }
		public string URL { get; set; }
		public string Nome { get; set; }
		public bool Activo { get; set; }
		public string NIF { get; set; }
	}
}