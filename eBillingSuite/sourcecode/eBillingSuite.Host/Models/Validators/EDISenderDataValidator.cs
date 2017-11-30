using eBillingSuite.Resources;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class EDISenderDataValidator : AbstractValidator<EDISenderData>
	{
		private IeBillingSuiteRequestContext _context;
		//Password
		public const string url = @"^(https?:\/\/)[a-zA-Z0-9.:/]{3,100}$";
		
		[Inject]
		public EDISenderDataValidator(IeBillingSuiteRequestContext context)
		{
			_context = context;

			RuleFor(c => c.Nome)
				.NotEmpty();

			RuleFor(c => c.NIF)
				.NotEmpty();

			RuleFor(c => c.URL)
				.Matches(url)
				.WithMessage(Texts.URLIncorrectFormat);

		}
	}
}