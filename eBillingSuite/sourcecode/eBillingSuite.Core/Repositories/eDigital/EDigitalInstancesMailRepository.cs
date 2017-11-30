using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Repositories.Interfaces.eDigital;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories.eDigital
{
    public class EDigitalInstancesMailRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, InstancesMail>, IEDigitalInstancesMailRepository
    {
        [Inject]
        public EDigitalInstancesMailRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
        }

        public List<InstancesMail> GetAllSenders()
        {
            var senders = this.Set.ToList();

            return senders;
        }
    }
}
