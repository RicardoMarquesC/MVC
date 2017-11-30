using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class CredentialsVM
	{
		public CredentialsVM(
			LoginAT entry,
			CredentialsData dataFromPost,
			ICredentialsRepository credentialsRepository,
			IInstancesRepository instancesRepository,
            IeBillingSuiteRequestContext _context
            )
		{
			pkid = entry.pkid;
			usrat = entry.usrat;
			pwdat = entry.pwdat;
			confirmpwdat = "";
			fkEmpresa = entry.fkEmpresa;

			var values = instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
			AvailableInstances = values
				.Select(v => new SelectListItem
				{
					Text = v.Name,
					Value = v.PKID.ToString(),
					Selected = v.PKID == pkid
				})
				.ToList();
		}

		[DoNotGenerateDictionaryEntry]
		public System.Guid pkid { get; set; }
		public string usrat { get; set; }
		public string pwdat { get; set; }
		public string confirmpwdat { get; set; }
		public System.Guid fkEmpresa { get; set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }
	}
}