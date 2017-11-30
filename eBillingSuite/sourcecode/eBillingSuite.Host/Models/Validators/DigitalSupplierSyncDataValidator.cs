using eBillingSuite.Repositories;
using FluentValidation;
using Ninject;

namespace eBillingSuite.Models.Validators
{
	public class DigitalSupplierSyncDataValidator : AbstractValidator<DigitalSupplierSyncData>
	{
		[Inject]
		public DigitalSupplierSyncDataValidator()
		{

			RuleFor(c => c.SyncUrlConfig)
				.NotEmpty()
				.When(c => c.WantSync);

			RuleFor(c => c.SyncUserConfig)
				.NotEmpty()
				.When(c => c.WantSync);

			RuleFor(c => c.SyncPassConfig)
				.NotEmpty()
				.When(c => c.WantSync);
		}
	}
}