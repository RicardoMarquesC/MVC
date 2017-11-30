[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(eBillingSuite.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(eBillingSuite.App_Start.NinjectWebCommon), "Stop")]

namespace eBillingSuite.App_Start
{
	using FluentValidation.Mvc;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Ninject;
	using Ninject.Web.Common;
	using System;
	using System.Web;
	using System.Web.Mvc;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
			kernel.Load<CoreModule>();
			kernel.Load<HostMVCModule>();

			FluentValidationModelValidatorProvider.Configure(provider => 
			{ provider.ValidatorFactory = new FluentValidationConfig(kernel); });

			//ModelMetadataProviders.Current = new eBillingSuite.Globalization.DataAnnotationsModelMetadataProvider(kernel);

			ViewExtensions.SetKernel(kernel);
        }
	}
}

namespace eBillingSuite
{
	using eBillingSuite.Globalization;
	using Ninject;
	using System.Web.Mvc;

	public static class ViewExtensions
	{
		private static IKernel _kernel;

		public static void SetKernel(IKernel kernel)
		{
			_kernel = kernel;
		}

		//public static string Get<T>(this WebViewPage<T> view, DictionaryEntryKeys key)
		//{
		//	return _kernel.Get<IeBillingSuiteRequestContext>().GetDictionaryValue(key);
		//}

		//public static string Get<T>(this WebViewPage view, string value)
		//{
		//	return _kernel.Get<IeBillingSuiteRequestContext>().GetDictionaryValue(string.Format("{0}_{1}", typeof(T).Name, value));
		//}

		public static IeBillingSuiteRequestContext GetContext<T>(this WebViewPage<T> view)
		{
			return _kernel.Get<IeBillingSuiteRequestContext>();
		}

		public static string GetjQDataTableLanguageUrl<T>(this WebViewPage<T> view)
		{
			return view.Url.Content("~/assets/SH/portuguese.json");
		}
	}
}
