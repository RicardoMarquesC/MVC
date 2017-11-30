using eBillingSuite.Repositories;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class DigitalDocumentTypeDataValidator : AbstractValidator<DigitalDocumentTypeData>
	{
		private IEDigitalDocTypeRepository _eDigitalDocTypeRepository;
		private IEDigitalTemplateNameRepository _eDigitalTemplateNameRepository;

		[Inject]
		public DigitalDocumentTypeDataValidator(IEDigitalDocTypeRepository eDigitalDocTypeRepository,
			IEDigitalTemplateNameRepository eDigitalTemplateNameRepository)
		{
			_eDigitalDocTypeRepository = eDigitalDocTypeRepository;
			_eDigitalTemplateNameRepository = eDigitalTemplateNameRepository;

			RuleFor(c => c.TipoFactura.nome)
				.NotEmpty();

			RuleFor(c => c.TipoFactura.nome)
				.Must(NameIsUnique)
				.When(c => !string.IsNullOrWhiteSpace(c.TipoFactura.nome));

			RuleFor(c => c.NomeTemplate)
				.NotEmpty();

			RuleFor(c => c.NomeTemplate)
				.Must(TemplateNameIsUnique)
				.When(c => !string.IsNullOrWhiteSpace(c.NomeTemplate));

			RuleFor(c => c.TipoFactura.RecognitionTags)
				.NotEmpty();
		}

		private bool TemplateNameIsUnique(string nomeTemplate)
		{
			return !_eDigitalTemplateNameRepository.ExistsTemplateName(nomeTemplate);
		}

		private bool NameIsUnique(string nome)
		{
			return !_eDigitalDocTypeRepository.ExistsName(nome);
		}
	}
}