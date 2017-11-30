using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Security;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
    public class PermissionsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, eSuite_Produtos>, IPermissionsRepository
    {
        [Inject]
        public PermissionsRepository(IeBillingSuiteConfigurationsContext context)
            : base(context)
        {
        }

        
        public List<eSuite_Produtos> GetProductsByUser(Security.IeBillingSuiteIdentity user)
        {
            return this
                .Where(p => p.utilizador == user.Name)
                .OrderByDescending(p => p.activo)
                .ToList();
        }

        public List<eSuite_Produtos> GetProductsByUser(string userName)
        {
            return this
                        .Where(p => p.utilizador == userName)
                        .OrderByDescending(p => p.activo)
                        .ToList();
        }

        public List<Permissions> GetPermissionsByUser(Security.IeBillingSuiteIdentity user)
        {
            List<Permissions> permsUser = new List<Permissions>();
            List<eSuiteUserPermissions> esuiteUserPermissions = Context.eSuiteUserPermissions.Where(esp => esp.Username == user.Name).ToList();
            foreach (eSuiteUserPermissions e in esuiteUserPermissions)
            {
                foreach (Permissions perm in Enum.GetValues(typeof(Permissions)))
                {
                    if (e.eSuitePermissions.Nome.Contains(perm.ToString()))
                        permsUser.Add(perm);
                }
            }

            return permsUser;

        }

        public List<Permissions> GetAll()
        {
            List<Permissions> permsUser = new List<Permissions>();

            foreach (Permissions perm in Enum.GetValues(typeof(Permissions)))
            {
                permsUser.Add(perm);
            }


            return permsUser;
        }

        public List<Permissions> GetPermissionsByUser(string user)
        {
            List<Permissions> permsUser = new List<Permissions>();
            List<eSuiteUserPermissions> esuiteUserPermissions = Context.eSuiteUserPermissions.Where(esp => esp.Username.Contains(user.Replace(" ", ""))).ToList();
            foreach (eSuiteUserPermissions e in esuiteUserPermissions)
            {
                foreach (Permissions perm in Enum.GetValues(typeof(Permissions)))
                {
                    if (e.eSuitePermissions.Nome.Contains(perm.ToString()))
                        permsUser.Add(perm);
                }
            }

            return permsUser;
        }
    }
}
