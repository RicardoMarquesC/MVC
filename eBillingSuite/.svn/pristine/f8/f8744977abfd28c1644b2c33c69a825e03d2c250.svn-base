using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorMarketsRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_Mercados>, IEMarketsRepository
	{
		[Inject]
		public EConnectorMarketsRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}
	}
}
