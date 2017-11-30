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
	public class EConnectorXmlHeadInboundRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_XMLHeadInbound>, IEConnectorXmlHeadInboundRepository
	{
		[Inject]
		public EConnectorXmlHeadInboundRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public string GetXmlTypeByXmlNumber(int xmlNumber)
		{
			// check in the header
			var header = this.Set.FirstOrDefault(x => x.NumeroXML == xmlNumber);
			if (header != null)
				return header.TipoXML;

			// check in the lines
			var line = this.Context.EBC_XMLLinesInbound.FirstOrDefault(x => x.NumeroXML == xmlNumber);
			if (line != null)
				return line.TipoXML;

			// check in the vat line
			var vat = this.Context.EBC_XMLResumoIVAInbound.FirstOrDefault(x => x.NumeroXML == xmlNumber);
			if (vat != null)
				return vat.TipoXML;

			return null;
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
