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
	public interface IEConnectorXmlTemplateRepository : IGenericRepository<IeBillingSuiteEBCDBContext, xmlTemplate>
	{
		List<xmlTemplate> GetDataByType(string type);

		List<xmlTemplate> GetAllXmlDataByType(string type);

		int GetCustomXmlCount();

		void InsertXmlField(string xmlPath, string fieldName, string docArea, string xmlTypeName, bool isATField, int customCount);

		List<IGrouping<string, xmlTemplate>> GetExistingXmlTypes();
		List<string> GetExistingXmlFields(IGrouping<string, xmlTemplate> grouping);
	}
}
