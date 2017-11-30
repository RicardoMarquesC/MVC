using eBillingSuite.Model.EBC_DB;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories.Interfaces
{
    public interface IUnknownListRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_UnknownList>
    {
        List<EBC_UnknownList> getAll();
    }
}
