using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
	public interface IDictionaryEntryRepository : IGenericRepository<IeBillingSuiteEBCDBContext, DictionaryEntries>
	{
	}
}
