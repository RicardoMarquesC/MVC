using Ninject;
using Shortcut.Repositories;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
	public class ESuiteUserPermissionsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, eSuiteUserPermissions>, IESuiteUserPermissionsRepository
	{
		[Inject]
		public ESuiteUserPermissionsRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
		}
	}
}
