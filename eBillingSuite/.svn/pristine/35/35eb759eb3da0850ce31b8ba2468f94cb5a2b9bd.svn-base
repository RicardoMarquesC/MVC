using eBillingSuite.Repositories;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class DigitalDocumentExpirationDataValidator : AbstractValidator<DigitalDocumentExpirationData>
	{
		private IeBillingSuiteRequestContext _context;
		
		[Inject]
		public DigitalDocumentExpirationDataValidator(IeBillingSuiteRequestContext context)
		{
			_context = context;

			RuleFor(c => c.Processamento)
				.NotEmpty();

			RuleFor(c => c.Separacao)
				.NotEmpty();

			RuleFor(c => c.WaitList)
				.NotEmpty();
		}
	}
}