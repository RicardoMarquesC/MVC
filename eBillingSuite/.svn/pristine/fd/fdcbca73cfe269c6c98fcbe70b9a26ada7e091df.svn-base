using eBillingSuite.Models;
using eBillingSuite.Models.Validators;
using eBillingSuite.Security;
using eBillingSuite.Repositories;
using FluentValidation;
using Ninject.Modules;
using Ninject.Web.Common;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleCrypto;
using eBillingSuite.HelperTools.Interfaces;
using System.Web.Configuration;
using eBillingSuite.Repositories.Interfaces;

namespace eBillingSuite
{
	public class HostMVCModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IPixelAdminPageContext>()
				.To<EBillingSuitePixelAdminPageContext>()
				.InRequestScope();

            bool usesAD = bool.Parse(WebConfigurationManager.AppSettings["UsesAD"].ToString());

            if(usesAD)
            {
                Bind<IeBillingSuiteIdentity>()
                //.ToMethod((ai) => IeBillingSuiteIde eBillingSuiteIdentity.TryCreateFromWindowsCredentials(IPermissionsRepository))
                    .To<eBillingSuiteIdentity>()
                    .InRequestScope();

                Bind<IeBillingSuiteRequestContext>()
                .To<EBillingSuiteRequestContext>()
                         .InRequestScope();
                //.WithConstructorArgument("userIdentity", (i, o) => HttpContext.Current.User.Identity as IeBillingSuiteIdentity);

            }
            else
            {
               // Bind<IeBillingSuiteIdentity>()
               //     .ToMethod((ai) => IeBillingSuiteIdentity.TryCreateFromWindowsCredentials(IPermissionsRepository))
               ////.To<eBillingSuiteIdentity>()
               //.InRequestScope();

                Bind<IeBillingSuiteRequestContext>()
                .To<EBillingSuiteRequestContext>()
                         .InRequestScope()
                            .WithConstructorArgument("userIdentity", (i, o) => HttpContext.Current.User.Identity as IeBillingSuiteIdentity);
            }


            Bind<IECCListRepositories>()
				.To<ECListRepositories>()
				.InRequestScope();

            Bind<IOutboundInboundRepository>()
                .To<OutboundInboundRepository>()
                .InRequestScope();

            // VALIDATORS BINDING
            Bind<IValidator<CredentialsData>>()
				.To<CredentialsDataValidator>()
				.InRequestScope();

            Bind<IValidator<LoginData>>()
                .To<LoginDataValidator>()
                .InRequestScope();

			Bind<IValidator<MCATConfigSendInfoData>>()
				.To<MCATConfigSendInfoDataValidator>()
				.InRequestScope();

			Bind<IValidator<EDICostumerData>>()
				.To<EDICostumerDataValidator>()
				.InRequestScope();
			
			Bind<IValidator<EDISenderData>>()
				.To<EDISenderDataValidator>()
				.InRequestScope();

			Bind<IValidator<DigitalSupplierData>>()
				.To<DigitalSupplierDataValidator>()
				.InRequestScope();

			Bind<IValidator<DigitalCreateSupplierData>>()
				.To<DigitalCreateSupplierDataValidator>()
				.InRequestScope();

			Bind<IValidator<DigitalSupplierSyncData>>()
				.To<DigitalSupplierSyncDataValidator>()
				.InRequestScope();

			Bind<IValidator<DigitalDocumentTypeData>>()
				.To<DigitalDocumentTypeDataValidator>()
				.InRequestScope();

			Bind<IValidator<DigitalXmlFieldData>>()
				.To<DigitalXmlFieldDataValidator>()
				.InRequestScope();

			Bind<IValidator<MarketCertData>>()
				.To<MarketCertDataValidator>()
				.InRequestScope();

			Bind<IValidator<PacketConfigsData>>()
				.To<PacketConfigsDataValidator>()
				.InRequestScope();

			Bind<IValidator<ConfigTXTData>>()
				.To<ConfigTXTDataValidator>()
				.InRequestScope();

			Bind<IValidator<CustomerData>>()
				.To<CustomerDataValidator>()
				.InRequestScope();

			Bind<IValidator<XmlConfigData>>()
				.To<XmlConfigDataValidator>()
				.InRequestScope();

			Bind<IValidator<SenderData>>()
				.To<SenderDataValidator>()
				.InRequestScope();

            Bind<IValidator<SignInData>>()
                .To<SignInDatataValidator>()
                .InRequestScope();
        }
	}
}