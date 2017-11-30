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
	public class EConnectorXmlMappingRepository : GenericRepository<IeBillingSuiteEBCDBContext, MapeamentoXML>, IEConnectorXmlMappingRepository
	{
		[Inject]
		public EConnectorXmlMappingRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}
	}
}
