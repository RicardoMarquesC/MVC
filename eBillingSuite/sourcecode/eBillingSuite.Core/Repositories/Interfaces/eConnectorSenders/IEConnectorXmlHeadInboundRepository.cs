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
	public interface IEConnectorXmlHeadInboundRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_XMLHeadInbound>
	{
		string GetXmlTypeByXmlNumber(int xmlNumber);

		int GetLastPosition(int xmlNumber, string fieldName, string xmlType);

		bool IsFieldNameUnique(string name);
	}
}
