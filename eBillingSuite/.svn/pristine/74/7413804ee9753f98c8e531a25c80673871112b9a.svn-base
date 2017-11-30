namespace eBillingSuite.Model.EDI_DB
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class EDI_DB : DbContext, IeBillingSuiteEDIDBContext
	{
		public EDI_DB()
			: base("name=EDI_DB")
		{
		}

		public virtual DbSet<Clientes> Clientes { get; set; }
		public virtual DbSet<InboundPacket> InboundPacket { get; set; }
		public virtual DbSet<Instancias> Instancias { get; set; }
		public virtual DbSet<OutboundPacket> OutboundPacket { get; set; }
		public virtual DbSet<Remetentes> Remetentes { get; set; }
		public virtual DbSet<InboundAllowanceCharge> InboundAllowanceCharge { get; set; }
		public virtual DbSet<InboundLineItens> InboundLineItens { get; set; }
		public virtual DbSet<InboundResumoIVA> InboundResumoIVA { get; set; }
		public virtual DbSet<OutboundLineItens> OutboundLineItens { get; set; }
		public virtual DbSet<OutboundProcesses> OutboundProcesses { get; set; }
		public virtual DbSet<OutboundResumoIVA> OutboundResumoIVA { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Clientes>()
				.Property(e => e.Nome)
				.IsUnicode(false);

			modelBuilder.Entity<Clientes>()
				.Property(e => e.NIF)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.PKProcessID)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NomeEmissor)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NIFemissor)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NomeReceptor)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NIFreceptor)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.NumFactura)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.DataFactura)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.Ficheiro)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.DocOriginal)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.Moeda)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.FicheiroEstadoSAP)
				.IsUnicode(false);

			modelBuilder.Entity<InboundPacket>()
				.Property(e => e.FicheiroResposta)
				.IsUnicode(false);

			modelBuilder.Entity<Instancias>()
				.Property(e => e.Nome)
				.IsUnicode(false);

			modelBuilder.Entity<Instancias>()
				.HasMany(e => e.Clientes)
				.WithRequired(e => e.Instancias)
				.HasForeignKey(e => e.FKInstanciaID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.DMSID)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NomeReceptor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.URLReceptor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NIFReceptor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NomeEmissor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NIFEmissor)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.NumFactura)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.DataFactura)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.Ficheiro)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.DocOriginal)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.Moeda)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.TaxaCambio)
				.HasPrecision(10, 5);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.CondicaoPagamento)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.SWIFT)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.IBAN)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.TipoDocumento)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.Estado)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.Pasta)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundPacket>()
				.Property(e => e.TaxaIVA)
				.HasPrecision(13, 2);

			modelBuilder.Entity<Remetentes>()
				.Property(e => e.URL)
				.IsUnicode(false);

			modelBuilder.Entity<Remetentes>()
				.Property(e => e.Nome)
				.IsUnicode(false);

			modelBuilder.Entity<Remetentes>()
				.Property(e => e.NIF)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundLineItens>()
				.Property(e => e.TaxaIVA)
				.HasPrecision(13, 2);

			modelBuilder.Entity<OutboundProcesses>()
				.Property(e => e.FicheiroProcessado)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundProcesses>()
				.Property(e => e.FicheiroOriginal)
				.IsUnicode(false);

			modelBuilder.Entity<OutboundResumoIVA>()
				.Property(e => e.TaxaIVA)
				.HasPrecision(13, 2);
		}
	}
}
