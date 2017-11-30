using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.EDI_DB;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EEDICostumersVM
	{

		public EEDICostumersVM(List<Clientes> data, List<Instancias> instances)
		{
			this._data = data;
			this._instances = instances;

			AvailableInstances = instances
				.Select(v => new SelectListItem
				{
					Text = v.Nome,
					Value = v.PKID.ToString(),
					Selected = v.PKID == data[0].FKInstanciaID
				})
				.ToList();
		}

		public EEDICostumersVM(Clientes data)
		{
			PKID = data.PKID;
			Nome = data.Nome;
			FKInstanciaID = data.FKInstanciaID;
			NIF = data.NIF;
			URL = data.URL;
			TempoEspera = data.TempoEspera;
			TempoEsperaUnidade = data.TempoEsperaUnidade;
			Tentativas = data.Tentativas;
			Intervalo = data.Intervalo;
			IntervaloUnidade = data.IntervaloUnidade;

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
					Selected = t.id == TempoEsperaUnidade
				})
				.ToList();
		}

		public EEDICostumersVM(EDICostumerData data)
		{
			PKID = data.PKID;
			Nome = data.Nome;
			FKInstanciaID = data.FKInstanciaID;
			NIF = data.NIF;
			URL = data.URL;
			TempoEspera = data.TempoEspera;
			TempoEsperaUnidade = data.TempoEsperaUnidade;
			Tentativas = data.Tentativas;
			Intervalo = data.Intervalo;
			IntervaloUnidade = data.IntervaloUnidade;

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
					Selected = t.id == TempoEsperaUnidade
				})
				.ToList();
		}

		[DoNotGenerateDictionaryEntry]
		public List<Clientes> _data { get; set; }
		[DoNotGenerateDictionaryEntry]
		private List<Instancias> _instances { get; set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public System.Guid PKID { get; set; }
		public string Nome { get; set; }
		public System.Guid FKInstanciaID { get; set; }
		public string NIF { get; set; }
		public string URL { get; set; }

		public int TempoEspera { get; set; }

		public int TempoEsperaUnidade { get; set; }

		public int Tentativas { get; set; }

		public int Intervalo { get; set; }

		public int IntervaloUnidade { get; set; }

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