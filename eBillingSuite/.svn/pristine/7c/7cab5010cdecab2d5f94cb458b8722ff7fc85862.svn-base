using eBillingSuite.Enumerations;
using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.CIC_DB;
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
	public class EBCSenderXmlMappingVM
	{
		public EBCSenderXmlMappingVM(List<EBC_Instances> instances, List<Whitelist> senders,
			List<IGrouping<string, xmlTemplate>> xmls,
			Guid? selectedInstance, Guid? selectedSender,
			string selectedXmlToMap, string selectedXmlBase,
			List<xmlTemplate> xmlBaseFields, List<xmlTemplate> xmlToMapFields)
		{
			// check for nulls
			if (!selectedInstance.HasValue)
				selectedInstance = Guid.Empty;

			if (!selectedSender.HasValue)
				selectedSender = Guid.Empty;

			if (selectedXmlToMap == null)
				selectedXmlToMap = "";

			if (selectedXmlBase == null)
				selectedXmlBase = "";

			this.XmlToMap = selectedXmlToMap;
			this.XmlBase = selectedXmlBase;

			this.FkInstance = selectedInstance.Value;
			this.FkSender = selectedSender.Value;

			// instances
			instances.Insert(0, new EBC_Instances { Name = "", PKID = Guid.Empty });
			AvailableInstances = instances
				.Select(a => new SelectListItem
				{
					Text = a.Name,
					Value = a.PKID.ToString(),
					Selected = a.PKID == selectedInstance.Value
				})
				.ToList();

			// senders
			senders.Insert(0, new Whitelist { EmailName = "", PKWhitelistID = Guid.Empty });
			AvailableSenders = senders
				.Select(a => new SelectListItem
				{
					Text = a.EmailName,
					Value = a.PKWhitelistID.ToString(),
					Selected = a.PKWhitelistID == selectedSender.Value
				})
				.ToList();

			// xmls base
			if (XmlToMap != null)
			{
				var xmlsBase = xmls.Where(x => x.Key.ToLower() != XmlToMap.ToLower()).ToList();
				List<string> xmlsBaseList = new List<string>();
				xmlsBaseList.Add("");
				foreach (IGrouping<string, xmlTemplate> item in xmlsBase)
					xmlsBaseList.Add(item.Key);

				AvailableXmlsBase = xmlsBaseList
					.Select(a => new SelectListItem
					{
						Text = a,
						Value = a,
						Selected = a.ToLower() == XmlBase.ToLower()
					})
					.ToList();
			}

			// xmls to map
			var xmlsToMap = xmls.Where(x => x.Key.Contains("Custom")).ToList();
			List<string> xmlsToMapList = new List<string>();
			xmlsToMapList.Add("");
			foreach (IGrouping<string, xmlTemplate> item in xmlsToMap)
				xmlsToMapList.Add(item.Key);

			AvailableXmlsToMap = xmlsToMapList
				.Select(a => new SelectListItem
				{
					Text = a,
					Value = a,
					Selected = a.ToLower() == XmlToMap.ToLower()
				})
				.ToList();

			// xml base fields
			xmlBaseFields.Insert(0, new xmlTemplate { NomeCampo = "", pkid = Guid.Empty });
			AvailableXmlBaseFields = xmlBaseFields
				.Select(a => new SelectListItem
				{
					Text = a.NomeCampo,
					Value = a.pkid.ToString(),
					Selected = a.pkid == Guid.Empty
				})
				.ToList();

			this.AvailableXmlToMapFields = xmlToMapFields;
			this.MapRows = new List<MapRow>();
			foreach (xmlTemplate path in xmlToMapFields)
			{
				MapRows.Add(new MapRow { ToMapXmlPath = path.CaminhoXML, ToMapXmlFieldPkid = path.pkid, BaseXmlFieldPkid = this.FkXmlBaseField });
			}
		}

		public IEnumerable<SelectListItem> AvailableSenders { get; private set; }
		public Guid FkSender { get; set; }

		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }
		public Guid FkInstance { get; set; }

		public IEnumerable<SelectListItem> AvailableXmlsBase { get; private set; }
		public string XmlBase { get; set; }

		public IEnumerable<SelectListItem> AvailableXmlsToMap { get; private set; }
		public string XmlToMap { get; set; }

		public IEnumerable<SelectListItem> AvailableXmlBaseFields { get; private set; }
		public Guid FkXmlBaseField { get; set; }

		public List<xmlTemplate> AvailableXmlToMapFields { get; private set; }
		//public string FkXmlToMapField { get; set; }

		[DoNotGenerateDictionaryEntry]
		public List<MapRow> MapRows { get; set; }

		[DoNotGenerateDictionaryEntry]
		public class MapRow
		{
			public string ToMapXmlPath { get; set; }
			public Guid ToMapXmlFieldPkid { get; set; }
			public Guid BaseXmlFieldPkid { get; set; }
		}
	}
}