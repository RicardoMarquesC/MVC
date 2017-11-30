using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;


namespace eBillingSuite.Controllers
{
	public class SerialCheckerController : Controller
	{
		public string isSerialValid(string serial, string produto, string macaddress)
		{
			try
			{
				string empresa = WebConfigurationManager.AppSettings["NomeEmpresa"];
				//WSLicensing.Licensing service = new WSLicensing.Licensing();
				Licensing service = new Licensing();
				string result = service.SerialChecker(serial, empresa, produto, macaddress);

				return result;
			}
			catch (Exception e)
			{
				return ("false, " + e.Message);
			}
		}

	}
}