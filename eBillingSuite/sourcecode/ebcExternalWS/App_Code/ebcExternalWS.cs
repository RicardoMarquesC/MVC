using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for ebcExternalWS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ebcExternalWS : System.Web.Services.WebService
{
    public ebcExternalWS()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        //return "Hello World";
        return Context.User.Identity.Name + ", Authen. type: " + Context.User.Identity.AuthenticationType +
       ", Authenticated: " + Context.User.Identity.IsAuthenticated.ToString() + ", " +
       DateTime.Now.Minute + ":".ToString() + DateTime.Now.Second;
    }

    [WebMethod]
    //retorna tudo o que existe na base de dados Out e In
    public string GetAll()
    {
        string json = "";
        //obter os dados pretendidos
        json = GetAllInboundinDB();
        json += "|";
        json += GetAllOutboundinDB();

        //transformar em objecto json
        return json;
    }

    [WebMethod]
    public string GetAllOutbound()
    {
        string json = "";
        //retorna tudo o que é Outbound
        return json = GetAllOutboundinDB();
    }

    [WebMethod]
    public string GetAllOutboundByNIF(string nifRecetor, string nifEmissor = null)
    {
        string json = "";
        //retorna tudo o que é Outbound
        if(string.IsNullOrWhiteSpace(nifEmissor))
            return json = GetAllOutboundinDB(nifRecetor);

        return json = GetAllOutboundinDB(nifRecetor, nifEmissor);
    }

    [WebMethod]
    public string GetAllOutboundByRangeDate(string dataI, string dataF)
    {
        string json = "";
        //retorna tudo o que é Outbound
        DateTime dataInicial = DateTime.Parse(dataI);
        DateTime dataFinal = string.IsNullOrWhiteSpace(dataF) ? DateTime.Now : DateTime.Parse(dataF);

        return json = GetAllOutboundinDB(dataInicial, dataFinal);
    }

    [WebMethod]
    public string GetAllOutboundByNumDoc(string numDocument)
    {
        string json = "";
        //retorna tudo o que é Outbound

        return json = GetAllOutboundinDB(numDocument, 0);
    }
    

    [WebMethod]
    public string GetAllInbound()
    {
        string json = "";
        //retorna tudo o que é Inbound
        return json = GetAllInboundinDB();

    }

   


    [WebMethod(Description = "Out = 0 | In = 1")]
    public string GetByID(int ID, int OutOrIn)
    {
        if (OutOrIn == 0)
        {
            List<OutboundPacket> listOutboundP = new List<OutboundPacket>();

            string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string query = "SELECT [DMSID],[NomeReceptor],[EmailReceptor],[NIFReceptor],[NumFactura],[DataFactura],[QuantiaComIVA],[CreationDate],[CurrentEBCState],[LastUpdate],[DocOriginal],[QuantiaSemIVA],[Moeda],[QuantiaComIVAMoedaInterna],[QuantiaSemIVAMoedaInterna],[TaxaCambio],[CondicaoPagamento],[SWIFT],[IBAN],[TipoDocumento],[NomeEmissor],[NIFEmissor],[Referencia],[Contentor],[CGA],[NumEncomenda],[Identificador] FROM [dbo].[OutboundPacket] where Identificador=@num";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@num", SqlDbType.Int).Value = ID;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            OutboundPacket outboundP = new OutboundPacket();

                            #region passar os dados para a variavel inboundP
                            outboundP.DMSID = dr.GetValue(0).ToString();
                            outboundP.NomeReceptor = dr.GetValue(1).ToString();
                            outboundP.EmailReceptor = dr.GetValue(2).ToString();
                            outboundP.NIFReceptor = dr.GetValue(3).ToString();
                            outboundP.NumFactura = dr.GetValue(4).ToString();
                            outboundP.DataFactura = dr.GetValue(5).ToString();
                            outboundP.QuantiaComIVA = dr.GetValue(6).ToString();
                            outboundP.CreationDate = DateTime.Parse(dr.GetValue(7).ToString());
                            outboundP.CurrentEBCState = dr.GetValue(8).ToString();
                            outboundP.LastUpdate = DateTime.Parse(dr.GetValue(9).ToString());
                            outboundP.DocOriginal = dr.GetValue(10).ToString();
                            outboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                            outboundP.Moeda = dr.GetValue(12).ToString();
                            outboundP.QuantiaComIVAMoedaInterna = dr.GetValue(13).ToString();
                            outboundP.QuantiaSemIVAMoedaInterna = dr.GetValue(14).ToString();
                            outboundP.TaxaCambio = dr.GetValue(15).ToString();
                            outboundP.CondicaoPagamento = dr.GetValue(16).ToString();
                            outboundP.SWIFT = dr.GetValue(17).ToString();
                            outboundP.IBAN = dr.GetValue(18).ToString();
                            outboundP.TipoDocumento = dr.GetValue(19).ToString();
                            outboundP.NomeEmissor = dr.GetValue(20).ToString();
                            outboundP.NIFEmissor = dr.GetValue(21).ToString();
                            outboundP.Referencia = dr.GetValue(22).ToString();
                            outboundP.Contentor = dr.GetValue(23).ToString();
                            outboundP.CGA = dr.GetValue(24).ToString();
                            outboundP.NumEncomenda = dr.GetValue(25).ToString();
                            outboundP.Identificador = int.Parse(dr.GetValue(26).ToString());
                            outboundP.InorOut = false;
                            #endregion

                            listOutboundP.Add(outboundP);
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(listOutboundP);
        }
        else
        {
            List<InboundPacket> listInboundP = new List<InboundPacket>();

            string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string query = "SELECT [NIF],[NumEncomenda],[NumFactura],[DataFactura],[Quantia],[ReceptionDate],[FKWhiteListID],[NIFE],[DocOriginal],[Obs],[Devolvido],[QuantiaSemIVA],[NomeFornec],[Moeda],[QuantIVAMI],[QuantSIVAMI],[TCambio],[CondicaoPag],[SWIFT],[IBAN],[TipoDoc],[NomeReceptor],[EmailCliente],[Referencia],[ImportExport],[ViagemPartida],[CGA],[Contentor],[Identificador] FROM [dbo].[InboundPacket] where Identificador=@num";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@num", SqlDbType.Int).Value = ID;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            InboundPacket inboundP = new InboundPacket();

                            #region passar os dados para a variavel inboundP
                            inboundP.NIF = dr.GetValue(0).ToString();
                            inboundP.NumEncomenda = dr.GetValue(1).ToString();
                            inboundP.NumFactura = dr.GetValue(2).ToString();
                            inboundP.DataFactura = dr.GetValue(3).ToString();
                            inboundP.Quantia = dr.GetValue(4).ToString();
                            inboundP.ReceptionDate = DateTime.Parse(dr.GetValue(5).ToString());
                            inboundP.FKWhiteListID = Guid.Parse(dr.GetValue(6).ToString());
                            inboundP.NIFE = dr.GetValue(7).ToString();
                            inboundP.DocOriginal = dr.GetValue(8).ToString();
                            inboundP.Obs = dr.GetValue(9).ToString();
                            inboundP.Devolvido = bool.Parse(dr.GetValue(10).ToString());
                            inboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                            inboundP.NomeFornec = dr.GetValue(12).ToString();
                            inboundP.Moeda = dr.GetValue(13).ToString();
                            inboundP.QuantIVAMI = dr.GetValue(14).ToString();
                            inboundP.QuantSIVAMI = dr.GetValue(15).ToString();
                            inboundP.TCambio = dr.GetValue(16).ToString();
                            inboundP.CondicaoPag = dr.GetValue(17).ToString();
                            inboundP.SWIFT = dr.GetValue(18).ToString();
                            inboundP.IBAN = dr.GetValue(19).ToString();
                            inboundP.TipoDoc = dr.GetValue(20).ToString();
                            inboundP.NomeReceptor = dr.GetValue(21).ToString();
                            inboundP.EmailCliente = dr.GetValue(22).ToString();
                            inboundP.Referencia = dr.GetValue(23).ToString();
                            inboundP.ImportExport = dr.GetValue(24).ToString();
                            inboundP.ViagemPartida = dr.GetValue(25).ToString();
                            inboundP.CGA = dr.GetValue(26).ToString();
                            inboundP.Contentor = dr.GetValue(27).ToString();
                            inboundP.Identificador = int.Parse(dr.GetValue(28).ToString());
                            inboundP.InorOut = true;
                            #endregion

                            listInboundP.Add(inboundP);
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(listInboundP);
        }
    }


    #region HELPERS
    //private string GetDocumentList(int sentido, int? numDocument, string nifRecetor, string nifEmissor, string dataInicial, string dataFinal, int? estado)
    //{
    //    switch (sentido)
    //    {
    //        case 1:
    //            List<InboundPacket> listaIN = new List<InboundPacket>();


    //            return JsonConvert.SerializeObject(listaIN);
    //        case 2:
    //            List<OutboundPacket> listaOut = new List<OutboundPacket>();

    //            return JsonConvert.SerializeObject(listaOut);
    //        default:
    //            return JsonConvert.SerializeObject("Opção Errada!");
    //    }
    //}

    #region Outbound
    private string GetAllOutboundinDB(string nifRecetor)
    {
        List<OutboundPacket> listOutboundP = new List<OutboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [DMSID],[NomeReceptor],[EmailReceptor],[NIFReceptor],[NumFactura],[DataFactura],[QuantiaComIVA],[CreationDate],[CurrentEBCState],[LastUpdate],[DocOriginal],[QuantiaSemIVA],[Moeda],[QuantiaComIVAMoedaInterna],[QuantiaSemIVAMoedaInterna],[TaxaCambio],[CondicaoPagamento],[SWIFT],[IBAN],[TipoDocumento],[NomeEmissor],[NIFEmissor],[Referencia],[Contentor],[CGA],[NumEncomenda],[Identificador] FROM [dbo].[OutboundPacket] where NIFRecetor = @nifR";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.Add("@nifR", SqlDbType.NVarChar).Value = nifRecetor;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        OutboundPacket outboundP = new OutboundPacket();

                        #region passar os dados para a variavel outboundP
                        outboundP.DMSID = dr.GetValue(0).ToString();
                        outboundP.NomeReceptor = dr.GetValue(1).ToString();
                        outboundP.EmailReceptor = dr.GetValue(2).ToString();
                        outboundP.NIFReceptor = dr.GetValue(3).ToString();
                        outboundP.NumFactura = dr.GetValue(4).ToString();
                        outboundP.DataFactura = dr.GetValue(5).ToString();
                        outboundP.QuantiaComIVA = dr.GetValue(6).ToString();
                        outboundP.CreationDate = DateTime.Parse(dr.GetValue(7).ToString());
                        outboundP.CurrentEBCState = dr.GetValue(8).ToString();
                        outboundP.LastUpdate = DateTime.Parse(dr.GetValue(9).ToString());
                        outboundP.DocOriginal = dr.GetValue(10).ToString();
                        outboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        outboundP.Moeda = dr.GetValue(12).ToString();
                        outboundP.QuantiaComIVAMoedaInterna = dr.GetValue(13).ToString();
                        outboundP.QuantiaSemIVAMoedaInterna = dr.GetValue(14).ToString();
                        outboundP.TaxaCambio = dr.GetValue(15).ToString();
                        outboundP.CondicaoPagamento = dr.GetValue(16).ToString();
                        outboundP.SWIFT = dr.GetValue(17).ToString();
                        outboundP.IBAN = dr.GetValue(18).ToString();
                        outboundP.TipoDocumento = dr.GetValue(19).ToString();
                        outboundP.NomeEmissor = dr.GetValue(20).ToString();
                        outboundP.NIFEmissor = dr.GetValue(21).ToString();
                        outboundP.Referencia = dr.GetValue(22).ToString();
                        outboundP.Contentor = dr.GetValue(23).ToString();
                        outboundP.CGA = dr.GetValue(24).ToString();
                        outboundP.NumEncomenda = dr.GetValue(25).ToString();
                        outboundP.Identificador = int.Parse(dr.GetValue(26).ToString());
                        outboundP.InorOut = false;
                        #endregion

                        listOutboundP.Add(outboundP);
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(listOutboundP);
    }

    private string GetAllOutboundinDB(string nifRecetor, string nifEmissor)
    {
        List<OutboundPacket> listOutboundP = new List<OutboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [DMSID],[NomeReceptor],[EmailReceptor],[NIFReceptor],[NumFactura],[DataFactura],[QuantiaComIVA],[CreationDate],[CurrentEBCState],[LastUpdate],[DocOriginal],[QuantiaSemIVA],[Moeda],[QuantiaComIVAMoedaInterna],[QuantiaSemIVAMoedaInterna],[TaxaCambio],[CondicaoPagamento],[SWIFT],[IBAN],[TipoDocumento],[NomeEmissor],[NIFEmissor],[Referencia],[Contentor],[CGA],[NumEncomenda],[Identificador] FROM [dbo].[OutboundPacket] where NIFRecetor = @nifR AND NIFEmissor = @nifE";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.Add("@nifR", SqlDbType.NVarChar).Value = nifRecetor;
                cmd.Parameters.Add("@nifE", SqlDbType.NVarChar).Value = nifEmissor;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        OutboundPacket outboundP = new OutboundPacket();

                        #region passar os dados para a variavel outboundP
                        outboundP.DMSID = dr.GetValue(0).ToString();
                        outboundP.NomeReceptor = dr.GetValue(1).ToString();
                        outboundP.EmailReceptor = dr.GetValue(2).ToString();
                        outboundP.NIFReceptor = dr.GetValue(3).ToString();
                        outboundP.NumFactura = dr.GetValue(4).ToString();
                        outboundP.DataFactura = dr.GetValue(5).ToString();
                        outboundP.QuantiaComIVA = dr.GetValue(6).ToString();
                        outboundP.CreationDate = DateTime.Parse(dr.GetValue(7).ToString());
                        outboundP.CurrentEBCState = dr.GetValue(8).ToString();
                        outboundP.LastUpdate = DateTime.Parse(dr.GetValue(9).ToString());
                        outboundP.DocOriginal = dr.GetValue(10).ToString();
                        outboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        outboundP.Moeda = dr.GetValue(12).ToString();
                        outboundP.QuantiaComIVAMoedaInterna = dr.GetValue(13).ToString();
                        outboundP.QuantiaSemIVAMoedaInterna = dr.GetValue(14).ToString();
                        outboundP.TaxaCambio = dr.GetValue(15).ToString();
                        outboundP.CondicaoPagamento = dr.GetValue(16).ToString();
                        outboundP.SWIFT = dr.GetValue(17).ToString();
                        outboundP.IBAN = dr.GetValue(18).ToString();
                        outboundP.TipoDocumento = dr.GetValue(19).ToString();
                        outboundP.NomeEmissor = dr.GetValue(20).ToString();
                        outboundP.NIFEmissor = dr.GetValue(21).ToString();
                        outboundP.Referencia = dr.GetValue(22).ToString();
                        outboundP.Contentor = dr.GetValue(23).ToString();
                        outboundP.CGA = dr.GetValue(24).ToString();
                        outboundP.NumEncomenda = dr.GetValue(25).ToString();
                        outboundP.Identificador = int.Parse(dr.GetValue(26).ToString());
                        outboundP.InorOut = false;
                        #endregion

                        listOutboundP.Add(outboundP);
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(listOutboundP);
    }

    private string GetAllOutboundinDB(string numDocument, int? estado)
    {
        List<OutboundPacket> listOutboundP = new List<OutboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [DMSID],[NomeReceptor],[EmailReceptor],[NIFReceptor],[NumFactura],[DataFactura],[QuantiaComIVA],[CreationDate],[CurrentEBCState],[LastUpdate],[DocOriginal],[QuantiaSemIVA],[Moeda],[QuantiaComIVAMoedaInterna],[QuantiaSemIVAMoedaInterna],[TaxaCambio],[CondicaoPagamento],[SWIFT],[IBAN],[TipoDocumento],[NomeEmissor],[NIFEmissor],[Referencia],[Contentor],[CGA],[NumEncomenda],[Identificador] FROM [dbo].[OutboundPacket] where NumFatura = @numDoc";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@numDoc", SqlDbType.NVarChar).Value = numDocument;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        OutboundPacket outboundP = new OutboundPacket();

                        #region passar os dados para a variavel outboundP
                        outboundP.DMSID = dr.GetValue(0).ToString();
                        outboundP.NomeReceptor = dr.GetValue(1).ToString();
                        outboundP.EmailReceptor = dr.GetValue(2).ToString();
                        outboundP.NIFReceptor = dr.GetValue(3).ToString();
                        outboundP.NumFactura = dr.GetValue(4).ToString();
                        outboundP.DataFactura = dr.GetValue(5).ToString();
                        outboundP.QuantiaComIVA = dr.GetValue(6).ToString();
                        outboundP.CreationDate = DateTime.Parse(dr.GetValue(7).ToString());
                        outboundP.CurrentEBCState = dr.GetValue(8).ToString();
                        outboundP.LastUpdate = DateTime.Parse(dr.GetValue(9).ToString());
                        outboundP.DocOriginal = dr.GetValue(10).ToString();
                        outboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        outboundP.Moeda = dr.GetValue(12).ToString();
                        outboundP.QuantiaComIVAMoedaInterna = dr.GetValue(13).ToString();
                        outboundP.QuantiaSemIVAMoedaInterna = dr.GetValue(14).ToString();
                        outboundP.TaxaCambio = dr.GetValue(15).ToString();
                        outboundP.CondicaoPagamento = dr.GetValue(16).ToString();
                        outboundP.SWIFT = dr.GetValue(17).ToString();
                        outboundP.IBAN = dr.GetValue(18).ToString();
                        outboundP.TipoDocumento = dr.GetValue(19).ToString();
                        outboundP.NomeEmissor = dr.GetValue(20).ToString();
                        outboundP.NIFEmissor = dr.GetValue(21).ToString();
                        outboundP.Referencia = dr.GetValue(22).ToString();
                        outboundP.Contentor = dr.GetValue(23).ToString();
                        outboundP.CGA = dr.GetValue(24).ToString();
                        outboundP.NumEncomenda = dr.GetValue(25).ToString();
                        outboundP.Identificador = int.Parse(dr.GetValue(26).ToString());
                        outboundP.InorOut = false;
                        #endregion

                        listOutboundP.Add(outboundP);
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(listOutboundP);
    }

    private string GetAllOutboundinDB(DateTime dataI, DateTime dataF)
    {
        List<OutboundPacket> listOutboundP = new List<OutboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [DMSID],[NomeReceptor],[EmailReceptor],[NIFReceptor],[NumFactura],[DataFactura],[QuantiaComIVA],[CreationDate],[CurrentEBCState],[LastUpdate],[DocOriginal],[QuantiaSemIVA],[Moeda],[QuantiaComIVAMoedaInterna],[QuantiaSemIVAMoedaInterna],[TaxaCambio],[CondicaoPagamento],[SWIFT],[IBAN],[TipoDocumento],[NomeEmissor],[NIFEmissor],[Referencia],[Contentor],[CGA],[NumEncomenda],[Identificador] FROM [dbo].[OutboundPacket] where DataFactura >= @dateI AND DataFactura <= @dateF";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@dateI", SqlDbType.DateTime).Value = dataI;
                cmd.Parameters.Add("@dateF", SqlDbType.DateTime).Value = dataF;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        OutboundPacket outboundP = new OutboundPacket();

                        #region passar os dados para a variavel outboundP
                        outboundP.DMSID = dr.GetValue(0).ToString();
                        outboundP.NomeReceptor = dr.GetValue(1).ToString();
                        outboundP.EmailReceptor = dr.GetValue(2).ToString();
                        outboundP.NIFReceptor = dr.GetValue(3).ToString();
                        outboundP.NumFactura = dr.GetValue(4).ToString();
                        outboundP.DataFactura = dr.GetValue(5).ToString();
                        outboundP.QuantiaComIVA = dr.GetValue(6).ToString();
                        outboundP.CreationDate = DateTime.Parse(dr.GetValue(7).ToString());
                        outboundP.CurrentEBCState = dr.GetValue(8).ToString();
                        outboundP.LastUpdate = DateTime.Parse(dr.GetValue(9).ToString());
                        outboundP.DocOriginal = dr.GetValue(10).ToString();
                        outboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        outboundP.Moeda = dr.GetValue(12).ToString();
                        outboundP.QuantiaComIVAMoedaInterna = dr.GetValue(13).ToString();
                        outboundP.QuantiaSemIVAMoedaInterna = dr.GetValue(14).ToString();
                        outboundP.TaxaCambio = dr.GetValue(15).ToString();
                        outboundP.CondicaoPagamento = dr.GetValue(16).ToString();
                        outboundP.SWIFT = dr.GetValue(17).ToString();
                        outboundP.IBAN = dr.GetValue(18).ToString();
                        outboundP.TipoDocumento = dr.GetValue(19).ToString();
                        outboundP.NomeEmissor = dr.GetValue(20).ToString();
                        outboundP.NIFEmissor = dr.GetValue(21).ToString();
                        outboundP.Referencia = dr.GetValue(22).ToString();
                        outboundP.Contentor = dr.GetValue(23).ToString();
                        outboundP.CGA = dr.GetValue(24).ToString();
                        outboundP.NumEncomenda = dr.GetValue(25).ToString();
                        outboundP.Identificador = int.Parse(dr.GetValue(26).ToString());
                        outboundP.InorOut = false;
                        #endregion

                        listOutboundP.Add(outboundP);
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(listOutboundP);
    }
    private string GetAllOutboundinDB()
    {
        List<OutboundPacket> listOutboundP = new List<OutboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [DMSID],[NomeReceptor],[EmailReceptor],[NIFReceptor],[NumFactura],[DataFactura],[QuantiaComIVA],[CreationDate],[CurrentEBCState],[LastUpdate],[DocOriginal],[QuantiaSemIVA],[Moeda],[QuantiaComIVAMoedaInterna],[QuantiaSemIVAMoedaInterna],[TaxaCambio],[CondicaoPagamento],[SWIFT],[IBAN],[TipoDocumento],[NomeEmissor],[NIFEmissor],[Referencia],[Contentor],[CGA],[NumEncomenda],[Identificador] FROM [dbo].[OutboundPacket]";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        OutboundPacket outboundP = new OutboundPacket();

                        #region passar os dados para a variavel inboundP
                        outboundP.DMSID = dr.GetValue(0).ToString();
                        outboundP.NomeReceptor = dr.GetValue(1).ToString();
                        outboundP.EmailReceptor = dr.GetValue(2).ToString();
                        outboundP.NIFReceptor = dr.GetValue(3).ToString();
                        outboundP.NumFactura = dr.GetValue(4).ToString();
                        outboundP.DataFactura = dr.GetValue(5).ToString();
                        outboundP.QuantiaComIVA = dr.GetValue(6).ToString();
                        outboundP.CreationDate = DateTime.Parse(dr.GetValue(7).ToString());
                        outboundP.CurrentEBCState = dr.GetValue(8).ToString();
                        outboundP.LastUpdate = DateTime.Parse(dr.GetValue(9).ToString());
                        outboundP.DocOriginal = dr.GetValue(10).ToString();
                        outboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        outboundP.Moeda = dr.GetValue(12).ToString();
                        outboundP.QuantiaComIVAMoedaInterna = dr.GetValue(13).ToString();
                        outboundP.QuantiaSemIVAMoedaInterna = dr.GetValue(14).ToString();
                        outboundP.TaxaCambio = dr.GetValue(15).ToString();
                        outboundP.CondicaoPagamento = dr.GetValue(16).ToString();
                        outboundP.SWIFT = dr.GetValue(17).ToString();
                        outboundP.IBAN = dr.GetValue(18).ToString();
                        outboundP.TipoDocumento = dr.GetValue(19).ToString();
                        outboundP.NomeEmissor = dr.GetValue(20).ToString();
                        outboundP.NIFEmissor = dr.GetValue(21).ToString();
                        outboundP.Referencia = dr.GetValue(22).ToString();
                        outboundP.Contentor = dr.GetValue(23).ToString();
                        outboundP.CGA = dr.GetValue(24).ToString();
                        outboundP.NumEncomenda = dr.GetValue(25).ToString();
                        outboundP.Identificador = int.Parse(dr.GetValue(26).ToString());
                        outboundP.InorOut = false;
                        #endregion

                        listOutboundP.Add(outboundP);
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(listOutboundP);
    }
    #endregion

    #region inbound
    private string GetAllInboundinDB(string nifE)
    {
        List<InboundPacket> listInboundP = new List<InboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [NIF],[NumEncomenda],[NumFactura],[DataFactura],[Quantia],[ReceptionDate],[FKWhiteListID],[NIFE],[DocOriginal],[Obs],[Devolvido],[QuantiaSemIVA],[NomeFornec],[Moeda],[QuantIVAMI],[QuantSIVAMI],[TCambio],[CondicaoPag],[SWIFT],[IBAN],[TipoDoc],[NomeReceptor],[EmailCliente],[Referencia],[ImportExport],[ViagemPartida],[CGA],[Contentor],[Identificador] FROM [dbo].[InboundPacket] where NIFE = @nifE";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@nifE", SqlDbType.NVarChar).Value = nifE;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InboundPacket inboundP = new InboundPacket();

                        #region passar os dados para a variavel inboundP
                        inboundP.NIF = dr.GetValue(0).ToString();
                        inboundP.NumEncomenda = dr.GetValue(1).ToString();
                        inboundP.NumFactura = dr.GetValue(2).ToString();
                        inboundP.DataFactura = dr.GetValue(3).ToString();
                        inboundP.Quantia = dr.GetValue(4).ToString();
                        inboundP.ReceptionDate = DateTime.Parse(dr.GetValue(5).ToString());
                        inboundP.FKWhiteListID = Guid.Parse(dr.GetValue(6).ToString());
                        inboundP.NIFE = dr.GetValue(7).ToString();
                        inboundP.DocOriginal = dr.GetValue(8).ToString();
                        inboundP.Obs = dr.GetValue(9).ToString();
                        inboundP.Devolvido = bool.Parse(dr.GetValue(10).ToString());
                        inboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        inboundP.NomeFornec = dr.GetValue(12).ToString();
                        inboundP.Moeda = dr.GetValue(13).ToString();
                        inboundP.QuantIVAMI = dr.GetValue(14).ToString();
                        inboundP.QuantSIVAMI = dr.GetValue(15).ToString();
                        inboundP.TCambio = dr.GetValue(16).ToString();
                        inboundP.CondicaoPag = dr.GetValue(17).ToString();
                        inboundP.SWIFT = dr.GetValue(18).ToString();
                        inboundP.IBAN = dr.GetValue(19).ToString();
                        inboundP.TipoDoc = dr.GetValue(20).ToString();
                        inboundP.NomeReceptor = dr.GetValue(21).ToString();
                        inboundP.EmailCliente = dr.GetValue(22).ToString();
                        inboundP.Referencia = dr.GetValue(23).ToString();
                        inboundP.ImportExport = dr.GetValue(24).ToString();
                        inboundP.ViagemPartida = dr.GetValue(25).ToString();
                        inboundP.CGA = dr.GetValue(26).ToString();
                        inboundP.Contentor = dr.GetValue(27).ToString();
                        inboundP.Identificador = int.Parse(dr.GetValue(28).ToString());
                        inboundP.InorOut = true;
                        #endregion

                        listInboundP.Add(inboundP);
                    }
                }
            }
        }

        return JsonConvert.SerializeObject(listInboundP);
    }
    private string GetAllInboundinDB(string nifE, string nifR)
    {
        List<InboundPacket> listInboundP = new List<InboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [NIF],[NumEncomenda],[NumFactura],[DataFactura],[Quantia],[ReceptionDate],[FKWhiteListID],[NIFE],[DocOriginal],[Obs],[Devolvido],[QuantiaSemIVA],[NomeFornec],[Moeda],[QuantIVAMI],[QuantSIVAMI],[TCambio],[CondicaoPag],[SWIFT],[IBAN],[TipoDoc],[NomeReceptor],[EmailCliente],[Referencia],[ImportExport],[ViagemPartida],[CGA],[Contentor],[Identificador] FROM [dbo].[InboundPacket] where NIFE = @nifE AND NIF = @nifR";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@nifE", SqlDbType.NVarChar).Value = nifE;
                cmd.Parameters.Add("@nifR", SqlDbType.NVarChar).Value = nifR;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InboundPacket inboundP = new InboundPacket();

                        #region passar os dados para a variavel inboundP
                        inboundP.NIF = dr.GetValue(0).ToString();
                        inboundP.NumEncomenda = dr.GetValue(1).ToString();
                        inboundP.NumFactura = dr.GetValue(2).ToString();
                        inboundP.DataFactura = dr.GetValue(3).ToString();
                        inboundP.Quantia = dr.GetValue(4).ToString();
                        inboundP.ReceptionDate = DateTime.Parse(dr.GetValue(5).ToString());
                        inboundP.FKWhiteListID = Guid.Parse(dr.GetValue(6).ToString());
                        inboundP.NIFE = dr.GetValue(7).ToString();
                        inboundP.DocOriginal = dr.GetValue(8).ToString();
                        inboundP.Obs = dr.GetValue(9).ToString();
                        inboundP.Devolvido = bool.Parse(dr.GetValue(10).ToString());
                        inboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        inboundP.NomeFornec = dr.GetValue(12).ToString();
                        inboundP.Moeda = dr.GetValue(13).ToString();
                        inboundP.QuantIVAMI = dr.GetValue(14).ToString();
                        inboundP.QuantSIVAMI = dr.GetValue(15).ToString();
                        inboundP.TCambio = dr.GetValue(16).ToString();
                        inboundP.CondicaoPag = dr.GetValue(17).ToString();
                        inboundP.SWIFT = dr.GetValue(18).ToString();
                        inboundP.IBAN = dr.GetValue(19).ToString();
                        inboundP.TipoDoc = dr.GetValue(20).ToString();
                        inboundP.NomeReceptor = dr.GetValue(21).ToString();
                        inboundP.EmailCliente = dr.GetValue(22).ToString();
                        inboundP.Referencia = dr.GetValue(23).ToString();
                        inboundP.ImportExport = dr.GetValue(24).ToString();
                        inboundP.ViagemPartida = dr.GetValue(25).ToString();
                        inboundP.CGA = dr.GetValue(26).ToString();
                        inboundP.Contentor = dr.GetValue(27).ToString();
                        inboundP.Identificador = int.Parse(dr.GetValue(28).ToString());
                        inboundP.InorOut = true;
                        #endregion

                        listInboundP.Add(inboundP);
                    }
                }
            }
        }

        return JsonConvert.SerializeObject(listInboundP);
    }
    private string GetAllInboundinDB(string numDocument, int? estado)
    {
        List<InboundPacket> listInboundP = new List<InboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [NIF],[NumEncomenda],[NumFactura],[DataFactura],[Quantia],[ReceptionDate],[FKWhiteListID],[NIFE],[DocOriginal],[Obs],[Devolvido],[QuantiaSemIVA],[NomeFornec],[Moeda],[QuantIVAMI],[QuantSIVAMI],[TCambio],[CondicaoPag],[SWIFT],[IBAN],[TipoDoc],[NomeReceptor],[EmailCliente],[Referencia],[ImportExport],[ViagemPartida],[CGA],[Contentor],[Identificador] FROM [dbo].[InboundPacket] where NumFactura = @numDoc";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@numDoc", SqlDbType.NVarChar).Value = numDocument;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InboundPacket inboundP = new InboundPacket();

                        #region passar os dados para a variavel inboundP
                        inboundP.NIF = dr.GetValue(0).ToString();
                        inboundP.NumEncomenda = dr.GetValue(1).ToString();
                        inboundP.NumFactura = dr.GetValue(2).ToString();
                        inboundP.DataFactura = dr.GetValue(3).ToString();
                        inboundP.Quantia = dr.GetValue(4).ToString();
                        inboundP.ReceptionDate = DateTime.Parse(dr.GetValue(5).ToString());
                        inboundP.FKWhiteListID = Guid.Parse(dr.GetValue(6).ToString());
                        inboundP.NIFE = dr.GetValue(7).ToString();
                        inboundP.DocOriginal = dr.GetValue(8).ToString();
                        inboundP.Obs = dr.GetValue(9).ToString();
                        inboundP.Devolvido = bool.Parse(dr.GetValue(10).ToString());
                        inboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        inboundP.NomeFornec = dr.GetValue(12).ToString();
                        inboundP.Moeda = dr.GetValue(13).ToString();
                        inboundP.QuantIVAMI = dr.GetValue(14).ToString();
                        inboundP.QuantSIVAMI = dr.GetValue(15).ToString();
                        inboundP.TCambio = dr.GetValue(16).ToString();
                        inboundP.CondicaoPag = dr.GetValue(17).ToString();
                        inboundP.SWIFT = dr.GetValue(18).ToString();
                        inboundP.IBAN = dr.GetValue(19).ToString();
                        inboundP.TipoDoc = dr.GetValue(20).ToString();
                        inboundP.NomeReceptor = dr.GetValue(21).ToString();
                        inboundP.EmailCliente = dr.GetValue(22).ToString();
                        inboundP.Referencia = dr.GetValue(23).ToString();
                        inboundP.ImportExport = dr.GetValue(24).ToString();
                        inboundP.ViagemPartida = dr.GetValue(25).ToString();
                        inboundP.CGA = dr.GetValue(26).ToString();
                        inboundP.Contentor = dr.GetValue(27).ToString();
                        inboundP.Identificador = int.Parse(dr.GetValue(28).ToString());
                        inboundP.InorOut = true;
                        #endregion

                        listInboundP.Add(inboundP);
                    }
                }
            }
        }

        return JsonConvert.SerializeObject(listInboundP);
    }
    private string GetAllInboundinDB(DateTime dataI, DateTime dataF)
    {
        List<InboundPacket> listInboundP = new List<InboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [NIF],[NumEncomenda],[NumFactura],[DataFactura],[Quantia],[ReceptionDate],[FKWhiteListID],[NIFE],[DocOriginal],[Obs],[Devolvido],[QuantiaSemIVA],[NomeFornec],[Moeda],[QuantIVAMI],[QuantSIVAMI],[TCambio],[CondicaoPag],[SWIFT],[IBAN],[TipoDoc],[NomeReceptor],[EmailCliente],[Referencia],[ImportExport],[ViagemPartida],[CGA],[Contentor],[Identificador] FROM [dbo].[InboundPacket] where DataFactura >= @dataI AND DataFactura <= @dataF";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@dateI", SqlDbType.DateTime).Value = dataI;
                cmd.Parameters.Add("@dateF", SqlDbType.DateTime).Value = dataF;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InboundPacket inboundP = new InboundPacket();

                        #region passar os dados para a variavel inboundP
                        inboundP.NIF = dr.GetValue(0).ToString();
                        inboundP.NumEncomenda = dr.GetValue(1).ToString();
                        inboundP.NumFactura = dr.GetValue(2).ToString();
                        inboundP.DataFactura = dr.GetValue(3).ToString();
                        inboundP.Quantia = dr.GetValue(4).ToString();
                        inboundP.ReceptionDate = DateTime.Parse(dr.GetValue(5).ToString());
                        inboundP.FKWhiteListID = Guid.Parse(dr.GetValue(6).ToString());
                        inboundP.NIFE = dr.GetValue(7).ToString();
                        inboundP.DocOriginal = dr.GetValue(8).ToString();
                        inboundP.Obs = dr.GetValue(9).ToString();
                        inboundP.Devolvido = bool.Parse(dr.GetValue(10).ToString());
                        inboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        inboundP.NomeFornec = dr.GetValue(12).ToString();
                        inboundP.Moeda = dr.GetValue(13).ToString();
                        inboundP.QuantIVAMI = dr.GetValue(14).ToString();
                        inboundP.QuantSIVAMI = dr.GetValue(15).ToString();
                        inboundP.TCambio = dr.GetValue(16).ToString();
                        inboundP.CondicaoPag = dr.GetValue(17).ToString();
                        inboundP.SWIFT = dr.GetValue(18).ToString();
                        inboundP.IBAN = dr.GetValue(19).ToString();
                        inboundP.TipoDoc = dr.GetValue(20).ToString();
                        inboundP.NomeReceptor = dr.GetValue(21).ToString();
                        inboundP.EmailCliente = dr.GetValue(22).ToString();
                        inboundP.Referencia = dr.GetValue(23).ToString();
                        inboundP.ImportExport = dr.GetValue(24).ToString();
                        inboundP.ViagemPartida = dr.GetValue(25).ToString();
                        inboundP.CGA = dr.GetValue(26).ToString();
                        inboundP.Contentor = dr.GetValue(27).ToString();
                        inboundP.Identificador = int.Parse(dr.GetValue(28).ToString());
                        inboundP.InorOut = true;
                        #endregion

                        listInboundP.Add(inboundP);
                    }
                }
            }
        }

        return JsonConvert.SerializeObject(listInboundP);
    }

    private string GetAllInboundinDB()
    {
        List<InboundPacket> listInboundP = new List<InboundPacket>();

        string connectionString = ConfigurationManager.ConnectionStrings["CIC_DB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = "SELECT [NIF],[NumEncomenda],[NumFactura],[DataFactura],[Quantia],[ReceptionDate],[FKWhiteListID],[NIFE],[DocOriginal],[Obs],[Devolvido],[QuantiaSemIVA],[NomeFornec],[Moeda],[QuantIVAMI],[QuantSIVAMI],[TCambio],[CondicaoPag],[SWIFT],[IBAN],[TipoDoc],[NomeReceptor],[EmailCliente],[Referencia],[ImportExport],[ViagemPartida],[CGA],[Contentor],[Identificador] FROM [dbo].[InboundPacket]";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InboundPacket inboundP = new InboundPacket();

                        #region passar os dados para a variavel inboundP
                        inboundP.NIF = dr.GetValue(0).ToString();
                        inboundP.NumEncomenda = dr.GetValue(1).ToString();
                        inboundP.NumFactura = dr.GetValue(2).ToString();
                        inboundP.DataFactura = dr.GetValue(3).ToString();
                        inboundP.Quantia = dr.GetValue(4).ToString();
                        inboundP.ReceptionDate = DateTime.Parse(dr.GetValue(5).ToString());
                        inboundP.FKWhiteListID = Guid.Parse(dr.GetValue(6).ToString());
                        inboundP.NIFE = dr.GetValue(7).ToString();
                        inboundP.DocOriginal = dr.GetValue(8).ToString();
                        inboundP.Obs = dr.GetValue(9).ToString();
                        inboundP.Devolvido = bool.Parse(dr.GetValue(10).ToString());
                        inboundP.QuantiaSemIVA = dr.GetValue(11).ToString();
                        inboundP.NomeFornec = dr.GetValue(12).ToString();
                        inboundP.Moeda = dr.GetValue(13).ToString();
                        inboundP.QuantIVAMI = dr.GetValue(14).ToString();
                        inboundP.QuantSIVAMI = dr.GetValue(15).ToString();
                        inboundP.TCambio = dr.GetValue(16).ToString();
                        inboundP.CondicaoPag = dr.GetValue(17).ToString();
                        inboundP.SWIFT = dr.GetValue(18).ToString();
                        inboundP.IBAN = dr.GetValue(19).ToString();
                        inboundP.TipoDoc = dr.GetValue(20).ToString();
                        inboundP.NomeReceptor = dr.GetValue(21).ToString();
                        inboundP.EmailCliente = dr.GetValue(22).ToString();
                        inboundP.Referencia = dr.GetValue(23).ToString();
                        inboundP.ImportExport = dr.GetValue(24).ToString();
                        inboundP.ViagemPartida = dr.GetValue(25).ToString();
                        inboundP.CGA = dr.GetValue(26).ToString();
                        inboundP.Contentor = dr.GetValue(27).ToString();
                        inboundP.Identificador = int.Parse(dr.GetValue(28).ToString());
                        inboundP.InorOut = true;
                        #endregion

                        listInboundP.Add(inboundP);
                    }
                }
            }
        }

        return JsonConvert.SerializeObject(listInboundP);
    }
    #endregion
    #endregion

}
