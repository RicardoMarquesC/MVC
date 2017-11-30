using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Repositories
{
	public class EDigitalDocExpirationRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, ExpirarFactura>, IEDigitalDocExpirationRepository
	{
		[Inject]
		public EDigitalDocExpirationRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
		}

	}
}
