using eBillingSuite.Model;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
	public class EDigitalDocumentExpirationVM
	{
		public EDigitalDocumentExpirationVM(List<ExpirarFactura> expFact)
		{
			foreach (var item in expFact)
			{
				int tempo = item.TempoExpirarFactura;
				Guid pkid = item.pkid;

				if (item.Fase == "Lista de Espera")
				{
					this.WaitList = tempo;
					this.WaitListPkid = pkid;
				}
				else if (item.Fase == "Separação")
				{
					this.Separacao = tempo;
					this.SeparacaoPkid = pkid;
				}
				else
				{
					this.Processamento = tempo;
					this.ProcessamentoPkid = pkid;
				}
			}
		}

		public EDigitalDocumentExpirationVM(DigitalDocumentExpirationData data)
		{
			this.WaitListPkid = data.WaitListPkid;
			this.WaitList = data.WaitList;
			this.SeparacaoPkid = data.SeparacaoPkid;
			this.Separacao = data.Separacao;
			this.ProcessamentoPkid = data.ProcessamentoPkid;
			this.Processamento = data.Processamento;
		}

		public Guid WaitListPkid { get; set; }
		public int WaitList { get; set; }
		public Guid SeparacaoPkid { get; set; }
		public int Separacao { get; set; }
		public Guid ProcessamentoPkid { get; set; }
		public int Processamento { get; set; }
	}
}