using eBillingSuite.Enumerations;
using eBillingSuite.Model;
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
	public class EDigitalXmlFieldVM
	{
		public EDigitalXmlFieldVM()
		{
		}

		public EDigitalXmlFieldVM(List<CamposXML> camposXml, string localizacaoCampo, Guid tipoDocId)
		{
			Localizacao = localizacaoCampo;

			TipoDocPkid = tipoDocId;

			camposXml.Insert(0, new CamposXML { NomeCampo = "" });
			AvailableXmlFields = camposXml
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == ""
				})
				.ToList();

			List<string> decimals = new List<string>()
			{
				"",
				DigitalDecimalPlaces.ZERO,
				DigitalDecimalPlaces.ONE,
				DigitalDecimalPlaces.TWO,
				DigitalDecimalPlaces.THREE,
				DigitalDecimalPlaces.FOUR
			};
			AvailableDecimalPlaces = decimals
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v,
					Selected = v == ""
				})
				.ToList();

			List<string> extraction = new List<string>()
			{
				"",
				DigitalExtractionTypes.EXTRACTED_EXT,
				DigitalExtractionTypes.FORMULA_EXT,
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT
			};
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v ==DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : "")),
					Selected = v == ""
				})
				.ToList();

			List<string> operations = new List<string>()
			{
				"",
				DigitalOperations.ADD,
				DigitalOperations.SUB,
				DigitalOperations.DIV,
				DigitalOperations.MUL,
				DigitalOperations.O_BRACK,
				DigitalOperations.C_BRACK
			};
			AvailableFormulaOperations = operations
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v,
					Selected = v == ""
				})
				.ToList();
		}

		public EDigitalXmlFieldVM(TipoFacturaDadosXML dataFromDb, List<CamposXML> camposXml)
		{
			this.Pkid = dataFromDb.pkid;
			this.TipoDocPkid = dataFromDb.fkTipoFactura;
			this.Localizacao = dataFromDb.Localizacao;
			this.NomeCampo = dataFromDb.NomeCampo;
			this.IsRequired = dataFromDb.Obrigatorio.HasValue ? dataFromDb.Obrigatorio.Value : false;
			this.DecimalPlaces = dataFromDb.Formato.HasValue ? dataFromDb.Formato.Value : 0;
			this.ExtractionType = dataFromDb.TipoExtraccao;
			this.Formula = dataFromDb.Formula;

			camposXml.Insert(0, new CamposXML { NomeCampo = "" });
			AvailableXmlFields = camposXml
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == dataFromDb.NomeCampo
				})
				.ToList();

			List<string> decimals = new List<string>()
			{
				"",
				DigitalDecimalPlaces.ZERO,
				DigitalDecimalPlaces.ONE,
				DigitalDecimalPlaces.TWO,
				DigitalDecimalPlaces.THREE,
				DigitalDecimalPlaces.FOUR
			};
			AvailableDecimalPlaces = decimals
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v,
					Selected = v == dataFromDb.Formato.ToString()
				})
				.ToList();

			List<string> extraction = new List<string>()
			{
				"",
				DigitalExtractionTypes.EXTRACTED_EXT,
				DigitalExtractionTypes.FORMULA_EXT,
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT
			};
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v == DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : "")),
					Selected = v == dataFromDb.TipoExtraccao
				})
				.ToList();

			List<string> operations = new List<string>()
			{
				"",
				DigitalOperations.ADD,
				DigitalOperations.SUB,
				DigitalOperations.DIV,
				DigitalOperations.MUL,
				DigitalOperations.O_BRACK,
				DigitalOperations.C_BRACK
			};
			AvailableFormulaOperations = operations
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v,
					Selected = v == ""
				})
				.ToList();
		}

		public EDigitalXmlFieldVM(List<CamposXML> camposXml, DigitalXmlFieldData data)
		{
			camposXml.Insert(0, new CamposXML { NomeCampo = "" });
			AvailableXmlFields = camposXml
				.Select(v => new SelectListItem
				{
					Text = v.NomeCampo,
					Value = v.NomeCampo,
					Selected = v.NomeCampo == data.NomeCampo
				})
				.ToList();

			List<string> decimals = new List<string>()
			{
				"",
				DigitalDecimalPlaces.ZERO,
				DigitalDecimalPlaces.ONE,
				DigitalDecimalPlaces.TWO,
				DigitalDecimalPlaces.THREE,
				DigitalDecimalPlaces.FOUR
			};
			AvailableDecimalPlaces = decimals
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v,
					Selected = v == data.DecimalPlaces.ToString()
				})
				.ToList();

			List<string> extraction = new List<string>()
			{
				"",
				DigitalExtractionTypes.EXTRACTED_EXT,
				DigitalExtractionTypes.FORMULA_EXT,
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT
			};
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v == DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : "")),
					Selected = v == data.ExtractionType
				})
				.ToList();

			List<string> operations = new List<string>()
			{
				"",
				DigitalOperations.ADD,
				DigitalOperations.SUB,
				DigitalOperations.DIV,
				DigitalOperations.MUL,
				DigitalOperations.O_BRACK,
				DigitalOperations.C_BRACK
			};
			AvailableFormulaOperations = operations
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v,
					Selected = v == ""
				})
				.ToList();

			this.Pkid = data.Pkid;
			this.TipoDocPkid = data.TipoDocPkid;
			this.Localizacao = data.Localizacao;
			this.NomeCampo = data.NomeCampo;
			this.IsRequired = data.IsRequired;
			this.DecimalPlaces = data.DecimalPlaces;
			this.ExtractionType = data.ExtractionType;
			this.Formula = data.Formula;
		}

		#region Propreties

		public Guid Pkid { get; set; }

		public Guid TipoDocPkid { get; set; }

		public string Localizacao { get; set; }

		public IEnumerable<SelectListItem> AvailableXmlFields { get; set; }
		public string NomeCampo { get; set; }

		public bool IsRequired { get; set; }

		public IEnumerable<SelectListItem> AvailableDecimalPlaces { get; set; }
		public int DecimalPlaces { get; set; }

		public IEnumerable<SelectListItem> AvailableExtractionTypes { get; set; }
		public string ExtractionType { get; set; }

		public IEnumerable<SelectListItem> AvailableFormulaOperations { get; set; }

		public string Formula { get; set; }

		#endregion
	}
}