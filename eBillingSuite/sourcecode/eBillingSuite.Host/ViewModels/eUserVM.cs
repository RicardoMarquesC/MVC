using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Security;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;

namespace eBillingSuite.ViewModels
{
	public class eUserVM
	{
		private string user;
		private IPermissionsRepository _permissionsRepository;
        private ILoginRepository _loginRepository;


        public eUserVM()
		{
			GetUsers();			
		}

        public eUserVM(bool aux, ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
            if (aux)
            {
                GetUsersbyLogin();
            }
        }

        public eUserVM(bool aux, ILoginRepository loginRepository, UserData user)
        {
            _loginRepository = loginRepository;
            if (aux)
            {
                Utilizador = user;         
            }
        }

        private void GetUsersbyLogin()
        {
            UsersbyLogin = new List<eBC_Login>();
            UsersbyLogin = _loginRepository.Set.ToList();

        }

        public eUserVM(string user, IPermissionsRepository _permissionsRepository)
		{
			AvailablePermissions = new List<Permissions>();
			PermissionsUser = new List<Permissions>();
			// TODO: Complete member initialization
			this.user = user;
			this._permissionsRepository = _permissionsRepository;

			foreach (Permissions val in Enum.GetValues(typeof(Permissions)))
			{
				AvailablePermissions.Add(val);
			}

			//GET THE USER PERMISSIONS
			PermissionsUser = _permissionsRepository.GetPermissionsByUser(user);

		}

        public UserData Utilizador { get; set; }

		public List<UsersDomains> Users { get; set; }
        public List<eBC_Login> UsersbyLogin { get; set; }

		public List<Permissions> PermissionsUser { get; set; }

		public List<Permissions> AvailablePermissions { get; set; }

		private void GetUsers()
		{
			Users = new List<UsersDomains>();
			using (var forest = Forest.GetCurrentForest())
			{
				foreach (Domain d in forest.Domains)
				{
					// create your domain context and define the OU container to search in
					PrincipalContext ctx = new PrincipalContext(ContextType.Domain, d.Name);
					// define a "query-by-example" principal - here, we search for a UserPrincipal (user)
					UserPrincipal qbeUser = new UserPrincipal(ctx);
					// create your principal searcher passing in the QBE principal    
					PrincipalSearcher srch = new PrincipalSearcher(qbeUser);
					// find all matches
					foreach (var found in srch.FindAll())
					{											
						if (found.DisplayName != null)
						{
							UsersDomains ud = new UsersDomains();
							ud.User = found.DisplayName;
							ud.Domain = d.Name;
							Users.Add(ud);
						}							
					}
				}
			}
		}

		public class UsersDomains
		{
			public string User { get; set; }
			public string Domain { get; set; }
		}

	}
}
