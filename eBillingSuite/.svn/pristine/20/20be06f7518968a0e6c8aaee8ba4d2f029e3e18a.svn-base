using eBillingSuite.Enumerations;
using eBillingSuite.Model;
using eBillingSuite.Model.Desmaterializacao;
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

		/* BEGIND: USED TO XML FIELDS BASED ON DOCUMENT TYPE */
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
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT,
                DigitalExtractionTypes.DEFAULT_VALUE_EXT
			};
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
                    Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v == DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : (v == DigitalExtractionTypes.DEFAULT_VALUE_EXT ? DigitalExtractionTypes.DEFAULT_VALUE : ""))),
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
			this.IsComboBox = dataFromDb.IsComboBox.HasValue ? dataFromDb.IsComboBox.Value : false;
			this.IsReadOnly = dataFromDb.IsReadOnly.HasValue ? dataFromDb.IsReadOnly.Value : false;
			this.LabelUI = dataFromDb.LabelUI == null ? "" : dataFromDb.LabelUI;
            this.PersistValueToNextDoc = dataFromDb.PersistValueToNextDoc;
            this.DefaultValue = dataFromDb.DefaultValue;

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
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT,
                DigitalExtractionTypes.DEFAULT_VALUE_EXT
			};
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
                    Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v == DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : (v == DigitalExtractionTypes.DEFAULT_VALUE_EXT ? DigitalExtractionTypes.DEFAULT_VALUE : ""))),
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
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT,
                DigitalExtractionTypes.DEFAULT_VALUE_EXT
			};
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
                    Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v == DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : (v == DigitalExtractionTypes.DEFAULT_VALUE_EXT ? DigitalExtractionTypes.DEFAULT_VALUE : ""))),
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

			List<string> expressionItems = new List<string>()
			{
				"",
				DigitalExpressionItems.NUMBER_EXT,
				DigitalExpressionItems.LETTER_EXT,
				DigitalExpressionItems.SPACE_EXT,
				DigitalExpressionItems.SLASH,
				DigitalExpressionItems.INVERTED_SLASH,
				DigitalExpressionItems.MINUS,
				DigitalExpressionItems.DOT
			};
			AvailableExpressionItems = expressionItems
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v == DigitalExpressionItems.NUMBER_EXT ? DigitalExpressionItems.NUMBER : (v == DigitalExpressionItems.LETTER_EXT ? DigitalExpressionItems.LETTER : (v == DigitalExpressionItems.SPACE_EXT ? DigitalExpressionItems.SPACE : v)),
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
			this.SupplierPkid = data.SupplierPkid;
			this.Expression = data.Expression;
			this.IsComboBox = data.IsComboBox;
			this.IsReadOnly = data.IsReadOnly;
			this.LabelUI = data.LabelUI;
            this.PersistValueToNextDoc = data.PersistValueToNextDoc;
            this.DefaultValue = data.DefaultValue;
		}
		/* END: USED TO XML FIELDS BASED ON DOCUMENT TYPE */

		/* BEGIND: USED TO XML FIELDS BASED ON SUPPLIER */
		public EDigitalXmlFieldVM(List<CamposXML> camposXml, string localizacaoCampo, Guid tipoDocId, Guid supplierId)
		{
			Localizacao = localizacaoCampo;

			TipoDocPkid = tipoDocId;

			SupplierPkid = supplierId;

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
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT,
                DigitalExtractionTypes.DEFAULT_VALUE_EXT
            };
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v == DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : (v == DigitalExtractionTypes.DEFAULT_VALUE_EXT ? DigitalExtractionTypes.DEFAULT_VALUE : ""))),
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

			List<string> expressionItems = new List<string>()
			{
				"",
				DigitalExpressionItems.NUMBER_EXT,
				DigitalExpressionItems.LETTER_EXT,
				DigitalExpressionItems.SPACE_EXT,
				DigitalExpressionItems.SLASH,
				DigitalExpressionItems.INVERTED_SLASH,
				DigitalExpressionItems.MINUS,
				DigitalExpressionItems.DOT
			};
			AvailableExpressionItems = expressionItems
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v == DigitalExpressionItems.NUMBER_EXT ? DigitalExpressionItems.NUMBER : (v == DigitalExpressionItems.LETTER_EXT ? DigitalExpressionItems.LETTER : (v == DigitalExpressionItems.SPACE_EXT ? DigitalExpressionItems.SPACE : v)),
					Selected = v == ""
				})
				.ToList();
		}

		public EDigitalXmlFieldVM(HelperTools.DadosTemplateXmlDBTable dataFromDb, List<CamposXML> camposXml, Guid tipoDocId, Guid supplierId, Guid fkNomeTemplate)
		{
			this.Pkid = dataFromDb.Pkid;
			this.TipoDocPkid = tipoDocId;
			this.SupplierPkid = supplierId;
			this.Localizacao = dataFromDb.Localizacao;
			this.NomeCampo = dataFromDb.NomeCampo;
			this.IsRequired = dataFromDb.Obrigatorio;
			this.DecimalPlaces = dataFromDb.CasasDecimais;
			this.ExtractionType = dataFromDb.TipoExtraccao;
			this.Formula = dataFromDb.Formula;
			this.Expression = dataFromDb.Expressao;
			this.IsFromOrigin = dataFromDb.DeOrigem;
			this.IsComboBox = dataFromDb.IsComboBox;
            this.FkNomeTemplate = fkNomeTemplate;
            this.PersistValueToNextDoc = dataFromDb.PersistValueToNextDoc;
            this.DefaultValue = dataFromDb.DefaultValue;

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
					Selected = v == dataFromDb.CasasDecimais.ToString()
				})
				.ToList();

			List<string> extraction = new List<string>()
			{
				"",
				DigitalExtractionTypes.EXTRACTED_EXT,
				DigitalExtractionTypes.FORMULA_EXT,
				DigitalExtractionTypes.EXTRACTED_FORMULA_EXT,
                DigitalExtractionTypes.DEFAULT_VALUE_EXT
			};
			AvailableExtractionTypes = extraction
				.Select(v => new SelectListItem
				{
					Text = v,
                    Value = v == DigitalExtractionTypes.EXTRACTED_EXT ? DigitalExtractionTypes.EXTRACTED : (v == DigitalExtractionTypes.FORMULA_EXT ? DigitalExtractionTypes.FORMULA : (v == DigitalExtractionTypes.EXTRACTED_FORMULA_EXT ? DigitalExtractionTypes.EXTRACTED_FORMULA : (v == DigitalExtractionTypes.DEFAULT_VALUE_EXT ? DigitalExtractionTypes.DEFAULT_VALUE : ""))),
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

			List<string> expressionItems = new List<string>()
			{
				"",
				DigitalExpressionItems.NUMBER_EXT,
				DigitalExpressionItems.LETTER_EXT,
				DigitalExpressionItems.SPACE_EXT,
				DigitalExpressionItems.SLASH,
				DigitalExpressionItems.INVERTED_SLASH,
				DigitalExpressionItems.MINUS,
				DigitalExpressionItems.DOT
			};
			AvailableExpressionItems = expressionItems
				.Select(v => new SelectListItem
				{
					Text = v,
					Value = v == DigitalExpressionItems.NUMBER_EXT ? DigitalExpressionItems.NUMBER : (v == DigitalExpressionItems.LETTER_EXT ? DigitalExpressionItems.LETTER : (v == DigitalExpressionItems.SPACE_EXT ? DigitalExpressionItems.SPACE : v)),
					Selected = v == ""
				})
				.ToList();
		}

		/* END: USED TO XML FIELDS BASED ON SUPPLIER */

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

		// used only in xml managment based on supplier
		public Guid SupplierPkid { get; set; }
		public IEnumerable<SelectListItem> AvailableExpressionItems { get; set; }
		public string Expression { get; set; }
		public bool IsFromOrigin { get; set; }
		public bool IsComboBox { get; set; }
		public bool IsReadOnly { get; set; }
		public string LabelUI { get; set; }
        public bool PersistValueToNextDoc { get; set; }
        public string DefaultValue { get; set; }

        public Guid FkNomeTemplate { get; set; }

        #endregion
    }
}