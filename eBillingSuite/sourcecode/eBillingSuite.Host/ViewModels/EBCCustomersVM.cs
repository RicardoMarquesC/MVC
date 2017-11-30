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
	public class EBCCustomersVM
	{
		public List<EBC_Customers> customers;
		private List<EBC_Instances> instances;
		private IECCListRepositories _eCConfigRepositories;
		private IeBillingSuiteRequestContext _context;

		public EBCCustomersVM(List<EBC_Instances> model,
			IECCListRepositories _eBCConfigurationsRepository,			 
			Guid instance)
		{
			this.FKInstanceID = instance;
			this.instances = model;
			
			AvailableInstances = model
				.Select(v => new SelectListItem
				{
					Text = v.Name,
					Value = v.PKID.ToString(),
					Selected = v.PKID == this.FKInstanceID
				})
				.ToList();

			customers = _eBCConfigurationsRepository.eConnectorCustomersRepository
				.Where(ec => ec.FKInstanceID == instance)
				.ToList();		
		}

		public EBCCustomersVM(EBC_Customers model, IECCListRepositories _eCConfigRepositories, IeBillingSuiteRequestContext _context, Guid instance, Guid pkid, List<EBC_Mercados> markets)
		{
			this._eCConfigRepositories = _eCConfigRepositories;
			this._context = _context;
			this.FKInstanceID = instance;
			this.PKID = pkid;
			this.Name = model.Name;
			this.Email = model.Email;
			this.FKEmailContentID = model.FKEmailContentID;
			this.FKSpecificDeliveryOptionsID = model.FKSpecificDeliveryOptionsID;
			this.NIF = model.NIF;
			this.Mercado = model.Mercado;
			this.XMLAss = model.XMLAss;
			this.XMLNAss = model.XMLNAss;
			this.PDFAss = model.PDFAss;
			this.PDFNAss = model.PDFNAss;

			AvailableMarkets = markets
				.Select(v => new SelectListItem
				{
					Text = v.Mercado,
					Value = v.Mercado.ToString(),
					Selected = v.Mercado == model.Mercado
				})
				.ToList();
		}

		
		[DoNotGenerateDictionaryEntry]
		public System.Guid PKID { get; set; }
		public System.Guid FKInstanceID { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public System.Guid FKEmailContentID { get; set; }
		public System.Guid FKSpecificDeliveryOptionsID { get; set; }
		public string NIF { get; set; }
		public string Mercado { get; set; }
		public Nullable<bool> XMLAss { get; set; }
		public Nullable<bool> XMLNAss { get; set; }
		public Nullable<bool> PDFAss { get; set; }
		public Nullable<bool> PDFNAss { get; set; }

		
		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableMarkets { get; private set; }

	}
}