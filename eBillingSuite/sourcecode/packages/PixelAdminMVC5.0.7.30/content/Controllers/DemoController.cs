using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;

namespace PixelAdminMvc5.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult SignIn()
        {
			this.SetPixelAdminPageContext(CreateDemoContext());

            return View();
        }

		public ActionResult Blank()
		{
			this.SetPixelAdminPageContext(CreateDemoContext());

			return View();
		}

		public ActionResult BreadCrumbs()
		{
			this.SetPixelAdminPageContext(
				CreateDemoContext()
				.SetBreadcrumbStep("Pages")
				.SetBreadcrumbStep("Breadcrumbs"));

			return View();
		}

		public PixelAdminPageContext CreateDemoContext()
		{
			var requestContext = Request.RequestContext;

			var controller = requestContext.RouteData.Values["controller"].ToString().ToLower();
			var action = requestContext.RouteData.Values["action"].ToString().ToLower();

			var url = new UrlHelper(requestContext);

			return new PixelAdminPageContext
			{
				ApplicationName = "PixelAdmin",
				ApplicationLogo = url.Content("~/assets/pixeladmin/images/pixel-admin/main-navbar-logo.png"),

				UserName = "John Doe",
				UserAvatar = url.Content("~/assets/pixeladmin/demo/avatars/1.jpg"),

				Menu = new List<BasePixelAdminMenuEntry> { 
					new PixelAdminMenuEntry{
						Label = "Blank",
						Icon = "fa-files-o",
						Url = url.Action("Blank","Demo"),
						Active = controller == "demo" && action == "blank"						
					},
					new PixelAdminMenuEntry{
						Label = "Other pages",
						Icon = "fa-files-o",
						SubMenu = new List<BasePixelAdminMenuEntry>{
							new PixelAdminMenuEntry{
								Label = "Sign in",
								Url = url.Action("SignIn","Demo"),
								Active = false
							},
							new PixelAdminMenuEntry{
								Label = "Breadcrumbs",
								Url = url.Action("Breadcrumbs","Demo"),
								Active = controller == "demo" && action == "breadcrumbs"
							}
						}
					}
				},

				Navigation = new List<BasePixelAdminMenuEntry>{
					new PixelAdminMenuEntry{
						Label = "Home",
						Url = url.Action("","Demo"),
					},
					new PixelAdminMenuEntry{
						Label = "Dropdown",
						SubMenu = new List<BasePixelAdminMenuEntry>{
							new PixelAdminMenuEntry {
								Label = "First Item"
							},
							new PixelAdminMenuEntry {
								Label = "Second Item",
							},
							new PixelAdminMenuDivider(),
							new PixelAdminMenuEntry {
								Label = "Third Item",
							},
						}
					}
				}
			};
		}
    }
}
