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
	public class EEDICostumersRepository : GenericRepository<IeBillingSuiteEDIDBContext, Clientes>, IEEDICostumersRepository
	{
		[Inject]
		public EEDICostumersRepository(IeBillingSuiteEDIDBContext context)
			: base(context)
		{
		}

		public List<Clientes> GetCostumersByInstanceID(Guid ID)
		{
			return this
				.Where(c => c.FKInstanciaID == ID)
				.ToList();
		}
	}
}
