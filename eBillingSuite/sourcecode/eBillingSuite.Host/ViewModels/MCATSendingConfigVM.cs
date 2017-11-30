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
	public class MCATSendingConfigVM
	{
		public MCATSendingConfigVM(
			ConfigEnvioAT entry,
			MCATConfigSendInfoData dataFromPost,
			IMCATSendingDefinitionsRepository mCATSendingDefinitionsRepository,
			IInstancesRepository instancesRepository,
			Guid? ID,
            IeBillingSuiteRequestContext _context
            )
		{
			if(entry == null)
			{
				pkid = Guid.NewGuid();
				NumberOfAttempts = 0;
				UnidadeTempo = 0;
				fkInstancia = ID.ToString();
				TipoUnidadeTempo = "Minuto(s)";
				EnderecoEmail = "";
			}
			else
			{
				pkid = entry.pkid;
				NumberOfAttempts = entry.NumberOfAttempts;
				UnidadeTempo = entry.UnidadeTempo;
				EnderecoEmail = entry.EnderecoEmail;
				fkInstancia = entry.fkInstancia;
				TipoUnidadeTempo = entry.TipoUnidadeTempo;
			}
			
			var values = instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
			AvailableInstances = values
				.Select(v => new SelectListItem
				{
					Text = v.Name,
					Value = v.PKID.ToString(),
					Selected = v.PKID == Guid.Parse(fkInstancia)
				})
				.ToList();

			List<TipoUnidadeTempoClass> TUTempo = new List<TipoUnidadeTempoClass>();
			TipoUnidadeTempoClass t1 = new TipoUnidadeTempoClass { id = 1, valor = "Minuto(s)" };
			TipoUnidadeTempoClass t2 = new TipoUnidadeTempoClass { id = 2, valor = "Hora(s)" };
			TipoUnidadeTempoClass t3 = new TipoUnidadeTempoClass { id = 3, valor = "Dia(s)" };
			TUTempo.Add(t1); TUTempo.Add(t2); TUTempo.Add(t3);

			AvailableTypes = TUTempo
				.Select(t => new SelectListItem
				{
					Text = t.valor,
					Value = t.id.ToString(),
					Selected = t.valor == TipoUnidadeTempo
				})
				.ToList();
		}

		[DoNotGenerateDictionaryEntry]
        public System.Guid pkid { get; set; }

        public int NumberOfAttempts { get; set; }

        public int UnidadeTempo { get; set; }

        public string TipoUnidadeTempo { get; set; }

        public string EnderecoEmail { get; set; }

        public string fkInstancia { get; set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableTypes { get; private set; }
		
		[DoNotGenerateDictionaryEntry]
		public class TipoUnidadeTempoClass
		{
			public int id;
			public string valor;
		}
	}
}