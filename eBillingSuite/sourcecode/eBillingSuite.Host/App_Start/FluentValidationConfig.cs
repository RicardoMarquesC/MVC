using FluentValidation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.App_Start
{
	public class FluentValidationConfig : ValidatorFactoryBase
	{
		private IKernel _kernel;

		public FluentValidationConfig(IKernel kernel)
		{
			_kernel = kernel;
		}

		public override IValidator CreateInstance(Type validatorType)
		{
           // IValidator teste = _kernel.TryGet(validatorType) as IValidator;

            return (validatorType == null) ? null : (IValidator)_kernel.TryGet(validatorType);
		}
	}
}