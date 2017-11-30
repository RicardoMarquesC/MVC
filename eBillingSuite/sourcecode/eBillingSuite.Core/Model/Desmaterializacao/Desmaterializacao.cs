namespace eBillingSuite.Model.Desmaterializacao
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class Desmaterializacao : DbContext, IeBillingSuiteDesmaterializacaoContext
	{
		public Desmaterializacao()
			: base("name=Desmaterializacao")
		{
		}

		public virtual DbSet<DadosTemplate> DadosTemplate { get; set; }
		public virtual DbSet<DocumentoXML> DocumentoXML { get; set; }
		public virtual DbSet<ExpirarFactura> ExpirarFactura { get; set; }
		public virtual DbSet<Fornecedores> Fornecedores { get; set; }
		public virtual DbSet<NomeTemplate> NomeTemplate { get; set; }
		public virtual DbSet<PermissionType> PermissionType { get; set; }
		public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
		public virtual DbSet<TipoFacturaDadosXML> TipoFacturaDadosXML { get; set; }
		public virtual DbSet<TipoFacturas> TipoFacturas { get; set; }
		public virtual DbSet<Users> Users { get; set; }
		public virtual DbSet<UsersPermissions> UsersPermissions { get; set; }
		public virtual DbSet<AlertasSite> AlertasSite { get; set; }
		public virtual DbSet<AplicationLayouts> AplicationLayouts { get; set; }
		public virtual DbSet<Cabecalho> Cabecalho { get; set; }
		public virtual DbSet<CamposXML> CamposXML { get; set; }
		public virtual DbSet<ConfigResolutions> ConfigResolutions { get; set; }
		public virtual DbSet<DicionarioPalavras> DicionarioPalavras { get; set; }
		public virtual DbSet<FactProcessStatus> FactProcessStatus { get; set; }
		public virtual DbSet<FacturasNIdent> FacturasNIdent { get; set; }
		public virtual DbSet<Linhas> Linhas { get; set; }
		public virtual DbSet<MasterizacaoAnexos> MasterizacaoAnexos { get; set; }
		public virtual DbSet<MasterizacaoCabecalho> MasterizacaoCabecalho { get; set; }
		public virtual DbSet<MasterizacaoIva> MasterizacaoIva { get; set; }
		public virtual DbSet<MasterizacaoLineItems> MasterizacaoLineItems { get; set; }
		public virtual DbSet<Moedas> Moedas { get; set; }
		public virtual DbSet<ResumoIva> ResumoIva { get; set; }
		public virtual DbSet<TaxasIva> TaxasIva { get; set; }
		public virtual DbSet<TextosSite> TextosSite { get; set; }
        public virtual DbSet<Instances> Instances { get; set; }
        public virtual DbSet<InstancesMail> InstancesMail { get; set; }
        public virtual DbSet<ProcDocs> ProcDocs { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PermissionType>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<PermissionType>()
				.HasMany(e => e.UsersPermissions)
				.WithRequired(e => e.PermissionType)
				.HasForeignKey(e => e.FKPermission)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Users>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<Users>()
				.Property(e => e.Login)
				.IsUnicode(false);

			modelBuilder.Entity<Users>()
				.HasMany(e => e.UsersPermissions)
				.WithRequired(e => e.Users)
				.HasForeignKey(e => e.FKUser)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Moedas>()
				.Property(e => e.Simbolo)
				.IsFixedLength();

			modelBuilder.Entity<TaxasIva>()
				.Property(e => e.Taxa)
				.HasPrecision(4, 2);
		}
	}
}
