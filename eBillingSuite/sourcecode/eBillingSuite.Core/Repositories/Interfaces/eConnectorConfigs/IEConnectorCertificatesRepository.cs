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
	public interface IECertificatesRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_Config>
	{
		List<EBC_Config> GetConfigsByID(Guid ID);
		List<X509Certificate2> GetAllAvailableCertificates();

		
	}
}
