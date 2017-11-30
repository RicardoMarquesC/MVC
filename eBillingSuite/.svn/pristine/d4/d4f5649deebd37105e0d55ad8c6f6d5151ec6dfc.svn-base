using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model.CIC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EBCSenderXmlUploadVM
	{
		public EBCSenderXmlUploadVM(List<Whitelist> senders, Guid? selectedSender)
		{
			senders.Insert(0, new Whitelist { EmailName = "", PKWhitelistID = Guid.Empty });
			AvailableSenders = senders
				.Select(a => new SelectListItem
				{
					Text = a.EmailName,
					Value = a.PKWhitelistID.ToString(),
					Selected = a.PKWhitelistID == selectedSender.Value
				})
				.ToList();
		}

		public IEnumerable<SelectListItem> AvailableSenders { get; private set; }
		public Guid FkSender { get; set; }

		[DoNotGenerateDictionaryEntry]
		public string NovoCaminho { get; set; }
	}
}