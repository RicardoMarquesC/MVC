using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace eBillingSuite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            bool usesAD = bool.Parse(WebConfigurationManager.AppSettings["UsesAD"].ToString());

            if(usesAD)
            {
                routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{ID}",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
            }
            else
            {
                routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{ID}",
                defaults: new { controller = "SignIn", action = "Index", id = UrlParameter.Optional }
            );
            }

            

            //routes.MapRoute(
            // name: "Default",
            // url: "{controller}/{action}/{ID}",
            // defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //	name: "Enterprise SignIn Route",
            //	url: "{enterprise}/SignIn",
            //	defaults: new { controller = "Account", action = "SignIn" }
            //);

            //routes.MapRoute(
            //	name: "Enterprise SignOut Route",
            //	url: "{enterprise}/SignOut",
            //	defaults: new { controller = "Account", action = "SignOut" }
            //);

            //routes.MapRoute(
            //	name: "Enterprise Person Route",
            //	url: "{enterprise}/Person/{personUserID}/{controller}/{action}/{id}",
            //	defaults: new { controller = "PersonDetails", action = "Index", id = UrlParameter.Optional },
            //	constraints: new { personUserID = @"\d+" }
            //);

            //routes.MapRoute(
            //	name: "Enterprise Default",
            //	url: "{enterprise}/{controller}/{action}/{id}",
            //	defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}