using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.EDI_DB;

namespace eBillingSuite.Repositories
{
	public class EEDIReceivedDocsRepository : GenericRepository<IeBillingSuiteEDIDBContext, InboundPacket>, IEEDIReceivedDocsRepository
	{
		[Inject]
		public EEDIReceivedDocsRepository(IeBillingSuiteEDIDBContext context)
			: base(context)
		{
		}

		public List<InboundPacket> GetInboundPacket()
		{
			return Set
				.OrderByDescending(i => i.DataRecepcao)
				.ToList();
		}


		public List<InboundPacket> GetInboundPacket(string pesquisa)
		{
			List<InboundPacket> newlist = this.Where(gt => gt.NumFactura.ToLower().Contains(pesquisa.ToLower())
				||
				gt.NomeEmissor.ToLower().Contains(pesquisa.ToLower())
				||
				gt.NomeReceptor.ToLower().Contains(pesquisa.ToLower())
				||
				gt.QuantiaComIVA.ToLower().Contains(pesquisa.ToLower())
				||
				gt.DataRecepcao.ToString().ToLower().Contains(pesquisa.ToLower())).ToList();

			return newlist;
		}
	}
}
