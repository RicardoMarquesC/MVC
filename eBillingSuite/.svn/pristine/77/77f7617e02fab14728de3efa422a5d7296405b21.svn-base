using eBillingSuite.Globalization;
using eBillingSuite.Security;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite
{
	public class EBillingSuiteRequestContext : IeBillingSuiteRequestContext
	{
		public IeBillingSuiteIdentity UserIdentity { get; private set; }
		public ITimeAgoFormatter TimeAgoFormatter { get; private set; }

		[Inject]
		public EBillingSuiteRequestContext(IeBillingSuiteIdentity userIdentity)
		{
			TimeAgoFormatter = new TimeAgoFormatter(this);
			UserIdentity = userIdentity;
		}
	}
}