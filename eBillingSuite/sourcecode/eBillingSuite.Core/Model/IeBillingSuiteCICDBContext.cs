using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using System.Data.Entity;

namespace eBillingSuite.Model.CIC_DB
{
	public interface IeBillingSuiteCICDBContext : IDbContext
	{
		Database Database { get; }
		System.Data.Entity.DbSet<InboundIVA> InboundIVA { get; set; }
		System.Data.Entity.DbSet<InboundLineItems> InboundLineItems { get; set; }
		System.Data.Entity.DbSet<InboundPacket> InboundPacket { get; set; }
		System.Data.Entity.DbSet<IntegrationFilters> IntegrationFilters { get; set; }
		System.Data.Entity.DbSet<OutboundIVA> OutboundIVA { get; set; }
		System.Data.Entity.DbSet<OutboundLineItems> OutboundLineItems { get; set; }
		System.Data.Entity.DbSet<OutboundPacket> OutboundPacket { get; set; }
		System.Data.Entity.DbSet<OutboundProcesses> OutboundProcesses { get; set; }
		System.Data.Entity.DbSet<TipoNomenclaturaSender> TipoNomenclaturaSender { get; set; }
		System.Data.Entity.DbSet<TiposNomenclaturaPDF> TiposNomenclaturaPDF { get; set; }
		System.Data.Entity.DbSet<Whitelist> Whitelist { get; set; }

	}
}
