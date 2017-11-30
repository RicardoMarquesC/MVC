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
	public interface IECertificatesDetailsRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_CertSignatureDetails>
	{
		EBC_CertSignatureDetails GetCertSignaturaDetailsByInstanceMarket(Guid instance, Guid market);		
	}
}
