using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using System.Data.Entity;

namespace eBillingSuite.Model.EDI_DB
{
	public interface IeBillingSuiteEDIDBContext : IDbContext
	{
		Database Database { get; }

		System.Data.Entity.DbSet<Clientes> Clientes { get; set; }
		System.Data.Entity.DbSet<InboundPacket> InboundPacket { get; set; }
		System.Data.Entity.DbSet<Instancias> Instancias { get; set; }
		System.Data.Entity.DbSet<OutboundPacket> OutboundPacket { get; set; }
		System.Data.Entity.DbSet<Remetentes> Remetentes { get; set; }
		System.Data.Entity.DbSet<InboundAllowanceCharge> InboundAllowanceCharge { get; set; }
		System.Data.Entity.DbSet<InboundLineItens> InboundLineItens { get; set; }
		System.Data.Entity.DbSet<InboundResumoIVA> InboundResumoIVA { get; set; }
		System.Data.Entity.DbSet<OutboundLineItens> OutboundLineItens { get; set; }
		System.Data.Entity.DbSet<OutboundProcesses> OutboundProcesses { get; set; }
		System.Data.Entity.DbSet<OutboundResumoIVA> OutboundResumoIVA { get; set; }
	}
}
