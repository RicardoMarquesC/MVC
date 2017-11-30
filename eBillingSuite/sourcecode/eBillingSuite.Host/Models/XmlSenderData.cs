using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBillingSuite.Models
{
	public class XmlSenderData
	{
		public Guid Pkid { get; set; }
		public Guid SenderId { get; set; }
		public string XmlType { get; set; }
		public string XmlNumber { get; set; }
		public string Area { get; set; }
		public string NomeCampo { get; set; }
		public bool IsRequired { get; set; }
	}
}
