using eBillingSuite.Model.EBC_DB;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories.Support
{
    public class TicketsRepository : GenericRepository<IeBillingSuiteEBCDBContext, Tickets>, ITicketsRepository
    {
        [Inject]
        public TicketsRepository(IeBillingSuiteEBCDBContext context) 
            : base(context)
        {
        }
    }

    public interface ITicketsRepository : IGenericRepository<IeBillingSuiteEBCDBContext, Tickets>
    {

    }
}
