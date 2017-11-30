﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.EDI_DB;

namespace eBillingSuite.Repositories
{
	public interface IEEDISentDocsRepository : IGenericRepository<IeBillingSuiteEDIDBContext, OutboundPacket>
	{
		List<OutboundPacket> GetOutboundPackets(string pesquisa);
	}
}