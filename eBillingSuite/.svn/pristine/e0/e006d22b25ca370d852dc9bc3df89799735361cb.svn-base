using eBillingSuite.Enumerations;
using eBillingSuite.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System;

namespace eBillingSuite.Models.Validators
{
	public class XmlSenderDataValidator : AbstractValidator<XmlSenderData>
	{
		private IECCListRepositories _eCCListRepositories;

		[Inject]
		public XmlSenderDataValidator(IECCListRepositories eCCListRepositories)
		{
			_eCCListRepositories = eCCListRepositories;

			RuleFor(c => c.NomeCampo)
				.NotEmpty()
				.Must(NameIsUniqueHeader)
				.When(c => c.Area.Equals(DigitalDocumentAreas.HEADER, StringComparison.OrdinalIgnoreCase))
				.Must(NameIsUniqueLines)
				.When(c => c.Area.Equals(DigitalDocumentAreas.LINES, StringComparison.OrdinalIgnoreCase))
				.Must(NameIsUniqueVat)
				.When(c => c.Area.Equals(DigitalDocumentAreas.VAT, StringComparison.OrdinalIgnoreCase));
		}

		private bool NameIsUniqueHeader(string name)
		{
			return _eCCListRepositories.eConnectorXmlHeadInboundRepository.IsFieldNameUnique(name);
		}

		private bool NameIsUniqueLines(string name)
		{
			return _eCCListRepositories.eConnectorXmlLinesInboundRepository.IsFieldNameUnique(name);
		}

		private bool NameIsUniqueVat(string name)
		{
			return _eCCListRepositories.eConnectorXmlVatInboundRepository.IsFieldNameUnique(name);
		}

	}
}