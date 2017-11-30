using eBillingSuite.Model.eBillingConfigurations;
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
    public class LoginRepository : GenericRepository<IeBillingSuiteConfigurationsContext, eBC_Login>, ILoginRepository
    {
        [Inject]
        public LoginRepository(IeBillingSuiteConfigurationsContext context)
                : base(context)
        {
        }

        public eBC_Login getUserById(int CodUtilizador)
        {
            return Set.FirstOrDefault(l => l.CodUtilizador == CodUtilizador);
        }
    }
}
