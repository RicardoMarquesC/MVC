using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Repositories
{
	public class EDigitalTemplateNameRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, NomeTemplate>, IEDigitalTemplateNameRepository
	{
		[Inject]
		public EDigitalTemplateNameRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
		}

		public bool ExistsTemplateName(string nomeTemplate)
		{
			return this.Set.Any(n => n.NomeOriginal.ToLower() == nomeTemplate);
		}


		// Modificada 13-03-2015
		public List<Guid> InsertTemplateName(NomeTemplate newDBNomeTemplate)
		{
			List<Guid> insertedIds = new List<Guid>();

			List<Fornecedores> fornecedores = Context.Fornecedores.ToList();

			foreach (var forn in fornecedores)
			{
				var newId = Guid.NewGuid();

				this.Add(new NomeTemplate
				{
					pkid = newId,
					NomeTemplate1 = newDBNomeTemplate.NomeOriginal.ToLower() + "_" + forn.Contribuinte,
					TipoXML = newDBNomeTemplate.TipoXML,
					fkfornecedor = forn.pkid,
					fktipofact = newDBNomeTemplate.fktipofact,
					Masterizado = newDBNomeTemplate.Masterizado,
					NomeOriginal = newDBNomeTemplate.NomeOriginal
				});

				insertedIds.Add(newId);
			}

			this.Save();

			return insertedIds;
		}
	}
}
