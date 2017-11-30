using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Repositories.Interfaces;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
    public class UnknownListRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_UnknownList>, IUnknownListRepository
    {
        [Inject]
        public UnknownListRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
        }

        public List<EBC_UnknownList> getAll()
        {
            var list = this.Set.ToList();

            return list;
        }
    }

    
}
