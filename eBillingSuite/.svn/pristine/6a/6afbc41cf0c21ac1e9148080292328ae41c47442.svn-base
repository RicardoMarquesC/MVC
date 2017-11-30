using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
	public class DictionaryEntryRepository : GenericRepository<IeBillingSuiteEBCDBContext, DictionaryEntries>, IDictionaryEntryRepository
	{
		[Inject]
		public DictionaryEntryRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}
	}
}
