using eBillingSuite.Repositories;
using eBillingSuite.Resources;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System;

namespace eBillingSuite.Models.Validators
{
	public class SenderDataValidator : AbstractValidator<SenderData>
	{
		private IeBillingSuiteRequestContext _context;
		private IEConnectorSendersRepository _eConnectorSendersRepository;

		[Inject]
		public SenderDataValidator(IeBillingSuiteRequestContext context, IEConnectorSendersRepository eConnectorSendersRepository)
		{
			_context = context;
			_eConnectorSendersRepository = eConnectorSendersRepository;

			RuleFor(c => c.EmailName)
				.NotEmpty();

            RuleFor(c => c.Nif)
                .NotEmpty()
                .When(c => c.IsFromCreate);

            RuleFor(c => c.EmailAddress)
				.NotEmpty();

			RuleFor(c => c.XmlType)
				.NotEmpty()
				.When(c => c.XMLAss == true || c.XMLNAss == true);

			RuleFor(c => c.PDFAss)
				.Equal(true)
				.When(c => c.Mercado.Equals("portugal", StringComparison.OrdinalIgnoreCase));

			Custom(ValidateAction);
		}

		private bool NifIsUnique(string nif)
		{
			return _eConnectorSendersRepository.IsNifUnique(nif);
		}

		private ValidationFailure ValidateAction(SenderData data)
		{
			if(data.Nif == null)
				return new ValidationFailure("NIF", Texts.NIFFailedRequirement);

			if(data.Mercado.ToLower().Equals("portugal"))
			{
				if (data.Nif.Length > 9 && data.Nif.Length < 12)
				{
					var i = 0;
					foreach (char c in data.Nif)
					{
						if(i<2)
						{
							if (char.IsNumber(c))
								return new ValidationFailure("NIF", Texts.NIFFailedRequirement);
						}
						
						i++;
					}
				}
				else if (data.Nif.Length > 11)
					return new ValidationFailure("NIF", Texts.NIFFailedRequirement);

				bool NifValido = IsValidContrib(data.Nif);
				if(!NifValido)
					return new ValidationFailure("NIF", Texts.NIFFailedRequirement);

                bool nifUnico = NifIsUnique(data.Nif);
                if (!nifUnico && data.IsFromCreate)
                    return new ValidationFailure("NIF", Texts.NifJaExiste);
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