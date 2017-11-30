using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EBCInstancesConfigsVM
	{
		private List<EBC_Instances> models;
		private IECCListRepositories _eCConfigRepositories;
		private IeBillingSuiteRequestContext _context;
		public Guid FKInstanceID;		
		public EBC_Instances instance;

		public EBCInstancesConfigsVM(List<EBC_Instances> models, IECCListRepositories _eCConfigRepositories, IeBillingSuiteRequestContext _context, Guid instancia)
		{
			this.models = models;
			this._eCConfigRepositories = _eCConfigRepositories;
			this._context = _context;
			this.FKInstanceID = instancia;

			AvailableInstances = models
				.Select(v => new SelectListItem
				{
					Text = v.Name,
					Value = v.PKID.ToString(),
					Selected = v.PKID == FKInstanceID
				})
				.ToList();

			instance = _eCConfigRepositories.instancesRepository.Find(FKInstanceID);
		}

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableInstances { get; private set; }
		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableCustomers { get; private set; }
		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableTypes { get; private set; }
	}
}
