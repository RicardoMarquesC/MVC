using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Security;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
	public class ESuitePermissionsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, eSuitePermissions>, IESuitePermissionsRepository
	{
		[Inject]
		public ESuitePermissionsRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
		}


	}
}
