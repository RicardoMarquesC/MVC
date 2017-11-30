using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
    public interface ITicketSystemRepository : IGenericRepository<IeBillingSuiteConfigurationsContext, TicketSystemTypes>
    {

    }
    public class TicketSystemRepository : GenericRepository<IeBillingSuiteConfigurationsContext, TicketSystemTypes>, ITicketSystemRepository
    {
		[Inject]
		public TicketSystemRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
		}
	}
}
