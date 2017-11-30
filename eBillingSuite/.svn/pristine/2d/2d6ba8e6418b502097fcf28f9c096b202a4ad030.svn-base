using eBillingSuite.Repositories;
using eBillingSuite.Resources;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System.Linq;

namespace eBillingSuite.Models.Validators
{
	public class XmlConfigDataValidator : AbstractValidator<XmlConfigData>
	{
		private IeBillingSuiteRequestContext _context;
		private IECCListRepositories _eCConfigRepositories;
		//Password
		public const string password = @"^([a-zA-Z0-9\s:-_.]){8,16}$";
		//Username
		public const string username = @"^[0-9]{9,9}/[0-9]{4,4}$";

		[Inject]
		public XmlConfigDataValidator(IeBillingSuiteRequestContext context,
			IECCListRepositories eCConfigRepositories)
		{
			_context = context;
			_eCConfigRepositories = eCConfigRepositories;

			Custom(ValidateAction);

		}


		private ValidationFailure ValidateAction(XmlConfigData data)
		{
			if(!data.isEdit)
			{
				//verificar se o campo já existe
				bool exists = true;
				char[] delimitador = { '-' };
				string[] valuestemp = data.selectedField.Split(delimitador);
				string nomecampo = valuestemp[0].Trim();
				if (valuestemp[1].Trim().ToLower().Equals(eBillingSuite.Enumerations.DigitalDocumentAreas.HEADER))
				{
					exists = _eCConfigRepositories.eConnectorXmlHeaderRepository.Exists(exh => exh.NomeCampo == nomecampo
						&&
						exh.NumeroXML == data.numeroxml);
				}
				else if (valuestemp[1].Trim().ToLower().Equals(eBillingSuite.Enumerations.DigitalDocumentAreas.LINES))
				{
					exists = _eCConfigRepositories.eConnectorXmlLinesRepository.Exists(exh => exh.NomeCampo == nomecampo
						&&
						exh.NumeroXML == data.numeroxml);
				}
				else
				{
					exists = _eCConfigRepositories.eConnectorXmlResumoIvaRepository.Exists(exh => exh.NomeCampo == nomecampo
						&&
						exh.NumeroXML == data.numeroxml);
				}

				if (exists)
					return new ValidationFailure("selectedField", Texts.CampoJaExistente);

			}
			
			return null;
		}
	}
}