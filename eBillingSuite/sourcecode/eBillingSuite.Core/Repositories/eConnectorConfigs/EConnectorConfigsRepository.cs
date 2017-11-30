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
	public class EConnectorConfigsRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_Config>, IEConnectorConfigsRepository
	{
		[Inject]
		public EConnectorConfigsRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public List<EBC_Config> GetConfigsByID(Guid ID)
		{
			return this.Where(ebcc => ebcc.FKInstanceID == ID
				&& ebcc.ConfigSuiteType == "ebcConfig")
				.OrderBy(ebcc => ebcc.Position)
				.ToList();
		}
	}
}
