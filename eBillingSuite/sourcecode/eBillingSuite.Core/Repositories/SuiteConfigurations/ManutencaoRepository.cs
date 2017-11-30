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
	public class ManutencaoRepository : GenericRepository<IeBillingSuiteEBCDBContext, ManutencaoSistema>, IManutencaoRepository
	{
		[Inject]
		public ManutencaoRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}
	}
}
