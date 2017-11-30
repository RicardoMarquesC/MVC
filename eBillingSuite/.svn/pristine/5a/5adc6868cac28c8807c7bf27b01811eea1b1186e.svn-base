using eBillingSuite.Repositories.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Security
{
    public class AspNetSecurityModule : Shortcut.Security.AspNetSecurityModule
    {
        private IKernel _kernel;
        private eBC_Login user;

        [Inject]
        public AspNetSecurityModule(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Attempts to authenticate a user by replacing the default identity with an instance of IErpPortalIdentity
        /// </summary>
        /// <param name="context"></param>
        protected override void Handle_PostAuthenticateRequest(System.Web.HttpContext context)
        {
            // try to extract the UserID from the encripted cookie
            int? userID = null;
            try
            {
                userID = base.GetUserIdUsingFormsAuthentication(context);
            }
            catch (System.Web.HttpException ex)
            {
                // ugly hack for dealing with strange machine key behavior on some hosts...
                if (!ex.StackTrace.ToString().Contains("EncryptOrDecryptData"))
                {
                    throw;
                }
            }
            catch (CryptographicException cex)
            {
                // uglier hack for dealing with strange machine key behavior on some hosts...
            }

            // if no ID there is nothing to do here
            if (!userID.HasValue)
                return;

            // find user and generate the identity
            IeBillingSuiteIdentity identity = null;

            if (userID.Value == 0)
            {
                user = new eBC_Login
                {
                    CodUtilizador = 0,
                    username = "tastas",
                    LastLogin = DateTime.Now
                };
            }
            else
            {
                user = _kernel.Get<ILoginRepository>().Find(userID.Value);
            }



            if (user != null)
                identity = new GenericBillingSuite(user);

            if (identity != null)
                HttpContext.Current.User = new GenericPrincipal(identity, null);
        }

        ///// <summary>
        ///// Adds a session cookie that enables the sign in page to bypass automatic windows authentication.
        ///// Allows a user to sign in with another account even if windows authentication is enabled
        ///// </summary>
        ///// <param name="cookieName"></param>
        //protected override void AfterSignOutUsingFormsAuthentication(string cookieName)
        //{
        //	HttpCookie bypassWindowsAuthenticationCookie = new HttpCookie("bypassWindowsAuthenticationCookie ", "");
        //	bypassWindowsAuthenticationCookie.Expires = DateTime.MinValue;

        //	HttpContext.Current.Response.Cookies.Add(bypassWindowsAuthenticationCookie);
        //}

        /// <summary>
        /// Checks if the bypass windows authentication cookie is present
        /// </summary>
        /// <returns></returns>
        public static bool BypassWindowsAuthentication()
        {
            return HttpContext.Current.Request.Cookies["bypassWindowsAuthenticationCookie"] != null;
        }
    }
}
