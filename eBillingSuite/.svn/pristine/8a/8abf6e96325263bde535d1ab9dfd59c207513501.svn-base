using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.Controllers
{
    public class eConnectorController : Controller
    {
		private IPixelAdminPageContext _pixelAdminPageContext;

		public eConnectorController(IPixelAdminPageContext pixelAdminPageContext)
		{
			_pixelAdminPageContext = pixelAdminPageContext;
		}

		public ActionResult InactiveModule()
		{
			this.SetPixelAdminPageContext(_pixelAdminPageContext);

			//string inactiveModuleText = _context.GetDictionaryValue(DictionaryEntryKeys.InactiveModuleAlertText);

			return View();
		}
    }
}