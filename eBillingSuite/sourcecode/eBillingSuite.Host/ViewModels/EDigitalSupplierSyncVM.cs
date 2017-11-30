using eBillingSuite.Model;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
	public class EDigitalSupplierSyncVM
	{
		public EDigitalSupplierSyncVM(List<DigitalConfigurations> configs)
		{
			foreach (var item in configs)
			{
				string val = item.Dados;
				Guid pkid = item.Pkid;

				if (item.Nome.ToLower() == "sincfornecedoresws")
				{
					this.SyncUrlConfig = val;
					this.SyncUrlConfigPkid = pkid;
				}
				else if (item.Nome.ToLower() == "sincfornecedoreswsuser")
				{
					this.SyncUserConfig = val;
					this.SyncUserConfigPkid = pkid;
				}
				else
				{
					this.SyncPassConfig = val;
					this.SyncPassConfigPkid = pkid;
				}
			}

			if (String.IsNullOrWhiteSpace(this.SyncUrlConfig))
				WantSync = false;
			else
				WantSync = true;
		}

		public EDigitalSupplierSyncVM(DigitalSupplierSyncData data)
		{
			this.WantSync = data.WantSync;

			this.SyncUrlConfigPkid = data.SyncUrlConfigPkid;
			this.SyncUrlConfig = data.SyncUrlConfig;

			this.SyncUserConfigPkid = data.SyncUserConfigPkid;
			this.SyncUserConfig = data.SyncUserConfig;

			this.SyncPassConfigPkid = data.SyncPassConfigPkid;
			this.SyncPassConfig = data.SyncPassConfig;
		}

		public bool WantSync { get; set; }
		public Guid SyncUrlConfigPkid { get; set; }
		public string SyncUrlConfig { get; set; }
		public Guid SyncUserConfigPkid { get; set; }
		public string SyncUserConfig { get; set; }
		public Guid SyncPassConfigPkid { get; set; }
		public string SyncPassConfig { get; set; }
	}
}