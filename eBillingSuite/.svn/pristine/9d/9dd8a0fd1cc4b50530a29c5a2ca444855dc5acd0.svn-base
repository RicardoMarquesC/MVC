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
	public interface IInstancesRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_Instances>
	{
		//List<EBC_Instances> GetEBC_Instances(_context.UserIdentity.Instances);

		Guid GetSpecificOptionIDByInstance(Guid id);

        List<EBC_Instances> GetEBC_Instances(string instances);

    }
}
