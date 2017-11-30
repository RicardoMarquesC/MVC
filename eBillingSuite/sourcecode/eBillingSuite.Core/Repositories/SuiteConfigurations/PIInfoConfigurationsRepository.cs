using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
	public class PIInfoConfigurationsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, PIInfo>, IPIInfoConfigurationsRepository
	{
		[Inject]
		public PIInfoConfigurationsRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
		}

		
	}
}
