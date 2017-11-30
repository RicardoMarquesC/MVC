using eBillingSuite.Rules;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace eBillingSuite.Security
{
	public class PersonAuthorizeAttribute : eSuiteAuthorizeAttribute
	{
		public PersonAuthorizeAttribute()
		{
		}

		public PersonAuthorizeAttribute(params Permissions[] required)
			: base(required)
		{
		}

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                var identity = httpContext.User.Identity as IeBillingSuiteIdentity;
                int? id = GetRoutePersonUserID(httpContext);


                // check if identity exists and is authenticated
                if (identity == null || !identity.IsAuthenticated)
                    return false;

                // check if identity has the required permissions
                if ((Required != null) && Required.Length > 0)
                {
                    foreach (Permissions perm in Required)
                        if (identity.Permissions.Contains(perm))
                            return true;

                    throw new HttpException((int)HttpStatusCode.Forbidden, "You are not authorized to access this page.");
                }

                //bool usesAD = bool.Parse(WebConfigurationManager.AppSettings["UsesAD"].ToString());

                //if (!usesAD)
                //{
                   


                //    if (ServiceLocator.Current.GetInstance<IKernel>().Get<IAccessRules>().IdentityHasAccess(identity, id))
                //        return true;

                //    throw new HttpException((int)HttpStatusCode.Forbidden, "You are not authorized to access this page.");
                //}
                //else
                //{
                //    return true;
                //}
            }

            return false;
        }
    }
}
