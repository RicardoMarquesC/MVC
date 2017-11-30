using eBillingSuite.Resources;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class CredentialsDataValidator : AbstractValidator<CredentialsData>
	{
		private IeBillingSuiteRequestContext _context;
		//Password
		public const string password = @"^([a-zA-Z0-9\s:-_.]){8,16}$";
		//Username
		public const string username = @"^[0-9]{9,9}/[0-9]{4,4}$";

		[Inject]
		public CredentialsDataValidator(IeBillingSuiteRequestContext context)
		{
			_context = context;

			RuleFor(c => c.usrat)
				.Matches(username)
				.WithMessage(Texts.UsernameIncorrectFormat);

			RuleFor(c => c.pwdat)
				.Matches(password)
				.WithMessage(Texts.PasswordIncorrectFormat);

			RuleFor(c => c.confirmpwdat)
				.Matches(password)
				.WithMessage(Texts.PasswordIncorrectFormat);

			RuleFor(c => c.confirmpwdat)
				.Equal(c => c.pwdat)
				.When(c => !string.IsNullOrWhiteSpace(c.confirmpwdat))
				.WithMessage(Texts.ConfirmAndPassMustBeEqual);
		}
	}
}