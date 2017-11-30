using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite
{
	public interface IECConfigRepositories
	{
		IInstancesRepository instancesRepository {get; set;}
		IEConnectorConfigsRepository eBCConfigurationsRepository { get; set; }
		IECertificatesRepository eBCCertificatesRepository { get; set; }
		IEMarketCertificatesRepository eBCMarketCertificatesRepository { get; set; }
		IEMarketsRepository eBCMarketsRepository { get; set; }
		ISuiteConfigurationsRepository suiteConfigurationsRepository { get; set; }
		IConnectorSpecificDeliveryOptionsRepository connectorSpecificDeliveryOptionsRepository { get; set; }
		ICustomersRepository customersRepository { get; set; }
		IConnectorEmailContentRepository connectorEmailContentRepository { get; set; }
		IEConnectorInvoiceRegionTypesRepository connectorInvoiceRegionTypesRepository{ get; set; }
		IConnectorConfigTXTRepository connectorConfigTXTRepository { get; set; }
		IEConnectorRegexTypesRepository connectorRegexTypesRepository { get; set; }
		IConnectorConfigInboundTXTRepository connectorConfigInboundTXTRepository { get; set; }
		IConnectorInboundPacketInfoObjectPropertiesRepository connectorInboundPacketInfoObjectPropertiesRepository { get; set; }
	}
}
