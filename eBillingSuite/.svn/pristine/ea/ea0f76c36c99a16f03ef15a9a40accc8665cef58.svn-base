using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.EDI_DB;

namespace eBillingSuite.Repositories
{
	public class EEDIInstancesRepository : GenericRepository<IeBillingSuiteEDIDBContext, Instancias>, IEEDIInstancesRepository
	{
		[Inject]
		public EEDIInstancesRepository(IeBillingSuiteEDIDBContext context)
			: base(context)
		{
		}

		public List<Instancias> GetInstances()
		{
			return this
				.Set.ToList();
		}
	}
}
