using eBillingSuite.Resources;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class MCATConfigSendInfoDataValidator : AbstractValidator<MCATConfigSendInfoData>
	{
		private IeBillingSuiteRequestContext _context;
		//Password
		public const string password = @"^([a-zA-Z0-9\s:-_.]){8,16}$";
		//Username
		public const string username = @"^[0-9]{9,9}/[0-9]{4,4}$";
		//email
		public const string email = @"^[_+a-zA-Z0-9-]+(\.[_+a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]{1,})*\.([a-zA-Z]{2,}){1}$";

		[Inject]
		public MCATConfigSendInfoDataValidator(IeBillingSuiteRequestContext context)
		{
			_context = context;

			RuleFor(c => c.EnderecoEmail)
				.NotEmpty();

			RuleFor(c => c.EnderecoEmail)
				.Matches(email)
				.WithMessage(Texts.EmailIncorrectFormat);

			RuleFor(c => c.UnidadeTempo)
				.NotEmpty();

			RuleFor(c => c.TipoUnidadeTempo)
				.NotEmpty();

			RuleFor(c => c.NumberOfAttempts)
				.NotEmpty();	

		}
	}
}