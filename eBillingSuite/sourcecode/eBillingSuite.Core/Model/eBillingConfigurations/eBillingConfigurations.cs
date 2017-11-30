namespace eBillingSuite.Model.eBillingConfigurations
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class eBillingConfigurations : DbContext, IeBillingSuiteConfigurationsContext
	{
		public eBillingConfigurations()
			: base("name=eBillingConfigurations")
		{
		}

		public virtual DbSet<EBC_Configurations> EBC_Configurations { get; set; }
		public virtual DbSet<eSuite_Produtos> eSuite_Produtos { get; set; }
		public virtual DbSet<eSuitePermissions> eSuitePermissions { get; set; }
		public virtual DbSet<eSuiteUserPermissions> eSuiteUserPermissions { get; set; }
        public virtual DbSet<eBC_Login> eBC_Login { get; set; }
		public virtual DbSet<PIInfo> PIInfo { get; set; }
		public virtual DbSet<Produtos> Produtos { get; set; }
		public virtual DbSet<SaphetyCredentials> SaphetyCredentials { get; set; }
		public virtual DbSet<SSME_Configurations> SSME_Configurations { get; set; }
        public virtual DbSet<DigitalConfigurations> DigitalConfigurations { get; set; }
        public virtual DbSet<UserGraphs> UserGraphs { get; set; }
 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<eSuitePermissions>()
				.HasMany(e => e.eSuiteUserPermissions)
				.WithRequired(e => e.eSuitePermissions)
				.HasForeignKey(e => e.FKPermissions)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<SaphetyCredentials>()
				.Property(e => e.instance)
				.IsUnicode(false);
		}
	}
}