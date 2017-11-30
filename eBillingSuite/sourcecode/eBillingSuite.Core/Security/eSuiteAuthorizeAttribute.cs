using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eBillingSuite.Security
{
	public class eSuiteAuthorizeAttribute : AuthorizeAttribute
	{
		public Permissions[] Required { get; set; }

		public eSuiteAuthorizeAttribute()
		{
		}

		public eSuiteAuthorizeAttribute(params Permissions[] required)
		{
			Required = required;
		}

		protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
			var identity = httpContext.User.Identity as IeBillingSuiteIdentity;

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

			return base.AuthorizeCore(httpContext);
		}

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new
                {
                    action = "Index",
                    controller = "SignIn",
                    redirectUrl = filterContext.RequestContext.HttpContext.Request.RawUrl
                }));
        }

        protected int? GetRouteID(System.Web.HttpContextBase httpContext)
        {
            int tmp = 0;
            if (int.TryParse(httpContext.Request.RequestContext.RouteData.Values["id"] as string, out tmp))
                return tmp;

            return null;
        }

        protected int? GetRoutePersonUserID(System.Web.HttpContextBase httpContext)
        {
            int tmp = 0;
            if (int.TryParse(httpContext.Request.RequestContext.RouteData.Values["personUserID"] as string, out tmp))
                return tmp;

            return null;
        }
    }
}
