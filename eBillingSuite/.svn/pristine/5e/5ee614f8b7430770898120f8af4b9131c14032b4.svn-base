using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using eBillingSuite.Model.EDI_DB;

namespace eBillingSuite.Repositories
{
	public class EEDISentDocsRepository : GenericRepository<IeBillingSuiteEDIDBContext, OutboundPacket>, IEEDISentDocsRepository
	{
		[Inject]
		public EEDISentDocsRepository(IeBillingSuiteEDIDBContext context)
			: base(context)
		{
		}

		public List<OutboundPacket> GetOutboundPackets(string pesquisa)
		{
			List<OutboundPacket> newlist = this.Where(gt => gt.NumFactura.ToLower().Contains(pesquisa.ToLower())
				||
				gt.NomeEmissor.ToLower().Contains(pesquisa.ToLower())
				||
				gt.NomeReceptor.ToLower().Contains(pesquisa.ToLower())
				||
				gt.QuantiaComIVA.ToLower().Contains(pesquisa.ToLower())
				||
				gt.Estado.ToLower().Contains(pesquisa.ToLower())
				||
				gt.DataCriacao.ToString().ToLower().Contains(pesquisa.ToLower())).ToList();

			return newlist;
		}
	}
}
