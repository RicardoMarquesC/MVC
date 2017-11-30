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
	public class EDigitalDocTypeRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, TipoFacturas>, IEDigitalDocTypeRepository
	{
		[Inject]
		public EDigitalDocTypeRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
		}

		// Retorna a lista de tipo de documento que tenham template XML definido
		public List<TipoFacturas> GetTypesWithTemplate()
		{
			return this
				.Where(t => this.Context.TipoFacturaDadosXML.Any(d => d.fkTipoFactura == t.pkid))
				.ToList();
		}

		public bool ExistsName(string nome)
		{
			return Set.Any(s => s.nome.ToLower() == nome);
		}
	}
}
