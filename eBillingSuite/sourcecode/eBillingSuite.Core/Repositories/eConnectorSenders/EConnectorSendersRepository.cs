using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.CIC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorSendersRepository : GenericRepository<IeBillingSuiteCICDBContext, Whitelist>, IEConnectorSendersRepository
	{
		[Inject]
		public EConnectorSendersRepository(IeBillingSuiteCICDBContext context)
			: base(context)
		{
		}

		public bool IsNifUnique(string nif)
		{
			return !(this.Set.Any(wl => wl.NIF.ToLower().Contains(nif.ToLower())));
		}

		public string GetSenderNifById(Guid senderId)
		{
			var sender = this.Set
				.FirstOrDefault(wl => wl.PKWhitelistID == senderId);

			if (sender == null)
				return null;
			else
				return sender.NIF;
		}

		public string GetSenderNameById(Guid senderId)
		{
			var sender = this.Set
				.FirstOrDefault(wl => wl.PKWhitelistID == senderId);

			if (sender == null)
				return null;
			else
				return sender.EmailName;
		}

		public List<Whitelist> GetAllSendersNotInstances(IInstancesRepository instancesRepository)
		{
			var instances = instancesRepository.Set.ToList();

			var senders = this.Set.ToList();

			List<Whitelist> finalSenders = new List<Whitelist>();
			foreach (var sender in senders)
			{
				bool isInstance = false;
				foreach (var instance in instances)
				{
					if (sender.NIF.ToLower() == instance.NIF.ToLower())
					{
						isInstance = true;
						break;
					}
				}

				if(!isInstance)
					finalSenders.Add(sender);
			}

			return finalSenders;
		}

		public List<Whitelist> GetAllSenders()
		{
			var senders = this.Set.ToList();

			return senders;
		}
	}
}
