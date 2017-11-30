using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.CIC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorTipoNomenclaturaSenderRepository : GenericRepository<IeBillingSuiteCICDBContext, TipoNomenclaturaSender>, IEConnectorTipoNomenclaturaSenderRepository
	{
		[Inject]
		public EConnectorTipoNomenclaturaSenderRepository(IeBillingSuiteCICDBContext context)
			: base(context)
		{
		}

		public TipoNomenclaturaSender GetBySenderID(Guid ID)
		{
			bool exists = Set.Any(tns => tns.FKRemetente == ID);
			if (exists)
				return Set.FirstOrDefault(tns => tns.FKRemetente == ID);
			else
				return null;
		}
	}
}
