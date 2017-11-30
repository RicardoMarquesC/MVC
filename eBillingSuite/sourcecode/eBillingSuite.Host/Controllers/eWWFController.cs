using eBillingSuite.Globalization;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using eBillingSuite.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;
using FluentValidation;
using System.Text.RegularExpressions;
using System.Xml;
using Sgml;
using System.Text;
using eBillingSuite.Security;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace eBillingSuite.Controllers
{
    public class eWWFController : Controller
    {
		private static List<string> allowed = new List<string> { ".cer", ".pfx", ".p7b" };
		private const string emailregex = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
		
		private IPixelAdminPageContext _pixelAdminPageContext;
		protected readonly IeBillingSuiteRequestContext _context;
		private IECCListRepositories _eCConfigRepositories;
		private IPIInfoConfigurationsRepository _pIInfoConfigurationsRepository;

		public eWWFController(
			IPixelAdminPageContext pixelAdminPageContext,			
			IECCListRepositories eCConfigRepositories,			
			IPIInfoConfigurationsRepository pIInfoConfigurationsRepository,
			IeBillingSuiteRequestContext context
		)
		{
			_pixelAdminPageContext = pixelAdminPageContext;
			_eCConfigRepositories = eCConfigRepositories;
			_context = context;
			_pIInfoConfigurationsRepository = pIInfoConfigurationsRepository;

		}

		#region WFDashboard

		public ActionResult Index()
        {
			this.SetPixelAdminPageContext(_pixelAdminPageContext);

			return View();
        }

		public ActionResult Details()
		{
			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(null), JsonRequestBehavior.AllowGet);
			else
				return PartialView();
		}

		#endregion

		#region WFConfigurations
		public ActionResult Configurations()
		{
			this.SetPixelAdminPageContext(_pixelAdminPageContext);

			return View();
		}

		public ActionResult Aprovacoes()
		{
			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(null), JsonRequestBehavior.AllowGet);
			else
				return PartialView();
		}

		#endregion
	}
}