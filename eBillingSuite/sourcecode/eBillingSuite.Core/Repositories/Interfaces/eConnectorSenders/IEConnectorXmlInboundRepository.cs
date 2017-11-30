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
	public interface IEConnectorXmlInboundRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_XMLInbound>
	{
		int GetLastInboundXmlNumber();

		int GetXmlNumberBySenderVat(string senderNif);

		string GetSenderXmlType(string senderNif);
	}
}
