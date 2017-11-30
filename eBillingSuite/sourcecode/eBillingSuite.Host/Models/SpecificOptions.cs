using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
	public class SpecificOptionsData
	{
		public System.Guid PKID { get; set; }
		public int resendAfterCount { get; set; }
		public int resendAfterPeriodUnitType { get; set; }
		public int resendAfterPeriodUnit { get; set; }
		public int WaitForEfectiveResponseUnit { get; set; }
		public int WaitForEffectiveResponseUnitType { get; set; }
		public string NotificationEmailTecnical { get; set; }
		public string NotificationEmailFunctional { get; set; }
	}
}