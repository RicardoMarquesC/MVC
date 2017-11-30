using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using System.Security.Cryptography.X509Certificates;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public interface IConnectorConfigInboundTXTRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_XmlToTxtTransform>
	{
		List<EBC_XmlToTxtTransform> GetConfigTXTbyInstanceID(Guid id);

		int GetMaxPositionFromType(string nomecampo);
	}
}
