using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorMarketCertificatesRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_MercadoCert>, IEMarketCertificatesRepository
	{
		[Inject]
		public EConnectorMarketCertificatesRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public EBC_MercadoCert GetMarketInfoByID(Guid id, Guid marketId)
		{
			return this.Where(emc => emc.fkInstance == id
				&&
				emc.fkMercado == marketId).FirstOrDefault();
		}
	}
}
