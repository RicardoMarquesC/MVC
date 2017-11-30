using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.CIC_DB;

namespace eBillingSuite.Repositories
{
	public interface IEConnectorIntegrationFiltersRepository : IGenericRepository<IeBillingSuiteCICDBContext, IntegrationFilters>
	{
	}
}
