using eBillingSuite.Repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite
{
	public class ECConfigRepositories: IECConfigRepositories
	{
		
		[Inject]
		public ECConfigRepositories(
			IInstancesRepository _instancesRepository,
			IEConnectorConfigsRepository _eBCConfigurationsRepository,
			IECertificatesRepository _eBCCertificatesRepository,
			IEMarketCertificatesRepository _eBCMarketCertificatesRepository,
			IEMarketsRepository _eBCMarketsRepository,
			ISuiteConfigurationsRepository _suiteConfigurationsRepository,
			IConnectorSpecificDeliveryOptionsRepository _connectorSpecificDeliveryOptionsRepository,
			ICustomersRepository _customersRepository,
			IConnectorEmailContentRepository _connectorEmailContentRepository,
			IEConnectorInvoiceRegionTypesRepository _connectorInvoiceRegionTypesRepository,
			IConnectorConfigTXTRepository _connectorConfigTXTRepository,
			IEConnectorRegexTypesRepository _connectorRegexTypesRepository,
			IConnectorConfigInboundTXTRepository _connectorConfigInboundTXTRepository,
			IConnectorInboundPacketInfoObjectPropertiesRepository _connectorInboundPacketInfoObjectPropertiesRepository)
		{
			instancesRepository = _instancesRepository;
			eBCConfigurationsRepository = _eBCConfigurationsRepository;
			eBCCertificatesRepository = _eBCCertificatesRepository;
			eBCMarketCertificatesRepository = _eBCMarketCertificatesRepository;
			eBCMarketsRepository = _eBCMarketsRepository;
			suiteConfigurationsRepository = _suiteConfigurationsRepository;
			connectorSpecificDeliveryOptionsRepository = _connectorSpecificDeliveryOptionsRepository;
			customersRepository = _customersRepository;
			connectorEmailContentRepository = _connectorEmailContentRepository;
			connectorInvoiceRegionTypesRepository = _connectorInvoiceRegionTypesRepository;
			connectorConfigTXTRepository = _connectorConfigTXTRepository;
			connectorRegexTypesRepository = _connectorRegexTypesRepository;
			connectorConfigInboundTXTRepository = _connectorConfigInboundTXTRepository;
			connectorInboundPacketInfoObjectPropertiesRepository = _connectorInboundPacketInfoObjectPropertiesRepository;
		}


		public IInstancesRepository instancesRepository {get; set;}
		public IEConnectorConfigsRepository eBCConfigurationsRepository {get; set;}
		public IECertificatesRepository eBCCertificatesRepository {get; set;}
		public IEMarketCertificatesRepository eBCMarketCertificatesRepository {get; set;}
		public IEMarketsRepository eBCMarketsRepository { get; set; }
		public ISuiteConfigurationsRepository suiteConfigurationsRepository { get; set; }
		public IConnectorSpecificDeliveryOptionsRepository connectorSpecificDeliveryOptionsRepository { get; set; }
		public ICustomersRepository customersRepository { get; set; }
		public IConnectorEmailContentRepository connectorEmailContentRepository { get; set; }
		public IEConnectorInvoiceRegionTypesRepository connectorInvoiceRegionTypesRepository { get; set; }
		public IConnectorConfigTXTRepository connectorConfigTXTRepository { get; set; }
		public IEConnectorRegexTypesRepository connectorRegexTypesRepository { get; set; }
		public IConnectorConfigInboundTXTRepository connectorConfigInboundTXTRepository { get; set; }
		public IConnectorInboundPacketInfoObjectPropertiesRepository connectorInboundPacketInfoObjectPropertiesRepository { get; set; }
	}
}