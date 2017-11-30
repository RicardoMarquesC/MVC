using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EBCRealTimeAlertsConfigVM
	{
		private List<EBC_Config> model;
		private IECCListRepositories _eCConfigRepositories;
		private IeBillingSuiteRequestContext _context;
		public Guid FKInstanceID {get; private set; }

		public EBCRealTimeAlertsConfigVM(List<EBC_Config> model, IECCListRepositories _eCConfigRepositories, IeBillingSuiteRequestContext _context, Guid instance)
		{
			this.model = model;
			this._eCConfigRepositories = _eCConfigRepositories;
			this._context = _context;
			this.FKInstanceID = instance;

			var values = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
			AvailableInstances = values
				.Select(v => new SelectListItem
				{
					Text = v.Name,
					Value = v.PKID.ToString(),
					Selected = v.PKID == FKInstanceID
				})
				.ToList();

			Configs = new List<ConfigInformation>();
			//e agora começa o caos
			foreach (EBC_Config config in model)
			{
				ConfigInformation ci = new ConfigInformation();
				ci.PKID = config.PKID;
				ci.Text = GetConfigName(config.KeyName);
				if (config.KeyValue.ToLower().Equals("true"))
				{
					ci.Value = true;
					ci.isSwitcher = true;
				}
				else if (config.KeyValue.ToLower().Equals("false"))
				{
					ci.Value = false;
					ci.isSwitcher = true;
				}
				else
				{
					ci.Value = config.KeyValue;
					ci.isSwitcher = false;
				}
				Configs.Add(ci);
			}
		}

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }

		[DoNotGenerateDictionaryEntry]
		public List<ConfigInformation> Configs { get; set; }

		[DoNotGenerateDictionaryEntry]
		public class ConfigInformation
		{
			public string Text { get; set; }
			public object Value { get; set; }
			public Guid PKID { get; set; }
			public bool isSwitcher { get; set; }
		}

		private string GetConfigName(string KeyName)
		{
			if (KeyName.Equals("RealTimeErrorsReports"))
				return "Alertas em tempo real";
			else if (KeyName.Equals("RealTimeEmailReportsOut"))
				return "Emails para receber alertas (Outbound)";
			else if (KeyName.Equals("RealTimeEmailReportsIn"))
				return "Emails para receber alertas (Inbound)";
			else
				return null;
			
			////////MUDAR PARA QUANDO FOR PARA PRODUCAO
			//return _context.GetDictionaryValue(KeyName);
		}
	}
}