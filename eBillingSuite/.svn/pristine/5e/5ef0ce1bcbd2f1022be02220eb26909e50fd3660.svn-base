using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.EDI_DB;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
	public class EEDIOutboundDetailVM
	{
		public OutboundPacket outboundpacket;
		public OutboundProcesses outboundprocess;
		private IEEDISentDocsDetailsRepository _eEDISentDocsDetailsRepository;

		public EEDIOutboundDetailVM(OutboundPacket modelDB, IEEDISentDocsDetailsRepository _eEDISentDocsDetailsRepository)
		{
			this.outboundpacket = modelDB;
			this._eEDISentDocsDetailsRepository = _eEDISentDocsDetailsRepository;
			this.outboundprocess = _eEDISentDocsDetailsRepository.GetProcessByPacketID(modelDB.FKOutboundProcessID);
		}
	}
}