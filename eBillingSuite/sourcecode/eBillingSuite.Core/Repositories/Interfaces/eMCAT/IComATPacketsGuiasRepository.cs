using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public interface IComATPacketsGuiasRepository : IGenericRepository<IeBillingSuiteEBCDBContext, ComATPackets_Guias>
	{
		//List<ComATPackets_Guias> GetComATPacketsGuias();

		List<ComATPackets_Guias> GetComATPacketsGuias(string pesquisa);

        List<ComATPackets_Guias> GetFilteredComATPackets(string pesquisa, string dateRange, string state, string numDoc);
    }
}
