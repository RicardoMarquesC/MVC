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
	public class EBCCustomersXMLConfigsVM
	{
		public List<EBC_Customers> customers;
		private List<EBC_Instances> instances;
		private IECCListRepositories _eCConfigRepositories;
		private IeBillingSuiteRequestContext _context;

		public List<EBC_XML> xmlheaders;
		public List<EBC_XMLLines> xmllines;
		public List<EBC_XMLResumoIVA> xmliva;
		public List<xmlTemplate> dataFields;
		private string tipoxml;
		public int numeroxml;		
		public System.Guid FKInstanceID { get; set; }
		public System.Guid FKCustomerID { get; set; }
		public string tipoXml { get; set; }
		public bool mandatory { get; set; }
		public string selectedField { get; set; }
		public bool isEdit { get; set; }

		public EBCCustomersXMLConfigsVM(List<EBC_Instances> model,
			IECCListRepositories _eBCConfigurationsRepository,			 
			Guid instance,
			Guid customer,
			string tipoxml)
		{
			this.FKInstanceID = instance;
			this.FKCustomerID = customer;
			this.instances = model;
			this.tipoXml = tipoxml;
			this._eCConfigRepositories = _eBCConfigurationsRepository;
			
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

			List<TiposXML> tx = new List<TiposXML>();			
			TiposXML tx1 = new TiposXML { id = 1, valor = eBillingSuite.Enumerations.XmlTypes.UBL };
			TiposXML tx2 = new TiposXML { id = 2, valor = eBillingSuite.Enumerations.XmlTypes.BASIC };
			TiposXML tx3 = new TiposXML { id = 3, valor = eBillingSuite.Enumerations.XmlTypes.FACTURA_E };
			TiposXML tx4 = new TiposXML { id = 4, valor = eBillingSuite.Enumerations.XmlTypes.PHC };
			tx.Add(tx1); tx.Add(tx2); tx.Add(tx3); tx.Add(tx4);

			

			if(customers.Count>0)
			{
				if(String.IsNullOrWhiteSpace(this.tipoXml))
					this.tipoXml = _eCConfigRepositories.eConnectorXmlClientRepository.GetXmlTypeByFKClient(this.FKCustomerID == null ? customers[0].PKID : this.FKCustomerID);
				
				if(this.FKCustomerID == null)
					this.FKCustomerID = customers[0].PKID;
				
				AvailableCustomers = customers
					.Select(v => new SelectListItem
					{
						Text = v.Name,
						Value = v.PKID.ToString(),
						Selected = v.PKID == this.FKCustomerID
					})
				   .ToList();
				
				AvailableTypes = tx
				   .Select(v => new SelectListItem
				   {
					   Text = v.valor,
					   Value = v.valor,
					   Selected = v.valor == tipoXml
				   })
				   .ToList();

				var xmlnumber = _eCConfigRepositories.eConnectorXmlClientRepository.GetXMLClientNumberByFKClient(this.FKCustomerID, this.tipoXml);
				xmlheaders = _eCConfigRepositories.eConnectorXmlHeaderRepository.Where(exh => exh.NumeroXML == xmlnumber).ToList();
				xmllines = _eCConfigRepositories.eConnectorXmlLinesRepository.Where(exh => exh.NumeroXML == xmlnumber).ToList();
				xmliva = _eCConfigRepositories.eConnectorXmlResumoIvaRepository.Where(exh => exh.NumeroXML == xmlnumber).ToList();
			}
			else
			{
				AvailableTypes = tx
				.Select(v => new SelectListItem
				{
					Text = v.valor,
					Value = v.valor,					
				})
				.ToList();
			}
			

		}

		public EBCCustomersXMLConfigsVM(IECCListRepositories _eCConfigRepositories, Guid id, Guid idcustomer, string tipoxml, int numeroxml)
		{
			this._eCConfigRepositories = _eCConfigRepositories;
			this.FKInstanceID = id;
			this.FKCustomerID = idcustomer;
			this.tipoxml = tipoxml;
			this.numeroxml = numeroxml;
			this.mandatory = false;

			List<TiposXML> tx = new List<TiposXML>();
			TiposXML tx1 = new TiposXML { id = 1, valor = eBillingSuite.Enumerations.DigitalDocumentAreas.HEADER };
			TiposXML tx2 = new TiposXML { id = 1, valor = eBillingSuite.Enumerations.DigitalDocumentAreas.LINES };
			TiposXML tx3 = new TiposXML { id = 1, valor = eBillingSuite.Enumerations.DigitalDocumentAreas.VAT };
			tx.Add(tx1); tx.Add(tx2); tx.Add(tx3);

			dataFields = _eCConfigRepositories.eConnectorXmlTemplateRepository.Where(exh => exh.TipoXML == tipoxml).ToList();
			this.selectedField = dataFields[0].NomeCampo;

			AvailableFields = dataFields
				   .Select(v => new SelectListItem
				   {
					   Text = v.NomeCampo + " - " + v.Tipo,
					   Value = v.NomeCampo + " - " + v.Tipo,
					   Selected = v.NomeCampo == selectedField
				   })
				   .ToList();			
		}

		public EBCCustomersXMLConfigsVM(IECCListRepositories _eCConfigRepositories, Guid id, Guid idcustomer, string tipoxml, int numeroxml, string nomecampo, bool isEdit)
		{
			this._eCConfigRepositories = _eCConfigRepositories;
			this.FKInstanceID = id;
			this.FKCustomerID = idcustomer;
			this.tipoxml = tipoxml;
			this.numeroxml = numeroxml;
			this.isEdit = isEdit;

			List<TiposXML> tx = new List<TiposXML>();
			TiposXML tx1 = new TiposXML { id = 1, valor = eBillingSuite.Enumerations.DigitalDocumentAreas.HEADER };
			TiposXML tx2 = new TiposXML { id = 1, valor = eBillingSuite.Enumerations.DigitalDocumentAreas.LINES };
			TiposXML tx3 = new TiposXML { id = 1, valor = eBillingSuite.Enumerations.DigitalDocumentAreas.VAT };
			tx.Add(tx1); tx.Add(tx2); tx.Add(tx3);

			dataFields = _eCConfigRepositories.eConnectorXmlTemplateRepository.Where(exh => exh.TipoXML == tipoxml).ToList();
			this.selectedField = nomecampo;

			var exists = _eCConfigRepositories.eConnectorXmlHeaderRepository.Set.Any(exh => exh.NomeCampo == nomecampo && exh.NumeroXML == numeroxml);
			if(!exists)
			{
				exists = _eCConfigRepositories.eConnectorXmlLinesRepository.Set.Any(exh => exh.NomeCampo == nomecampo && exh.NumeroXML == numeroxml);
				if(!exists)
					this.mandatory = _eCConfigRepositories.eConnectorXmlResumoIvaRepository.Where(exh => exh.NomeCampo == nomecampo && exh.NumeroXML == numeroxml).FirstOrDefault().Obrigatorio.Value;
				else
					this.mandatory = _eCConfigRepositories.eConnectorXmlLinesRepository.Where(exh => exh.NomeCampo == nomecampo && exh.NumeroXML == numeroxml).FirstOrDefault().Obrigatorio.Value;
			}
			else
				this.mandatory = _eCConfigRepositories.eConnectorXmlHeaderRepository.Where(exh => exh.NomeCampo == nomecampo && exh.NumeroXML == numeroxml).FirstOrDefault().Obrigatorio.Value;
			

			AvailableFields = dataFields
				   .Select(v => new SelectListItem
				   {
					   Text = v.NomeCampo + " - " + v.Tipo,
					   Value = v.NomeCampo + " - " + v.Tipo,
					   Selected = v.NomeCampo == selectedField
				   })
				   .ToList();			
		}

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableCustomers { get; private set; }
		
		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableTypes { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableHeaders { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableLines { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableIva { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableFields { get; private set; }

		public class TiposXML
		{
			public int id { get; set; }
			public string valor { get; set; }
		}

	}
}