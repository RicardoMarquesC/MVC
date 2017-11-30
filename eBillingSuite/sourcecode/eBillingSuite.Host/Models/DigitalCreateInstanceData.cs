using eBillingSuite.Globalization.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	[DoNotGenerateDictionaryEntry]
	public class DigitalCreateInstanceData
    {		
		public int ID { get; set; }
		public string Name { get; set; }
		public string VatNumber { get; set; }
		public string InternalCode { get; set; }
	}
}