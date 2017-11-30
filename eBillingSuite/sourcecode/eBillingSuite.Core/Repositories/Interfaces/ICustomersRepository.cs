using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public interface ICustomersRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_Customers>
	{
		List<EBC_Customers> GetEBCCustomersByInstance(Guid id);
	}
}
