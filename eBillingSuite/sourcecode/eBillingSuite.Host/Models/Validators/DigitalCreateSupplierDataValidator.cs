using eBillingSuite.Repositories;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class DigitalCreateSupplierDataValidator : AbstractValidator<DigitalCreateSupplierData>
	{
		private IeBillingSuiteRequestContext _context;
		private IEDigitalSuppliersRepository _eDigitalSuppliersRepository;
		
		[Inject]
		public DigitalCreateSupplierDataValidator(IeBillingSuiteRequestContext context,
			IEDigitalSuppliersRepository eDigitalSuppliersRepository)
		{
			_context = context;
			_eDigitalSuppliersRepository = eDigitalSuppliersRepository;

			RuleFor(c => c.Nome)
				.NotEmpty();

			RuleFor(c => c.Contribuinte)
				.NotEmpty();
				//.Must(NifIsUnique);
		}

		//private bool NifIsUnique(string nif)
		//{
		//	return !_eDigitalSuppliersRepository.ExistsNif(nif);
		//}
	}
}