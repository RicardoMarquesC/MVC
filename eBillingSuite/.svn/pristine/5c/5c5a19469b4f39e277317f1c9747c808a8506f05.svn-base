using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.CIC_DB;

namespace eBillingSuite.Repositories
{
	public interface IEConnectorSendersRepository : IGenericRepository<IeBillingSuiteCICDBContext, Whitelist>
	{
		bool IsNifUnique(string nif);

		string GetSenderNifById(Guid senderId);

		string GetSenderNameById(Guid fkSender);

		List<Whitelist> GetAllSendersNotInstances(IInstancesRepository instancesRepository);

		List<Whitelist> GetAllSenders();
	}
}
