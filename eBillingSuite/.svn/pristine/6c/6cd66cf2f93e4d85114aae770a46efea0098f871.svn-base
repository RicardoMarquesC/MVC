namespace eBillingSuite.Model.CIC_DB
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class CIC_DB : DbContext, IeBillingSuiteCICDBContext
	{
		public CIC_DB()
			: base("name=CIC_DB")
		{
		}

		public virtual DbSet<InboundIVA> InboundIVA { get; set; }
		public virtual DbSet<InboundLineItems> InboundLineItems { get; set; }
		public virtual DbSet<InboundPacket> InboundPacket { get; set; }
		public virtual DbSet<IntegrationFilters> IntegrationFilters { get; set; }
		public virtual DbSet<OutboundIVA> OutboundIVA { get; set; }
		public virtual DbSet<OutboundLineItems> OutboundLineItems { get; set; }
		public virtual DbSet<OutboundPacket> OutboundPacket { get; set; }
		public virtual DbSet<OutboundProcesses> OutboundProcesses { get; set; }
		public virtual DbSet<TipoNomenclaturaSender> TipoNomenclaturaSender { get; set; }
		public virtual DbSet<TiposNomenclaturaPDF> TiposNomenclaturaPDF { get; set; }
		public virtual DbSet<Whitelist> Whitelist { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<InboundLineItems>()
				.Property(e => e.NotaEncomenda)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.PKProcessID)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NIF)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NumEncomenda)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.DataFactura)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.DigitalInfoFilename)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.SubmissionFile)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.AutoResponseFile)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NIFE)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.DocOriginal)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NomeFornec)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.HasMany(e => e.InboundIVA)
				.WithRequired(e => e.InboundPacket)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<InboundPacket>()
				.HasMany(e => e.InboundLineItems)
				.WithRequired(e => e.InboundPacket)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<IntegrationFilters>()
				.Property(e => e.FriendlyName)
				.IsUnicode(false);

			modelBuilder.Entity<IntegrationFilters>()
				.Property(e => e.TypeName)
				.IsUnicode(false);

			modelBuilder.Entity<IntegrationFilters>()
				.HasMany(e => e.Whitelist)
				.WithRequired(e => e.IntegrationFilters)
				.HasForeignKey(e => e.FKIntegrationFilterID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<OutboundLineItems>()
				.Property(e => e.TaxaIVA)
				.HasPrecision(13, 2);

			modelBuilder.Entity<OutboundLineItems>()
				.Property(e => e.NotaEncomenda)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.DMSID)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NomeReceptor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.EmailReceptor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NIFReceptor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NumFactura)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.DataFactura)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.DigitalInfoFileName)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.CurrentEBCState)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.Source)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.DocOriginal)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NomeEmissor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NIFEmissor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundProcesses>()
				.Property(e => e.ProcessedFilename)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundProcesses>()
				.Property(e => e.OriginalFileName)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundProcesses>()
				.HasMany(e => e.OutboundIVA)
				.WithRequired(e => e.OutboundProcesses)
				.HasForeignKey(e => e.FKOutboundProcessID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<OutboundProcesses>()
				.HasMany(e => e.OutboundLineItems)
				.WithRequired(e => e.OutboundProcesses)
				.HasForeignKey(e => e.FKOutboundProcessID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<OutboundProcesses>()
				.HasMany(e => e.OutboundPacket)
				.WithOptional(e => e.OutboundProcesses)
				.HasForeignKey(e => e.FKOutboundProcessID);

			modelBuilder.Entity<TiposNomenclaturaPDF>()
				.HasMany(e => e.TipoNomenclaturaSender)
				.WithRequired(e => e.TiposNomenclaturaPDF)
				.HasForeignKey(e => e.FKtiponomenclatura)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Whitelist>()
				.Property(e => e.EmailAddress)
				.IsUnicode(false);

			modelBuilder.Entity<Whitelist>()
				.Property(e => e.EmailName)
				.IsUnicode(false);

			modelBuilder.Entity<Whitelist>()
				.HasMany(e => e.InboundPacket)
				.WithOptional(e => e.Whitelist)
				.HasForeignKey(e => e.FKWhiteListID);
		}
	}
}
