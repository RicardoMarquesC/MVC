using eBillingSuite.Model.CIC_DB;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Support;
using eBillingSuite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
    public class EBC_PackagedRepository : IEBC_PackageRepository
    {
        public EBC_PackagedRepository()
        {
        }

        public string GetFilenameWithRealExtension(string filename)
        {
            PhysicalPackage physicalPack = new PhysicalPackage();

            return physicalPack.PhysicalFilenameWithRealExtension(filename);
        }

        public string GetFilePath(string filename, string sentido, string ano, string mes)
        {
            PhysicalPackage physicalPack = new PhysicalPackage();

            return physicalPack.PhysicalFilePath(filename, ano, mes, sentido);
        }

        public PacketDetailVM GetPacketById(string id, string direction)
        {
            PacketDetailVM packetDetail = new PacketDetailVM();
            if (direction.ToLower() == "out")
            {
                using (CIC_DB entidadeCIC = new CIC_DB())
                {
                    Guid pkid = Guid.Parse(id);
                    /********** Dados Documento e Informação acerca do processo de integração **********/
                    var outPacket = entidadeCIC.OutboundPacket.FirstOrDefault(x => x.PKEBCPackageID == pkid);
                    var outProcess = entidadeCIC.OutboundProcesses
                        .FirstOrDefault(op => op.PKOutboundProcessID == outPacket.FKOutboundProcessID);
                    OutboundDetail outPacketDetail = new OutboundDetail(
                        new DocumentDetail
                        {
                            EbcPackageId = outPacket.PKEBCPackageID != null ? outPacket.PKEBCPackageID.ToString() : "-",
                            Dmsid = outPacket.DMSID,
                        },
                        new DocumentIntegrationDetail
                        {
                            ProcessId = outPacket.FKOutboundProcessID.HasValue ? outPacket.FKOutboundProcessID.Value.ToString() : Guid.Empty.ToString(),
                            CreationDate = outPacket.CreationDate.HasValue ? outPacket.CreationDate.Value.ToString() : "-",
                            ProcessState = outPacket.CurrentEBCState,
                            StateLastUpdate = outPacket.LastUpdate.HasValue ? outPacket.LastUpdate.Value.ToString() : "-",
                            SapLastReport = outPacket.LastReportGenerated.HasValue ? outPacket.LastReportGenerated.Value.ToString() : "-",
                            MetadataFilename = ((OutboundProcesses)outProcess).ProcessedFilename,
                            DigitalFileName = outPacket.DigitalInfoFileName,
                        },
                        null,
                        null,
                        null,
                        null,
                        null
                    );
                    int ebcPackageYear, ebcPackageMonth;
                    /********** Ficheiros do envelope **********/
                    using (EBC_DB entidadeEBC = new EBC_DB())
                    {
                        // get ebc package
                        var ebcPackage = entidadeEBC.EBC_Packages
                            .FirstOrDefault(p => p.PKID == outPacket.PKEBCPackageID);

                        ebcPackageYear = ebcPackage.CreatedOn.Year;
                        ebcPackageMonth = ebcPackage.CreatedOn.Month;

                    }

                    PhysicalPackage physicalPack = new PhysicalPackage();

                    outPacketDetail.documentEnvelopeFiles = new DocumentEnvelopeFiles
                    {
                        PdfSigned = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.SIGNED_PDF_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.SIGNED_PDF_FILE_SUFFIX : "",
                        BasicMetadata = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.BASIC_METADATA_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.BASIC_METADATA_FILE_SUFFIX : "",
                    };
                    /********** Ano e Mes de criação do pacote **********/
                    outPacketDetail.AnoCriacaoPacote = ebcPackageYear.ToString();
                    outPacketDetail.MesCriacaoPacote = ebcPackageMonth.ToString();

                    packetDetail.outboundDetail = outPacketDetail;

                }
            }

            return packetDetail;
        }

        public void InserEventsByIDwObs(string ebcPackageId, int packagestate, int eventtype, string obs)
        {
            using (EBC_DB entidadeEBC = new EBC_DB())
            {
                var guidPackageID = Guid.Parse(ebcPackageId);

                switch (packagestate)
                {
                    case 3:
                        entidadeEBC.EBC_PackageEvents.Add(
                            new EBC_PackageEvents
                            {
                                PKID = Guid.NewGuid(),
                                FKPackageID = guidPackageID,
                                PackageState = packagestate,
                                EventDate = DateTime.Now,
                                EventType = eventtype,
                                EventMessage = "Capturado click no link. Aceite como Recibo de leitura/entrega"
                            });
                        break;
                    case 7:
                        entidadeEBC.EBC_PackageEvents.Add(
                            new EBC_PackageEvents
                            {
                                PKID = Guid.NewGuid(),
                                FKPackageID = guidPackageID,
                                PackageState = packagestate,
                                EventDate = DateTime.Now,
                                EventType = eventtype,
                                EventMessage = "Package is ready to send.",
                                Obs = obs
                            });

                        entidadeEBC.EBC_PackageEvents.Add(
                            new EBC_PackageEvents
                            {
                                PKID = Guid.NewGuid(),
                                FKPackageID = guidPackageID,
                                PackageState = packagestate,
                                EventDate = DateTime.Now,
                                EventType = eventtype,
                                EventMessage = "Envelope capturado pelo eBillingConnector Package Processor."
                            });
                        break;
                }


                entidadeEBC.SaveChanges();
            }
        }


        public void InserEventsByID(string ebcPackageId, int packagestate, int eventtype)
        {
            using (EBC_DB entidadeEBC = new EBC_DB())
            {
                var guidPackageID = Guid.Parse(ebcPackageId);

                switch (packagestate)
                {
                    case 3:
                        entidadeEBC.EBC_PackageEvents.Add(
                            new EBC_PackageEvents
                            {
                                PKID = Guid.NewGuid(),
                                FKPackageID = guidPackageID,
                                PackageState = packagestate,
                                EventDate = DateTime.Now,
                                EventType = eventtype,
                                EventMessage = "Capturado click no link. Aceite como Recibo de leitura/entrega"
                            });
                        break;
                    case 7:
                        entidadeEBC.EBC_PackageEvents.Add(
                            new EBC_PackageEvents
                            {
                                PKID = Guid.NewGuid(),
                                FKPackageID = guidPackageID,
                                PackageState = packagestate,
                                EventDate = DateTime.Now,
                                EventType = eventtype,
                                EventMessage = "Package is ready to send."
                            });

                        entidadeEBC.EBC_PackageEvents.Add(
                            new EBC_PackageEvents
                            {
                                PKID = Guid.NewGuid(),
                                FKPackageID = guidPackageID,
                                PackageState = packagestate,
                                EventDate = DateTime.Now,
                                EventType = eventtype,
                                EventMessage = "Envelope capturado pelo eBillingConnector Package Processor."
                            });
                        break;
                }


                entidadeEBC.SaveChanges();
            }
        }

        public void UpdateStatusByID(string ebcPackageId)
        {
            using (EBC_DB entidadeEBC = new EBC_DB())
            {
                var guidPackageID = Guid.Parse(ebcPackageId);
                var entity = entidadeEBC.EBC_Packages.FirstOrDefault(p => p.PKID == guidPackageID);
                entity.PackageState = 3;
                entity.SentLinkEmail = true;
                entidadeEBC.SaveChanges();
            }
        }
    }
}
