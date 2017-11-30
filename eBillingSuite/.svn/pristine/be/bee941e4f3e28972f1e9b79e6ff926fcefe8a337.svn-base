using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using System.Data.Entity;

namespace eBillingSuite.Model.eBillingConfigurations
{
	public interface IeBillingSuiteConfigurationsContext : IDbContext
	{
		Database Database { get; }

		System.Data.Entity.DbSet<EBC_Configurations> EBC_Configurations { get; set; }
		System.Data.Entity.DbSet<eSuite_Produtos> eSuite_Produtos { get; set; }
		System.Data.Entity.DbSet<eSuitePermissions> eSuitePermissions { get; set; }
		System.Data.Entity.DbSet<eSuiteUserPermissions> eSuiteUserPermissions { get; set; }
		System.Data.Entity.DbSet<eBC_Login> eBC_Login { get; set; }
        System.Data.Entity.DbSet<PIInfo> PIInfo { get; set; }
		System.Data.Entity.DbSet<Produtos> Produtos { get; set; }
		System.Data.Entity.DbSet<SaphetyCredentials> SaphetyCredentials { get; set; }
		System.Data.Entity.DbSet<SSME_Configurations> SSME_Configurations { get; set; }
        System.Data.Entity.DbSet<DigitalConfigurations> DigitalConfigurations { get; set; }
        System.Data.Entity.DbSet<UserGraphs> UserGraphs { get; set; }
    }
}
