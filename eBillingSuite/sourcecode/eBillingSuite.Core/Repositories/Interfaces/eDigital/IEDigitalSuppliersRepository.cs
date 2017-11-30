using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Repositories
{
	public interface IEDigitalSuppliersRepository : IGenericRepository<IeBillingSuiteDesmaterializacaoContext, Fornecedores>
	{
		bool ExistsNif(string nif);
	}
}
