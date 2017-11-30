using Shortcut.Repositories;
using eBillingSuite.Enumerations;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
    public interface IDigitalConfigurationsRepository : IGenericRepository<IeBillingSuiteConfigurationsContext, DigitalConfigurations>
	{
		object GetConfigurationByKey(string keyName);

		void SaveSetting(DigitalSettingKeys settingKey, object value);

        string GetFilePath(string filename);
    }
}
