using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using eBillingSuite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class TicketSystemVM
	{
        private Guid? iD;
        private IECCListRepositories _eCConfigRepositories;
        private IeBillingSuiteRequestContext _context;

        public TicketSystemVM(IECCListRepositories _eCConfigRepositories, IeBillingSuiteRequestContext _context)
        {
            this._eCConfigRepositories = _eCConfigRepositories;
            this._context = _context;

            var values = _eCConfigRepositories.instancesRepository.GetEBC_Instances();
            AvailableInstances = values
                .Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.PKID.ToString(),
                    Selected = v.PKID == values[0].PKID
                })
                .ToList();

            var valuesTicketTypes = _eCConfigRepositories.ticketSystemRepository.Set.ToList();
            AvailableOptions = valuesTicketTypes
                .Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.pkid.ToString(),
                    Selected = v.Name.ToLower() == valuesTicketTypes[0].Name.ToLower()
                })
                .ToList();
        
        }

        [DoNotGenerateDictionaryEntry]
		public System.Guid pkid { get; set; }
		public string Email { get; set; }
		public string Texto { get; set; }		
		public System.Guid fkInstance { get; set; }

		[DoNotGenerateDictionaryEntry]
		public IEnumerable<SelectListItem> AvailableOptions { get; private set; }

        [DoNotGenerateDictionaryEntry]
        public IEnumerable<SelectListItem> AvailableInstances { get; private set; }
    }
}