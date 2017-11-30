using eBillingSuite.Enumerations;
using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EBCSendersVM
	{
		private IECCListRepositories _eCConfigRepositories;
		//private IeBillingSuiteRequestContext _context;

		public EBCSendersVM(Whitelist model, IECCListRepositories _eBCConfigurationsRepository)
		{
			AvailableIntegrationMethods = _eBCConfigurationsRepository
				.eConnectorIntegrationFiltersRepository
				.Set
				.Select(v => new SelectListItem
				{
					Text = v.FriendlyName,
					Value = v.PKIntegrationFilterID.ToString(),
					Selected = v.FriendlyName.ToLower() == IntegrationFiltersName.DEFAULT.ToLower()
				})
				.ToList();

			AvailableMarkets = _eBCConfigurationsRepository
				.eBCMarketsRepository
				.Set
				.Select(v => new SelectListItem
				{
					Text = v.Mercado,
					Value = v.Mercado,
					Selected = v.Mercado == "Portugal"
				})
				.ToList();

			this.Pkid = model.PKWhitelistID;
			this.EmailName = model.EmailName;
			this.EmailAddress = model.EmailAddress;
			this.Nif = model.NIF;
			this.Enabled = model.Enabled;
			this.ConcatAnexos = model.ConcatAnexos;
			this.XMLAss = model.XMLAss.Value;
			this.XMLNAss = model.XMLNAss.Value;
			this.PDFAss = model.PDFAss.Value;
			this.PDFNAss = model.PDFNAss.Value;
			this.PdfLink = model.PdfLink.Value;

			if (String.IsNullOrWhiteSpace(this.Nif))
				this.IsFromCreate = true;
			else
				this.IsFromCreate = false;
		}

		#region Properties
		public Guid Pkid { get; set; }

		public string EmailName { get; set; }
		public string EmailAddress { get; set; }
		public string Nif { get; set; }

		public IEnumerable<SelectListItem> AvailableIntegrationMethods { get; private set; }
		public Guid FkIntegrationMethod { get; set; }

		public bool Enabled { get; set; }
		public bool ConcatAnexos { get; set; }

		public IEnumerable<SelectListItem> AvailableMarkets { get; private set; }
		public string Mercado { get; set; }

		public bool XMLAss { get; set; }
		public bool XMLNAss { get; set; }
		public bool PDFAss { get; set; }
		public bool PDFNAss { get; set; }

		public bool PdfLink { get; set; }

		public bool IsFromCreate { get; set; }
		#endregion
	}
}