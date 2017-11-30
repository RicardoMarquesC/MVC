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
	public class EConnectorXmlLinesInboundRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_XMLLinesInbound>, IEConnectorXmlLinesInboundRepository
	{
		[Inject]
		public EConnectorXmlLinesInboundRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public int GetLastPosition(int xmlNumber, string fieldName, string xmlType)
		{
			return this.Set
				.Where(x => x.NumeroXML == xmlNumber
					&& x.TipoXML.Equals(xmlType, StringComparison.OrdinalIgnoreCase))
				.Max(x => x.Posicao.Value);
		}


		public bool IsFieldNameUnique(string name)
		{
			return !(this.Set.Any(x => x.NomeCampo.Equals(name, StringComparison.OrdinalIgnoreCase)));
		}
	}
}
