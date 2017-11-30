using eBillingSuite.Repositories;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class DigitalSupplierDataValidator : AbstractValidator<DigitalSupplierData>
	{
		private IeBillingSuiteRequestContext _context;
		private IEDigitalSuppliersRepository _eDigitalSuppliersRepository;
		
		[Inject]
		public DigitalSupplierDataValidator(IeBillingSuiteRequestContext context,
			IEDigitalSuppliersRepository eDigitalSuppliersRepository)
		{
			_context = context;
			_eDigitalSuppliersRepository = eDigitalSuppliersRepository;

			RuleFor(c => c.Nome)
				.NotEmpty();

			RuleFor(c => c.Contribuinte)
				.NotEmpty();
		}
	}
}