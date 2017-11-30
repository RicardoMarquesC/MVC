﻿using eBillingSuite.Enumerations;
using eBillingSuite.Model.CIC_DB;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Model.HelpingClasses;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Security;
using eBillingSuite.Support;
using eBillingSuite.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
    public class OutboundInboundRepository : IOutboundInboundRepository
    {
        public OutboundInboundRepository()
        {
        }

        /// <summary>
        /// Obtém todos os pacotes In e Outbound
        /// </summary>
        /// <returns>Lista com os pacotes</returns>
        public List<OutboundInbound> GetAllPackets(IeBillingSuiteIdentity userIdentity)
        {
            List<OutboundInbound> finalList = new List<OutboundInbound>();
            List<OutboundPacket> outPacket = new List<OutboundPacket>();
            List<InboundPacket> inPacket = new List<InboundPacket>();


            try
            {
                using (CIC_DB entidadeCIC = new CIC_DB())
                {
                    List<string> instanceList = userIdentity.Instances.Split(';').ToList();

                    if (userIdentity.Instances == "*")
                    {
                        outPacket = entidadeCIC.OutboundPacket.Select(o => o).ToList();
                        inPacket = entidadeCIC.InboundPacket.Select(o => o).ToList();
                    }
                    else
                    {
                        outPacket = entidadeCIC.OutboundPacket.Where(o => instanceList.Contains(o.NIFEmissor)).ToList();
                        inPacket = entidadeCIC.InboundPacket.Where(o => instanceList.Contains(o.NIF)).ToList();
                    }

                    finalList = getOutboundPackets(outPacket);
                    finalList = getInboundPackets(finalList, inPacket);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return finalList;
        }

        private List<OutboundInbound> getInboundPackets(List<OutboundInbound> finalList, List<InboundPacket> inPacket)
        {
            foreach (InboundPacket inPack in inPacket)
            {
                finalList.Add(new OutboundInbound
                {
                    ID = inPack.Identificador,
                    processId = inPack.PKProcessID.ToString(),
                    Sentido = OutboundInbound.inbound,
                    NomeEmiss = string.IsNullOrEmpty(inPack.NomeFornec) ? string.Empty : inPack.NomeFornec,
                    NifEmiss = string.IsNullOrEmpty(inPack.NIFE) ? string.Empty : inPack.NIFE,
                    NomeRecept = string.IsNullOrEmpty(inPack.NomeReceptor) ? string.Empty : inPack.NomeReceptor,
                    NifRecept = string.IsNullOrEmpty(inPack.NIF) ? string.Empty : inPack.NIF,
                    Quantia = string.IsNullOrEmpty(inPack.Quantia) ? 0 : inPack.Quantia.EndsWith("-") ? float.Parse($"-{inPack.Quantia.Replace("-", "")}") : float.Parse(inPack.Quantia),
                    Ano = string.IsNullOrEmpty(inPack.DataFactura) ? "1990" : DateTime.ParseExact(inPack.DataFactura.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture).Year.ToString(),
                    Mes = string.IsNullOrEmpty(inPack.DataFactura) ? "01" : DateTime.ParseExact(inPack.DataFactura.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture).Month.ToString(),
                    Dia = string.IsNullOrEmpty(inPack.DataFactura) ? "01" : DateTime.ParseExact(inPack.DataFactura.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture).Day.ToString(),
                    DataCriacao = inPack.ReceptionDate.HasValue ? inPack.ReceptionDate.Value.ToString("yyyy-MM-dd") : "1990-01-01",
                    TipoDoc = string.IsNullOrEmpty(inPack.TipoDoc) ? string.Empty : inPack.TipoDoc,
                    Estado = getStateIN(inPack),
                    NumDoc = string.IsNullOrEmpty(inPack.NumFactura) ? string.Empty : inPack.NumFactura
                });
            }

            return finalList;
        }

        private string getStateIN(InboundPacket inPack)
        {
            if (inPack.Reprocessado.HasValue && inPack.Reprocessado.Value == true)
                return DocumentState.InReprocessado.ToString("d");
            else
            {
                if (!String.IsNullOrEmpty(inPack.SubmissionFile) && !inPack.SubmissionFile.ToLower().Equals("null")
                    && inPack.SubmissionDate != null)
                {
                    if (!String.IsNullOrEmpty(inPack.IntegracaoERP) && inPack.IntegracaoERP.ToLower().Equals("integrado com erp"))
                        return DocumentState.InLidoErp.ToString("d");
                    else
                        return DocumentState.InIntegrado.ToString("d");
                }
                else
                    return DocumentState.InNaoIntegrado.ToString("d");
            }
        }

        private List<OutboundInbound> getOutboundPackets(List<OutboundPacket> outPacket)
        {
            List<OutboundInbound> finalList = new List<OutboundInbound>();
            foreach (OutboundPacket outPack in outPacket)
            {
                finalList.Add(new OutboundInbound
                {
                    ID = outPack.Identificador,
                    processId = outPack.PKEBCPackageID.ToString(),
                    Sentido = OutboundInbound.outbound,
                    NomeEmiss = string.IsNullOrEmpty(outPack.NomeEmissor) ? string.Empty : outPack.NomeEmissor,
                    NifEmiss = string.IsNullOrEmpty(outPack.NIFEmissor) ? string.Empty : outPack.NIFEmissor,
                    NomeRecept = string.IsNullOrEmpty(outPack.NomeReceptor) ? string.Empty : outPack.NomeReceptor,
                    NifRecept = string.IsNullOrEmpty(outPack.NIFReceptor) ? string.Empty : outPack.NIFReceptor,
                    Quantia = string.IsNullOrEmpty(outPack.QuantiaComIVA) ? 0 : outPack.QuantiaComIVA.EndsWith("-") ? float.Parse($"-{outPack.QuantiaComIVA.Replace("-", "")}") : float.Parse(outPack.QuantiaComIVA),
                    Ano = string.IsNullOrEmpty(outPack.DataFactura) ? "1990" : DateTime.Parse(outPack.DataFactura).Year.ToString(),
                    Mes = string.IsNullOrEmpty(outPack.DataFactura) ? "01" : DateTime.Parse(outPack.DataFactura).Month.ToString(),
                    Dia = string.IsNullOrEmpty(outPack.DataFactura) ? "01" : DateTime.Parse(outPack.DataFactura).Day.ToString(),
                    DataCriacao = outPack.CreationDate.HasValue ? outPack.CreationDate.Value.ToString("yyyy-MM-dd") : "1990-01-01",
                    TipoDoc = string.IsNullOrEmpty(outPack.TipoDocumento) ? string.Empty : outPack.TipoDocumento,
                    Estado = getState(outPack),
                    NumDoc = string.IsNullOrEmpty(outPack.NumFactura) ? string.Empty : outPack.NumFactura
                });
            }

            return finalList;
        }

        private static string getState(OutboundPacket outPack)
        {
            if (outPack.Reprocessado.HasValue && outPack.Reprocessado.Value == true)
                return DocumentState.OutReprocessado.ToString("d");
            else
            {
                if (outPack.CurrentEBCState != null)
                {
                    if (outPack.CurrentEBCState.ToLower().Trim().Equals("delivered"))
                        return DocumentState.OutEntregue.ToString("d");
                    else if (outPack.CurrentEBCState.ToLower().Trim().Equals("new"))
                        return DocumentState.OutEspera.ToString("d");
                    else
                        return DocumentState.OutNaoEntregue.ToString("d");
                }
                else
                    return string.Empty;
            }
        }



        /// <summary>
        /// Obtém todos os pacotes In e Outbound
        /// </summary>
        /// <returns>Lista com os pacotes</returns>
        public List<OutboundInbound> GetAllPacketsCurrentMonth()
        {
            List<OutboundInbound> finalList = new List<OutboundInbound>();

            try
            {
                using (CIC_DB entidadeCIC = new CIC_DB())
                {
                    //obter os pacotes IN e OUT
                    var outboundPackets = (from outpack in entidadeCIC.OutboundPacket select outpack)
                        .Where(op => (op.CreationDate.Value.Year.Equals(DateTime.Now.Year) && op.CreationDate.Value.Month.Equals(DateTime.Now.Month)));
                    var inboundPackets = (from intpack in entidadeCIC.InboundPacket select intpack)
                        .Where(ip => (ip.ReceptionDate.Value.Year.Equals(DateTime.Now.Year) && ip.ReceptionDate.Value.Month.Equals(DateTime.Now.Month)));

                    //guardar ambos na lista que será retornada ao controlador
                    foreach (OutboundPacket outPack in outboundPackets.ToList())
                    {
                        OutboundInbound outObj = new OutboundInbound();
                        outObj.ID = outPack.Identificador;

                        outObj.processId = outPack.PKEBCPackageID.ToString();

                        outObj.Sentido = OutboundInbound.outbound;

                        if (outPack.NomeEmissor != null)
                            outObj.NomeEmiss = outPack.NomeEmissor;
                        else
                            outObj.NomeEmiss = String.Empty;

                        if (outPack.NIFEmissor != null)
                            outObj.NifEmiss = outPack.NIFEmissor;
                        else
                            outObj.NifEmiss = String.Empty;

                        if (outPack.NomeReceptor != null)
                            outObj.NomeRecept = outPack.NomeReceptor;
                        else
                            outObj.NomeRecept = String.Empty;

                        if (outPack.NIFReceptor != null)
                            outObj.NifRecept = outPack.NIFReceptor;
                        else
                            outObj.NifRecept = String.Empty;

                        if (!String.IsNullOrEmpty(outPack.QuantiaComIVA))
                        {
                            //se o sinal de "-" estiver no fim, coloca-lo no início, senão dá erro no cast para float
                            if (outPack.QuantiaComIVA.EndsWith("-"))
                                outPack.QuantiaComIVA = "-" + outPack.QuantiaComIVA.Replace("-", String.Empty);
                            outObj.Quantia = float.Parse(outPack.QuantiaComIVA.Replace(".", ","));
                        }
                        else
                            outObj.Quantia = 0;

                        if (outPack.DataFactura != null)
                        {
                            DateTime docDate = DateTime.ParseExact(outPack.DataFactura.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //DateTime docDate = DateTime.Parse(outPack.DataFactura);
                            outObj.Ano = docDate.Year.ToString();
                            outObj.Mes = docDate.Month.ToString();
                            outObj.Dia = docDate.Day.ToString();
                        }
                        else
                        {
                            outObj.Ano = "0000";
                            outObj.Mes = "00";
                            outObj.Dia = "00";
                        }

                        if (outPack.CreationDate != null && outPack.CreationDate.Value != null)
                        {
                            outObj.DataCriacao = outPack.CreationDate.Value.Year.ToString() + "-" + outPack.CreationDate.Value.Month.ToString()
                                + "-" + outPack.CreationDate.Value.Day.ToString();
                        }
                        else
                        {
                            outObj.DataCriacao = "0000-00-00";
                        }

                        if (outPack.TipoDocumento != null)
                            outObj.TipoDoc = outPack.TipoDocumento;
                        else
                            outObj.TipoDoc = String.Empty;

                        if (outPack.Reprocessado.HasValue && outPack.Reprocessado.Value == true)
                            outObj.Estado = DocumentState.OutReprocessado.ToString("d");
                        else
                        {
                            if (outPack.CurrentEBCState != null)
                            {
                                if (outPack.CurrentEBCState.ToLower().Trim().Equals("delivered"))
                                    outObj.Estado = DocumentState.OutEntregue.ToString("d");
                                else if (outPack.CurrentEBCState.ToLower().Trim().Equals("new"))
                                    outObj.Estado = DocumentState.OutEspera.ToString("d");
                                else
                                    outObj.Estado = DocumentState.OutNaoEntregue.ToString("d");
                            }
                            else
                                outObj.Estado = String.Empty;
                        }

                        if (outPack.NumFactura != null)
                            outObj.NumDoc = outPack.NumFactura;
                        else
                            outObj.NumDoc = String.Empty;

                        finalList.Add(outObj);
                    }

                    foreach (InboundPacket inPack in inboundPackets.ToList())
                    {
                        OutboundInbound inObj = new OutboundInbound();
                        inObj.ID = inPack.Identificador;

                        inObj.processId = inPack.PKProcessID;

                        inObj.Sentido = OutboundInbound.inbound;

                        if (inPack.NomeFornec != null)
                            inObj.NomeEmiss = inPack.NomeFornec;
                        else
                            inObj.NomeEmiss = String.Empty;

                        if (inPack.NIFE != null)
                            inObj.NifEmiss = inPack.NIFE;
                        else
                            inObj.NifEmiss = String.Empty;

                        if (inPack.NomeReceptor != null)
                            inObj.NomeRecept = inPack.NomeReceptor;
                        else
                            inObj.NomeRecept = String.Empty;

                        if (inPack.NIF != null)
                            inObj.NifRecept = inPack.NIF;
                        else
                            inObj.NifRecept = String.Empty;

                        if (!String.IsNullOrEmpty(inPack.Quantia))
                        {
                            //se o sinal de "-" estiver no fim, coloca-lo no início, senão dá erro no cast para float
                            if (inPack.Quantia.EndsWith("-"))
                                inPack.Quantia = "-" + inPack.Quantia.Replace("-", String.Empty);
                            inObj.Quantia = float.Parse(inPack.Quantia.Replace(".", ","));
                        }
                        else
                            inObj.Quantia = 0;

                        if (!String.IsNullOrEmpty(inPack.DataFactura))
                        {
                            DateTime docDate = DateTime.Parse(inPack.DataFactura);
                            inObj.Ano = docDate.Year.ToString();
                            inObj.Mes = docDate.Month.ToString();
                            inObj.Dia = docDate.Day.ToString();
                        }
                        else
                        {
                            inObj.Ano = "0000";
                            inObj.Mes = "00";
                            inObj.Dia = "00";
                        }

                        if (inPack.ReceptionDate != null && inPack.ReceptionDate.Value != null)
                        {
                            inObj.DataCriacao = inPack.ReceptionDate.Value.Year.ToString() + "-" + inPack.ReceptionDate.Value.Month.ToString()
                                + "-" + inPack.ReceptionDate.Value.Day.ToString();
                        }
                        else
                        {
                            inObj.DataCriacao = "0000-00-00";
                        }

                        if (inPack.TipoDoc != null)
                            inObj.TipoDoc = inPack.TipoDoc;
                        else
                            inObj.TipoDoc = String.Empty;

                        if (inPack.Reprocessado.HasValue && inPack.Reprocessado.Value == true)
                            inObj.Estado = DocumentState.InReprocessado.ToString("d");
                        else
                        {
                            if (!String.IsNullOrEmpty(inPack.SubmissionFile) && !inPack.SubmissionFile.ToLower().Equals("null")
                                && inPack.SubmissionDate != null)
                                inObj.Estado = DocumentState.InIntegrado.ToString("d");
                            else
                                inObj.Estado = DocumentState.InNaoIntegrado.ToString("d");
                        }

                        if (inPack.NumFactura != null)
                            inObj.NumDoc = inPack.NumFactura;
                        else
                            inObj.NumDoc = String.Empty;

                        finalList.Add(inObj);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return finalList;
        }

        /// <summary>
        /// Obtém o total de valores que passaram pelo sistema, quer In quer Outbound, desde sempre.
        /// </summary>
        /// <returns>Valor em string</returns>
        public string GetOutboundInboundSumValue()
        {
            string sum = String.Empty;

            try
            {
                using (CIC_DB entidadeCIC = new CIC_DB())
                {
                    decimal total = 0;
                    foreach (string value in (from op in entidadeCIC.OutboundPacket select op.QuantiaComIVA))
                        total = total + Parse.ToDecimal(value, true);

                    foreach (string value in (from ip in entidadeCIC.InboundPacket select ip.Quantia))
                        total = total + Parse.ToDecimal(value, true);

                    sum = total.ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return sum;
        }

        /// <summary>
        /// Obtém o nº de documentos transacionados (In e Out) para cada um dos últimos 12 meses
        /// </summary>
        /// <returns>os valores em string</returns>
        public string[] GetOutboundInboudMothValues()
        {
            string[] monthValues = new string[12];
            try
            {
                using (CIC_DB entidadeCIC = new CIC_DB())
                {
                    var outpacks = (from op in entidadeCIC.OutboundPacket.AsEnumerable() select op);
                    var inpacks = (from ip in entidadeCIC.InboundPacket.AsEnumerable() select ip);

                    DateTime now2 = DateTime.Now;
                    //obter os valores dos últimos 12 meses
                    int j = 0;
                    for (int i = -1; i > -13; i--)
                    {
                        //outbound
                        var temp = (outpacks
                            .Where(op => (!String.IsNullOrEmpty(op.DataFactura)))
                            .Where(op => Convert.ToDateTime(op.DataFactura).Month.Equals(DateTime.Now.Date.AddMonths(i).Month))
                            .Where(op => Convert.ToDateTime(op.DataFactura).Year.Equals(DateTime.Now.Date.AddMonths(i).Year)))
                            .Count();

                        //inbound
                        var temp2 = (inpacks
                            .Where(ip => (!String.IsNullOrEmpty(ip.DataFactura)))
                            .Where(ip => Convert.ToDateTime(ip.DataFactura).Month.Equals(DateTime.Now.Date.AddMonths(i).Month))
                            .Where(ip => Convert.ToDateTime(ip.DataFactura).Year.Equals(DateTime.Now.Date.AddMonths(i).Year)))
                            .Count();

                        monthValues[j] = (temp + temp2).ToString();

                        j++;
                    }
                    DateTime now3 = DateTime.Now;

                    TimeSpan dif2 = now3.Subtract(now2);
                    int flag = 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return monthValues;
        }

        /// <summary>
        /// Actualiza pacotes Outbound para 'Reprocessados'
        /// </summary>
        /// <param name="ids">ids dos pacotes a actualizar</param>
        public void UpdatePackets(int[] finalOutIds, int[] finalInIds)
        {
            using (CIC_DB entidadeCIC = new CIC_DB())
            {
                var matchedOutPackets = entidadeCIC.OutboundPacket
                    .Where(x => finalOutIds.Contains(x.Identificador))
                    .ToList();

                var matchedInPackets = entidadeCIC.InboundPacket
                    .Where(x => finalInIds.Contains(x.Identificador))
                    .ToList();

                foreach (var matchedOutPacket in matchedOutPackets)
                    matchedOutPacket.Reprocessado = true;

                foreach (var matchedInPacket in matchedInPackets)
                    matchedInPacket.Reprocessado = true;

                entidadeCIC.SaveChanges();
            }
        }

        public PacketDetailVM GetPacketById(int? id, string direction)
        {

            PacketDetailVM packetDetail = new PacketDetailVM();

            if (direction.ToLower() == "out")
            {
                using (CIC_DB entidadeCIC = new CIC_DB())
                {
                    //BUSCAR PKEBCPackageID a parte do id.value (identificador)

                    /********** Dados Documento e Informação acerca do processo de integração **********/
                    var outPacket = entidadeCIC.OutboundPacket.Where(t => t.Identificador == id.Value).FirstOrDefault();

                    var outProcess = entidadeCIC.OutboundProcesses
                        .FirstOrDefault(op => op.PKOutboundProcessID == outPacket.FKOutboundProcessID);
                    // model to view
                    OutboundDetail outPacketDetail = new OutboundDetail(
                        new DocumentDetail
                        {
                            EbcPackageId = outPacket.PKEBCPackageID != null ? outPacket.PKEBCPackageID.ToString() : "-",
                            Dmsid = outPacket.DMSID,
                            ClientName = outPacket.NomeEmissor,
                            ClientEmail = outPacket.EmailReceptor,
                            ClientNif = outPacket.NIFReceptor,
                            SupplierNif = outPacket.NomeEmissor,
                            OriginalDoc = outPacket.DocOriginal,
                            DocNumber = outPacket.NumFactura,
                            DocDate = outPacket.DataFactura,
                            Amount = outPacket.QuantiaComIVA
                        },
                        new DocumentIntegrationDetail
                        {
                            ProcessId = outPacket.FKOutboundProcessID.HasValue ? outPacket.FKOutboundProcessID.Value.ToString() : Guid.Empty.ToString(),
                            CreationDate = outPacket.CreationDate.HasValue ? outPacket.CreationDate.Value.ToString() : "-",
                            ProcessOrigin = outPacket.Source,
                            ProcessState = outPacket.CurrentEBCState,
                            StateLastUpdate = outPacket.LastUpdate.HasValue ? outPacket.LastUpdate.Value.ToString() : "-",
                            SapLastReport = outPacket.LastReportGenerated.HasValue ? outPacket.LastReportGenerated.Value.ToString() : "-",
                            MetadataFilename = ((OutboundProcesses)outProcess).ProcessedFilename,
                            DigitalFileName = outPacket.DigitalInfoFileName,
                            TotalLines = ((OutboundProcesses)outProcess).TotalNumberOfLines.HasValue ? ((OutboundProcesses)outProcess).TotalNumberOfLines.Value.ToString() : "-",
                            TotalProcessedLines = ((OutboundProcesses)outProcess).NumberOfLinesProcessed.ToString(),
                            ProcessedDate = outProcess.SucceededProcessingDate.HasValue ? outProcess.SucceededProcessingDate.Value.ToString() : "-",
                            ProcessedCorrectly = outProcess.ProcessedCorrectly ? "Sim" : "Não"
                        },
                        null,
                        null,
                        null,
                        null,
                        null
                    );
                    /********** Informação do envelope **********/
                    int ebcPackageYear, ebcPackageMonth;
                    using (EBC_DB entidadeEBC = new EBC_DB())
                    {
                        // get ebc package
                        var ebcPackage = entidadeEBC.EBC_Packages
                            .FirstOrDefault(p => p.PKID == outPacket.PKEBCPackageID);

                        ebcPackageYear = ebcPackage.CreatedOn.Year;
                        ebcPackageMonth = ebcPackage.CreatedOn.Month;

                        // get instance name
                        var ebcInstancePackage = entidadeEBC.EBC_InstancePackages
                            .FirstOrDefault(ip => ip.FKPackageID == ebcPackage.PKID);
                        var instanceName = entidadeEBC.EBC_Instances
                            .FirstOrDefault(i => i.PKID == ebcInstancePackage.FKInstanceID)
                            .Name;

                        // spec deliv opt
                        var ebcDelivOpt = entidadeEBC.EBC_SpecificDeliveryOptions
                            .FirstOrDefault(d => d.PKID == ebcPackage.FKSpecificDeliveryOptionsID);

                        // events
                        var events = entidadeEBC.EBC_PackageEvents
                            .Where(e => e.FKPackageID == ebcPackage.PKID)
                            .OrderByDescending(e => e.EventDate)
                            .ToList();

                        string waitUnitType = String.Empty;
                        switch (ebcDelivOpt.WaitForEffectiveResponseUnitType)
                        {
                            case (int)PeriodUnitTypes.Minutes:
                                waitUnitType = "Minuto";
                                break;
                            case (int)PeriodUnitTypes.Hours:
                                waitUnitType = "Hora";
                                break;
                            case (int)PeriodUnitTypes.Days:
                                waitUnitType = "Dia";
                                break;
                            case (int)PeriodUnitTypes.Months:
                                waitUnitType = "Mês";
                                break;
                            default:
                                waitUnitType = "-";
                                break;
                        }

                        string resendUnitType = String.Empty;
                        switch (ebcDelivOpt.resendAfterPeriodUnitType)
                        {
                            case (int)PeriodUnitTypes.Minutes:
                                resendUnitType = "Minuto";
                                break;
                            case (int)PeriodUnitTypes.Hours:
                                resendUnitType = "Hora";
                                break;
                            case (int)PeriodUnitTypes.Days:
                                resendUnitType = "Dia";
                                break;
                            case (int)PeriodUnitTypes.Months:
                                resendUnitType = "Mês";
                                break;
                            default:
                                resendUnitType = "-";
                                break;
                        }

                        outPacketDetail.documentEnvelopeInformation = new DocumentEnvelopeInformation
                        {
                            InstanceName = instanceName,
                            IntegrationMode = ebcPackage.IntegrationEnvironment == true ? "Sim" : "Não",
                            AplicationName = ebcPackage.ApplicationName,
                            EmailId = ebcPackage.MessageID == null ? "-" : ebcPackage.MessageID,
                            CreationDate = ebcPackage.CreatedOn.ToString(),
                            LastActivityDate = events.First().EventDate.ToString(),
                            AcknowledgeSchedule = String.Format("Esperar {0} {1}(s) pela resposta", ebcDelivOpt.WaitForEfectiveResponseUnit.ToString(), waitUnitType),
                            FollowupAttempts = ebcPackage.CurrentFollowupRetry.ToString(),
                            FollowupSchedule = String.Format("Reenviar {0} vez(es), com recorrência de {1} {2}(s)", ebcDelivOpt.resendAfterCount.ToString(), ebcDelivOpt.resendAfterPeriodUnit.ToString(), resendUnitType),
                            EmailTechnical = ebcDelivOpt.NotificationEmailTecnical == null ? "-" : ebcDelivOpt.NotificationEmailTecnical,
                            EmailFunctional = ebcDelivOpt.NotificationEmailFunctional == null ? "-" : ebcDelivOpt.NotificationEmailFunctional,
                            EmailMonitoring = ebcDelivOpt.NotificationEmailMonitoring == null ? "-" : ebcDelivOpt.NotificationEmailMonitoring
                        };

                        foreach (var ev in events)
                        {
                            string eventLabel = String.Empty;
                            switch (ev.EventType)
                            {
                                case 0:
                                    eventLabel = EVENT_TYPE.SUCCESS.ToString();
                                    break;
                                case 1:
                                    eventLabel = EVENT_TYPE.FAILURE.ToString();
                                    break;
                                case 2:
                                    eventLabel = EVENT_TYPE.INFORMATION.ToString();
                                    break;
                                default:
                                    eventLabel = "-";
                                    break;
                            }

                            outPacketDetail.documentEnvelopeEvents.Add(
                                new DocumentEvent
                                {
                                    Date = ev.EventDate.ToString(),
                                    EventType = ev.EventType,
                                    EventTypeLabel = eventLabel,
                                    Detail = ev.EventMessage
                                }
                            );
                        }
                    }

                    /********** Ficheiros do envelope **********/
                    PhysicalPackage physicalPack = new PhysicalPackage();

                    outPacketDetail.documentEnvelopeFiles = new DocumentEnvelopeFiles
                    {
                        PdfUnsigned = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.UNSIGNED_PDF_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.UNSIGNED_PDF_FILE_SUFFIX : "",
                        PdfSigned = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.SIGNED_PDF_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.SIGNED_PDF_FILE_SUFFIX : "",
                        Postscript = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.POSTSCRIPT_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.POSTSCRIPT_FILE_SUFFIX : "",
                        BasicMetadata = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.BASIC_METADATA_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.BASIC_METADATA_FILE_SUFFIX : "",
                        AttachOne = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.CUSTOM_METADATA_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.CUSTOM_METADATA_FILE_SUFFIX : "",
                        AttachTwo = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.ATT1_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.ATT1_SUFFIX : "",
                        AttachThree = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.ATT2_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.ATT2_SUFFIX : "",
                        Cancellation = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.CANCELLATION_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.CANCELLATION_SUFFIX : "",
                        EmailSent = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.EMAIL_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.EMAIL_FILE_SUFFIX : "",
                        UndeliveredReceipt = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.UNDELIVERED_RECEIPT_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.UNDELIVERED_RECEIPT_FILE_SUFFIX : "",
                        DeliveredReceipt = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.DELIVERED_RECEIPT_FILE_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.DELIVERED_RECEIPT_FILE_SUFFIX : "",
                        ReadReceipt = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.READ_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.READ_SUFFIX : "",
                        TransmissionReceipt = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.RELAY_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.RELAY_SUFFIX : "",
                        DelayReceipt = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.DELAY_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.DELAY_SUFFIX : "",
                        ExplicitReceipt = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.EMAIL_REPLY_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.EMAIL_REPLY_SUFFIX : "",
                        SuspensionEmail = physicalPack.PhysicalFileExists(outPacket.PKEBCPackageID.ToString(), PhysicalPackage.SUSPENSION_SUFFIX, ebcPackageYear.ToString(), ebcPackageMonth.ToString(), "out") ? outPacket.PKEBCPackageID + PhysicalPackage.SUSPENSION_SUFFIX : ""
                    };
                    /********** Ano e Mes de criação do pacote **********/
                    outPacketDetail.AnoCriacaoPacote = ebcPackageYear.ToString();
                    outPacketDetail.MesCriacaoPacote = ebcPackageMonth.ToString();

                    packetDetail.outboundDetail = outPacketDetail;
                }
            }
            else
            {
                using (CIC_DB entidadeCIC = new CIC_DB())
                {
                    /********** Dados Documento e Informação acerca do processo de integração **********/
                    var inPack = entidadeCIC.InboundPacket.Where(t => t.Identificador == id.Value).FirstOrDefault();

                    //email fornecedor
                    var emailFornecedor = entidadeCIC.Whitelist
                        .FirstOrDefault(wl => wl.PKWhitelistID == inPack.FKWhiteListID)
                        .EmailAddress;

                    InboundDetail inPacketDetail = new InboundDetail(
                        new InboundDocumentDetail
                        {
                            SupplierName = inPack.NomeFornec,
                            SupplierEmail = emailFornecedor,
                            SupplierNif = inPack.NIFE,
                            ClientNif = inPack.NIF,
                            OriginalDoc = inPack.DocOriginal,
                            DocNumber = inPack.NumFactura,
                            DocDate = inPack.DataFactura,
                            Amount = inPack.Quantia,
                            AmountWithoutTax = inPack.QuantiaSemIVA,
                            OrderNumber = inPack.NumEncomenda
                        },
                        new InboundDocumentIntegrationDetail
                        {
                            ProcessId = inPack.InternalProcessID != null ? inPack.InternalProcessID.ToString() : "",
                            TransformationMethod = "eBilling Connector Default Filter",
                            ReceptionDate = inPack.ReceptionDate.HasValue ? inPack.ReceptionDate.Value.ToString() : "",
                            ReceptionDateMonth = inPack.ReceptionDate.HasValue ? inPack.ReceptionDate.Value.Month.ToString() : "",
                            ReceptionDateYear = inPack.ReceptionDate.HasValue ? inPack.ReceptionDate.Value.Year.ToString() : "",
                            RequiresManualValidaiton = inPack.RequiresManualApproval.HasValue ? (inPack.RequiresManualApproval.Value == true ? "Sim" : "Não") : "",
                            IsValid = inPack.IsValid.HasValue ? (inPack.IsValid.Value == true ? "Sim" : "Não") : "",
                            ValidationDate = inPack.ApprovalDate.HasValue ? inPack.ApprovalDate.Value.ToString() : "",
                            SubmissionDate = inPack.SubmissionDate.HasValue ? inPack.SubmissionDate.Value.ToString() : "",
                            SubmissionFile = !String.IsNullOrWhiteSpace(inPack.SubmissionFile) ? inPack.SubmissionFile : "",
                            Returned = inPack.Devolvido.HasValue ? (inPack.Devolvido.Value == true ? "Sim" : "Não") : "",
                            Observations = inPack.Obs
                        },
                        null
                    );

                    /********** Ficheiros do envelope **********/
                    PhysicalPackage physicalPack = new PhysicalPackage();

                    inPacketDetail.inboundDocumentEnvelopeFiles = new InboundDocumentEnvelopeFiles
                    {
                        Email = physicalPack.PhysicalFileExists(inPack.InternalProcessID.ToString(), PhysicalPackage.INBOUND_EMAIL_SUFFIX, inPack.ReceptionDate.Value.Year.ToString(), inPack.ReceptionDate.Value.Month.ToString(), "in") ? inPack.InternalProcessID.ToString() + PhysicalPackage.INBOUND_EMAIL_SUFFIX : "",
                        Pdf = physicalPack.PhysicalFileExists(inPack.InternalProcessID.ToString(), PhysicalPackage.INBOUND_PDF_SUFFIX, inPack.ReceptionDate.Value.Year.ToString(), inPack.ReceptionDate.Value.Month.ToString(), "in") ? inPack.InternalProcessID.ToString() + PhysicalPackage.INBOUND_PDF_SUFFIX : "",
                        Xml = physicalPack.PhysicalFileExists(inPack.InternalProcessID.ToString(), PhysicalPackage.INBOUND_XML_SUFFIX, inPack.ReceptionDate.Value.Year.ToString(), inPack.ReceptionDate.Value.Month.ToString(), "in") ? inPack.InternalProcessID.ToString() + PhysicalPackage.INBOUND_XML_SUFFIX : "",
                    };

                    packetDetail.inboundDetail = inPacketDetail;
                }
            }
            return packetDetail;
        }

        public string GetFilePath(string filename, string sentido, string ano, string mes)
        {
            PhysicalPackage physicalPack = new PhysicalPackage();

            return physicalPack.PhysicalFilePath(filename, ano, mes, sentido);
        }

        public string GetFilenameWithRealExtension(string filename)
        {
            PhysicalPackage physicalPack = new PhysicalPackage();

            return physicalPack.PhysicalFilenameWithRealExtension(filename);
        }

        public static void LogMessageToFile(string msg, string path)
        {
            StreamWriter sw = File.AppendText(path);
            try
            {
                string logLine = System.String.Format("{0:G}:" + System.DateTime.Now.Millisecond.ToString() + ": {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

        public void UpdateOutStatToDelivered(string id)
        {
            using (CIC_DB entidadeCIC = new CIC_DB())
            {
                Guid pkid = Guid.Parse(id);
                var matchedOutPackets = entidadeCIC.OutboundPacket
                    .FirstOrDefault(x => x.PKEBCPackageID == pkid);

                matchedOutPackets.CurrentEBCState = "DELIVERED";

                entidadeCIC.SaveChanges();
            }
        }
    }
}
