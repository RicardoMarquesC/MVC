using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class InstancesDeniedSendersRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_InstanceDeniedSenders>, IInstancesDeniedSendersRepository
	{
		[Inject]
		public InstancesDeniedSendersRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public bool SenderIsAllowedByInstanceId(Guid instanceId, string senderNif)
		{
			return (this.Set.Any(x => x.fkInstance == instanceId && x.senderNIF.ToLower() == senderNif.ToLower()));
		}


		public void UpdateAllowedSendersByInstanceId(Guid instanceId, List<string> whitelistEntries)
		{
			// delete all
			var actualAllowedSenders = this.Set.Where(x => x.fkInstance == instanceId).ToList();

			this.Set.RemoveRange(actualAllowedSenders);

			actualAllowedSenders.Clear();

			// insert
			if (whitelistEntries != null && whitelistEntries.Count > 0)
			{
				foreach (string allowedSenderNif in whitelistEntries)
				{
					EBC_InstanceDeniedSenders instanceAllowedSender = new EBC_InstanceDeniedSenders
					{
						pkid = Guid.NewGuid(),
						fkInstance = instanceId,
						senderNIF = allowedSenderNif
					};
					actualAllowedSenders.Add(instanceAllowedSender);
				}

				this.Set.AddRange(actualAllowedSenders);
			}

			this.Save();
		}
	}
}
