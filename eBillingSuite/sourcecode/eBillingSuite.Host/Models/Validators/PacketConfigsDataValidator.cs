using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class PacketConfigsDataValidator : AbstractValidator<PacketConfigsData>
	{
		private IeBillingSuiteRequestContext _context;

		[Inject]
		public PacketConfigsDataValidator(IeBillingSuiteRequestContext context)
		{
			_context = context;
		}
	}


    public class TicketsDataValidator: AbstractValidator<TicketsData>
    {
        private IeBillingSuiteRequestContext _context;

        [Inject]
        public TicketsDataValidator(IeBillingSuiteRequestContext context)
        {
            _context = context;
        }
    }
}