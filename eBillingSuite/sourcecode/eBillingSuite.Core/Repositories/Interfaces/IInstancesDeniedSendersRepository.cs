using System;
using System.Collections.Generic;
using Shortcut.Repositories;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public interface IInstancesDeniedSendersRepository : IGenericRepository<IeBillingSuiteEBCDBContext, EBC_InstanceDeniedSenders>
	{
		bool SenderIsAllowedByInstanceId(Guid instanceId, string senderNif);

		void UpdateAllowedSendersByInstanceId(Guid instanceId, List<string> whitelistEntries);
	}
}
