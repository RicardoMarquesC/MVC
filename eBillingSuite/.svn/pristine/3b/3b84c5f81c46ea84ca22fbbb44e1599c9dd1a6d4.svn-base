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
	public class EConnectorSpecificDeliveryOptionsRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_SpecificDeliveryOptions>, IConnectorSpecificDeliveryOptionsRepository
	{
		[Inject]
		public EConnectorSpecificDeliveryOptionsRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public EBC_SpecificDeliveryOptions GetSpecificOptionsByID(Guid id)
		{
			return this
				.Where(sdo => sdo.PKID == id)
				.FirstOrDefault();
		}
	}
}
