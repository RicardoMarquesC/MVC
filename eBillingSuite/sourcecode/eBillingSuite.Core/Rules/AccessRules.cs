using eBillingSuite.Repositories;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Security;
using System.Collections.Generic;
using System.Linq;

namespace eBillingSuite.Rules
{
    public interface IAccessRules
    {
        bool IdentityHasAccess(IeBillingSuiteIdentity identity, int? id);
    }


    public class AccessRules : eBillingSuite.Rules.IAccessRules
    {
        private ILoginRepository _loginRepository;

        public AccessRules(
            ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public bool IdentityHasAccess(IeBillingSuiteIdentity identity, int? id)
        {
            // from this point onwards, the personID has value
            var targetPerson = _loginRepository.Find(id.Value);

            // unable to verify valid access condition
            return true;
        }
    }
}
