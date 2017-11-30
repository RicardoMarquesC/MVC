using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.ViewModels
{
    public class PacketDetailVM
    {
        public PacketDetailVM(OutboundDetail outDetail, InboundDetail inDetail)
        {
            //outboundDetail = outDetail;
            //inboundDetail = inDetail;

            if (outDetail == null && inDetail != null)
                inboundDetail = inDetail;
            else
                outboundDetail = outDetail;
        }

        public PacketDetailVM()
        {

        }

        public OutboundDetail outboundDetail { get; set; }

        public InboundDetail inboundDetail { get; set; }
    }

    #region Outbound

    public class OutboundDetail
    {
        public OutboundDetail(DocumentDetail docDetails, DocumentIntegrationDetail docIntegDetails, DocumentEnvelopeInformation docEnvInfo,
            DocumentEnvelopeFiles docEnvFiles, List<DocumentEvent> docEnvEvents, string anoPacote, string mesPacote)
        {
            AnoCriacaoPacote = anoPacote;
            MesCriacaoPacote = mesPacote;
            documentDetails = docDetails;
            documentIntegrationDetails = docIntegDetails;
            documentEnvelopeInformation = docEnvInfo;
            documentEnvelopeFiles = docEnvFiles;
            documentEnvelopeEvents = docEnvEvents == null ? new List<DocumentEvent>() : docEnvEvents;
        }

        public string AnoCriacaoPacote { get; set; }
        public string MesCriacaoPacote { get; set; }
        public DocumentDetail documentDetails { get; set; }
        public DocumentIntegrationDetail documentIntegrationDetails { get; set; }
        public DocumentEnvelopeInformation documentEnvelopeInformation { get; set; }
        public DocumentEnvelopeFiles documentEnvelopeFiles { get; set; }
        public List<DocumentEvent> documentEnvelopeEvents { get; set; }
    }

    public class DocumentDetail
    {
        public string EbcPackageId { get; set; }
        public string Dmsid { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientNif { get; set; }
        public string SupplierNif { get; set; }
        public string OriginalDoc { get; set; }
        public string DocNumber { get; set; }
        public string DocDate { get; set; }
        public string Amount { get; set; }
    }

    public class DocumentIntegrationDetail
    {
        public string ProcessId { get; set; }
        public string CreationDate { get; set; }
        public string ProcessOrigin { get; set; }
        public string ProcessState { get; set; }
        public string StateLastUpdate { get; set; }
        public string SapLastReport { get; set; }
        public string MetadataFilename { get; set; }
        public string DigitalFileName { get; set; }
        public string TotalLines { get; set; }
        public string TotalProcessedLines { get; set; }
        public string ProcessedDate { get; set; }
        public string ProcessedCorrectly { get; set; }
    }

    public class DocumentEnvelopeInformation
    {
        public string InstanceName { get; set; }
        public string IntegrationMode { get; set; }
        public string AplicationName { get; set; }
        public string EmailId { get; set; }
        public string CreationDate { get; set; }
        public string LastActivityDate { get; set; }
        public string AcknowledgeSchedule { get; set; }
        public string FollowupAttempts { get; set; }
        public string FollowupSchedule { get; set; }
        public string EmailTechnical { get; set; }
        public string EmailFunctional { get; set; }
        public string EmailMonitoring { get; set; }
    }

    public class DocumentEnvelopeFiles
    {
        public string PdfUnsigned { get; set; }
        public string PdfSigned { get; set; }
        public string Postscript { get; set; }
        public string BasicMetadata { get; set; }
        public string AttachOne { get; set; }
        public string AttachTwo { get; set; }
        public string AttachThree { get; set; }
        public string Cancellation { get; set; }
        public string EmailSent { get; set; }
        public string UndeliveredReceipt { get; set; }
        public string DeliveredReceipt { get; set; }
        public string ReadReceipt { get; set; }
        public string TransmissionReceipt { get; set; }
        public string DelayReceipt { get; set; }
        public string ExplicitReceipt { get; set; }
        public string SuspensionEmail { get; set; }
    }

    public class DocumentEvent
    {
        public string Date { get; set; }
        public int EventType { get; set; }
        public string EventTypeLabel { get; set; }
        public string Detail { get; set; }
        public string Obs { get; set; }
    }

    #endregion

    #region Inbound

    public class InboundDetail
    {
        public InboundDetail()
        {
        }

        public InboundDetail(InboundDocumentDetail inDocDetail, InboundDocumentIntegrationDetail inDocIntegratDetail, InboundDocumentEnvelopeFiles inDocEnvFiles)
        {
            inboundDocumentDetail = inDocDetail;
            inboundDocumentIntegrationDetail = inDocIntegratDetail;
            inboundDocumentEnvelopeFiles = inDocEnvFiles;
        }

        public InboundDocumentDetail inboundDocumentDetail { get; set; }
        public InboundDocumentIntegrationDetail inboundDocumentIntegrationDetail { get; set; }
        public InboundDocumentEnvelopeFiles inboundDocumentEnvelopeFiles { get; set; }
    }

    public class InboundDocumentDetail
    {
        public string SupplierName { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierNif { get; set; }
        public string ClientNif { get; set; }
        public string OriginalDoc { get; set; }
        public string DocNumber { get; set; }
        public string DocDate { get; set; }
        public string Amount { get; set; }
        public string AmountWithoutTax { get; set; }
        public string OrderNumber { get; set; }
    }

    public class InboundDocumentIntegrationDetail
    {
        public string ProcessId { get; set; }
        public string TransformationMethod { get; set; }
        public string ReceptionDate { get; set; }
        public string RequiresManualValidaiton { get; set; }
        public string IsValid { get; set; }
        public string ValidationDate { get; set; }
        public string SubmissionDate { get; set; }
        public string SubmissionFile { get; set; }
        public string Returned { get; set; }
        public string Observations { get; set; }
        public string ReceptionDateMonth { get; internal set; }
        public string ReceptionDateYear { get; internal set; }
    }

    public class InboundDocumentEnvelopeFiles
    {
        public string Email { get; set; }
        public string Pdf { get; set; }
        public string Xml { get; set; }
    }

    #endregion
}
