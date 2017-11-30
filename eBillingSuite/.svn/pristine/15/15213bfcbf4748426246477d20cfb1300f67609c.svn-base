using Ninject;
using Shortcut.Repositories;
using System;
using System.Linq;
using eBillingSuite.Enumerations;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Model.eBillingConfigurations;
using System.IO;

namespace eBillingSuite.Repositories
{
    public class DigitalConfigurationsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, DigitalConfigurations>, IDigitalConfigurationsRepository
    {
		[Inject]
		public DigitalConfigurationsRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
		}

		public object GetConfigurationByKey(string configName)
		{
            var configuration = this.Context.DigitalConfigurations.FirstOrDefault(x => x.Nome == configName.ToString());

            if (configuration == null)
                return null;

            if (configuration.Tipo == "Boolean")
            {
                var value = false;
                bool.TryParse(configuration.Dados, out value);

                return value;
            }
            else if (configuration.Tipo == "Int")
            {
                var value = 0;
                int.TryParse(configuration.Dados, out value);

                return value;
            }
            else
                return configuration.Dados;
        }

		/// <summary>
		/// Gravação de Settings
		/// </summary>
		/// <param name="settingKey"></param>
		/// <param name="value"></param>
		public void SaveSetting(DigitalSettingKeys settingKey, object value)
		{
			var setting = Set.FirstOrDefault(x => x.Nome.ToLower() == settingKey.ToString().ToLower());
			if (setting == null)
			{
				setting = new DigitalConfigurations
                {
					Pkid = Guid.NewGuid(),
					Nome = settingKey.ToString(),
                    Dados = value.ToString()
				};
				Add(setting).Save();
			}
			else
			{
				setting.Dados = value.ToString();
				Edit(setting).Save();
			}
		}

        public enum ConfigurationsNames
        {
            anexos,
            anexosConcat,
            CaminhoFilesScanner,
            ConfigDigitalizationModule,
            DataInstalacao,
            DigitalizationActive,
            DigitalizationEXE,
            DocAtraso,
            eBDProcess,
            Eliminadas,
            EmailCert,
            EmRede,
            Expiradas,
            FinalDocumentCreationInServer,
            gestaoDocIntegracao,
            gestaoDocIntegracaoEnviaTiff,
            gestaoDocIntegracaoSequencial,
            gestaoDocIntegracaoURL,
            GhostScript,
            HasDigitalizationControl,
            Imagens,
            ImagensErro,
            ImagensSep,
            InsertNewSuppliersForNonGenericDocTypes,
            Instalado,
            IntegracaoBDIntegraUBL,
            IntegracaoCaminhoFS,
            IntegracaoErp,
            IntegracaoGestaoDoc,
            IntegracaoInstanciaIntegraUBL,
            IntegracaoPassIntegraUBL,
            IntegracaoPIQuer,
            IntegracaoUserIntegraUBL,
            IntegrationErrorTiffs,
            LogConfig,
            LogFileImageProc,
            LogFileImageProcOCR,
            LogFileProcessamento,
            LogFilesDigital,
            LogFileSeparacao,
            ManualUtilizador,
            NaoIdentificadas,
            NextPageCounter,
            NextPageTimeStamp,
            PastaAIRC1,
            PastaAIRC2,
            PastaEBC,
            PastaPartilha,
            PDFConvertedPath,
            PDFPath,
            pdfResolution,
            Processar,
            ProcessarMODI,
            ReceivedPath,
            SaltarTratamento,
            SincFornecedoresWS,
            SincFornecedoresWSPass,
            SincFornecedoresWSUser,
            Temps,
            Thumbnails,
            ThumbnailsSep,
            TIF,
            TIFaSeparar,
            TIFSEPARADOR,
            TIFsSeparados,
            TIFtemp,
            TXT,
            TXTEmail,
            Validar,
            XML
        }

        public string GetFilePath(string filename)
        {
            if (filename.ToLower().EndsWith("xml"))
            {
                if (File.Exists(System.IO.Path.Combine(GetConfigurationByKey(ConfigurationsNames.XML.ToString()).ToString(), filename)))
                    return System.IO.Path.Combine(GetConfigurationByKey(ConfigurationsNames.XML.ToString()).ToString(), filename);
                return String.Empty;
            }
            else
            {
                if (File.Exists(System.IO.Path.Combine(GetConfigurationByKey(ConfigurationsNames.Processar.ToString()).ToString(), filename)))
                    return System.IO.Path.Combine(GetConfigurationByKey(ConfigurationsNames.Processar.ToString()).ToString(), filename);
                return String.Empty;
            }
        }
    }
}
