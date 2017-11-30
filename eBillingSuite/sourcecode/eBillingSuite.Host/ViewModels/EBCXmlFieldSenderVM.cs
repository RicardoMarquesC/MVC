using eBillingSuite.Enumerations;
using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EBCXmlFieldSenderVM
	{
		public EBCXmlFieldSenderVM()
		{
		}

		public EBCXmlFieldSenderVM(IECCListRepositories eCCListRepositories, Guid senderId, string xmlType, string xmlNumber)
		{
			this.SenderId = senderId;
			this.XmlType = xmlType;
			this.XmlNumber = xmlNumber;

			// get availbale XML fields areas
			List<string> areas = new List<string> { "", DigitalDocumentAreas.HEADER, DigitalDocumentAreas.LINES, DigitalDocumentAreas.VAT };
			AvailableAreas = areas
				.Select(a => new SelectListItem
				{
					Text = a,
					Value = a,
					Selected = a == ""
				})
				.ToList();

			// get available XML fields
			List<xmlTemplate> xmlFields = eCCListRepositories.eConnectorXmlTemplateRepository.Set
				.Where(xf => xf.TipoXML.Equals(xmlType, StringComparison.OrdinalIgnoreCase))
				.OrderBy(xf => xf.NomeCampo)
				.ToList();

			AvailableHeaders = xmlFields
				.Where(xf => xf.Tipo.Equals(DigitalDocumentAreas.HEADER, StringComparison.OrdinalIgnoreCase))
				.ToList()
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == ""
				})
				.ToList();

			AvailableLines = xmlFields
				.Where(xf => xf.Tipo.Equals(DigitalDocumentAreas.LINES, StringComparison.OrdinalIgnoreCase))
				.ToList()
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == ""
				})
				.ToList();

			AvailableIva = xmlFields
				.Where(xf => xf.Tipo.Equals(DigitalDocumentAreas.VAT, StringComparison.OrdinalIgnoreCase))
				.ToList()
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == ""
				})
				.ToList();
		}

		public EBCXmlFieldSenderVM(XmlSenderData data, IECCListRepositories eCCListRepositories)
		{
			this.SenderId = data.SenderId;
			this.XmlType = data.XmlType;
			this.XmlNumber = data.XmlNumber;

			// get availbale XML fields areas
			List<string> areas = new List<string> { "", DigitalDocumentAreas.HEADER, DigitalDocumentAreas.LINES, DigitalDocumentAreas.VAT };
			AvailableAreas = areas
				.Select(a => new SelectListItem
				{
					Text = a,
					Value = a,
					Selected = a == data.Area
				})
				.ToList();

			// get available XML fields
			List<xmlTemplate> xmlFields = eCCListRepositories.eConnectorXmlTemplateRepository.Set
				.Where(xf => xf.TipoXML.Equals(XmlType, StringComparison.OrdinalIgnoreCase))
				.ToList();

			AvailableHeaders = xmlFields
				.Where(xf => xf.Tipo.Equals(DigitalDocumentAreas.HEADER, StringComparison.OrdinalIgnoreCase))
				.ToList()
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == data.NomeCampo
				})
				.ToList();

			AvailableLines = xmlFields
				.Where(xf => xf.Tipo.Equals(DigitalDocumentAreas.LINES, StringComparison.OrdinalIgnoreCase))
				.ToList()
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == data.NomeCampo
				})
				.ToList();

			AvailableIva = xmlFields
				.Where(xf => xf.Tipo.Equals(DigitalDocumentAreas.VAT, StringComparison.OrdinalIgnoreCase))
				.ToList()
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == data.NomeCampo
				})
				.ToList();
		}

		#region Propreties

		public Guid Pkid { get; set; } // used only to Edit field

		public Guid SenderId { get; set; }
		public string XmlType { get; set; }
		public string XmlNumber { get; set; }

		public IEnumerable<SelectListItem> AvailableAreas { get; private set; }
		public string Area { get; set; }

		public IEnumerable<SelectListItem> AvailableHeaders { get; private set; }
		public IEnumerable<SelectListItem> AvailableLines { get; private set; }
		public IEnumerable<SelectListItem> AvailableIva { get; private set; }
		public string NomeCampo { get; set; }

		public bool IsRequired { get; set; }

		#endregion
	}
}