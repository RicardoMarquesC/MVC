using eBillingSuite.Globalization.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	[DoNotGenerateDictionaryEntry]
	public class DigitalSupplierSyncData
	{
		public bool WantSync { get; set; }
		public Guid SyncUrlConfigPkid { get; set; }
		public string SyncUrlConfig { get; set; }
		public Guid SyncUserConfigPkid { get; set; }
		public string SyncUserConfig { get; set; }
		public Guid SyncPassConfigPkid { get; set; }
		public string SyncPassConfig { get; set; }
	}
}