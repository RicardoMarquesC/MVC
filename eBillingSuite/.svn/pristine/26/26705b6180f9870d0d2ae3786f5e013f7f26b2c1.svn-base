using eBillingSuite.Enumerations;
using eBillingSuite.Model;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EDigitalTemplatesVM
	{
		public EDigitalTemplatesVM(List<TipoFacturas> tipoFacturas, Guid selectedDocType,
			IEDigitalDocTypeXmlDataRepository eDigitalDocTypeXmlDataRepository,
			IEDigitalTemplateNameRepository eDigitalTemplateNameRepository)
		{
			// tipo facturas
			tipoFacturas.Insert(0, new TipoFacturas { pkid = Guid.Empty, nome = "" });
			AvailableDocTypes = tipoFacturas
				.Select(v => new SelectListItem
				{
					Text = v.nome,
					Value = v.pkid.ToString(),
					Selected = v.pkid == selectedDocType
				})
				.OrderBy(x => x.Text)
				.ToList();

			FkDocumentType = selectedDocType;

			// dados do xml
			DadosXmlCabecalho = new List<TipoFacturaDadosXML>();
			DadosXmlLinhas = new List<TipoFacturaDadosXML>();
			DadosXmlIva = new List<TipoFacturaDadosXML>();
			if (selectedDocType != Guid.Empty)
			{
				DadosXmlCabecalho = eDigitalDocTypeXmlDataRepository
					.Where(d => d.fkTipoFactura == selectedDocType && d.Localizacao.ToLower() == DigitalDocumentAreas.HEADER)
					.OrderByDescending(d => d.Posicao)
					.ToList();

				DadosXmlLinhas = eDigitalDocTypeXmlDataRepository
					.Where(d => d.fkTipoFactura == selectedDocType && d.Localizacao.ToLower() == DigitalDocumentAreas.LINES)
					.OrderByDescending(d => d.Posicao)
					.ToList();

				DadosXmlIva = eDigitalDocTypeXmlDataRepository
					.Where(d => d.fkTipoFactura == selectedDocType && d.Localizacao.ToLower() == DigitalDocumentAreas.VAT)
					.OrderByDescending(d => d.Posicao)
					.ToList();
			}

			// template name, associated with the selected document type
			var nomeTemplate = eDigitalTemplateNameRepository.Set.FirstOrDefault(n => n.fktipofact.Value == FkDocumentType);
			NomeTemplate = nomeTemplate != null ? nomeTemplate.NomeOriginal : "";

			// document type choosed identification tags
			var docType = tipoFacturas.FirstOrDefault(c => c.pkid == FkDocumentType);
			DocumentTypeTags = docType != null ? docType.RecognitionTags : "";

			// if it's generic type
			IsGenericDocument = docType != null ? docType.IsGenericDocument.HasValue ? docType.IsGenericDocument.Value : false : false;
		}

		public EDigitalTemplatesVM(DigitalTemplatesData data)
		{
			this.FkDocumentType = data.FkDocumentType;

			this.DadosXmlCabecalho = data.DadosXmlCabecalho;
			this.DadosXmlLinhas = data.DadosXmlLinhas;
			this.DadosXmlIva = data.DadosXmlIva;

			this.NomeTemplate = data.NomeTemplate;
		}

		public IEnumerable<SelectListItem> AvailableDocTypes { get; set; }
		public System.Guid FkDocumentType { get; set; }

		public List<TipoFacturaDadosXML> DadosXmlCabecalho { get; set; }
		public List<TipoFacturaDadosXML> DadosXmlLinhas { get; set; }
		public List<TipoFacturaDadosXML> DadosXmlIva { get; set; }

		public string NomeTemplate { get; set; }

		public string DocumentTypeTags { get; set; }

		public bool IsGenericDocument { get; set; }

		public string Action { get; set; }
	}
}