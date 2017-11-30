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
	public class EDigitalMasterizationLinesRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, MasterizacaoLineItems>, IEDigitalMasterizationLinesRepository
	{
		[Inject]
		public EDigitalMasterizationLinesRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
		}
        public void DeleteMasterization(Guid fkNomeTemplate, string fieldName)
        {
            var entry = this.Set.FirstOrDefault(x => x.FKNomeTemplate == fkNomeTemplate && x.NomeCampo == fieldName);

            if (entry != null)
            {
                this.Set.Remove(entry);
                this.Save();
            }
        }
    }
}
