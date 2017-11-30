using eBillingSuite.Model.eBillingConfigurations;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories.Interfaces
{
    public interface ILoginRepository : IGenericRepository<IeBillingSuiteConfigurationsContext, eBC_Login>
    {
        eBC_Login getUserById(int CodUtilizador);
    }
}
