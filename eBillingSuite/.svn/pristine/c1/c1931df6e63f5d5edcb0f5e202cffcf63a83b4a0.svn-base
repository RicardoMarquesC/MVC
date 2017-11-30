using eBillingSuite.Model;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
	public class EDigitalCreateDocTypeVM
	{
		public EDigitalCreateDocTypeVM(TipoFacturas tipoFactura, string nomeTemplate, bool isGenericDocument)
		{
			TipoFactura = tipoFactura;
			NomeTemplate = nomeTemplate;
			IsGenericDocument = isGenericDocument;
		}

		public EDigitalCreateDocTypeVM(DigitalDocumentTypeData data)
		{
			this.TipoFactura = data.TipoFactura;
			this.NomeTemplate = data.NomeTemplate;
		}

		public TipoFacturas TipoFactura { get; set; }
		public string NomeTemplate { get; set; }
		public bool IsGenericDocument { get; set; }
	}
}