using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.CIC_DB;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EBCInstanceEditVM
	{
		public EBC_Instances Instance { get; set; }
		public List<WhiteListEntriesVM> WhitelistEntries { get; set; }

		public EBCInstanceEditVM(EBC_Instances instance, List<Whitelist> whitelistEntries, IInstancesDeniedSendersRepository instancesDeniedSendersRepository)
		{
			this.Instance = instance;

			this.WhitelistEntries = new List<WhiteListEntriesVM>();
			foreach (Whitelist wl in whitelistEntries)
			{
				bool hasPermission = instancesDeniedSendersRepository.SenderIsAllowedByInstanceId(instance.PKID, wl.NIF);

				WhitelistEntries.Add(new WhiteListEntriesVM {
					WhitelistEntry = wl,
					WhitelistHasPermission = hasPermission
				});
			}
		}
	}

	public class WhiteListEntriesVM
	{
		public Whitelist WhitelistEntry { get; set; }
		public bool WhitelistHasPermission { get; set; }

	}
}
