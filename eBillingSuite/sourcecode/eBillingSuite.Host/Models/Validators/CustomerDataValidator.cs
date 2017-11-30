using eBillingSuite.Resources;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System;

namespace eBillingSuite.Models.Validators
{
	public class CustomerDataValidator : AbstractValidator<CustomerData>
	{
		private IeBillingSuiteRequestContext _context;
		//Password
		public const string password = @"^([a-zA-Z0-9\s:-_.]){8,16}$";
		//Username
		public const string username = @"^[0-9]{9,9}/[0-9]{4,4}$";

		//email
		public const string email = @"^[_+a-zA-Z0-9-]+(\.[_+a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]{1,})*\.([a-zA-Z]{2,}){1}$";


		[Inject]
		public CustomerDataValidator(IeBillingSuiteRequestContext context)
		{
			_context = context;

			RuleFor(c => c.Name)
				.NotEmpty();

			RuleFor(c => c.Email)
				.NotEmpty()
				.Matches(email)
				.WithMessage(Texts.EmailIncorrectFormat);

			RuleFor(c => c.NIF)
				.NotEmpty();

			Custom(ValidateAction);

		}

		private ValidationFailure ValidateAction(CustomerData data)
		{
			if (data.NIF == null)
				return new ValidationFailure("NIF", Texts.NIFFailedRequirement);

			if(data.Mercado.ToLower().Equals("portugal"))
			{
				if(data.NIF.Length>9 && data.NIF.Length < 12)
				{
					var i = 0;
					foreach (char c in data.NIF)
					{
						if(i<2)
						{
							if (char.IsNumber(c))
								return new ValidationFailure("NIF", Texts.NIFFailedRequirement);
						}
						
						i++;
					}
				}
				else if(data.NIF.Length>11)
					return new ValidationFailure("NIF", Texts.NIFFailedRequirement);
				
				bool NifValido = IsValidContrib(data.NIF);
				if(!NifValido)
					return new ValidationFailure("NIF", Texts.NIFFailedRequirement);
				
			}
			
			return null;
		}

		public bool IsValidContrib(string Contrib)
		{
			bool functionReturnValue = false;
			functionReturnValue = false;
			string[] s = new string[9];
			string Ss = null;
			string C = null;
			int i = 0;
			long checkDigit = 0;

			if(Contrib.Length>9)
				Contrib = Contrib.Remove(0, 2);

			s[0] = Convert.ToString(Contrib[0]);
			s[1] = Convert.ToString(Contrib[1]);
			s[2] = Convert.ToString(Contrib[2]);
			s[3] = Convert.ToString(Contrib[3]);
			s[4] = Convert.ToString(Contrib[4]);
			s[5] = Convert.ToString(Contrib[5]);
			s[6] = Convert.ToString(Contrib[6]);
			s[7] = Convert.ToString(Contrib[7]);
			s[8] = Convert.ToString(Contrib[8]);

			if (Contrib.Length == 9)
			{
				C = s[0];
				if (s[0] == "1" || s[0] == "2" || s[0] == "5" || s[0] == "6" || s[0] == "9")
				{
					checkDigit = Convert.ToInt32(C) * 9;
					for (i = 2; i <= 8; i++)
					{
						checkDigit = checkDigit + (Convert.ToInt32(s[i - 1]) * (10 - i));
					}
					checkDigit = 11 - (checkDigit % 11);
					if ((checkDigit >= 10))
						checkDigit = 0;
					Ss = s[0] + s[1] + s[2] + s[3] + s[4] + s[5] + s[6] + s[7] + s[8];
					if ((checkDigit == Convert.ToInt32(s[8])))
						functionReturnValue = true;
				}
			}
			return functionReturnValue;
		}
	}
}