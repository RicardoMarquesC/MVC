using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using System.Security.Cryptography.X509Certificates;
using Ninject;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorCustomersRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_Customers>, IEConnectorCustomersRepository
	{
		[Inject]
		public EConnectorCustomersRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}		
	}
}
