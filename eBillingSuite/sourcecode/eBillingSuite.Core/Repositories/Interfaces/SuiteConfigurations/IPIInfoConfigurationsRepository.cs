﻿using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
	public interface IPIInfoConfigurationsRepository : IGenericRepository<IeBillingSuiteConfigurationsContext, PIInfo>
	{
	}
}