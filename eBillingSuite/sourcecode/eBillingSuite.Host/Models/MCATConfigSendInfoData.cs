using eBillingSuite.Globalization.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	[DoNotGenerateDictionaryEntry]
	public class MCATConfigSendInfoData
	{
		public System.Guid pkid { get; set; }
		public int NumberOfAttempts { get; set; }
		public int UnidadeTempo { get; set; }
		public string TipoUnidadeTempo { get; set; }
		public string EnderecoEmail { get; set; }
		public string fkInstancia { get; set; }
	}
}