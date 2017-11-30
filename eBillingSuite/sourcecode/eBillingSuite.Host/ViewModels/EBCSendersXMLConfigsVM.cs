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
	public class EBCSendersXMLConfigsVM
	{
		private IECCListRepositories _eCCListRepositories;
		private IEConnectorSendersRepository _eConnectorSendersRepository;

		public EBCSendersXMLConfigsVM(Guid selectedSenderID,
			IECCListRepositories eCCListRepositories,
			IEConnectorSendersRepository eConnectorSendersRepository)
		{
			this._eCCListRepositories = eCCListRepositories;
			this._eConnectorSendersRepository = eConnectorSendersRepository;
			this.FkSenderID = selectedSenderID;

			List<Whitelist> senders = _eConnectorSendersRepository.Set
				.Where(s=> s.Enabled && s.HaveXML == true)
				.OrderBy(s => s.EmailName)
				.ToList();
			senders.Insert(0, new Whitelist { PKWhitelistID = Guid.Empty, EmailName = String.Empty });
			AvailableSenders = senders
				.Select(v => new SelectListItem
				{
					Text = v.EmailName,
					Value = v.PKWhitelistID.ToString(),
					Selected = v.PKWhitelistID == selectedSenderID
				})
				.ToList();


			List<string> tx = new List<string> { String.Empty, XmlTypes.UBL, XmlTypes.BASIC, XmlTypes.FACTURA_E, XmlTypes.PHC };
			AvailableTypes = tx
				   .Select(v => new SelectListItem
				   {
					   Text = v,
					   Value = v,
					   Selected = v == String.Empty
				   })
				   .ToList();

			
			xmlHeaders = new List<EBC_XMLHeadInbound>();
			xmlLines = new List<EBC_XMLLinesInbound>();
			xmlVat = new List<EBC_XMLResumoIVAInbound>();

			// if user selected a Sender(Whitelist)
			if (selectedSenderID != null && selectedSenderID != Guid.Empty)
			{
				// get sender NIF
				string senderNif = _eConnectorSendersRepository.GetSenderNifById(selectedSenderID);
				if (!String.IsNullOrWhiteSpace(senderNif))
				{
					// get XML number associated with this sender
					int xmlNumber = _eCCListRepositories.eConnectorXmlInboundRepository.GetXmlNumberBySenderVat(senderNif);
					if (xmlNumber != -1)
					{
						xmlHeaders = _eCCListRepositories.eConnectorXmlHeadInboundRepository
							.Where(h => h.NumeroXML == xmlNumber)
							.OrderBy(h => h.NomeCampo)
							.ToList();
						xmlLines = _eCCListRepositories.eConnectorXmlLinesInboundRepository
							.Where(l => l.NumeroXML == xmlNumber)
							.OrderBy(l => l.NomeCampo)
							.ToList();
						xmlVat = _eCCListRepositories.eConnectorXmlVatInboundRepository
							.Where(v => v.NumeroXML == xmlNumber)
							.OrderBy(v => v.NomeCampo)
							.ToList();

						this.xmlNumber = xmlNumber;
						this.XmlType = xmlHeaders.ElementAt(0).TipoXML;
					}
				}
			}
		}

		public IEnumerable<SelectListItem> AvailableSenders { get; private set; }
		public Guid FkSenderID { get; set; }

		public IEnumerable<SelectListItem> AvailableTypes { get; private set; }
		public string XmlType { get; set; }

		public int xmlNumber { get; set; }

		public List<EBC_XMLHeadInbound> xmlHeaders;
		public List<EBC_XMLLinesInbound> xmlLines;
		public List<EBC_XMLResumoIVAInbound> xmlVat;
	}
}