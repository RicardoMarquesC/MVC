namespace eBillingSuite.Model.EBC_DB
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class EBC_DB : DbContext, IeBillingSuiteEBCDBContext
	{
		public EBC_DB()
			: base("name=EBC_DB")
		{
		}

		public virtual DbSet<ComATPackets> ComATPackets { get; set; }
		public virtual DbSet<ComATPackets_Guias> ComATPackets_Guias { get; set; }
		public virtual DbSet<ConfigEnvioAT> ConfigEnvioAT { get; set; }
		public virtual DbSet<DocumentosErros> DocumentosErros { get; set; }
		public virtual DbSet<EBC_CertSignatureDetails> EBC_CertSignatureDetails { get; set; }
		public virtual DbSet<EBC_Config> EBC_Config { get; set; }
		public virtual DbSet<EBC_CustomerMetadata> EBC_CustomerMetadata { get; set; }
		public virtual DbSet<EBC_CustomerPackages> EBC_CustomerPackages { get; set; }
		public virtual DbSet<EBC_Customers> EBC_Customers { get; set; }
		public virtual DbSet<EBC_EmailContent> EBC_EmailContent { get; set; }
		public virtual DbSet<EBC_InstanceDeniedSenders> EBC_InstanceDeniedSenders { get; set; }
		public virtual DbSet<EBC_InstanceMetadata> EBC_InstanceMetadata { get; set; }
		public virtual DbSet<EBC_InstancePackages> EBC_InstancePackages { get; set; }
		public virtual DbSet<EBC_Instances> EBC_Instances { get; set; }
		public virtual DbSet<EBC_MercadoCert> EBC_MercadoCert { get; set; }
		public virtual DbSet<EBC_Mercados> EBC_Mercados { get; set; }
		public virtual DbSet<EBC_Metadata> EBC_Metadata { get; set; }
		public virtual DbSet<EBC_PackageEvents> EBC_PackageEvents { get; set; }
		public virtual DbSet<EBC_Packages> EBC_Packages { get; set; }
		public virtual DbSet<EBC_SpecificDeliveryOptions> EBC_SpecificDeliveryOptions { get; set; }
		public virtual DbSet<EBC_XML> EBC_XML { get; set; }
		public virtual DbSet<EBC_XMLClient> EBC_XMLClient { get; set; }
		public virtual DbSet<EBC_XMLHeadInbound> EBC_XMLHeadInbound { get; set; }
		public virtual DbSet<EBC_XMLInbound> EBC_XMLInbound { get; set; }
		public virtual DbSet<EBC_XMLLines> EBC_XMLLines { get; set; }
		public virtual DbSet<EBC_XMLLinesInbound> EBC_XMLLinesInbound { get; set; }
		public virtual DbSet<EBC_XMLResumoIVAInbound> EBC_XMLResumoIVAInbound { get; set; }
		public virtual DbSet<EBC_XmlToTxtTransform> EBC_XmlToTxtTransform { get; set; }
		public virtual DbSet<InboundPacketInfoObjectProperties> InboundPacketInfoObjectProperties { get; set; }
		public virtual DbSet<LoginAT> LoginAT { get; set; }
		public virtual DbSet<ManutencaoSistema> ManutencaoSistema { get; set; }
		public virtual DbSet<TipoRegexs> TipoRegexs { get; set; }
		public virtual DbSet<TiposRegioesFatura> TiposRegioesFatura { get; set; }
		public virtual DbSet<xmlTemplate> xmlTemplate { get; set; }
		public virtual DbSet<EBC_ConfigTXT> EBC_ConfigTXT { get; set; }
		public virtual DbSet<EBC_XMLResumoIVA> EBC_XMLResumoIVA { get; set; }
		public virtual DbSet<MapeamentoXML> MapeamentoXML { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }
        public virtual DbSet<EBC_UnknownList> EBC_UnknownList { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EBC_Config>()
				.Property(e => e.KeyName)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Config>()
				.Property(e => e.KeyValue)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Config>()
				.Property(e => e.ConfigSuiteType)
				.IsFixedLength()
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Customers>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Customers>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Customers>()
				.Property(e => e.NIF)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_EmailContent>()
				.Property(e => e.Subject)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Instances>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Instances>()
				.HasMany(e => e.EBC_CertSignatureDetails)
				.WithRequired(e => e.EBC_Instances)
				.HasForeignKey(e => e.fkInstance)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<EBC_Mercados>()
				.HasMany(e => e.EBC_CertSignatureDetails)
				.WithRequired(e => e.EBC_Mercados)
				.HasForeignKey(e => e.fkMercado)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<EBC_Metadata>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Metadata>()
				.Property(e => e.DefaultValue)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Packages>()
				.Property(e => e.ApplicationName)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_Packages>()
				.Property(e => e.MessageID)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_SpecificDeliveryOptions>()
				.Property(e => e.NotificationEmailTecnical)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_SpecificDeliveryOptions>()
				.Property(e => e.NotificationEmailFunctional)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_SpecificDeliveryOptions>()
				.Property(e => e.NotificationEmailMonitoring)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_XML>()
				.Property(e => e.Element)
				.IsUnicode(false);

			modelBuilder.Entity<TipoRegexs>()
				.Property(e => e.TipoRegex)
				.IsUnicode(false);

			modelBuilder.Entity<EBC_XMLResumoIVA>()
				.Property(e => e.Element)
				.IsUnicode(false);
		}
	}
}
