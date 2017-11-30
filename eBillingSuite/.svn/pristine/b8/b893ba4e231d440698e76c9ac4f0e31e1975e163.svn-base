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
	public class EEDISendersRepository : GenericRepository<IeBillingSuiteEDIDBContext, Remetentes>, IEEDISendersRepository
	{
		[Inject]
		public EEDISendersRepository(IeBillingSuiteEDIDBContext context)
			: base(context)
		{
		}
	}
}
