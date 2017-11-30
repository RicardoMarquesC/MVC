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
	public class CredentialsRepository : GenericRepository<IeBillingSuiteEBCDBContext, LoginAT>, ICredentialsRepository
	{
		[Inject]
		public CredentialsRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public LoginAT GetLoginATByID(Guid? ID)
		{
			if (!ID.HasValue)
				return Set.FirstOrDefault();
			else
				return
					this.Where(la => la.fkEmpresa == ID)
					.FirstOrDefault();
					
					
		}
	}
}
