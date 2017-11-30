using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace eBillingSuite.Model.HelpingClasses
{
    public class Dashboard
    {
        
        public class DashboardChart
        {
            public string label { get; set; }
            public double data { get; set; }
        }

        public class DashboardEvent
        {
            public string eventName { get; set; }
            public string eventInfo { get; set; }
            public DateTime? eventDate { get; set; }
            public string eventState { get; set; }
            public int eventIdentifier { get; set; }
            public string eventDirection { get; set; }
        }

        /* Facturação Electrónica */
        /// <summary>
        /// Obtém o nº total de pacotes In ou Out
        /// </summary>
        /// <param name="type">Tipo a obter, se Inbound ou Outbound</param>
        /// <returns>nº de pacotes encontrados</returns>
        public static string GetTotalPackets(string type)
        {
            int count = 0;
            try
            {                
                using(CIC_DB.CIC_DB entidadeCIC = new CIC_DB.CIC_DB())
                {
                    if (type.ToLower().Equals("out"))
                    {
                        //alterada 16-04-2014
                        //int count2 = (from op in entidadeCIC.OutboundPacket.AsEnumerable() select op.Identificador)
                        //    .Count();
                        count = entidadeCIC.OutboundPacket.Count();
                    }
                    else
                    {
                        //alterada 16-04-2014
                        //count = (from op in entidadeCIC.InboundPacket.AsEnumerable() select op.Identificador)
                        //    .Count();
                        count = entidadeCIC.InboundPacket.Count();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return count.ToString();
        }

        /// <summary>
        /// Obtém o nº de pacotes In ou Out para cada mês
        /// </summary>
        /// <param name="type">Tipo a obter, se Inbound ou Outbound</param>
        /// <param name="pastMonthNum">nº de meses a retroceder</param>
        /// <returns>array com o nº total de pacotes para cada mês</returns>
        public static string[] GetMonthlyTotalPackets(string type, int pastMonthNum)
        {
            string[] monthValues = new string[pastMonthNum];
            try
            {
                using (CIC_DB.CIC_DB entidadeCIC = new CIC_DB.CIC_DB())
                {
                    //int count = 0;

                    DateTime now = DateTime.Now;

                    if (type.ToLower().Equals("out"))
                    {
                        /************ OLD FASHION *************/
                        ////Iterar de forma descendente pelo nº de meses exigidos
                        //var outpacks = (from op in entidadeCIC.OutboundPacket.AsEnumerable() select op);
                        //int temp = outpacks.ToList().Count;
                        ////Iterar de forma descendente pelo nº de meses exigidos
                        //for (int i = -1; i >= pastMonthNum * (-1); i--)
                        //{
                        //    count = (outpacks
                        //        .Where(op => (!String.IsNullOrEmpty(op.DataFactura)))
                        //        .Where(op => Convert.ToDateTime(op.DataFactura).Month.Equals(DateTime.Now.Date.AddMonths(i).Month))
                        //        .Where(op => Convert.ToDateTime(op.DataFactura).Year.Equals(DateTime.Now.Date.AddMonths(i).Year)))
                        //        .Count();

                        //    monthValues[j] = count.ToString();
                        //    count = 0;
                        //    j++;
                        //}

                        /*************** IMPROVED ******************/
                        //Alterado 14-04-2014
                        //var outpacks = (from op in entidadeCIC.OutboundPacket.AsEnumerable() select op)
                        //    .Where(op => (op.CreationDate != null && op.CreationDate.Value != null))
                        //    .Where(op => op.CreationDate >= new DateTime(now.Year, now.Month, 1).AddMonths(pastMonthNum * -1))
                        //    .OrderByDescending(op => op.CreationDate.Value.Year)
                        //    .ThenByDescending(op => op.CreationDate.Value.Month)
                        //    .GroupBy(op => op.CreationDate.Value.Month, op2 => op2.CreationDate.Value.Year)
                        //    .Select(grp=>grp.ToList());

                        //int j = 0;
                        //foreach (List<int> l in outpacks)
                        //{
                        //    monthValues[j] = l.Count.ToString();
                        //    j++;
                        //}

                        /***************** AD HOC ***************/
                        string sql = "SELECT COUNT(OutboundPacket.Identificador) FROM OutboundPacket WHERE CreationDate is not null and" +
                            " CreationDate > DATEADD(m, @NumberMonths, DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0)) GROUP BY MONTH(CreationDate), YEAR(CreationDate)" +
                            " ORDER BY YEAR(CreationDate) desc, MONTH(CreationDate) desc";
                        try
                        {
                            using (DbConnection conn = entidadeCIC.Database.Connection)
                            {
                                ConnectionState initialState = conn.State;
                                if (initialState != ConnectionState.Open)
                                    conn.Open();

                                using (DbCommand cmd = conn.CreateCommand())
                                {
                                    cmd.Parameters.Add(new SqlParameter("@NumberMonths", -1 * pastMonthNum));

                                    cmd.CommandText = sql;
                                    using (DbDataReader reader = cmd.ExecuteReader())
                                    {
                                        int j = 0;
                                        while (reader.Read())
                                        {
                                            monthValues[j] = reader.GetInt32(0).ToString();
                                            j++;
                                        }
                                    }
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        /************ OLD FASHION *************/
                        //var inpacks = (from ip in entidadeCIC.InboundPacket.AsEnumerable() select ip);
                        //int j = 0;
                        ////Iterar de forma descendente pelo nº de meses exigidos
                        //for (int i = -1; i >= pastMonthNum * (-1); i--)
                        //{
                        //    count = (inpacks
                        //        .Where(ip => (ip.ReceptionDate != null && ip.ReceptionDate.Value != null))
                        //        .Where(ip => ip.ReceptionDate.Value.Month.Equals(DateTime.Now.Date.AddMonths(i).Month))
                        //        .Where(ip => ip.ReceptionDate.Value.Year.Equals(DateTime.Now.Date.AddMonths(i).Year)))
                        //        .Count();

                        //    monthValues[j] = count.ToString();
                        //    count = 0;
                        //    j++;
                        //}

                        /*************** IMPROVED ******************/
                        //Alterado 14-04-2014
                        //var inpacks = (from ip in entidadeCIC.InboundPacket.AsEnumerable() select ip)
                        //    .Where(ip => (ip.ReceptionDate != null && ip.ReceptionDate.Value != null))
                        //    .Where(ip => ip.ReceptionDate >= new DateTime(now.Year, now.Month, 1).AddMonths(pastMonthNum * -1))
                        //    .OrderByDescending(ip => ip.ReceptionDate.Value.Year)
                        //    .ThenByDescending(ip => ip.ReceptionDate.Value.Month)
                        //    .GroupBy(ip => ip.ReceptionDate.Value.Month, ip2 => ip2.ReceptionDate.Value.Year)
                        //    .Select(grp => grp.ToList());

                        //int j = 0;
                        //foreach (List<int> l in inpacks)
                        //{
                        //    monthValues[j] = l.Count.ToString();
                        //    j++;
                        //}

                        /***************** AD HOC ***************/
                        string sql = "SELECT COUNT(InboundPacket.Identificador) FROM InboundPacket" +
                            " WHERE ReceptionDate is not null and" +
                            " ReceptionDate > DATEADD(m, @NumberMonths, DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0))" +
                            " GROUP BY MONTH(ReceptionDate), YEAR(ReceptionDate)" +
                            " ORDER BY YEAR(ReceptionDate) desc, MONTH(ReceptionDate) desc";
                        try
                        {
                            using (DbConnection conn = entidadeCIC.Database.Connection)
                            {
                                ConnectionState initialState = conn.State;
                                if (initialState != ConnectionState.Open)
                                    conn.Open();

                                using (DbCommand cmd = conn.CreateCommand())
                                {
                                    cmd.Parameters.Add(new SqlParameter("@NumberMonths", -1 * pastMonthNum));

                                    cmd.CommandText = sql;
                                    using (DbDataReader reader = cmd.ExecuteReader())
                                    {
                                        int j = 0;
                                        while (reader.Read())
                                        {
                                            monthValues[j] = reader.GetInt32(0).ToString();
                                            j++;
                                        }
                                    }
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return monthValues;
        }

        /// <summary>
        /// Obtém o nº total de pacotes em cada estado
        /// </summary>
        /// <param name="type">Tipo a obter, se Inbound ou Outbound</param>
        /// <returns>Lista com o nº total de pacotes para cada estado</returns>
        public static List<DashboardChart> GetPacketsByState(string type, string totalPackages)
        {
            List<DashboardChart> lista = new List<DashboardChart>();
            try
            {
                using (CIC_DB.CIC_DB entidadeCIC = new CIC_DB.CIC_DB())
                {
                    if (type.ToLower().Equals("out"))
                    {
                        var query = (from p in entidadeCIC.OutboundPacket
                                     group p by p.CurrentEBCState.ToLower() into g
                                     select new
                                     {
                                         Valor = g.Count(),
                                         CurrentEBCState = g.Key.ToLower()

                                     }).OrderByDescending(x => x.Valor);

                        //para cada linha, contruir objecto a passar ao controlador
                        int i = 0;
                        double totalDeliv = 0;
                        foreach (var item in query)
                        {
                            if (item.CurrentEBCState.ToLower().Equals("delivered") || item.CurrentEBCState.ToLower().Equals("delivered_attach_error"))
                                totalDeliv = totalDeliv + Math.Round((double.Parse(item.Valor.ToString()) / double.Parse(totalPackages)) * 100, 2);
                            else
                            {
                                DashboardChart dc = new DashboardChart();
                                if (item.CurrentEBCState.ToLower().Equals("undelivered"))
                                    dc.label = "UNDEVLIV.";
                                else
                                    dc.label = item.CurrentEBCState.ToUpper();
                                dc.data = Math.Round((double.Parse(item.Valor.ToString()) / double.Parse(totalPackages)) * 100, 2); //valor em percentagem                                      
                                lista.Add(dc);
                            }
                            i++;
                        }
                        DashboardChart dac = new DashboardChart();
                        dac.label = "DELIVERED";
                        dac.data = totalDeliv;
                        lista.Add(dac);
                    }
                    else
                    {
                        //se tem submissionFile significa que foi enviado para integração
                        var submitedCount = (from ip in entidadeCIC.InboundPacket.AsEnumerable() select ip)
                            .Where(ip => (!String.IsNullOrEmpty(ip.SubmissionFile) && !ip.SubmissionFile.ToLower().Equals("null")))
                            .Count();
                        var notSubmitedCount = (from ip in entidadeCIC.InboundPacket.AsEnumerable() select ip)
                            .Where(ip => (String.IsNullOrEmpty(ip.SubmissionFile) || ip.SubmissionFile.ToLower().Equals("null")))
                            .Count();

                        //contruir objectos para o controlador
                        lista.Add(
                            new DashboardChart
                            {
                                label = "INTEGRATED",
                                data = (Math.Round((double.Parse(submitedCount.ToString()) / double.Parse(totalPackages)) * 100, 2))
                            });
                        lista.Add(
                            new DashboardChart
                            {
                                label = "UNINT.",
                                data = (Math.Round((double.Parse(notSubmitedCount.ToString()) / double.Parse(totalPackages)) * 100, 2))
                            });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return lista;
        }

        /* Comunicação à AT (fact. e guias)*/
        /// <summary>
        /// Método que obtém percentagem de envios AT com sucesso, nº de envios com sucesso e nº de envios insucesso
        /// </summary>
        /// <param name="type">se é para facturas ("invoice") ou transportes ("transport")</param>
        /// <returns>na 'label' de 'DashboardChart' vai 'nº sucesso/nº total'. Na 'data' vai a percentagem de sucessos</returns>
        public static DashboardChart GetComAtInfo(string type)
        {
            DashboardChart dc = new DashboardChart();
            int totalsuccess = 0, total = 0;
            double successPercent = 0;
            try
            {
                using (EBC_DB.EBC_DB entidadeEBC = new EBC_DB.EBC_DB())
                {
                    if (type.ToLower().Equals("invoice"))
                    {
                        totalsuccess = (from comat in entidadeEBC.ComATPackets select comat)
                            .Where(comat => (comat.EstadoAT.Equals("1") || comat.EstadoAT.Equals("-3"))) //entregue OU repetido
                            .Count();
                        total = (from comat in entidadeEBC.ComATPackets select comat.pkid).Count();

                        if ((totalsuccess.Equals(0) && total.Equals(0)) || (!totalsuccess.Equals(0) && total.Equals(0)))
                            successPercent = 0;
                        else
                        {
                            successPercent = ((double)totalsuccess / (double)total) * (double)100;
                            successPercent = Math.Round(successPercent, 0);
                        }
                    }
                    else
                    {
                        totalsuccess = (from comat in entidadeEBC.ComATPackets_Guias select comat)
                            .Where(comat => comat.EstadoAT.Equals("1"))
                            .Count();
                        total = (from comat in entidadeEBC.ComATPackets_Guias select comat.pkid).Count();

                        if ((totalsuccess.Equals(0) && total.Equals(0)) || (!totalsuccess.Equals(0) && total.Equals(0)))
                            successPercent = 0;
                        else
                        {
                            successPercent = ((double)totalsuccess / (double)total) * (double)100;
                            successPercent = Math.Round(successPercent, 0);
                        }
                    }

                    dc.label = totalsuccess.ToString() + "/" + total.ToString();
                    dc.data = successPercent;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return dc;
        }

        /// <summary>
        /// Obtém para cada mês, o nº de erros ocorridos
        /// </summary>
        /// <param name="type">se é para facturas ("invoice") ou transportes ("transport")</param>
        /// <param name="pastMonthNum">nº de meses a obter</param>
        /// <returns>array com o nº total de erros para cada mês</returns>
        public static string[] GetMonthlyComAtErrors(string type, int pastMonthNum)
        {
            string[] monthValues = new string[pastMonthNum];
            try
            {
                using (EBC_DB.EBC_DB entidadeEBC = new EBC_DB.EBC_DB())
                {
                    int count = 0;
                    if (type.ToLower().Equals("invoice"))
                    {
                        var atpacks = (from cap in entidadeEBC.ComATPackets.AsEnumerable() select cap);
                        int j = 0;
                        //Iterar de forma descendente pelo nº de meses exigidos
                        for (int i = -1; i >= pastMonthNum * (-1); i--)
                        {
                            count = (atpacks
                                .Where(cap => (cap.LastSentDate != null)))
                                .Where(cap => (!cap.EstadoAT.Equals("1") && !cap.EstadoAT.Equals("-3"))) //não está entregue NEM repetido
                                .Where(cap => cap.LastSentDate.Value.Month.Equals(DateTime.Now.Date.AddMonths(i).Month))
                                .Where(cap => cap.LastSentDate.Value.Year.Equals(DateTime.Now.Date.AddMonths(i).Year))
                                .Count();

                            monthValues[j] = count.ToString();
                            count = 0;
                            j++;
                        }
                    }
                    else
                    {
                        var atpacks = (from capg in entidadeEBC.ComATPackets_Guias.AsEnumerable() select capg);
                        int j = 0;
                        //Iterar de forma descendente pelo nº de meses exigidos
                        for (int i = -1; i >= pastMonthNum * (-1); i--)
                        {
                            count = (atpacks
                                .Where(capg => (capg.LastSentDate != null)))
                                .Where(capg => !capg.EstadoAT.Equals("1")) //não esteja entregue
                                .Where(capg => capg.LastSentDate.Value.Month.Equals(DateTime.Now.Date.AddMonths(i).Month))
                                .Where(capg => capg.LastSentDate.Value.Year.Equals(DateTime.Now.Date.AddMonths(i).Year))
                                .Count();

                            monthValues[j] = count.ToString();
                            count = 0;
                            j++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return monthValues;
        }

        /// <summary>
        /// Obtém o nº total de erros existentes na comunicação à AT
        /// </summary>
        /// <param name="type">se é para facturas ("invoice") ou transportes ("transport")</param>
        /// <returns>nº total de erros</returns>
        public static string GetTotalComAtErrors(string type)
        {
            int totalErrors = 0;
            try
            {
                using (EBC_DB.EBC_DB entidadeEBC = new EBC_DB.EBC_DB())
                {
                    if (type.ToLower().Equals("invoice"))
                    {
                        totalErrors = (from comat in entidadeEBC.ComATPackets select comat)
                            .Where(comat => (!comat.EstadoAT.Equals("1") && !comat.EstadoAT.Equals("-3"))) //não esteja entregue NEM repetido
                            .Count();
                    }
                    else
                    {
                        totalErrors = (from comat in entidadeEBC.ComATPackets_Guias select comat)
                            .Where(comat => !comat.EstadoAT.Equals("1"))
                            .Count();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return totalErrors.ToString();
        }

        /* Usada para FE e AT*/
        /// <summary>
        /// Obtém os últimos eventos
        /// </summary>
        /// <param name="type">Tipo a obter, se Facturação Electrónica ou Comunicações AT</param>
        /// <param name="pastEventNum">nº de eventos a obter</param>
        /// <returns>Lista de eventos</returns>
        public static List<DashboardEvent> GetLastEvents(string type, int pastEventNum)
        {
            List<DashboardEvent> events = new List<DashboardEvent>();
            List<DashboardEvent> orderedEvents = new List<DashboardEvent>();
            try
            {

                if (type.ToLower().Equals("fe"))
                {
                    //eventos de FE
                    using (CIC_DB.CIC_DB entidadeCIC = new CIC_DB.CIC_DB())
                    {
                        //obter os pacotes IN e OUT
                        var outboundPackets = (from outpack in entidadeCIC.OutboundPacket select outpack)
                            .OrderByDescending(op => op.CreationDate)
                            .Take(15);
                        var inboundPackets = (from intpack in entidadeCIC.InboundPacket select intpack)
                            .OrderByDescending(ip => ip.ReceptionDate)
                            .Take(15);

                        foreach (var item in outboundPackets)
                        {
                            DashboardEvent de = new DashboardEvent();
                            de.eventName = "Documento enviado";
                            de.eventInfo = item.NumFactura;
                            de.eventDate = item.CreationDate;
                            if (item.CurrentEBCState.ToLower().Equals("delivered"))
                                de.eventState = "delivered";
                            else
                                de.eventState = "undelivered";
                            de.eventIdentifier = item.Identificador;
                            de.eventDirection = "OUT";
                            events.Add(de);
                        }
                        foreach (var item in inboundPackets)
                        {
                            DashboardEvent de = new DashboardEvent();
                            de.eventName = "Documento recebido";
                            de.eventInfo = item.NumFactura;
                            de.eventDate = item.ReceptionDate;
                            if (!String.IsNullOrEmpty(item.SubmissionFile) && !item.SubmissionFile.ToLower().Equals("null"))
                                de.eventState = "delivered";
                            else
                                de.eventState = "undelivered";
                            de.eventIdentifier = item.Identificador;
                            de.eventDirection = "IN";
                            events.Add(de);
                        }

                        orderedEvents = events
                            .OrderByDescending(op => op.eventDate)
                            .Take(pastEventNum)
                            .ToList();
                    }
                }
                else
                {
                    //eventos da comunicação AT
                    using (EBC_DB.EBC_DB entidadeEBC = new EBC_DB.EBC_DB())
                    {
                        var invoiceAtPackets = (from outpack in entidadeEBC.ComATPackets select outpack)
                            .OrderByDescending(op => op.LastSentDate)
                            .Take(15);
                        var transportAtPackets = (from intpack in entidadeEBC.ComATPackets_Guias select intpack)
                            .OrderByDescending(ip => ip.LastSentDate)
                            .Take(15);

                        foreach (var item in invoiceAtPackets)
                        {
                            DashboardEvent de = new DashboardEvent();
                            de.eventName = "Documento comunicado";
                            de.eventInfo = item.NumeroDocumento;
                            de.eventDate = item.LastSentDate;
                            if (item.EstadoAT.ToLower().Equals("1"))
                                de.eventState = "success";
                            else
                                de.eventState = "insuccess";
                            events.Add(de);
                        }
                        foreach (var item in transportAtPackets)
                        {
                            DashboardEvent de = new DashboardEvent();
                            de.eventName = "Documento comunicado";
                            de.eventInfo = item.NumeroDocumento;
                            de.eventDate = item.LastSentDate;
                            if (item.EstadoAT.ToLower().Equals("1"))
                                de.eventState = "success";
                            else
                                de.eventState = "insuccess";
                            events.Add(de);
                        }

                        orderedEvents = events
                            .OrderByDescending(op => op.eventDate)
                            .Take(pastEventNum)
                            .ToList();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return orderedEvents;
        }
    }
}
