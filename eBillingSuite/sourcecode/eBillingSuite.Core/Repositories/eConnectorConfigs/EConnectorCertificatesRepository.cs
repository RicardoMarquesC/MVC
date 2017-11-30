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
	public class EConnectorCertificatesRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_Config>, IECertificatesRepository
	{
		[Inject]
		public EConnectorCertificatesRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public List<EBC_Config> GetConfigsByID(Guid ID)
		{
			return this.Where(ebcc => ebcc.FKInstanceID == ID
				&& ebcc.ConfigSuiteType == "ebcCerts")
				.OrderBy(ebcc => ebcc.Position)
				.ToList();
		}

		public List<X509Certificate2> GetAllAvailableCertificates()
		{
			List<X509Certificate2> certificates = new List<X509Certificate2>();

			X509Store store = new X509Store(StoreName.Root,
                StoreLocation.LocalMachine);
			store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

			X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
			foreach (X509Certificate2 x509 in collection)
				certificates.Add(x509);
			
			return certificates;
		}
	}
}
