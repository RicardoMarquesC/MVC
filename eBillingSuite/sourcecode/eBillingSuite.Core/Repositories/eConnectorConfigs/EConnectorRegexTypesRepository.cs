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
	public class EConnectorRegexTypesRepository : GenericRepository<IeBillingSuiteEBCDBContext, TipoRegexs>, IEConnectorRegexTypesRepository
	{
		[Inject]
		public EConnectorRegexTypesRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public string GetRegexTypeByName(string regex)
		{
			bool exists = this.Exists(rt => rt.Regex == regex);

			if(exists)
				return this.Where(rt => rt.Regex == regex).FirstOrDefault().TipoRegex;
			
			return String.Empty;
		}
	}
}
