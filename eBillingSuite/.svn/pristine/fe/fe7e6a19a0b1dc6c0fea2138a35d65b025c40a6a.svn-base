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
	public class ECertificatesDetailsRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_CertSignatureDetails>, IECertificatesDetailsRepository
	{
		[Inject]
		public ECertificatesDetailsRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public EBC_CertSignatureDetails GetCertSignaturaDetailsByInstanceMarket(Guid instance, Guid market)
		{	
			return 
				Set.FirstOrDefault(ecsd => ecsd.fkInstance == instance
								&&
								ecsd.fkMercado == market);
		}
	}
}
