using eBillingSuite.Repositories;
using eBillingSuite.Repositories.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite
{
	public class ECListRepositories : IECCListRepositories
	{

		[Inject]
		public ECListRepositories(
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
			IConnectorInboundPacketInfoObjectPropertiesRepository _connectorInboundPacketInfoObjectPropertiesRepository,
			IEConnectorCustomersRepository _eConnectorCustomersRepository,
			IEConnectorXmlTemplateRepository _eConnectorXmlTemplateRepository,
			IEConnectorXmlClientRepository _eConnectorXmlClientRepository,
			IEConnectorXmlHeaderRepository _eConnectorXmlHeaderRepository,
			IEConnectorXmlLinesRepository _eConnectorXmlLinesRepository,
			IEConnectorXmlResumoIvaRepository _eConnectorXmlResumoIvaRepository,
			IEConnectorXmlInboundRepository _eConnectorXmlInboundRepository,
			IEConnectorXmlHeadInboundRepository _eConnectorXmlHeadInboundRepository,
			IEConnectorXmlLinesInboundRepository _eConnectorXmlLinesInboundRepository,
			IEConnectorXmlVatInboundRepository _eConnectorXmlVatInboundRepository,
			IEConnectorIntegrationFiltersRepository _eConnectorIntegrationFiltersRepository,
			IEConnectorXmlMappingRepository _eConnectorXmlMappingRepository,
			IManutencaoRepository _eManutencaoRepository,
			IEConnectorTipoNomenclaturaPDFRepository _eConnectorTipoNomenclaturaPDFRepository,
			IEConnectorTipoNomenclaturaSenderRepository _eConnectorTipoNomenclaturaSenderRepository,
			IECertificatesDetailsRepository _eCertificatesDetailsRepository,
			IEConnectorSendersRepository _eConnectorSendersRepository,
			IInstancesDeniedSendersRepository _instancesDeniedSendersRepository,
			ISaphetyCredentialsRepository _saphetyCredentialsRepository,
            ITicketSystemRepository _ticketSystemRepository,
            IOutboundInboundRepository _outboundInboundRepository,
            IComATPacketsRepository _comATPacketRepository,
            IComATPacketsGuiasRepository _comATPacketsGuiasRepository,
            IEBC_PackageRepository _eBC_PackegedRepository
            )
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
			eConnectorCustomersRepository = _eConnectorCustomersRepository;
			eConnectorXmlTemplateRepository = _eConnectorXmlTemplateRepository;
			eConnectorXmlClientRepository = _eConnectorXmlClientRepository;
			eConnectorXmlHeaderRepository = _eConnectorXmlHeaderRepository;
			eConnectorXmlLinesRepository = _eConnectorXmlLinesRepository;
			eConnectorXmlResumoIvaRepository = _eConnectorXmlResumoIvaRepository;
			eConnectorXmlInboundRepository = _eConnectorXmlInboundRepository;
			eConnectorXmlHeadInboundRepository = _eConnectorXmlHeadInboundRepository;
			eConnectorXmlLinesInboundRepository = _eConnectorXmlLinesInboundRepository;
			eConnectorXmlVatInboundRepository = _eConnectorXmlVatInboundRepository;
			eConnectorIntegrationFiltersRepository = _eConnectorIntegrationFiltersRepository;
			eConnectorXmlMappingRepository = _eConnectorXmlMappingRepository;
			eManutencaoRepository = _eManutencaoRepository;
			eConnectorTipoNomenclaturaPDFRepository = _eConnectorTipoNomenclaturaPDFRepository;
			eConnectorTipoNomenclaturaSenderRepository = _eConnectorTipoNomenclaturaSenderRepository;
			eCertificatesDetailsRepository = _eCertificatesDetailsRepository;
			eConnectorSendersRepository = _eConnectorSendersRepository;
			instancesDeniedSendersRepository = _instancesDeniedSendersRepository;
			saphetyCredentialsRepository = _saphetyCredentialsRepository;
            ticketSystemRepository = _ticketSystemRepository;
            outboundInboundRepository = _outboundInboundRepository;
            ComATPacketRepository = _comATPacketRepository;
            ComATPacketsGuiasRepository = _comATPacketsGuiasRepository;
            EBC_PackagedRepository = _eBC_PackegedRepository;
        }


		public IInstancesRepository instancesRepository { get; set; }
		public IEConnectorConfigsRepository eBCConfigurationsRepository { get; set; }
		public IECertificatesRepository eBCCertificatesRepository { get; set; }
		public IEMarketCertificatesRepository eBCMarketCertificatesRepository { get; set; }
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
		public IEConnectorCustomersRepository eConnectorCustomersRepository { get; set; }
		public IEConnectorXmlTemplateRepository eConnectorXmlTemplateRepository { get; set; }
		public IEConnectorXmlClientRepository eConnectorXmlClientRepository { get; set; }
		public IEConnectorXmlHeaderRepository eConnectorXmlHeaderRepository { get; set; }
		public IEConnectorXmlLinesRepository eConnectorXmlLinesRepository { get; set; }
		public IEConnectorXmlResumoIvaRepository eConnectorXmlResumoIvaRepository { get; set; }
		public IEConnectorXmlInboundRepository eConnectorXmlInboundRepository { get; set; }
		public IEConnectorXmlHeadInboundRepository eConnectorXmlHeadInboundRepository { get; set; }
		public IEConnectorXmlLinesInboundRepository eConnectorXmlLinesInboundRepository { get; set; }
		public IEConnectorXmlVatInboundRepository eConnectorXmlVatInboundRepository { get; set; }
		public IEConnectorIntegrationFiltersRepository eConnectorIntegrationFiltersRepository { get; set; }
		public IEConnectorXmlMappingRepository eConnectorXmlMappingRepository { get; set; }
		public IManutencaoRepository eManutencaoRepository { get; set; }
		public IEConnectorTipoNomenclaturaPDFRepository eConnectorTipoNomenclaturaPDFRepository { get; set; }
		public IEConnectorTipoNomenclaturaSenderRepository eConnectorTipoNomenclaturaSenderRepository { get; set; }
		public IECertificatesDetailsRepository eCertificatesDetailsRepository { get; set; }
		public IEConnectorSendersRepository eConnectorSendersRepository { get; set; }
		public IInstancesDeniedSendersRepository instancesDeniedSendersRepository { get; set; }
		public ISaphetyCredentialsRepository saphetyCredentialsRepository { get; set; }

        public ITicketSystemRepository ticketSystemRepository { get; set; }
        public IOutboundInboundRepository outboundInboundRepository { get; set; }
        public IComATPacketsRepository ComATPacketRepository { get; set; }
        public IComATPacketsGuiasRepository ComATPacketsGuiasRepository { get; set; }
        public IEBC_PackageRepository EBC_PackagedRepository { get; set; }

    }
}