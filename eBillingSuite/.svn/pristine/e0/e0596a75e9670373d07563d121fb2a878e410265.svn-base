using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.EDI_DB;

namespace eBillingSuite.Repositories
{
	public interface IEEDICostumersRepository : IGenericRepository<IeBillingSuiteEDIDBContext, Clientes>
	{
		List<Clientes> GetCostumersByInstanceID(Guid ID);

	}
}
