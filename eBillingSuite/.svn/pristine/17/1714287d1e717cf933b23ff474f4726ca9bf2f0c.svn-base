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
	public class EEDISentDocsDetailsRepository : GenericRepository<IeBillingSuiteEDIDBContext, OutboundProcesses>, IEEDISentDocsDetailsRepository
	{
		[Inject]
		public EEDISentDocsDetailsRepository(IeBillingSuiteEDIDBContext context)
			: base(context)
		{
		}

		public OutboundProcesses GetProcessByPacketID(Guid id)
		{
			return Set
				.FirstOrDefault(o => o.PKOutboundProcessID == id);
				
		}
	}
}
