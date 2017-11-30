using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBillingSuite.Models
{
	public class CustomerData
	{
		public System.Guid PKID { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public System.Guid FKInstanceID { get; set; }
		public System.Guid FKEmailContentID { get; set; }
		public System.Guid FKSpecificDeliveryOptionsID { get; set; }
		public string NIF { get; set; }
		public string Mercado { get; set; }
		public Nullable<bool> XMLAss { get; set; }
		public Nullable<bool> XMLNAss { get; set; }
		public Nullable<bool> PDFAss { get; set; }
		public Nullable<bool> PDFNAss { get; set; }
	}
}
