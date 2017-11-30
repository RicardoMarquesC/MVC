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
	public class EConnectorXmlInboundRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_XMLInbound>, IEConnectorXmlInboundRepository
	{
		[Inject]
		public EConnectorXmlInboundRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public int GetLastInboundXmlNumber()
		{
			if (this.Set.Count() <= 0)
				return 1;

			return (this.Set.Max(x=> x.NumeroXML) + 1);
		}


		public int GetXmlNumberBySenderVat(string senderNif)
		{
			var xmlNumberLine = this.Set
				.FirstOrDefault(x => x.Fornecedor.Equals(senderNif, StringComparison.OrdinalIgnoreCase));

			if (xmlNumberLine == null)
				return -1;
			else
				return xmlNumberLine.NumeroXML;
		}


		public string GetSenderXmlType(string senderNif)
		{
			var xmlNumber = this.GetXmlNumberBySenderVat(senderNif);
			if (xmlNumber != -1)
				return (this.Context.EBC_XMLHeadInbound.FirstOrDefault(x => x.NumeroXML == xmlNumber).TipoXML);
			else
				return String.Empty;
		}
	}
}
