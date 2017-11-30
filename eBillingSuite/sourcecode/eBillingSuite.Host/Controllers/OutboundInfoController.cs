using eBillingSuite.Repositories;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.Controllers
{
    public class OutboundInfoController : Controller
    {
        // GET: OutboundInfo
        public IEBC_PackageRepository ebc_packageRepository;
        public IOutboundInboundRepository outboundInboundRepository;

        public OutboundInfoController()
        {
            ebc_packageRepository = new EBC_PackagedRepository();
            outboundInboundRepository = new OutboundInboundRepository();
        }

        public ActionResult Download(string id, string direction)
        {

            var packetInfo = ebc_packageRepository.GetPacketById(id, direction);

            FileStream stream = new FileStream(ebc_packageRepository
                .GetFilePath(packetInfo.outboundDetail.documentEnvelopeFiles.PdfSigned,
                                direction.ToLower(),
                                packetInfo.outboundDetail.AnoCriacaoPacote,
                                packetInfo.outboundDetail.MesCriacaoPacote), FileMode.Open);


            FileStreamResult fsr = new FileStreamResult(stream, "application/octet-stream");
            if (direction.ToLower() == "out")
                fsr.FileDownloadName = ebc_packageRepository.GetFilenameWithRealExtension(packetInfo.outboundDetail.documentEnvelopeFiles.PdfSigned);
            else
                fsr.FileDownloadName = packetInfo.outboundDetail.documentEnvelopeFiles.PdfSigned;

            //ATUALIZAR O ESTADO PARA LIDO na EBC_PACKAGE
            ebc_packageRepository.UpdateStatusByID(packetInfo.outboundDetail.documentDetails.EbcPackageId);

            //INSERIR NOVA LINHA NO PACKAGEEVENTS
            ebc_packageRepository.InserEventsByID(packetInfo.outboundDetail.documentDetails.EbcPackageId, 3, 2);

            //ATUALIZAR NO OUTBOUNDPACKET PARA DELIVERED
            outboundInboundRepository.UpdateOutStatToDelivered(id);

            return fsr;

        }
    }
}