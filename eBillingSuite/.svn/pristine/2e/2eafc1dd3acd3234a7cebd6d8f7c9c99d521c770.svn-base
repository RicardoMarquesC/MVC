using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Enumerations
{

	#region Common

	public class XmlTypes
	{
		public const string UBL = "UBL2.0";
		public const string BASIC = "BasicMetadata";
		public const string FACTURA_E = "Facturae";
		public const string PHC = "PHCFULLXML";
	}

	public class StandardActions
	{
		public const string CREATE = "create";
		public const string EDIT = "edit";
	}

	public class DigitalDocumentAreas
	{
		public const string HEADER = "cabecalho";
		public const string LINES = "lineitem";
		public const string VAT = "resumoiva";
		public const string GENERIC = "Genérico";
	}

	#endregion

	# region eDigital

	public class DigitalExtractionTypes
	{
		public const string EXTRACTED = "E";
		public const string EXTRACTED_EXT = "Extraído";
		public const string FORMULA = "F";
		public const string FORMULA_EXT = "Fórmula";
		public const string EXTRACTED_FORMULA = "EF";
		public const string EXTRACTED_FORMULA_EXT = "Extraído + Fórmula";
        public const string DEFAULT_VALUE = "D";
        public const string DEFAULT_VALUE_EXT = "Valor por defeito";
    }

	public class DigitalOperations
	{
		public const string ADD = "+";
		public const string SUB = "-";
		public const string MUL = "*";
		public const string DIV = "/";
		public const string O_BRACK = "(";
		public const string C_BRACK = ")";
	}

	public class DigitalExpressionItems
	{
		public const string NUMBER = "A";
		public const string NUMBER_EXT = "Algarismo";
		public const string LETTER = "L";
		public const string LETTER_EXT = "Letra";
		public const string SPACE = " ";
		public const string SPACE_EXT = "Espaço";
		public const string SLASH = "/";
		public const string INVERTED_SLASH = @"\";
		public const string MINUS = "-";
		public const string DOT = ".";
	}

	public class DigitalDecimalPlaces
	{
		public const string ZERO = "0";
		public const string ONE = "1";
		public const string TWO = "2";
		public const string THREE = "3";
		public const string FOUR = "4";
	}

	/// <summary>
	/// Listagem disponíveis para a tabela de Configuracoes
	/// </summary>
	public enum DigitalSettingKeys
	{
		NextPageCounter,
		NextPageTimeStamp
	};

	#endregion

	#region eConnector

	public class IntegrationFiltersName
	{
		public const string DEFAULT = "eBilling Connector Default Filter";
		public const string MANUAL = "Manual Reception Filter";
		public const string NON_NAIVE = "Non Native Custom Filter";
	}

	public class ReceptionProtocols
	{
		public const string POP3 = "POP3";
		public const string IMAP4 = "IMAP4";
		public const string HTTP = "HTTP";
	}

	#endregion
}
