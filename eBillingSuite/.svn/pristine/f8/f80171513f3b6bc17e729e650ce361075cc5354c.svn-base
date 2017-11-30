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
	public class EConnectorTipoNomenclaturaPDFRepository : GenericRepository<IeBillingSuiteCICDBContext, TiposNomenclaturaPDF>, IEConnectorTipoNomenclaturaPDFRepository
	{
		[Inject]
		public EConnectorTipoNomenclaturaPDFRepository(IeBillingSuiteCICDBContext context)
			: base(context)
		{
		}
		
		public List<TiposNomenclaturaPDF> GetAvailableTiposNomenclaturaPDF()
		{
			return Set.ToList();
		}
	}
}
