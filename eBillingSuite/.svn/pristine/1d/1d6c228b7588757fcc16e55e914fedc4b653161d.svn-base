using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace eBillingSuite.Repositories
{
	public class SuiteConfigurationsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, EBC_Configurations>, ISuiteConfigurationsRepository
	{
		[Inject]
		public SuiteConfigurationsRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
		}

		public string ConfigValue(string name)
		{
			return this
				.Where(cv => cv.Name == name)
				.FirstOrDefault()
				.Data;
		}
	}
}
