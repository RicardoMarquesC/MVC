using eBillingSuite.Model.Desmaterializacao;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories.eDigital
{
    public class EDigitalIntancesRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, Instances>, IEDigitalIntancesRepository
    {
        [Inject]
        public EDigitalIntancesRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
        }

        public bool VatNumberExists(string vatNumber)
        {
            return this.Set.Any(x => x.VatNumber == vatNumber);
        }
    }
}
