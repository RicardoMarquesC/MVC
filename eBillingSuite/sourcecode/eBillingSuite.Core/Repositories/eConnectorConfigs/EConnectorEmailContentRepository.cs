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
	public class EConnectorEmailContentRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_EmailContent>, IConnectorEmailContentRepository
	{
		[Inject]
		public EConnectorEmailContentRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}


		public EBC_EmailContent GetEmailContentByID(Guid id)
		{
			return this
				.Where(ec => ec.PKID == id)
				.FirstOrDefault();
		}
	}
}
