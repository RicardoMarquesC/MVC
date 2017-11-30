using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class CustomersRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_Customers>, ICustomersRepository
	{
		[Inject]
		public CustomersRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public List<EBC_Customers> GetEBCCustomersByInstance(Guid id)
		{
			return this
				.Where(ebcc => ebcc.FKInstanceID == id)
				.ToList();
		}
	}
}
