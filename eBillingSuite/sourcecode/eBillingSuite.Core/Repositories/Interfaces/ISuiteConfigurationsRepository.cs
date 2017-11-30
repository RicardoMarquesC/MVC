using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
	public interface ISuiteConfigurationsRepository : IGenericRepository<IeBillingSuiteConfigurationsContext, EBC_Configurations>
	{
		string ConfigValue(string name);
	}
}
