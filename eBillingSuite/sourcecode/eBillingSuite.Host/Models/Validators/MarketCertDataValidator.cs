using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System.Collections.Generic;
using System.IO;

namespace eBillingSuite.Models.Validators
{
	public class MarketCertDataValidator : AbstractValidator<MarketCertData>
	{
		private IeBillingSuiteRequestContext _context;
		private static List<string> allowed = new List<string> { ".cer", ".pfx", ".p7b" };

		[Inject]
		public MarketCertDataValidator(IeBillingSuiteRequestContext context)
		{
			_context = context;
			
		}		
	}
}