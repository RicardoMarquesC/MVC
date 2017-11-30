using eBillingSuite.Model.eBillingConfigurations;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
    public class GraphsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, UserGraphs>, IGraphsRepository
    {
        [Inject]
        public GraphsRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
        }

    }

    public interface IGraphsRepository : IGenericRepository<IeBillingSuiteConfigurationsContext, UserGraphs>
    {
    }
}
