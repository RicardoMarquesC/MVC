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
	public class EDigitalSuppliersRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, Fornecedores>, IEDigitalSuppliersRepository
	{
		[Inject]
		public EDigitalSuppliersRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
		}

		public bool ExistsNif(string nif)
		{
			return Set.Any(s => s.Contribuinte.ToLower().Replace(" ", "") == nif.ToLower().Replace(" ", ""));
		}
	}
}
