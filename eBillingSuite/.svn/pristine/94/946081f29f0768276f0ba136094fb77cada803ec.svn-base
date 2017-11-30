using eBillingSuite.Model.Desmaterializacao;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
    public class EDigitalMasterizationProcRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, ProcDocs>, IEDigitalMasterizationProcRepository
    {
        [Inject]
        public EDigitalMasterizationProcRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
        }
    }
}
