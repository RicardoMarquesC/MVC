using eBillingSuite.Helper;
using eBillingSuite.Models.DigitalModel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.Controllers
{
    public class EBillingDigitalEXTController : Controller
    {
        private string finalProcessingLogPath = "";
        private SqlConnection conn_teste = null;
        private bool documentoNegocio;
        private string mainLogPath;
        ArrayList infoOcrXmlIvas = new ArrayList();
        ArrayList infoOcrXmlLines = new ArrayList();
        ArrayList infoOcrXmlAnexos = new ArrayList();

        // GET: EBillingDigitalEXT
        public ActionResult Index(object key1)
        {
            return Content("Teste");
            //return View();
        }

        [HttpPost]
        public ActionResult Index(JsonResult key1)
        {
            return Content("TESTE");
            //return View();
        }

        public static void LogMessageToFile(string msg, string path)
        {
            //StreamWriter sw = File.AppendText(path);
            try
            {
                StreamWriter sw = System.IO.File.AppendText(path);

                string logLine = System.String.Format("{0:G}:" + System.DateTime.Now.Millisecond.ToString() + ": {1}", System.DateTime.Now, msg);
                sw.WriteLine(logLine);

                sw.Close();
                sw.Dispose();
            }
            catch (Exception)
            {
            }
        }

        public void finalDocumentProcesses(object parametros, string nifRecetorEncontrado, string finalProcessingLogPath, string mainLogPath, bool documentoNegocio)
        {
            this.documentoNegocio = documentoNegocio;
            this.finalProcessingLogPath = finalProcessingLogPath;
            this.mainLogPath = mainLogPath;
            LogMessageToFile("******* Entrou no processamento final ********", finalProcessingLogPath);

            //obter os parametros
            object[] parameters = (object[])parametros;
            string actualDocNameTemp = (string)parameters[0];
            NomeTemplate thisXmlTemplateTemp = (NomeTemplate)parameters[1];
            InfoMetadados[] cabTemp = (InfoMetadados[])parameters[2];
            object[] linesTemp = (object[])parameters[3];
            object[] ivasTemp = (object[])parameters[4];
            string nifEncontradoTemp = (string)parameters[5];
            string tipodocTemp = (string)parameters[6];
            bool querAnexosTemp = (bool)parameters[7];
            bool anexosConcatenadosTemp = (bool)parameters[8];
            bool documentoNegocioTemp = (bool)parameters[9];
            object[] anexosTemp = (object[])parameters[10];
            ResolutionObject pdfResTemp = (ResolutionObject)parameters[11];
            string selectedTaxTemp = (string)parameters[12];

            ArrayList infoOcrXmlCabTemp = new ArrayList();
            ArrayList infoOcrXmlLinesTemp = new ArrayList();
            ArrayList infoOcrXmlIvasTemp = new ArrayList();
            ArrayList infoOcrXmlAnexosTemp = new ArrayList();

            string numDoc = "";

            //transformar os objectos recebidos em ArrayList
            //CAB: InfoMetadados[] -> ArrayList<InfoMetadados>
            for (int i = 0; i < cabTemp.Length; i++)
            {
                // obter o nº do documento
                if (cabTemp[i].DadosTemplXml.NomeCampo.Equals("Nº Documento"))
                    numDoc = cabTemp[i].ValorTB.Text;

                infoOcrXmlCabTemp.Add(cabTemp[i]);
            }
            //LINES: object[]<InfoMetadados[]> -> ArrayList<ArrayList<InfoMetadados>>
            foreach (object obj in linesTemp)
            {
                ArrayList lineTemp = new ArrayList();
                InfoMetadados[] temp = (InfoMetadados[])obj;
                foreach (InfoMetadados im in temp)
                {
                    lineTemp.Add(im);
                }
                infoOcrXmlLinesTemp.Add(lineTemp);
            }
            //IVA: object[]<InfoMetadados[]> -> ArrayList<ArrayList<InfoMetadados>>
            foreach (object obj in ivasTemp)
            {
                ArrayList ivaTemp = new ArrayList();
                InfoMetadados[] temp = (InfoMetadados[])obj;
                foreach (InfoMetadados im in temp)
                {
                    ivaTemp.Add(im);
                }
                infoOcrXmlIvasTemp.Add(ivaTemp);
            }
            //ANEXOS
            foreach (object obj in anexosTemp)
            {
                ArrayList anexoTemp = new ArrayList();
                InfoMetadados[] temp = (InfoMetadados[])obj;
                foreach (InfoMetadados im in temp)
                {
                    anexoTemp.Add(im);
                }
                infoOcrXmlAnexosTemp.Add(anexoTemp);
            }
            LogMessageToFile("Obteve os parametro necessários", finalProcessingLogPath);

            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                LogMessageToFile("BEGIN: Transaction scope", finalProcessingLogPath);

                int i = 0;
                try
                {
                    string connection = DatabaseConnection.GetConnString("Desmaterializacao");
                    conn_teste = new SqlConnection(connection);
                    if (conn_teste.State.Equals(ConnectionState.Closed))
                        conn_teste.Open();

                    //nome final do PDF e XML
                    int ano = DateTime.Now.Year;
                    //string finalFilesName = nifEncontradoTemp + ano.ToString() + docTypeCode + numDoc;
                    string finalFilesName = this.buildFinalDocName(tipodocTemp, Helpers.RemoveSpecialCharsForFilename(nifEncontradoTemp, "-"), Helpers.RemoveSpecialCharsForFilename(numDoc, "-"));

                    LogMessageToFile("A guardar informação na BD", finalProcessingLogPath);
                    //grava info na BD
                    Guid fornPkid = this.getFornPkidFromNifFinalProcessing(nifEncontradoTemp);

                    Guid savedFacturaId = this.saveDocumentProcess(finalFilesName, fornPkid);

                    this.saveDocumentProcessCab(savedFacturaId, infoOcrXmlCabTemp, nifEncontradoTemp, tipodocTemp);
                    this.saveDocumentProcessLinhas(savedFacturaId, infoOcrXmlLinesTemp, nifEncontradoTemp, tipodocTemp);
                    this.saveDocumentProcessIva(savedFacturaId, infoOcrXmlIvasTemp, nifEncontradoTemp, tipodocTemp);
                    this.saveDocumentProcessAnexo(savedFacturaId, infoOcrXmlAnexosTemp, nifEncontradoTemp, tipodocTemp);
                    LogMessageToFile("Guardou informação na BD", finalProcessingLogPath);

                    //gerar XML e PDF
                    string createdXmlPath = this.createXML(thisXmlTemplateTemp.TipoXml, actualDocNameTemp, infoOcrXmlCabTemp, infoOcrXmlLinesTemp, infoOcrXmlIvasTemp, finalFilesName, documentoNegocioTemp, infoOcrXmlAnexosTemp, tipodocTemp, selectedTaxTemp);
                    LogMessageToFile("Criou XML", finalProcessingLogPath);



                    this.createPdfAndAttachs(actualDocNameTemp, querAnexosTemp, anexosConcatenadosTemp, nifEncontradoTemp, finalFilesName, pdfResTemp);
                    LogMessageToFile("Criou PDF/A e Anexos", finalProcessingLogPath);

                    // integração PI (IntegraUBL - Pastas - ERP)
                    bool querIntegPI = getBooleanConfigValue("IntegracaoPIQuer");
                    if (querIntegPI)
                    {
                        LogMessageToFile("Quer integração PI (IntegraUBL).", finalProcessingLogPath);
                        //obter configs
                        string caminhoFS = this.getTextConfigValue("IntegracaoCaminhoFS");
                        string instancia = this.getTextConfigValue("IntegracaoInstanciaIntegraUBL");
                        string basedados = this.getTextConfigValue("IntegracaoBDIntegraUBL");
                        string user = this.getTextConfigValue("IntegracaoUserIntegraUBL");
                        string pass = this.getTextConfigValue("IntegracaoPassIntegraUBL");
                        pass = base64Decode(pass);

                        //string de conexão a IntegraUBL
                        string connStr = @"Data Source=" + instancia + ";Initial Catalog=" + basedados + ";User ID="
                            + user + ";Password=" + pass + ";MultipleActiveResultSets=True;";
                        instancia = null;
                        user = null;
                        pass = null;

                        //guardar XML na pasta PARTILHADA, onde estará associado o registo em 'IntegraUBL'
                        System.IO.File.Copy(createdXmlPath, caminhoFS + @"\" + finalFilesName + ".xml");
                        LogMessageToFile("XML copiado para: " + caminhoFS + @"\" + finalFilesName + ".xml", finalProcessingLogPath);

                        //obtem NIF da empresa receptora (cliente do Digital)
                        //string nifReceptor = this.getTextConfigValue("InstanceNif");
                        string nifReceptor = nifRecetorEncontrado;

                        //gravar linha na BD IntegraUBL
                        try
                        {
                            SqlConnection conn = new SqlConnection(connStr);
                            string q = "INSERT INTO RegistoControlo(ficheiro,nDoc,TipoOperacao,TipoDoc,DataCriacao,IdEmpresa)" +
                            "VALUES(@ficheiro,@nDoc,@TipoOperacao,@TipoDoc,@DataCriacao,(SELECT ID FROM Empresa WHERE NIF like @nif)); SELECT SCOPE_IDENTITY();";
                            if (conn.State.Equals(ConnectionState.Closed))
                                conn.Open();
                            SqlCommand cmd = new SqlCommand(q, conn);
                            cmd.Parameters.Add("@ficheiro", SqlDbType.Text).Value = finalFilesName + ".xml"; ;
                            cmd.Parameters.Add("@nDoc", SqlDbType.VarChar).Value = numDoc;
                            cmd.Parameters.Add("@TipoOperacao", SqlDbType.VarChar).Value = "Import";
                            cmd.Parameters.Add("@TipoDoc", SqlDbType.VarChar).Value = "Invoice";
                            cmd.Parameters.Add("@DataCriacao", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@nif", SqlDbType.NVarChar).Value = "%" + nifReceptor;
                            //cmd.ExecuteNonQuery();
                            decimal insertedId = (decimal)cmd.ExecuteScalar();
                            if (conn.State.Equals(ConnectionState.Open))
                                conn.Close();
                            cmd = null;
                            q = null;
                            conn = null;
                            LogMessageToFile("Registo gravado em '" + basedados + "', com ID=" + insertedId.ToString(), finalProcessingLogPath);
                        }
                        catch
                        {
                            LogMessageToFile("Erro ao gravar registo em '" + basedados, finalProcessingLogPath);
                            //se deu erro ao inserir registo na BD partilhada, apaga ficheiro XML copiado
                            if (System.IO.File.Exists(caminhoFS + @"\" + finalFilesName + ".xml"))
                                System.IO.File.Delete(caminhoFS + @"\" + finalFilesName + ".xml");
                        }
                        basedados = null;
                    }
                }
                catch (Exception ex)
                {
                    i = 1;
                    scope.Dispose();
                    LogMessageToFile("ERRO: ao gravar o processamento final (finalDocumentProcesses)", finalProcessingLogPath);
                    LogMessageToFile("MSG: " + ex.Message, finalProcessingLogPath);
                    LogMessageToFile("TRACE: " + ex.StackTrace, finalProcessingLogPath);
                }
                finally
                {
                    if (i == 0)
                    {
                        if (conn_teste.State == ConnectionState.Open)
                            conn_teste.Close();

                        scope.Complete();
                    }
                }

                LogMessageToFile("END: Transaction scope", finalProcessingLogPath);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            LogMessageToFile("******* Acabou no processamento final *******", finalProcessingLogPath);
        }

        //cria o XML em pastas/XML - retorna caminho XML final
        private string createXML(string TipoXML, string actualDocNameTemp, ArrayList infoOcrXmlCabTemp, ArrayList infoOcrXmlLinesTemp, ArrayList infoOcrXmlIvasTemp, string finalFilesName, bool documentoNegocioTemp, ArrayList infoOcrXmlAnexosTemp, string tipoDocExtenso, string selectedTaxTemp)
        {
            string createdXmlPath = String.Empty;
            string finalDocPath = String.Empty;

            int ano = DateTime.Now.Year;

            try
            {
                #region Regedit | comentado por Tiago Borges a 30/06/2016 alteração para BD
                //bool is64Bit = System.Environment.Is64BitOperatingSystem;
                //Microsoft.Win32.RegistryKey regKey2 = null;
                //if (!is64Bit)
                //    regKey2 = Microsoft.Win32.Registry.LocalMachine;
                //else
                //    regKey2 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);

                //Microsoft.Win32.RegistryKey ebcHive2 = regKey2.OpenSubKey(@"SOFTWARE\PI Portugal Informatico\FilePaths");
                //createdXmlPath = ebcHive2.GetValue("XML").ToString();
                #endregion
                createdXmlPath = Helpers.GetConfigFromDataBase("XML").ToString();

                string finalXmlPath = createdXmlPath;

                finalDocPath = finalXmlPath + finalFilesName + ".xml";

                if (TipoXML == "UBL2.0")
                {
                    string pdfNameField = finalFilesName + ".pdf";

                    //Buscar localizacao campos do template
                    //TRATAMENTO CABECALHO
                    string pathcampoxml = "";
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    System.Xml.XmlNode rootNode = xmlDoc.CreateElement("Invoice");
                    System.Xml.XmlNode childNode = null;
                    System.Xml.XmlNode childNode_2 = null;
                    System.Xml.XmlNode childNode_3 = null;
                    System.Xml.XmlNode childNode_4 = null;
                    System.Xml.XmlNode childNode_5 = null;
                    System.Xml.XmlNode childNode_6 = null;
                    System.Xml.XmlText text = null;


                    /*****************************************************************************************************/
                    /**************************************** CABEÇALHO *************************************************/
                    /*****************************************************************************************************/

                    ////////////Modified by Tiago Esteves version2.0//////////////
                    LogMessageToFile("Iniciou o Cabeçalho", finalProcessingLogPath);
                    System.Xml.XmlNode FatherNode_17 = xmlDoc.CreateElement("UBLVersionID");
                    text = xmlDoc.CreateTextNode("2.0");
                    FatherNode_17.AppendChild(text);
                    rootNode.AppendChild(FatherNode_17);


                    System.Xml.XmlNode FatherNode_18 = xmlDoc.CreateElement("ID");
                    pathcampoxml = "/Invoice/UUID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_18.AppendChild(text);
                    rootNode.AppendChild(FatherNode_18);

                    System.Xml.XmlNode FatherNode_19 = xmlDoc.CreateElement("CopyIndicator");
                    rootNode.AppendChild(FatherNode_19);
                    FatherNode_19.AppendChild(xmlDoc.CreateTextNode("false"));

                    System.Xml.XmlNode FatherNode_3 = xmlDoc.CreateElement("UUID");
                    pathcampoxml = "/Invoice/UUID";
                    rootNode.AppendChild(FatherNode_3);
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_3.AppendChild(text);

                    // indicar se é factura negócio ou não negócio Porto Editora
                    System.Xml.XmlNode FatherNode_3_1 = xmlDoc.CreateElement("BusinessDocumentIndicator");
                    rootNode.AppendChild(FatherNode_3_1);
                    text = xmlDoc.CreateTextNode(documentoNegocio.ToString());
                    FatherNode_3_1.AppendChild(text);

                    string logedUser = "";
                    try
                    {
                        System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
                        logedUser = wi.Name;
                    }
                    catch (Exception e)
                    {
                        LogMessageToFile("ERRO: obter o user desta máquina (createXML)", finalProcessingLogPath);
                    }
                    System.Xml.XmlNode FatherNode_3_2 = xmlDoc.CreateElement("Gestor");
                    rootNode.AppendChild(FatherNode_3_2);
                    text = xmlDoc.CreateTextNode(logedUser);
                    FatherNode_3_2.AppendChild(text);

                    System.Xml.XmlNode FatherNode_4 = xmlDoc.CreateElement("IssueDate");
                    pathcampoxml = "/Invoice/IssueDate";
                    rootNode.AppendChild(FatherNode_4);
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_4.AppendChild(text);


                    System.Xml.XmlNode FatherNode_4_1 = xmlDoc.CreateElement("InitialBillingPeriod");
                    pathcampoxml = "/Invoice/InitialBillingPeriod";
                    rootNode.AppendChild(FatherNode_4_1);
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_4_1.AppendChild(text);

                    System.Xml.XmlNode FatherNode_4_2 = xmlDoc.CreateElement("FinalBillingPeriod");
                    pathcampoxml = "/Invoice/FinalBillingPeriod";
                    rootNode.AppendChild(FatherNode_4_2);
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_4_2.AppendChild(text);


                    System.Xml.XmlNode FatherNode_20 = xmlDoc.CreateElement("InvoiceTypeCode");
                    rootNode.AppendChild(FatherNode_20);
                    pathcampoxml = "/Invoice/InvoiceTypeCode";
                    // obter da BD a descrição a colocar no XML
                    string tipoDocDescriptionToXml = GetFinalXmlDescriptionFromTipoDocNome(tipoDocExtenso);
                    text = xmlDoc.CreateTextNode(tipoDocDescriptionToXml);
                    FatherNode_20.AppendChild(text);


                    System.Xml.XmlNode FatherNode_21 = xmlDoc.CreateElement("Note");
                    rootNode.AppendChild(FatherNode_21);
                    FatherNode_21.AppendChild(xmlDoc.CreateTextNode("1"));

                    System.Xml.XmlNode FatherNode_22 = xmlDoc.CreateElement("TaxPointDate");
                    rootNode.AppendChild(FatherNode_22);
                    pathcampoxml = "/Invoice/TaxPointDate";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_22.AppendChild(text);

                    System.Xml.XmlNode FatherNode_8 = xmlDoc.CreateElement("EuroValue");
                    rootNode.AppendChild(FatherNode_8);
                    pathcampoxml = "/Invoice/EUROVALUE";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_8.AppendChild(text);


                    System.Xml.XmlNode FatherNode_9 = xmlDoc.CreateElement("EuroValueWithVat");
                    rootNode.AppendChild(FatherNode_9);
                    pathcampoxml = "/Invoice/EUROVALUEWITHVAT";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_9.AppendChild(text);

                    System.Xml.XmlNode FatherNode_10 = xmlDoc.CreateElement("ExchangeRate");
                    rootNode.AppendChild(FatherNode_10);
                    pathcampoxml = "/Invoice/EXCHANGERATE";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_10.AppendChild(text);

                    System.Xml.XmlNode FatherNode_11 = xmlDoc.CreateElement("PDFName");
                    rootNode.AppendChild(FatherNode_11);
                    pathcampoxml = "/Invoice/PDFNAME";
                    text = xmlDoc.CreateTextNode(pdfNameField);
                    FatherNode_11.AppendChild(text);

                    System.Xml.XmlNode FatherNode_12 = xmlDoc.CreateElement("OriginalDoc");
                    rootNode.AppendChild(FatherNode_12);
                    pathcampoxml = "/Invoice/ORIGINALDOC";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_12.AppendChild(text);

                    // 05-06-2015 - Tipo de entrada do documento na empresa, aqui vai o codigo da tabela "TiposEntrada"
                    System.Xml.XmlNode FatherNode_13 = xmlDoc.CreateElement("EntryCode");
                    rootNode.AppendChild(FatherNode_13);
                    pathcampoxml = "/Invoice/EntryCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_13.AppendChild(text);

                    // 08-06-2015 - Assunto do documento
                    System.Xml.XmlNode FatherNode_14_a = xmlDoc.CreateElement("DocumentSubject");
                    rootNode.AppendChild(FatherNode_14_a);
                    pathcampoxml = "/Invoice/DocumentSubject";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_14_a.AppendChild(text);

                    // 08-06-2015 - Imprint
                    System.Xml.XmlNode FatherNode_15_a = xmlDoc.CreateElement("Imprint");
                    rootNode.AppendChild(FatherNode_15_a);
                    pathcampoxml = "/Invoice/Imprint";
                    //string imprinter = GetImprinterFromTiffName(actualDocNameTemp.Replace(".tif", "")); // get imprinter from DB
                    string imprinter = GetImprinterFromTiffName(actualDocNameTemp.Remove(actualDocNameTemp.LastIndexOf(".tif"), ".tif".Length)); // get imprinter from DB
                    if (!String.IsNullOrWhiteSpace(imprinter))
                        SetImprinterAsProcessed(imprinter);
                    text = xmlDoc.CreateTextNode(imprinter);
                    FatherNode_15_a.AppendChild(text);

                    // 08-06-2015 - classe do documento
                    System.Xml.XmlNode FatherNode_15_b = xmlDoc.CreateElement("ClassType");
                    rootNode.AppendChild(FatherNode_15_b);
                    pathcampoxml = "/Invoice/ClassType";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_15_b.AppendChild(text);

                    // 08-06-2015 - Lote do documento
                    System.Xml.XmlNode FatherNode_15_c = xmlDoc.CreateElement("BatchName");
                    rootNode.AppendChild(FatherNode_15_c);
                    pathcampoxml = "/Invoice/BatchName";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_15_c.AppendChild(text);

                    // notas/observações
                    System.Xml.XmlNode FatherNode_30 = xmlDoc.CreateElement("Notes");
                    pathcampoxml = "/Invoice/Notes";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    FatherNode_30.AppendChild(text);
                    rootNode.AppendChild(FatherNode_30);


                    System.Xml.XmlNode FatherNode_23 = xmlDoc.CreateElement("OrderReference");
                    rootNode.AppendChild(FatherNode_23);
                    childNode = xmlDoc.CreateElement("ID");
                    pathcampoxml = "/Invoice/OrderReference/ID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_23.AppendChild(childNode);
                    childNode_2 = xmlDoc.CreateElement("SalesOrderID");
                    FatherNode_23.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("UUID");
                    FatherNode_23.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("IssueDate");
                    FatherNode_23.AppendChild(childNode_3);

                    System.Xml.XmlNode FatherNode = xmlDoc.CreateElement("AccountingSupplierParty");
                    rootNode.AppendChild(FatherNode);
                    childNode = xmlDoc.CreateElement("SupplierAssignedAccountID");
                    FatherNode.AppendChild(childNode);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/SupplierAssignedAccountID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);


                    childNode = xmlDoc.CreateElement("Party");
                    FatherNode.AppendChild(childNode);
                    childNode_2 = xmlDoc.CreateElement("PartyName");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("Name");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PartyName/Name";
                    //text = xmlDoc.CreateTextNode(ReturnValueCampoXML(pathcampoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("PostalAddress");
                    childNode.AppendChild(childNode_2);

                    childNode_3 = xmlDoc.CreateElement("StreetName");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PostalAddress/StreetName";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("BuildingName");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("BuildingNumber");
                    childNode_2.AppendChild(childNode_3);

                    childNode_3 = xmlDoc.CreateElement("CityName");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PostalAddress/CityName";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("PostalZone");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PostalAddress/PostalZone";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("CountrySubentity");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("AddressLine");
                    childNode_2.AppendChild(childNode_3);
                    childNode_4 = xmlDoc.CreateElement("Line");
                    childNode_3.AppendChild(childNode_4);

                    childNode_3 = xmlDoc.CreateElement("Country");
                    childNode_2.AppendChild(childNode_3);
                    childNode_4 = xmlDoc.CreateElement("IdentificationCode");
                    childNode_3.AppendChild(childNode_4);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PostalAddress/Country/IdentificationCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("PartyTaxScheme");
                    childNode_3 = xmlDoc.CreateElement("CompanyID");
                    childNode_2.AppendChild(childNode_3);
                    childNode.AppendChild(childNode_2);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PartyTaxScheme/CompanyID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("RegistrationName");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("ExemptionReason");
                    childNode_2.AppendChild(childNode_3);

                    childNode_3 = xmlDoc.CreateElement("TaxScheme");
                    childNode_2.AppendChild(childNode_3);
                    childNode_4 = xmlDoc.CreateElement("ID");
                    childNode_3.AppendChild(childNode_4);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PartyTaxScheme/TaxScheme/ID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_4.AppendChild(text);

                    childNode_4 = xmlDoc.CreateElement("TaxTypeCode");
                    childNode_3.AppendChild(childNode_4);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/PartyTaxScheme/TaxScheme/TaxTypeCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_4.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("Contact");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("Name");
                    childNode_2.AppendChild(childNode_3);

                    childNode_3 = xmlDoc.CreateElement("Telephone");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/Contact/Telephone";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("Telefax");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/Contact/Telefax";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("ElectronicMail");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingSupplierParty/Party/Contact/ElectronicMail";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    System.Xml.XmlNode FatherNode_2 = xmlDoc.CreateElement("AccountingCustomerParty");
                    rootNode.AppendChild(FatherNode_2);
                    childNode = xmlDoc.CreateElement("CustomerAssignedAccountID");
                    FatherNode_2.AppendChild(childNode);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/CustomerAssignedAccountID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);

                    childNode = xmlDoc.CreateElement("Party");
                    FatherNode_2.AppendChild(childNode);
                    childNode_2 = xmlDoc.CreateElement("PartyName");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("Name");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PartyName/Name";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("PostalAddress");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("StreetName");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PostalAddress/StreetName";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("BuildingName");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("BuildingNumber");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("CityName");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PostalAddress/CityName";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);


                    childNode_3 = xmlDoc.CreateElement("PostalZone");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PostalAddress/PostalZone";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("CountrySubentity");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("AddressLine");
                    childNode_2.AppendChild(childNode_3);
                    childNode_4 = xmlDoc.CreateElement("Line");
                    childNode_3.AppendChild(childNode_4);

                    childNode_3 = xmlDoc.CreateElement("Country");
                    childNode_2.AppendChild(childNode_3);
                    childNode_4 = xmlDoc.CreateElement("IdentificationCode");
                    childNode_3.AppendChild(childNode_4);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PostalAddress/Country/IdentificationCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);


                    childNode_2 = xmlDoc.CreateElement("PartyTaxScheme");
                    childNode_3 = xmlDoc.CreateElement("CompanyID");
                    childNode_2.AppendChild(childNode_3);
                    childNode.AppendChild(childNode_2);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PartyTaxScheme/CompanyID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("RegistrationName");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("ExemptionReason");
                    childNode_2.AppendChild(childNode_3);

                    childNode_3 = xmlDoc.CreateElement("TaxScheme");
                    childNode_2.AppendChild(childNode_3);
                    childNode_4 = xmlDoc.CreateElement("ID");
                    childNode_3.AppendChild(childNode_4);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PartyTaxScheme/TaxScheme/ID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_4.AppendChild(text);

                    childNode_4 = xmlDoc.CreateElement("TaxTypeCode");
                    childNode_3.AppendChild(childNode_4);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/PartyTaxScheme/TaxScheme/TaxTypeCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_4.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("Contact");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("Name");
                    childNode_2.AppendChild(childNode_3);
                    childNode_3 = xmlDoc.CreateElement("Telephone");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/Contact/Telephone";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("Telefax");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/Contact/Telefax";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    childNode_3 = xmlDoc.CreateElement("ElectronicMail");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/AccountingCustomerParty/Party/Contact/ElectronicMail";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    /***************************** Delivery ******************************/
                    System.Xml.XmlNode FatherNode_24 = xmlDoc.CreateElement("Delivery");
                    rootNode.AppendChild(FatherNode_24);
                    childNode = xmlDoc.CreateElement("ActualDeliveryDate");
                    FatherNode_24.AppendChild(childNode);
                    pathcampoxml = "/Invoice/Delivery/ActualDeliveryDate";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);

                    childNode = xmlDoc.CreateElement("ActualDeliveryTime");
                    FatherNode_24.AppendChild(childNode);
                    pathcampoxml = "/Invoice/Delivery/ActualDeliveryTime";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);

                    childNode = xmlDoc.CreateElement("DeliveryAddress");
                    FatherNode_24.AppendChild(childNode);
                    childNode_2 = xmlDoc.CreateElement("StreetName");
                    childNode.AppendChild(childNode_2);
                    pathcampoxml = "/Invoice/Delivery/DeliveryAddress/StreetName";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_2.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("BuildingName");
                    childNode.AppendChild(childNode_2);
                    childNode_2 = xmlDoc.CreateElement("BuildingNumber");
                    childNode.AppendChild(childNode_2);
                    childNode_2 = xmlDoc.CreateElement("CityName");
                    childNode.AppendChild(childNode_2);
                    pathcampoxml = "/Invoice/Delivery/DeliveryAddress/CityName";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_2.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("PostalZone");
                    childNode.AppendChild(childNode_2);
                    pathcampoxml = "/Invoice/Delivery/DeliveryAddress/PostalZone";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_2.AppendChild(text);

                    childNode_2 = xmlDoc.CreateElement("CountrySubentity");
                    childNode.AppendChild(childNode_2);
                    childNode_2 = xmlDoc.CreateElement("AddressLine");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("Line");
                    childNode_2.AppendChild(childNode_3);
                    childNode_2 = xmlDoc.CreateElement("Country");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("IdentificationCode");
                    childNode_2.AppendChild(childNode_3);
                    pathcampoxml = "/Invoice/Delivery/DeliveryAddress/Country/IdentificationCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);

                    /////////////////PaymentMeans/////////
                    System.Xml.XmlNode FatherNode_5 = xmlDoc.CreateElement("PaymentMeans");
                    childNode = xmlDoc.CreateElement("PaymentMeansCode");
                    pathcampoxml = "/Invoice/PaymentMeans/PaymentMeansCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_5.AppendChild(childNode);
                    rootNode.AppendChild(FatherNode_5);

                    childNode = xmlDoc.CreateElement("PaymentDueDate");
                    pathcampoxml = "/Invoice/PaymentMeans/PaymentDueDate";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_5.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("PayeeFinancialAccount");
                    FatherNode_5.AppendChild(childNode);
                    childNode_2 = xmlDoc.CreateElement("ID");
                    pathcampoxml = "/Invoice/PaymentMeans/PayeeFinancialAccount/ID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_2.AppendChild(text);


                    childNode.AppendChild(childNode_2);
                    childNode_2 = xmlDoc.CreateElement("Name");
                    childNode.AppendChild(childNode_2);
                    childNode_2 = xmlDoc.CreateElement("CurrencyCode");
                    pathcampoxml = "/Invoice/PaymentMeans/PayeeFinancialAccount/CurrencyCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_2.AppendChild(text);
                    childNode.AppendChild(childNode_2);

                    childNode_2 = xmlDoc.CreateElement("FinancialInstitutionBranch");
                    childNode_3 = xmlDoc.CreateElement("ID");
                    childNode_2.AppendChild(childNode_3);
                    childNode.AppendChild(childNode_2);

                    childNode_3 = xmlDoc.CreateElement("Name");
                    pathcampoxml = "/Invoice/PaymentMeans/PayeeFinancialAccount/FinancialInstitutionBranch/Name";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_3.AppendChild(text);
                    childNode_2.AppendChild(childNode_3);

                    childNode_3 = xmlDoc.CreateElement("FinancialInstitution");
                    childNode_4 = xmlDoc.CreateElement("ID");
                    pathcampoxml = "/Invoice/PaymentMeans/PayeeFinancialAccount/FinancialInstitution/ID";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_4.AppendChild(text);
                    childNode_3.AppendChild(childNode_4);
                    childNode_2.AppendChild(childNode_3);

                    childNode_4 = xmlDoc.CreateElement("Name");
                    pathcampoxml = "/Invoice/PaymentMeans/PayeeFinancialAccount/FinancialInstitution/Name";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode_4.AppendChild(text);
                    childNode_3.AppendChild(childNode_4);


                    childNode_4 = xmlDoc.CreateElement("Address");
                    childNode_3.AppendChild(childNode_4);
                    childNode_5 = xmlDoc.CreateElement("StreetName");
                    childNode_4.AppendChild(childNode_5);
                    childNode_5 = xmlDoc.CreateElement("BuildingName");
                    childNode_4.AppendChild(childNode_5);
                    childNode_5 = xmlDoc.CreateElement("BuildingNumber");
                    childNode_4.AppendChild(childNode_5);
                    childNode_5 = xmlDoc.CreateElement("CityName");
                    childNode_4.AppendChild(childNode_5);
                    childNode_5 = xmlDoc.CreateElement("PostalZone");
                    childNode_4.AppendChild(childNode_5);
                    childNode_5 = xmlDoc.CreateElement("CountrySubentity");
                    childNode_4.AppendChild(childNode_5);
                    childNode_5 = xmlDoc.CreateElement("AddressLine");
                    childNode_4.AppendChild(childNode_5);
                    childNode_6 = xmlDoc.CreateElement("Line");
                    childNode_5.AppendChild(childNode_6);
                    childNode_5 = xmlDoc.CreateElement("Country");
                    childNode_4.AppendChild(childNode_5);
                    childNode_6 = xmlDoc.CreateElement("IdentificationCode");
                    childNode_5.AppendChild(childNode_6);
                    childNode_3 = xmlDoc.CreateElement("Address");
                    childNode_2.AppendChild(childNode_3);
                    childNode_4 = xmlDoc.CreateElement("StreetName");
                    childNode_3.AppendChild(childNode_4);
                    childNode_4 = xmlDoc.CreateElement("BuildingName");
                    childNode_3.AppendChild(childNode_4);
                    childNode_4 = xmlDoc.CreateElement("BuildingNumber");
                    childNode_3.AppendChild(childNode_4);
                    childNode_4 = xmlDoc.CreateElement("CityName");
                    childNode_3.AppendChild(childNode_4);
                    childNode_4 = xmlDoc.CreateElement("PostalZone");
                    childNode_3.AppendChild(childNode_4);
                    childNode_4 = xmlDoc.CreateElement("CountrySubentity");
                    childNode_3.AppendChild(childNode_4);
                    childNode_4 = xmlDoc.CreateElement("AddressLine");
                    childNode_3.AppendChild(childNode_4);
                    childNode_5 = xmlDoc.CreateElement("Line");
                    childNode_4.AppendChild(childNode_5);
                    childNode_4 = xmlDoc.CreateElement("Country");
                    childNode_3.AppendChild(childNode_4);
                    childNode_5 = xmlDoc.CreateElement("IdentificationCode");
                    childNode_4.AppendChild(childNode_5);
                    childNode_2 = xmlDoc.CreateElement("Country");
                    childNode.AppendChild(childNode_2);
                    childNode_3 = xmlDoc.CreateElement("IdentificationCode");
                    childNode_2.AppendChild(childNode_3);

                    /////////PaymentTerms///////
                    System.Xml.XmlNode FatherNode_25 = xmlDoc.CreateElement("PaymentTerms");
                    rootNode.AppendChild(FatherNode_25);
                    childNode = xmlDoc.CreateElement("Note");
                    pathcampoxml = "/Invoice/PaymentTerms/Note";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_25.AppendChild(childNode);


                    #region AllowanceCharge
                    System.Xml.XmlNode FatherNode_26 = xmlDoc.CreateElement("AllowanceCharge");
                    childNode = xmlDoc.CreateElement("ChargeIndicator");
                    pathcampoxml = "/Invoice/AllowanceCharge/ChargeIndicator";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_26.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("AllowanceChargeReasonCode");
                    pathcampoxml = "/Invoice/AllowanceCharge/AllowanceChargeReasonCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_26.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("MultiplierFactorNumeric");
                    pathcampoxml = "/Invoice/AllowanceCharge/MultiplierFactorNumeric";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_26.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("Amount");
                    pathcampoxml = "/Invoice/AllowanceCharge/Amount";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_26.AppendChild(childNode);

                    rootNode.AppendChild(FatherNode_26);
                    #endregion

                    #region LegalTotal
                    System.Xml.XmlNode FatherNode_6 = xmlDoc.CreateElement("LegalTotal");
                    rootNode.AppendChild(FatherNode_6);
                    childNode = xmlDoc.CreateElement("LineExtensionAmount");
                    pathcampoxml = "/Invoice/LegalTotal/LineExtensionAmount";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    float qSiva = -1;
                    if (!text.Value.Equals(""))
                    {
                        if (text.Value.Contains('.'))
                            qSiva = float.Parse(text.Value.Replace(".", ","));
                        else if (text.Value.Contains(','))
                            qSiva = float.Parse(text.Value);
                    }
                    string lineextensionamount = text.Value;
                    childNode.AppendChild(text);
                    FatherNode_6.AppendChild(childNode);


                    childNode = xmlDoc.CreateElement("PayableAmount");
                    pathcampoxml = "/Invoice/LegalTotal/PayableAmount";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    float qCiva = -1;
                    if (!text.Value.Equals(""))
                    {
                        if (text.Value.Contains('.'))
                            qCiva = float.Parse(text.Value.Replace(".", ","));
                        else if (text.Value.Contains(','))
                            qCiva = float.Parse(text.Value);
                    }
                    childNode.AppendChild(text);
                    FatherNode_6.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("TaxExclusiveAmount");
                    pathcampoxml = "/Invoice/LegalTotal/TaxExclusiveAmount";
                    text = xmlDoc.CreateTextNode(lineextensionamount);
                    childNode.AppendChild(text);
                    FatherNode_6.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("GrossAmount");
                    pathcampoxml = "/Invoice/LegalTotal/GrossAmount";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_6.AppendChild(childNode);
                    #endregion LegalTotal

                    #region SingularEntityTax
                    System.Xml.XmlNode FatherNode_7 = xmlDoc.CreateElement("SingularEntityTax");

                    childNode = xmlDoc.CreateElement("Amount");
                    pathcampoxml = "/Invoice/SingularEntityTax/Amount";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_7.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("Percent");
                    pathcampoxml = "/Invoice/SingularEntityTax/Percent";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_7.AppendChild(childNode);

                    rootNode.AppendChild(FatherNode_7);
                    #endregion

                    #region CustomerSystemSpecificData
                    System.Xml.XmlNode FatherNode_29 = xmlDoc.CreateElement("CustomerSystemSpecificData");

                    childNode = xmlDoc.CreateElement("DocumentAccountingTransactionType");
                    pathcampoxml = "/Invoice/CustomerSystemSpecificData/DocumentAccountingTransactionType";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_29.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("DocumentSerie");
                    pathcampoxml = "/Invoice/CustomerSystemSpecificData/DocumentSerie";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_29.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("DocumentAccountingClassification");
                    pathcampoxml = "/Invoice/CustomerSystemSpecificData/DocumentAccountingClassification";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_29.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("DocumentUnblockingCode");
                    pathcampoxml = "/Invoice/CustomerSystemSpecificData/DocumentUnblockingCode";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_29.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("DocumentAccountingLot");
                    pathcampoxml = "/Invoice/CustomerSystemSpecificData/DocumentAccountingLot";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_29.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("DocumentAccountingNumber");
                    pathcampoxml = "/Invoice/CustomerSystemSpecificData/DocumentAccountingNumber";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_29.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("DocumentInfo");
                    pathcampoxml = "/Invoice/CustomerSystemSpecificData/DocumentInfo";
                    text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                    childNode.AppendChild(text);
                    FatherNode_29.AppendChild(childNode);

                    rootNode.AppendChild(FatherNode_29);
                    #endregion

                    //////////////// BEGIN: SingularEntityTax ///////////////

                    // 27-01-2015 - alterado para decimal
                    decimal totalIva = -1;
                    if (qSiva != -1 && qCiva != -1)
                        totalIva = Math.Round((decimal)(qCiva - qSiva), 2);

                    LogMessageToFile("Acabou o Cabeçalho", finalProcessingLogPath);

                    /*****************************************************************************************************/
                    /**************************************** LINE ITEMS *************************************************/
                    /*****************************************************************************************************/
                    LogMessageToFile("Começou linhas", finalProcessingLogPath);

                    int numeroLinhas = infoOcrXmlLinesTemp.Count;

                    string taxSchemeTypeCode = "VAT";
                    string taxSchemeID = "PT VAT";

                    System.Xml.XmlNode FatherNode_14 = xmlDoc.CreateElement("InvoiceLineList");
                    rootNode.AppendChild(FatherNode_14);
                    for (int i = 0; i < numeroLinhas; i++)
                    {
                        childNode = xmlDoc.CreateElement("InvoiceLine");
                        childNode_2 = xmlDoc.CreateElement("ID");
                        childNode.AppendChild(childNode_2);
                        //alterado 31-05-2012
                        //pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/ID";
                        //text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        text = xmlDoc.CreateTextNode((i + 1).ToString());
                        childNode_2.AppendChild(text);

                        childNode_2 = xmlDoc.CreateElement("InvoicedQuantity");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/InvoicedQuantity";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        string qtd = text.Value;
                        childNode_2.AppendChild(text);

                        childNode_2 = xmlDoc.CreateElement("LineExtensionAmount");
                        childNode.AppendChild(childNode_2);

                        //valor com iva da linha
                        childNode_2 = xmlDoc.CreateElement("TotalLineValueWithVat");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TOTALLINEVALUEWITHVAT";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_2.AppendChild(text);

                        //valor sem iva da linha
                        childNode_2 = xmlDoc.CreateElement("TotalLineValue");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TOTALLINEVALUE";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        string valorLinhaSemIva = text.Value;
                        childNode_2.AppendChild(text);


                        childNode_2 = xmlDoc.CreateElement("TaxTotal");
                        childNode.AppendChild(childNode_2);

                        childNode_3 = xmlDoc.CreateElement("TaxAmount");
                        childNode_2.AppendChild(childNode_3);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TaxTotal/TaxAmount";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        string lineTaxAmount = text.Value;

                        childNode_3.AppendChild(text);

                        childNode_3 = xmlDoc.CreateElement("TaxEvidenceIndicator");
                        childNode_2.AppendChild(childNode_3);

                        childNode_3 = xmlDoc.CreateElement("TaxSubTotal");
                        childNode_2.AppendChild(childNode_3);
                        childNode_4 = xmlDoc.CreateElement("TaxableAmount");
                        childNode_4.AppendChild(xmlDoc.CreateTextNode(valorLinhaSemIva));
                        childNode_3.AppendChild(childNode_4);
                        childNode_4 = xmlDoc.CreateElement("TaxAmount");
                        childNode_4.AppendChild(xmlDoc.CreateTextNode(lineTaxAmount));
                        childNode_3.AppendChild(childNode_4);

                        childNode_4 = xmlDoc.CreateElement("TaxCategory");
                        childNode_3.AppendChild(childNode_4);
                        childNode_5 = xmlDoc.CreateElement("ID");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TaxTotal/TaxSubTotal/TaxCategory/ID";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_5.AppendChild(text);
                        childNode_4.AppendChild(childNode_5);

                        childNode_5 = xmlDoc.CreateElement("Percent");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TaxTotal/TaxSubTotal/TaxCategory/Percent";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_5.AppendChild(text);
                        childNode_4.AppendChild(childNode_5);

                        childNode_5 = xmlDoc.CreateElement("TaxScheme");
                        childNode_4.AppendChild(childNode_5);
                        childNode_6 = xmlDoc.CreateElement("ID");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TaxTotal/TaxSubTotal/TaxCategory/TaxScheme/ID";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        taxSchemeID = text.Value;
                        childNode_6.AppendChild(text);
                        childNode_5.AppendChild(childNode_6);
                        childNode_6 = xmlDoc.CreateElement("TaxTypeCode");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TaxTotal/TaxSubTotal/TaxCategory/TaxScheme/TaxTypeCode";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        taxSchemeTypeCode = text.Value;
                        childNode_6.AppendChild(text);
                        childNode_5.AppendChild(childNode_6);

                        childNode_2 = xmlDoc.CreateElement("OrderLineItem");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/ORDERLINEITEM";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_2.AppendChild(text);

                        /******************************** acrescentado 13-06-2012 **********************************/
                        int discountsNumber = 0;
                        foreach (InfoMetadados im in (ArrayList)infoOcrXmlLinesTemp[i])
                        {
                            if (im.DadosTemplXml.NomeCampo.ToLower().Contains("desconto linha"))
                                discountsNumber++;
                        }

                        for (int ii = 1; ii <= discountsNumber; ii++)
                        {
                            childNode_2 = xmlDoc.CreateElement("Discount" + ii.ToString());
                            childNode.AppendChild(childNode_2);

                            childNode_3 = xmlDoc.CreateElement("LineDiscounting");
                            childNode_2.AppendChild(childNode_3);
                            pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Discount" + ii.ToString() + "/LINEDISCOUNTING";
                            text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                            childNode_3.AppendChild(text);

                            childNode_3 = xmlDoc.CreateElement("LineDiscountingValue");
                            childNode_2.AppendChild(childNode_3);
                            pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Discount" + ii.ToString() + "/LineDiscountingValue";
                            text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                            childNode_3.AppendChild(text);

                            childNode_3 = xmlDoc.CreateElement("BaseToNextDiscount");
                            childNode_2.AppendChild(childNode_3);
                            pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Discount" + ii.ToString() + "/BaseToNextDiscount";
                            text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                            childNode_3.AppendChild(text);

                        }
                        /*******************************************************************************************/

                        childNode_2 = xmlDoc.CreateElement("TotalDiscounting");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/TOTALDISCOUNTING";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_2.AppendChild(text);

                        childNode_2 = xmlDoc.CreateElement("OrderLineReference");
                        childNode.AppendChild(childNode_2);

                        childNode_3 = xmlDoc.CreateElement("LineID");
                        childNode_2.AppendChild(childNode_3);
                        childNode_3 = xmlDoc.CreateElement("SalesOrderLineID");
                        childNode_2.AppendChild(childNode_3);
                        childNode_3 = xmlDoc.CreateElement("LineStatusCode");
                        childNode_2.AppendChild(childNode_3);
                        childNode_3 = xmlDoc.CreateElement("OrderReference");
                        childNode_2.AppendChild(childNode_3);

                        childNode_4 = xmlDoc.CreateElement("LineID");
                        childNode_3.AppendChild(childNode_4);
                        childNode_4 = xmlDoc.CreateElement("SalesOrderLineID");
                        childNode_3.AppendChild(childNode_4);
                        childNode_4 = xmlDoc.CreateElement("UUID");
                        childNode_3.AppendChild(childNode_4);
                        childNode_4 = xmlDoc.CreateElement("IssueDate");
                        childNode_3.AppendChild(childNode_4);


                        childNode_2 = xmlDoc.CreateElement("Price");
                        childNode.AppendChild(childNode_2);

                        childNode_3 = xmlDoc.CreateElement("PriceAmount");
                        childNode_2.AppendChild(childNode_3);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Price/PriceAmount";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_3.AppendChild(text);

                        /****************** acrescentado 12-06-2012 ******************/
                        childNode_3 = xmlDoc.CreateElement("PriceAmountWithVAT");
                        childNode_2.AppendChild(childNode_3);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Price/PriceAmountWithVAT";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_3.AppendChild(text);

                        childNode_3 = xmlDoc.CreateElement("PVP");
                        childNode_2.AppendChild(childNode_3);
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Price/PVP";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_3.AppendChild(text);
                        /*************************************************************/

                        childNode_3 = xmlDoc.CreateElement("BaseQuantity");
                        //childNode_3.AppendChild(xmlDoc.CreateTextNode(qtd));
                        childNode_3.AppendChild(xmlDoc.CreateTextNode("1"));
                        childNode_2.AppendChild(childNode_3);


                        childNode_2 = xmlDoc.CreateElement("Item");
                        childNode.AppendChild(childNode_2);

                        childNode_3 = xmlDoc.CreateElement("Description");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Item/Description";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_3.AppendChild(text);
                        childNode_2.AppendChild(childNode_3);

                        childNode_3 = xmlDoc.CreateElement("Name");
                        childNode_2.AppendChild(childNode_3);

                        childNode_3 = xmlDoc.CreateElement("BuyersItemIdentification");
                        childNode_2.AppendChild(childNode_3);
                        childNode_4 = xmlDoc.CreateElement("ID");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Item/BuyersItemIdentification/ID";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_4.AppendChild(text);
                        childNode_3.AppendChild(childNode_4);

                        childNode_3 = xmlDoc.CreateElement("SellersItemIdentification");
                        childNode_2.AppendChild(childNode_3);
                        childNode_4 = xmlDoc.CreateElement("ID");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Item/SellersItemIdentification/ID";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_4.AppendChild(text);
                        childNode_3.AppendChild(childNode_4);

                        childNode_3 = xmlDoc.CreateElement("LotIdentification");
                        childNode_2.AppendChild(childNode_3);

                        childNode_4 = xmlDoc.CreateElement("LotNumberID");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Item/LotIdentification/LotNumberID";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_4.AppendChild(text);
                        childNode_3.AppendChild(childNode_4);

                        childNode_4 = xmlDoc.CreateElement("ExpiryDate");
                        pathcampoxml = "/Invoice/InvoiceLineList/InvoiceLine/Item/LotIdentification/ExpiryDate";
                        text = xmlDoc.CreateTextNode(this.getLinhasTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlLinesTemp));
                        childNode_4.AppendChild(text);
                        childNode_3.AppendChild(childNode_4);

                        FatherNode_14.AppendChild(childNode);
                    }

                    LogMessageToFile("Acabou linhas", finalProcessingLogPath);
                    /**************************************************************************************************/
                    /**************************************** RESUMO IVA **********************************************/
                    /**************************************************************************************************/
                    LogMessageToFile("Começou IVA", finalProcessingLogPath);
                    numeroLinhas = infoOcrXmlIvasTemp.Count;

                    System.Xml.XmlNode FatherNode_15 = xmlDoc.CreateElement("TAXTOTAL");
                    childNode = xmlDoc.CreateElement("TaxEvidenceIndicator");
                    FatherNode_15.AppendChild(childNode);
                    childNode.AppendChild(xmlDoc.CreateTextNode("true"));

                    childNode = xmlDoc.CreateElement("TaxAmount");
                    if (!totalIva.Equals(-1))
                    {
                        childNode.AppendChild(xmlDoc.CreateTextNode(totalIva.ToString()));
                    }
                    else
                    {
                        pathcampoxml = "/Invoice/TaxTotal/TaxAmount";
                        text = xmlDoc.CreateTextNode(this.getCabecalhoTextoToXml(pathcampoxml, "UBL2.0", infoOcrXmlCabTemp));
                        childNode.AppendChild(text);
                    }
                    FatherNode_15.AppendChild(childNode);

                    // 09-03-2015 - novo campo para quando é o utilizador a selecionar a taxa de IVA no CABEÇALHO
                    if (!String.IsNullOrWhiteSpace(selectedTaxTemp))
                    {
                        decimal decval = -1;
                        int intVal = -1;
                        if (decimal.TryParse(selectedTaxTemp, out decval))
                            intVal = (int)(decval * 100);

                        childNode = xmlDoc.CreateElement("HeaderTax");
                        childNode.AppendChild(xmlDoc.CreateTextNode(intVal.ToString()));
                        FatherNode_15.AppendChild(childNode);
                    }

                    for (int i = 0; i < numeroLinhas; i++)
                    {
                        childNode = xmlDoc.CreateElement("TaxSubTotal");

                        childNode_2 = xmlDoc.CreateElement("TaxLineID");
                        childNode_2.AppendChild(xmlDoc.CreateTextNode((i + 1).ToString()));
                        childNode.AppendChild(childNode_2);

                        childNode_2 = xmlDoc.CreateElement("BaseSummary");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/TaxTotal/TaxSubTotal/BASESUMMARY";
                        text = xmlDoc.CreateTextNode(this.getIvaTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlIvasTemp));
                        string baseIva = text.Value;
                        childNode_2.AppendChild(text);

                        childNode_2 = xmlDoc.CreateElement("TaxSummary");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/TaxTotal/TaxSubTotal/TOTALVATSUMMARY";
                        text = xmlDoc.CreateTextNode(this.getIvaTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlIvasTemp));

                        string valor = "";
                        if (text.Value.Contains("."))
                        {
                            string[] t = text.Value.Split('.');
                            valor = t[0];
                        }
                        else if (text.Value.Contains(","))
                        {
                            string[] t = text.Value.Split(',');
                            valor = t[0];
                        }
                        childNode_2.AppendChild(xmlDoc.CreateTextNode(valor));

                        childNode_2 = xmlDoc.CreateElement("TotalVATSummary");
                        childNode.AppendChild(childNode_2);
                        pathcampoxml = "/Invoice/TaxTotal/TaxSubTotal/TAXSUMMARY";
                        text = xmlDoc.CreateTextNode(this.getIvaTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlIvasTemp));
                        string taxvalue = text.Value;
                        childNode_2.AppendChild(text);

                        childNode_2 = xmlDoc.CreateElement("TaxableAmount");
                        childNode_2.AppendChild(xmlDoc.CreateTextNode(baseIva));
                        childNode.AppendChild(childNode_2);

                        childNode_2 = xmlDoc.CreateElement("TaxAmount");
                        childNode_2.AppendChild(xmlDoc.CreateTextNode(taxvalue));
                        childNode.AppendChild(childNode_2);


                        childNode_2 = xmlDoc.CreateElement("TaxCategory");
                        childNode.AppendChild(childNode_2);

                        childNode_3 = xmlDoc.CreateElement("ID");
                        childNode_2.AppendChild(childNode_3);

                        childNode_3 = xmlDoc.CreateElement("Percent");
                        childNode_2.AppendChild(childNode_3);
                        childNode_3.AppendChild(xmlDoc.CreateTextNode(valor));

                        childNode_3 = xmlDoc.CreateElement("TaxScheme");
                        childNode_2.AppendChild(childNode_3);

                        childNode_4 = xmlDoc.CreateElement("ID");
                        childNode_4.AppendChild(xmlDoc.CreateTextNode(taxSchemeID));
                        childNode_3.AppendChild(childNode_4);
                        childNode_4 = xmlDoc.CreateElement("TaxTypeCode");
                        childNode_4.AppendChild(xmlDoc.CreateTextNode(taxSchemeTypeCode));
                        childNode_3.AppendChild(childNode_4);

                        FatherNode_15.AppendChild(childNode);

                        //rootNode.AppendChild(FatherNode_15);
                    }
                    rootNode.AppendChild(FatherNode_15);
                    LogMessageToFile("Acabou IVA", finalProcessingLogPath);

                    /**************************************************************************************************/
                    /****************************************** ANEXOS ************************************************/
                    /**************************************************************************************************/
                    LogMessageToFile("Começou Anexos", finalProcessingLogPath);
                    numeroLinhas = infoOcrXmlAnexosTemp.Count;

                    System.Xml.XmlNode FatherNode_16 = xmlDoc.CreateElement("ListaAnexos");
                    rootNode.AppendChild(FatherNode_16);
                    for (int i = 0; i < numeroLinhas; i++)
                    {
                        childNode = xmlDoc.CreateElement("Anexo");

                        childNode_2 = xmlDoc.CreateElement("LinhaAnexoID");
                        childNode_2.AppendChild(xmlDoc.CreateTextNode((i + 1).ToString()));
                        childNode.AppendChild(childNode_2);

                        for (int j = 0; j < ((ArrayList)infoOcrXmlAnexosTemp[i]).Count; j++)
                        {
                            string nomeCampo = ((InfoMetadados)((ArrayList)infoOcrXmlAnexosTemp[i])[j]).DadosTemplXml.NomeCampo;
                            childNode_2 = xmlDoc.CreateElement(nomeCampo);
                            childNode.AppendChild(childNode_2);
                            pathcampoxml = "/Invoice/ListaAnexos/Anexo/" + nomeCampo;
                            text = xmlDoc.CreateTextNode(this.getAnexosTextoToXml(pathcampoxml, "UBL2.0", i, infoOcrXmlAnexosTemp));
                            childNode_2.AppendChild(text);
                        }

                        FatherNode_16.AppendChild(childNode);
                    }
                    LogMessageToFile("Acabou Anexos", finalProcessingLogPath);
                    /**************************************************************/

                    xmlDoc.AppendChild(rootNode);

                    //alterado 01-06-2012
                    //xmlDoc.Save(createdXmlPath);
                    xmlDoc.Save(finalDocPath);


                }
                else if (TipoXML == "BasicMetadata") /********************************* Basic Metadata********************************/
                {
                    //string newest = actualDocNameTemp.Replace(".tif", ".pdf");
                    string newest = actualDocNameTemp.Remove(actualDocNameTemp.LastIndexOf(".tif"), ".tif".Length);
                    newest = newest + ".pdf";

                    string pathcaminhoxml = "";

                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));

                    System.Xml.XmlNode rootNode = xmlDoc.CreateElement("basicMetadata");
                    System.Xml.XmlNode childNode = null;
                    System.Xml.XmlAttribute attr = null;
                    System.Xml.XmlCDataSection cdata = null;

                    System.Xml.XmlNode FatherNode = xmlDoc.CreateElement("Cabecalho");
                    rootNode.AppendChild(FatherNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "DOCID";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='DOCID']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);


                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "NomeReceptor";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='NomeReceptor']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "EmailReceptor";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='EmailCliente']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "NIFReceptor";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='NIFReceptor']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "NomeEmissor";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='NomeEmissor']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "NIFEmissor";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='NIFEmissor']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "NumFactura";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='NumFactura']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "DataFactura";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='DataFactura']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "Ficheiro";
                    cdata = xmlDoc.CreateCDataSection(newest);
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "DocumentoOriginal";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='ORIGINALDOC']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "QuantiaComIVA";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='QuantiaComIVA']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "QuantiaSemIVA";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='QuantiaSemIVA']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "Moeda";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='Moeda']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "QuantiaComIVAMoedaInterna";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='QuantiaComIVAMoedaInterna']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "QuantiaSemIVAMoedaInterna";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='QuantiaSemIVAMoedaInterna']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "TaxaCambio";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='EXCHANGERATE']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "CondicaoPagamento";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='PaymentMeans']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "SWIFT";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='SWIFT']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "IBAN";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='IBAN']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "TipoDocumento";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='DocumentType']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);

                    childNode = xmlDoc.CreateElement("key");
                    attr = xmlDoc.CreateAttribute("name");
                    attr.Value = "Nota Encomenda";
                    pathcaminhoxml = "/basicMetadata/Cabecalho/key[@name='ORDER']";
                    //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposCAB, ArrayValorCampoCAB));
                    cdata = xmlDoc.CreateCDataSection(this.getCabecalhoTextoToXml(pathcaminhoxml, "BasicMetadata", infoOcrXmlCabTemp));
                    childNode.Attributes.Append(attr);
                    childNode.AppendChild(cdata);
                    FatherNode.AppendChild(childNode);


                    /*************************************************************************************************/
                    /**************************************** LINE ITEMS *********************************************/
                    /*************************************************************************************************/
                    //int numeroLinhas = ReturnNumeroLinhas(ArrayCamposLI);
                    int numeroLinhas = infoOcrXmlLines.Count;

                    for (int i = 0; i < numeroLinhas; i++)
                    {
                        FatherNode = xmlDoc.CreateElement("LineItem");
                        rootNode.AppendChild(FatherNode);
                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "LinhaID";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='LinhaID']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "Quantidade";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='Quantidade']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "Preco";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='Preco']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "ValorComIVA";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='ValorComIVA']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "ValorSemIVA";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='ValorSemIVA']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "TaxaIVA";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='TaxaIVA']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "CentroCusto";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='CentroCusto']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "NotaEncomenda";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='NotaEncomenda']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "Contentor";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='Contentor']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "ViagemPartida";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='ViagemPartida']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "ImportacaoExportacao";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='ImportacaoExportacao']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "Referencia";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='Referencia']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "CGA";
                        pathcaminhoxml = "/basicMetadata/LineItem/key[@name='CGA']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposLI, ArrayValorCampoLI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getLinhasTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlLinesTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);
                    }



                    /*************************************************************************************************/
                    /**************************************** Resumo de IVA *********************************************/
                    /*************************************************************************************************/
                    //int numeroLinhas = ReturnNumeroLinhas(ArrayCamposRI);
                    numeroLinhas = infoOcrXmlIvas.Count;

                    for (int i = 0; i < numeroLinhas; i++)
                    {
                        FatherNode = xmlDoc.CreateElement("IVA");
                        rootNode.AppendChild(FatherNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "Base";
                        pathcaminhoxml = "/basicMetadata/IVA/key[@name='Base']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposRI, ArrayValorCampoRI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getIvaTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlIvasTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "Taxa";
                        pathcaminhoxml = "/basicMetadata/IVA/key[@name='Taxa']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposRI, ArrayValorCampoRI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getIvaTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlIvasTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);

                        childNode = xmlDoc.CreateElement("key");
                        attr = xmlDoc.CreateAttribute("name");
                        attr.Value = "Valor";
                        pathcaminhoxml = "/basicMetadata/IVA/key[@name='Valor']";
                        //cdata = xmlDoc.CreateCDataSection(ReturnValueCampoXML(pathcaminhoxml, ArrayCamposRI, ArrayValorCampoRI, i));
                        cdata = xmlDoc.CreateCDataSection(this.getIvaTextoToXml(pathcaminhoxml, "BasicMetadata", i, infoOcrXmlIvasTemp));
                        childNode.Attributes.Append(attr);
                        childNode.AppendChild(cdata);
                        FatherNode.AppendChild(childNode);
                    }

                    /*END*/
                    LogMessageToFile("Acabar XML", finalProcessingLogPath);
                    xmlDoc.AppendChild(rootNode);
                    xmlDoc.Save(finalDocPath);

                }
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao criar o XML (createXML)", finalProcessingLogPath);
                throw new Exception(ex.Message);
            }
            return finalDocPath;
        }

        #region  QQTextoToXml
        private string getIvaTextoToXml(string pathcampoxml, string tipoxml, int i, ArrayList infoOcrXmlIvasTemp)
        {
            string caminhoTemp = "";
            string texto = "";
            //string connection = this.getConnString();
            //SqlConnection conn = new SqlConnection(connection);

            if (conn_teste.State.Equals(ConnectionState.Closed))
                conn_teste.Open();


            foreach (InfoMetadados im in (ArrayList)infoOcrXmlIvasTemp[i])
            {
                SqlCommand cmd = null;
                try
                {
                    //conn.Open();

                    string q = "SELECT CaminhoXML from CamposXML where NomeCampo=@nCampo and Tipo='ResumoIVA' and TipoXML=@tipoXml";
                    cmd = new SqlCommand(q, conn_teste);
                    cmd.Parameters.Add("@nCampo", SqlDbType.NVarChar).Value = im.DadosTemplXml.NomeCampo;
                    cmd.Parameters.Add("@tipoXml", SqlDbType.NVarChar).Value = tipoxml;
                    SqlDataReader thisReader = cmd.ExecuteReader();
                    while (thisReader.Read())
                    {
                        caminhoTemp = thisReader.GetString(0);
                    }
                    thisReader.Close();
                    cmd = null;
                    q = null;
                }
                catch (Exception ex)
                {
                    LogMessageToFile("ERRO: ao obter o texto a por no XML (getIvaTextoToXml)", mainLogPath);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    //if (conn.State == ConnectionState.Open)
                    //    conn.Close();
                }

                if (caminhoTemp == pathcampoxml)
                {
                    texto = im.ValorTB.Text;
                    return texto;
                }

            }
            return "";
        }

        private string getLinhasTextoToXml(string pathcampoxml, string tipoxml, int i, ArrayList infoOcrXmlLinesTemp)
        {

            string caminhoTemp = "";
            string texto = "";
            //string connection = this.getConnString();
            //SqlConnection conn = new SqlConnection(connection);

            if (conn_teste.State.Equals(ConnectionState.Closed))
                conn_teste.Open();

            foreach (InfoMetadados im in (ArrayList)infoOcrXmlLinesTemp[i])
            {
                SqlCommand cmd = null;
                try
                {
                    //conn.Open();
                    //com o nome do campo e tipo XML (neste caso UBL), obter o caminho do xml
                    string q = "SELECT CaminhoXML from CamposXML where NomeCampo=@nCampo and Tipo='LineItem' and TipoXML=@tipoXml";
                    cmd = new SqlCommand(q, conn_teste);
                    cmd.Parameters.Add("@nCampo", SqlDbType.NVarChar).Value = im.DadosTemplXml.NomeCampo;
                    cmd.Parameters.Add("@tipoXml", SqlDbType.NVarChar).Value = tipoxml;
                    SqlDataReader thisReader = cmd.ExecuteReader();
                    while (thisReader.Read())
                    {
                        caminhoTemp = thisReader.GetString(0);
                    }
                    thisReader.Close();
                    cmd = null;
                    q = null;
                }
                catch (Exception ex)
                {
                    LogMessageToFile("ERRO: ao obter o texto a por no XML (getLinhasTextoToXml)", mainLogPath);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    //if (conn.State == ConnectionState.Open)
                    //    conn.Close();
                }

                if (caminhoTemp == pathcampoxml)
                {
                    texto = im.ValorTB.Text;
                    return texto;
                }


            }
            return "";
        }

        // obter o texto a por no Xml, dado o caminho do nó do XML
        private string getCabecalhoTextoToXml(string pathcampoxml, string tipoxml, ArrayList infoOcrXmlCabTemp)
        {
            string caminhoTemp = "";
            string texto = "";


            if (conn_teste.State.Equals(ConnectionState.Closed))
                conn_teste.Open();


            foreach (InfoMetadados im in infoOcrXmlCabTemp)
            {
                SqlCommand cmd = null;
                try
                {
                    //com o nome do campo e tipo XML (neste caso UBL), obter o caminho do xml
                    string q = "SELECT CaminhoXML from CamposXML where NomeCampo=@nCampo and Tipo='Cabecalho' and TipoXML=@tipoXml";
                    cmd = new SqlCommand(q, conn_teste);
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@nCampo", SqlDbType.NVarChar).Value = im.DadosTemplXml.NomeCampo;
                    cmd.Parameters.Add("@tipoXml", SqlDbType.NVarChar).Value = tipoxml;
                    SqlDataReader thisReader = cmd.ExecuteReader();
                    while (thisReader.Read())
                    {
                        caminhoTemp = thisReader.GetString(0);
                    }
                    thisReader.Close();
                    cmd = null;
                    q = null;
                }
                catch (Exception ex)
                {
                    LogMessageToFile("ERRO: ao obter o texto a por no XML (getCabecalhoTextoToXml)", mainLogPath);
                    LogMessageToFile(ex.Message, mainLogPath);
                    LogMessageToFile(ex.StackTrace, mainLogPath);
                    //throw new Exception(ex.Message);
                }
                finally
                {
                    //if (conn.State == ConnectionState.Open)
                    //    conn.Close();
                }

                if (caminhoTemp == pathcampoxml)
                {
                    texto = im.ValorTB.Text;
                    break;
                }

            }

            return texto;
        }

        private string getAnexosTextoToXml(string pathcampoxml, string tipoxml, int i, ArrayList infoOcrXmlAnexosTemp)
        {
            string caminhoTemp = "";
            string texto = "";
            //string connection = this.getConnString();
            //SqlConnection conn = new SqlConnection(connection);

            if (conn_teste.State.Equals(ConnectionState.Closed))
                conn_teste.Open();

            foreach (InfoMetadados im in (ArrayList)infoOcrXmlAnexosTemp[i])
            {
                SqlCommand cmd = null;
                try
                {
                    //conn.Open();
                    //com o nome do campo e tipo XML (neste caso UBL), obter o caminho do xml
                    string q = "SELECT CaminhoXML from CamposXML where NomeCampo=@nCampo and Tipo='Anexos' and TipoXML=@tipoXml";
                    cmd = new SqlCommand(q, conn_teste);
                    cmd.Parameters.Add("@nCampo", SqlDbType.NVarChar).Value = im.DadosTemplXml.NomeCampo;
                    cmd.Parameters.Add("@tipoXml", SqlDbType.NVarChar).Value = tipoxml;
                    SqlDataReader thisReader = cmd.ExecuteReader();
                    while (thisReader.Read())
                    {
                        caminhoTemp = thisReader.GetString(0);
                    }
                    thisReader.Close();
                    cmd = null;
                    q = null;
                }
                catch (Exception ex)
                {
                    LogMessageToFile("ERRO: ao obter o texto a por no XML (getAnexosTextoToXml)", mainLogPath);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    //if (conn.State == ConnectionState.Open)
                    //    conn.Close();
                }

                if (caminhoTemp == pathcampoxml)
                {
                    texto = im.ValorTB.Text;
                    return texto;
                }


            }
            return "";
        }
        #endregion

        #region saveDocumentProcess
        //guardar dados do Iva, após validação
        private void saveDocumentProcessIva(Guid savedFacturaId, ArrayList infoOcrXmlIvasTemp, string nifEncontradoTemp, string tipodocTemp)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {

                //string connStr = DatabaseConnection.GetConnString("Desmaterializacao");

                //using (conn = new SqlConnection(connStr))
                //{


                if (conn_teste.State.Equals(ConnectionState.Closed))
                    conn_teste.Open();

                //string connection = this.getConnString();
                //conn = new SqlConnection(connection);
                //conn.Open();
                string q = "INSERT INTO FacturaIVA (pkid,FKFactura,FKResumoIVA,Valor,ResumoID,NomeCampoXml)" +
                    " VALUES (newid(), @fkfact,@fkiva,@val,@resID,@nomeCampo)";
                int i = 0;
                foreach (ArrayList al in infoOcrXmlIvasTemp)
                {
                    foreach (InfoMetadados infometa in al)
                    {
                        cmd = new SqlCommand(q, conn_teste);
                        cmd.Parameters.Add("@fkfact", SqlDbType.UniqueIdentifier).Value = savedFacturaId;
                        cmd.Parameters.Add("@fkiva", SqlDbType.UniqueIdentifier).Value = this.getFieldIdFromMasterization(infometa.DadosTemplXml.NomeCampo, "ivaGroup", nifEncontradoTemp, tipodocTemp);
                        cmd.Parameters.Add("@val", SqlDbType.NVarChar).Value = infometa.ValorTB.Text;
                        cmd.Parameters.Add("@resID", SqlDbType.Int).Value = i;
                        cmd.Parameters.Add("@nomeCampo", SqlDbType.NVarChar).Value = infometa.DadosTemplXml.NomeCampo; //09-05-2012
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    i++;
                }
                q = null;
                //}
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao gravar o processamento do Resumo IVA (saveDocumentProcessIva)", finalProcessingLogPath);
                LogMessageToFile(ex.StackTrace, finalProcessingLogPath);
                throw new Exception(ex.Message);
            }
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
            }
        }

        //guardar dados das Linhas, após validação
        private void saveDocumentProcessLinhas(Guid savedFacturaId, ArrayList infoOcrXmlLinesTemp, string nifEncontradoTemp, string tipodocTemp)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                //string connection = this.getConnString();
                //conn = new SqlConnection(connection);
                //conn.Open();

                //string connStr = DatabaseConnection.GetConnString("Desmaterializacao");

                //using (conn = new SqlConnection(connStr))
                //{

                if (conn_teste.State.Equals(ConnectionState.Closed))
                    conn_teste.Open();

                string q = "INSERT INTO FacturaLinhas (pkid,FKFactura,FKLinha,Valor,LinhaID,NomeCampoXml)" +
                " VALUES (newid(), @fkfact,@fklinha,@val,@lineId,@nomeCampo)";
                int i = 0;
                foreach (ArrayList al in infoOcrXmlLinesTemp)
                {
                    foreach (InfoMetadados infometa in al)
                    {
                        cmd = new SqlCommand(q, conn_teste);
                        cmd.Parameters.Add("@fkfact", SqlDbType.UniqueIdentifier).Value = savedFacturaId;
                        cmd.Parameters.Add("@fklinha", SqlDbType.UniqueIdentifier).Value = this.getFieldIdFromMasterization(infometa.DadosTemplXml.NomeCampo, "line", nifEncontradoTemp, tipodocTemp);
                        cmd.Parameters.Add("@val", SqlDbType.NVarChar).Value = infometa.ValorTB.Text;
                        cmd.Parameters.Add("@lineID", SqlDbType.Int).Value = i;
                        cmd.Parameters.Add("@nomeCampo", SqlDbType.NVarChar).Value = infometa.DadosTemplXml.NomeCampo; //09-05-2012
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    i++;
                }
                q = null;
                //}
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao gravar o processamento dos Line Items (saveDocumentProcessLinhas)", finalProcessingLogPath);
                LogMessageToFile(ex.StackTrace, finalProcessingLogPath);
                throw new Exception(ex.Message);
            }
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
            }
        }

        //guardar dados do Cabecalho, após validação
        private void saveDocumentProcessCab(Guid savedFacturaId, ArrayList infoOcrXmlCabTemp, string nifEncontradoTemp, string tipodocTemp)
        {
            try
            {
                //string connStr = DatabaseConnection.GetConnString("Desmaterializacao");

                //using (SqlConnection conn = new SqlConnection(connStr))
                //{
                string q = "INSERT INTO FacturaCabecalho (pkid, FKFactura, FKCabecalho, Valor, NomeCampoXml)" +
                    " VALUES (newid(), @fkfact, @fkcab, @val, @nomeCampo)";

                if (conn_teste.State.Equals(ConnectionState.Closed))
                    conn_teste.Open();

                foreach (InfoMetadados infometa in infoOcrXmlCabTemp)
                {
                    using (SqlCommand cmd = new SqlCommand(q, conn_teste))
                    {
                        cmd.Parameters.Add("@fkfact", SqlDbType.UniqueIdentifier).Value = savedFacturaId;
                        cmd.Parameters.Add("@fkcab", SqlDbType.UniqueIdentifier).Value = this.getFieldIdFromMasterization(infometa.DadosTemplXml.NomeCampo, "cab", nifEncontradoTemp, tipodocTemp);
                        cmd.Parameters.Add("@val", SqlDbType.NVarChar).Value = infometa.ValorTB.Text;
                        cmd.Parameters.Add("@nomeCampo", SqlDbType.NVarChar).Value = infometa.DadosTemplXml.NomeCampo;

                        cmd.ExecuteNonQuery();
                    }
                }
                q = null;
                //}
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao gravar o processamento do Cabeçalho (saveDocumentProcessCab)", finalProcessingLogPath);
                LogMessageToFile(ex.StackTrace, finalProcessingLogPath);
                throw new Exception(ex.Message);
            }
        }

        //guardar dados das Linhas, após validação
        private void saveDocumentProcessAnexo(Guid savedFacturaId, ArrayList infoOcrXmlAnexosTemp, string nifEncontradoTemp, string tipodocTemp)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {

                //string connStr = DatabaseConnection.GetConnString("Desmaterializacao");

                //using (conn = new SqlConnection(connStr))
                //{

                if (conn_teste.State.Equals(ConnectionState.Closed))
                    conn_teste.Open();

                string q = "INSERT INTO FacturaAnexo (pkid,FKFactura,FKAnexo,Valor,AnexoID,NomeCampoXml)" +
                " VALUES (newid(), @fkfact,@fklinha,@val,@lineId,@nomeCampo)";
                int i = 0;
                foreach (ArrayList al in infoOcrXmlAnexosTemp)
                {
                    foreach (InfoMetadados infometa in al)
                    {
                        cmd = new SqlCommand(q, conn_teste);
                        cmd.Parameters.Add("@fkfact", SqlDbType.UniqueIdentifier).Value = savedFacturaId;
                        cmd.Parameters.Add("@fklinha", SqlDbType.UniqueIdentifier).Value = this.getFieldIdFromMasterization(infometa.DadosTemplXml.NomeCampo, "anexo", nifEncontradoTemp, tipodocTemp);
                        cmd.Parameters.Add("@val", SqlDbType.NVarChar).Value = infometa.ValorTB.Text;
                        cmd.Parameters.Add("@lineID", SqlDbType.Int).Value = i;
                        cmd.Parameters.Add("@nomeCampo", SqlDbType.NVarChar).Value = infometa.DadosTemplXml.NomeCampo; //09-05-2012
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    i++;
                }
                q = null;
                //}
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao gravar o processamento dos Anexos (saveDocumentProcessAnexo)", finalProcessingLogPath);
                LogMessageToFile(ex.StackTrace, finalProcessingLogPath);
                throw new Exception(ex.Message);
            }
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
            }
        }

        #endregion

        // gera .tif com menor tamanho, gera pdf e gera pdf-a (ORIGINAL EM CIMA)
        private void createPdfAndAttachs(string actualDocNameTemp, bool querAnexosTemp, bool anexosConcatenadosTemp, string nifEncontradoTemp, string finalFilesName, ResolutionObject pdfResTemp)
        {
            try
            {
                #region Regedit | comentado por Tiago Borges a 30/06/2016 alteração para BD
                //bool is64Bit = System.Environment.Is64BitOperatingSystem;
                //Microsoft.Win32.RegistryKey regKey = null;
                //if (!is64Bit)
                //    regKey = Microsoft.Win32.Registry.LocalMachine;
                //else
                //    regKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                //Microsoft.Win32.RegistryKey ebcHive = regKey.OpenSubKey(@"SOFTWARE\PI Portugal Informatico\FilePaths");
                //string nident = ebcHive.GetValue("NaoIdentificadas").ToString();
                //string PathProcessar = ebcHive.GetValue("Processar").ToString();
                //string PathImages = ebcHive.GetValue("Imagens").ToString();
                #endregion
                string PathProcessar = Helpers.GetConfigFromDataBase("Processar").ToString();
                string nident = Helpers.GetConfigFromDataBase("NaoIdentificadas").ToString();
                string PathImages = Helpers.GetConfigFromDataBase("Imagens").ToString();

                DirectoryInfo procurar = new DirectoryInfo(PathImages);
                FileInfo[] encontrado = procurar.GetFiles(actualDocNameTemp, SearchOption.AllDirectories);


                //string fich2 = actualDocNameTemp.Replace(".tif", "");
                string fich2 = actualDocNameTemp.Remove(actualDocNameTemp.LastIndexOf(".tif"), ".tif".Length);

                string ImgSource = PathImages + actualDocNameTemp;
                string imageFinal = PathProcessar + actualDocNameTemp;

                //acrescentado 12-04-2012 - se quer anexos
                LogMessageToFile("A ver se quer anexos.", finalProcessingLogPath);
                ArrayList anexos = null;
                if (querAnexosTemp)
                {
                    LogMessageToFile("Quer anexos, vai obtê-los.", finalProcessingLogPath);
                    anexos = this.getAttachs(nifEncontradoTemp, actualDocNameTemp, PathImages);
                    if (anexos.Count > 0)
                    { //se quer anexos e se tem anexos
                        LogMessageToFile("Encontrou " + anexos.Count + " que correspondem ao documento '" + actualDocNameTemp + "'.", finalProcessingLogPath);
                        LogMessageToFile("Vai redimensionar os anexos. ", finalProcessingLogPath);
                        int count = 0;
                        foreach (FileInfo anexo in anexos)
                        {
                            int numPags = this.resizeAndSaveTemporaryTifPagesAnexos(anexo.FullName, count, pdfResTemp, fich2); //11-09-2012
                            count = numPags;
                            anexo.Delete();
                        }
                    }
                    else
                        LogMessageToFile("Não encontrou anexos que correspondam ao documento '" + actualDocNameTemp + "'.", finalProcessingLogPath);
                }
                else
                    LogMessageToFile("Não quer anexos.", finalProcessingLogPath);


                string name = Path.GetFileNameWithoutExtension(imageFinal);
                //string pdfSource = PathProcessar + finalFilesName + "_basepdf.pdf";
                string pdfSource = PathProcessar + name + "_basepdf.pdf";

                //criar PDF - 28-03-2012
                PdfDocument doc = new PdfDocument();
                XImage img = null;
                XGraphics xgr = null;


                string imagesPath = Helpers.GetConfigFromDataBase("Processar").ToString();
                string temporaryFolder = imagesPath + @"Temp\";

                DirectoryInfo di = new DirectoryInfo(temporaryFolder);
                FileInfo[] tifFiles = di.GetFiles("*.tif");

                //FileInfo teste = new FileInfo()
                int i = 0;
                foreach (FileInfo fi in encontrado)
                {
                    //11-09-2012
                    bool isFromSameDoc = fi.Name.Contains(fich2);
                    if (isFromSameDoc)
                    {
                        Image image = Image.FromFile(fi.FullName);
                        int count = image.GetFrameCount(FrameDimension.Page);
                        for (int pageNum = 0; pageNum < count; pageNum++)
                        {
                            LogMessageToFile("a criar Pag. " + pageNum, finalProcessingLogPath);

                            image.SelectActiveFrame(FrameDimension.Page, pageNum);
                            PdfPage page = new PdfPage();
                            doc.Pages.Add(page);
                            xgr = XGraphics.FromPdfPage(page);
                            XImage ximg = XImage.FromGdiPlusImage(image);
                            xgr.DrawImage(ximg, 0, 0);
                        }
                    }
                }


                //acrescentado 12-04-2012 - se tem anexos
                string temporaryAttachFolder = imagesPath + @"TempAnexos\";
                DirectoryInfo diAttach = new DirectoryInfo(temporaryAttachFolder);
                FileInfo[] tifFilesAttach = diAttach.GetFiles("*.tif");
                if (anexos != null && anexos.Count > 0)
                {
                    //se quer anexos concatenados ao PDF/Factura
                    if (anexosConcatenadosTemp)
                    {
                        foreach (FileInfo fi3 in tifFilesAttach)
                        {
                            //11-09-2012
                            bool isFromSameDoc = fi3.Name.Contains(fich2);
                            if (isFromSameDoc)
                            {

                                PdfPage page = new PdfPage();
                                img = XImage.FromFile(fi3.FullName);

                                //alterado 24-07-2012
                                //page.Width = img.Width;
                                //page.Height = img.Height;
                                //page.Size = PdfSharp.PageSize.A4; //se quisermos que fique 100% = ao standard A4
                                double w = (double)img.Width / (double)pdfResTemp.DpiResolution;
                                double h = (double)img.Height / (double)pdfResTemp.DpiResolution;
                                page.Width = XUnit.FromInch(w);
                                page.Height = XUnit.FromInch(h);

                                doc.Pages.Add(page);
                                xgr = XGraphics.FromPdfPage(doc.Pages[i]);
                                //alterado 24-07-2012
                                //xgr.DrawImage(img, 0, 0, img.Width, img.Height);
                                xgr.DrawImage(img, 0, 0, doc.Pages[i].Width.Point, doc.Pages[i].Height.Point); //595, 842 para A4
                                page.Close();
                                i++;
                            }
                        }
                    }
                    else //se quer anexos separados
                    {
                        PdfDocument docAnex = new PdfDocument();
                        XImage imgAnex = null;
                        XGraphics xgrAnex = null;
                        i = 0;
                        foreach (FileInfo fi4 in tifFilesAttach)
                        {
                            //11-09-2012
                            bool isFromSameDoc = fi4.Name.Contains(fich2);
                            if (isFromSameDoc)
                            {

                                PdfPage page = new PdfPage();
                                imgAnex = XImage.FromFile(fi4.FullName);

                                //alterado 24-07-2012
                                //page.Width = imgAnex.Width;
                                //page.Height = imgAnex.Height;
                                //page.Size = PdfSharp.PageSize.A4; //se quisermos que fique 100% = ao standard A4
                                double w = (double)imgAnex.Width / (double)pdfResTemp.DpiResolution;
                                double h = (double)imgAnex.Height / (double)pdfResTemp.DpiResolution;
                                page.Width = XUnit.FromInch(w);
                                page.Height = XUnit.FromInch(h);


                                docAnex.Pages.Add(page);
                                xgrAnex = XGraphics.FromPdfPage(docAnex.Pages[i]);
                                //alterado 24-07-2012
                                //xgrAnex.DrawImage(imgAnex, 0, 0, imgAnex.Width, imgAnex.Height);
                                xgrAnex.DrawImage(imgAnex, 0, 0, docAnex.Pages[i].Width.Point, docAnex.Pages[i].Height.Point); //595, 842 para A4
                                imgAnex.Dispose();
                                page.Close();
                                i++;
                            }
                        }

                        //garva o PDF com os anexos em Processar - alterado 01-06-2012
                        //string anexoFullName = PathProcessar + fich2 + "_Anexo.pdf";
                        string anexoFullName = PathProcessar + finalFilesName + "_Anexo.pdf";

                        if (System.IO.File.Exists(anexoFullName))
                        {
                            System.IO.File.Delete(anexoFullName);
                            docAnex.Save(anexoFullName);
                            docAnex.Close();
                        }
                        else
                        {
                            docAnex.Save(anexoFullName);
                            docAnex.Close();
                        }
                    }
                }


                //garva o PDF em Processar
                if (System.IO.File.Exists(pdfSource))
                {
                    System.IO.File.Delete(pdfSource);
                    doc.Save(pdfSource);
                    doc.Close();
                }
                else
                {
                    doc.Save(pdfSource);
                    doc.Close();
                }
                xgr.Dispose();
                //img.Dispose();

                //passar a PDF/A Tiago Esteves 13-03-2012 - alterado 01-06-2012
                //string pdfASource = PathProcessar + fich2 + "_pdfa.pdf";
                string pdfASource = PathProcessar + name + ".pdf";
                this.createPDFA(pdfSource, pdfASource);

                //29-03-2012 - apagar o PDF original
                if (System.IO.File.Exists(pdfSource))
                    System.IO.File.Delete(pdfSource);

                //apaga os .tif temporario em Processar\Temp\
                foreach (FileInfo fi in tifFiles)
                {
                    bool isFromSameDoc = fi.Name.Contains(fich2); //11-09-2012
                    if (isFromSameDoc)
                        fi.Delete();
                }

                //apaga os .tif temporario em Processar\TempAnexos\
                foreach (FileInfo fi2 in tifFilesAttach)
                {
                    bool isFromSameDoc = fi2.Name.Contains(fich2); //11-09-2012
                    if (isFromSameDoc)
                        fi2.Delete();
                }
                foreach (FileInfo fi in encontrado)
                {
                    bool isFromSameDoc = fi.Name.Contains(fich2);
                    if (isFromSameDoc)
                    {
                        //fi.Delete();
                        Delete(fi.FullName, PathProcessar + "eliminar.dat");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao criar o PDF, PDF-A ou Anexos (createPdfAndAttachs)", finalProcessingLogPath);
                LogMessageToFile(ex.Message, finalProcessingLogPath);
                LogMessageToFile(ex.StackTrace, finalProcessingLogPath);
                throw new Exception(ex.Message);
            }

        }



        /// <summary>
        /// Método que transforma um PDF num PDF/A
        /// </summary>
        /// <param name="input">caminho do PDF a ser transformado</param>
        /// <param name="output">caminho do PDF/A que será criado</param>
        private void createPDFA(string input, string output)
        {
            //10-09-2012
            System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            string logedUser = wi.Name;
            string[] temp = logedUser.Split('\\');
            logedUser = System.Environment.MachineName; //temp[temp.Length - 1]; //13-09-2012

            #region Regedit | comentado por Tiago Borges a 30/06/2016 alteração para BD
            //bool is64Bit = System.Environment.Is64BitOperatingSystem;
            //Microsoft.Win32.RegistryKey regKey = null;
            //if (!is64Bit)
            //    regKey = Microsoft.Win32.Registry.LocalMachine;
            //else
            //    regKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            //Microsoft.Win32.RegistryKey ebcHive = regKey.OpenSubKey(@"SOFTWARE\PI Portugal Informatico\FilePaths");
            //string Path = ebcHive.GetValue("LogFilesDigital").ToString();
            #endregion
            string Path = Helpers.GetConfigFromDataBase("LogFilesDigital").ToString();

            string stderrx = "";
            try
            {
                LogMessageToFile("Iniciando o Processo de transformação PDFA...", Path + logedUser + "_PDFA_log.dat");
                Process p = new Process();
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;

                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                LogMessageToFile("Criando os argumentos...", Path + logedUser + "_PDFA_log.dat");

                StringBuilder sb = new StringBuilder();
                //sb.Append(" -dPDFA");
                //sb.Append(" -dBATCH");
                //sb.Append(" -dNOPAUSE");
                //sb.Append(" -dUseCIEColor");
                //sb.Append(" -sProcessColorModel=DeviceCMYK");
                //sb.Append(" -sDEVICE=pdfwrite");
                //sb.Append(" -sPDFACompatibilityPolicy=1");
                //sb.Append(" -sOutputFile=" + output);
                //sb.Append(" " + input);
                sb.Append(" -dPDFA");
                sb.Append(" -dBATCH");
                sb.Append(" -dNOPAUSE");
                sb.Append(" -dUseCIEColor");
                sb.Append(" -sProcessColorModel=DeviceCMYK");
                sb.Append(" -sDEVICE=pdfwrite");
                sb.Append(" -sPDFACompatibilityPolicy=1");
                sb.Append(" -sOutputFile=" + '"' + output + '"' + " " + '"' + input + '"');
                //sb.Append(" " + input);


                LogMessageToFile("Ficheiro input:" + input, Path + logedUser + "_PDFA_log.dat");
                LogMessageToFile("Ficheiro output:" + output, Path + logedUser + "_PDFA_log.dat");

                //23-05-2012
                //comentado por Tiago Borges a 30/06/2016 alteração para BD
                //string pathGhostScript = ebcHive.GetValue("GhostScript").ToString();
                string pathGhostScript = Helpers.GetConfigFromDataBase("GhostScript").ToString();

                if (!Environment.Is64BitOperatingSystem)
                    p.StartInfo.FileName = pathGhostScript + "gswin32c.exe";
                else
                    p.StartInfo.FileName = pathGhostScript + "gswin64c.exe";

                p.StartInfo.Arguments = sb.ToString();
                p.Start();
                string stdoutx = p.StandardOutput.ReadToEnd();
                stderrx = p.StandardError.ReadToEnd();
                p.WaitForExit();
            }
            catch (Exception e)
            {
                LogMessageToFile("ERROR: " + e.Message, Path + logedUser + "_PDFA_log.dat");
                LogMessageToFile("STACK TRACE: " + e.StackTrace, Path + logedUser + "_PDFA_log.dat");
                if (stderrx != "")
                    LogMessageToFile("Process error: " + stderrx, Path + logedUser + "_PDFA_log.dat");
            }
        }

        public static void Delete(string msg, string path)
        {
            //StreamWriter sw = File.AppendText(path);
            try
            {
                StreamWriter sw = System.IO.File.AppendText(path);

                //string logLine = System.String.Format("{0:G}:" + System.DateTime.Now.Millisecond.ToString() + ": {1}", System.DateTime.Now, msg);
                sw.WriteLine(msg);

                sw.Close();
                sw.Dispose();
            }
            catch (Exception)
            {
            }
        }

        // guarda dados da validação na BD
        private Guid saveDocumentProcess(string actualDocNameTemp, Guid fornPkid)
        {
            Guid facturaId = new Guid();
            try
            {
                LogMessageToFile("Entrou no saveDocumentProcess", finalProcessingLogPath);
                string logedUser = "";
                try
                {
                    System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
                    logedUser = wi.Name;
                }
                catch (Exception)
                {
                    LogMessageToFile("ERRO: obter o user desta máquina (saveDocumentProcess)", finalProcessingLogPath);
                }

                LogMessageToFile("Obteve o windowsIdentity", finalProcessingLogPath);
                DateTime now = DateTime.Now;
                LogMessageToFile("vai obter a connection string", finalProcessingLogPath);
                string connStr = DatabaseConnection.GetConnString("Desmaterializacao");

                LogMessageToFile("ConnectionString value: " + connStr, finalProcessingLogPath);
                LogMessageToFile("vai entrar no using connection", finalProcessingLogPath);

                string q = "INSERT INTO Facturas (pkid,ValidadoEm,ValidadoPor,Validado,Documento,FKFornecedor,EnviadoERP)" +
                    " VALUES (newid(),@valEm,@valPor,@val,@doc,@fkforn,0)";

                LogMessageToFile("entrou no using", finalProcessingLogPath);
                LogMessageToFile("Estado da conexao " + conn_teste.State, finalProcessingLogPath);

                LogMessageToFile(now.ToString(), finalProcessingLogPath);
                LogMessageToFile(logedUser, finalProcessingLogPath);
                LogMessageToFile(actualDocNameTemp + ".tif", finalProcessingLogPath);
                LogMessageToFile(fornPkid.ToString(), finalProcessingLogPath);


                using (SqlCommand cmd = new SqlCommand(q, conn_teste))
                {
                    cmd.Parameters.Add("@valEm", SqlDbType.DateTime).Value = now;
                    cmd.Parameters.Add("@valPor", SqlDbType.NVarChar).Value = logedUser;
                    cmd.Parameters.Add("@val", SqlDbType.Bit).Value = true;
                    cmd.Parameters.Add("@doc", SqlDbType.NVarChar).Value = actualDocNameTemp + ".tif";
                    cmd.Parameters.Add("@fkforn", SqlDbType.UniqueIdentifier).Value = fornPkid;
                    cmd.ExecuteNonQuery();
                }
                //}

                LogMessageToFile("fez a 1ª query", finalProcessingLogPath);

                //obter o ID da factura inserida
                q = "SELECT pkid from Facturas where ValidadoEm=@valEm and ValidadoPor=@valPor and Documento=@doc";

                LogMessageToFile(now.ToString(), finalProcessingLogPath);
                LogMessageToFile(logedUser, finalProcessingLogPath);
                //LogMessageToFile(actualDocName + ".tif", finalProcessingLogPath);

                using (SqlCommand cmd = new SqlCommand(q, conn_teste))
                {
                    cmd.Parameters.Add("@valEm", SqlDbType.DateTime).Value = now;
                    cmd.Parameters.Add("@valPor", SqlDbType.NVarChar).Value = logedUser;
                    cmd.Parameters.Add("@doc", SqlDbType.NVarChar).Value = actualDocNameTemp + ".tif";


                    using (SqlDataReader thisReader = cmd.ExecuteReader())
                    {
                        while (thisReader.Read())
                            facturaId = thisReader.GetGuid(0);
                    }
                }

                LogMessageToFile("FEZ", finalProcessingLogPath);
                LogMessageToFile(facturaId.ToString(), finalProcessingLogPath);

                //}
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao guardar o processo da masterização do documento (saveDocumentProcess)", finalProcessingLogPath);
                LogMessageToFile(ex.StackTrace, finalProcessingLogPath);
                throw;
            }

            return facturaId;
        }

        private string buildFinalDocName(string tipoDoc, string nif, string numDoc)
        {
            string docTypeCode = "";
            if (tipoDoc.ToLower().Equals("factura") || tipoDoc.ToLower().Equals("fatura"))
                docTypeCode = "FT";
            else if (tipoDoc.ToLower().Equals("nota de débito") || tipoDoc.ToLower().Equals("nota de debito")
                || tipoDoc.ToLower().Equals("nota débito") || tipoDoc.ToLower().Equals("nota debito"))
                docTypeCode = "ND";
            else if (tipoDoc.ToLower().Equals("nota de crédito") || tipoDoc.ToLower().Equals("nota de credito")
                || tipoDoc.ToLower().Equals("nota crédito") || tipoDoc.ToLower().Equals("nota credito"))
                docTypeCode = "NC";
            else if (tipoDoc.ToLower().Equals("recibo"))
                docTypeCode = "RC";
            else
                docTypeCode = "NULL";

            //nome final do PDF e XML
            int ano = DateTime.Now.Year;
            string finalFilesName = nif + ano.ToString() + docTypeCode + numDoc;
            return finalFilesName;
        }

        //dado o nif obtém o PKID do fornecedor
        private Guid getFornPkidFromNifFinalProcessing(string nif)
        {
            Guid fornecedor = new Guid();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                //string connection = this.getConnString();
                //conn = new SqlConnection(connection);
                //conn.Open();
                string q = "SELECT pkid FROM Fornecedores where Contribuinte=@contrib";
                cmd = new SqlCommand(q, conn_teste);
                cmd.Parameters.Add("@contrib", SqlDbType.NVarChar).Value = nif;
                SqlDataReader thisReader = cmd.ExecuteReader();
                while (thisReader.Read())
                {
                    fornecedor = thisReader.GetGuid(0);
                }
                thisReader.Close();
                cmd = null;
                q = null;
            }
            catch (Exception e)
            {
                LogMessageToFile("ERRO: ao obter pkid do Fornecedor (getFornPkidFromNifFinalProcessing)", mainLogPath);
                LogMessageToFile(e.StackTrace, mainLogPath);
                //throw new Exception(e.Message);
                //msgbox.Show("Ocorreu um erro.", layout);
            }
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
            }
            return fornecedor;
        }

        // obtem PKID do campo masterizado correspondente ao valor a inserir após a validação. Recebe o nome do campo.
        private Guid getFieldIdFromMasterization(string nomeCampo, string tipo, string nifEncontradoTemp, string tipodocTemp)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            Guid facturaId = new Guid();
            try
            {
                //string connection = this.getConnString();
                //conn = new SqlConnection(connection);
                //conn.Open();
                string q = "";
                if (tipo == "cab")
                    q = "SELECT pkid from MasterizacaoCabecalho where NomeCampo=@nCampo and" +
                    " FKNomeTemplate=(select pkid from Nometemplate where fkfornecedor=@fkforn and fktipofact=@fktipofact)";
                else if (tipo == "line")
                    q = "SELECT pkid from MasterizacaoLineItems where NomeCampo=@nCampo and" +
                    " FKNomeTemplate=(select pkid from Nometemplate where fkfornecedor=@fkforn and fktipofact=@fktipofact)";
                else if (tipo == "ivaGroup")
                    q = "SELECT pkid from MasterizacaoIva where NomeCampo=@nCampo and" +
                    " FKNomeTemplate=(select pkid from Nometemplate where fkfornecedor=@fkforn and fktipofact=@fktipofact)";
                else if (tipo == "anexo") //19-06-2012
                    q = "SELECT pkid from MasterizacaoAnexos where NomeCampo=@nCampo and" +
                    " FKNomeTemplate=(select pkid from Nometemplate where fkfornecedor=@fkforn and fktipofact=@fktipofact)";

                if (conn_teste.State.Equals(ConnectionState.Closed))
                    conn_teste.Open();

                cmd = new SqlCommand(q, conn_teste);
                cmd.Parameters.Add("@nCampo", SqlDbType.NVarChar).Value = nomeCampo;
                cmd.Parameters.Add("@fkforn", SqlDbType.UniqueIdentifier).Value = this.getFornPkidFromNifFinalProcessing(nifEncontradoTemp);
                cmd.Parameters.Add("@fktipofact", SqlDbType.UniqueIdentifier).Value = this.getTipoDocPkidFromNome(tipodocTemp);
                SqlDataReader thisReader = cmd.ExecuteReader();
                while (thisReader.Read())
                {
                    facturaId = thisReader.GetGuid(0);
                }
                thisReader.Close();
                cmd = null;
                q = null;
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao obter o pkid do campo masterizado (getFieldIdFromMasterization)", mainLogPath);
                LogMessageToFile(ex.StackTrace, mainLogPath);
                throw new Exception(ex.Message);
            }
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
            }
            return facturaId;
        }

        //dado o nome do tipo de doc obtém o PKID do mesmo
        private Guid getTipoDocPkidFromNome(string nome)
        {
            Guid fkTipoFact = new Guid();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                //string connection = this.getConnString();
                //conn = new SqlConnection(connection);
                //conn.Open();
                string q = "SELECT pkid FROM TipoFacturas where nome=@n";
                cmd = new SqlCommand(q, conn_teste);
                cmd.Parameters.Add("@n", SqlDbType.NVarChar).Value = nome;
                SqlDataReader thisReader = cmd.ExecuteReader();
                while (thisReader.Read())
                {
                    fkTipoFact = thisReader.GetGuid(0);
                }
                thisReader.Close();
                cmd = null;
                q = null;
            }
            catch (Exception e)
            {
                LogMessageToFile("ERRO: ao obter pkid do do tipo de documento (getTipoDocPkidFromNome)", mainLogPath);
                LogMessageToFile(e.StackTrace, mainLogPath);
                throw new Exception(e.Message);
            }
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
            }
            return fkTipoFact;
        }

        // obter valor de Configuracoes
        private bool getBooleanConfigValue(string nome)
        {
            bool value = false;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                string connection = DatabaseConnection.GetConnString("Desmaterializacao");
                conn = new SqlConnection(connection);
                conn.Open();
                string q = "select valor from Configuracoes where nome=@n";
                cmd = new SqlCommand(q, conn);
                cmd.Parameters.Add("@n", SqlDbType.NVarChar).Value = nome;
                SqlDataReader thisReader = cmd.ExecuteReader();
                while (thisReader.Read())
                {
                    string temp = String.Empty;
                    if (!thisReader.IsDBNull(0))
                        temp = thisReader.GetString(0);

                    if (temp.Equals(String.Empty))
                        value = false;
                    else
                        value = Boolean.Parse(thisReader.GetString(0));
                }

                thisReader.Close();
                cmd = null;
                q = null;
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao obter configuraçao boleana (getBooleanConfigValue)", mainLogPath);
                LogMessageToFile(ex.Message, mainLogPath);
                LogMessageToFile(ex.StackTrace, mainLogPath);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return value;
        }

        public string base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception cse)
            {
                throw new Exception(cse.Message);
            }
        }

        private string getTextConfigValue(string nome)
        {
            string val = String.Empty;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                string connection = DatabaseConnection.GetConnString("Desmaterializacao");
                conn = new SqlConnection(connection);
                conn.Open();
                string q = "select valor from Configuracoes where nome=@n";
                cmd = new SqlCommand(q, conn);
                cmd.Parameters.Add("@n", SqlDbType.NVarChar).Value = nome;
                SqlDataReader thisReader = cmd.ExecuteReader();
                while (thisReader.Read())
                    val = thisReader.GetString(0);

                thisReader.Close();
                cmd = null;
                q = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return val;
        }

        // dado o nome do tipo de doc obtém a descrição associada para colocar no XML (campo tipo documento)
        private string GetFinalXmlDescriptionFromTipoDocNome(string tipoDocExtenso)
        {
            string xmlDescription = "UNKNOWN";
            try
            {
                string q = "SELECT FinalXmlDescription FROM TipoFacturas where nome=@n";
                using (SqlCommand cmd = new SqlCommand(q, conn_teste))
                {
                    cmd.Parameters.Add("@n", SqlDbType.NVarChar).Value = tipoDocExtenso;
                    SqlDataReader thisReader = cmd.ExecuteReader();
                    while (thisReader.Read())
                    {
                        if (!thisReader.IsDBNull(0))
                            xmlDescription = thisReader.GetString(0);
                    }
                    thisReader.Close();
                }
                q = null;
            }
            catch (Exception e)
            {
                LogMessageToFile("ERRO: ao obter a descrição do tipo de documento para colocar no XML final (GetFinalXmlDescriptionFromTipoDocNome)", mainLogPath);
                LogMessageToFile(e.StackTrace, mainLogPath);
            }
            return xmlDescription;
        }

        // dado o nome do TIFF obtém o IMPRINTER associado
        private string GetImprinterFromTiffName(string tiffName)
        {
            string imprinter = "";
            try
            {
                string q = "SELECT originalFilename, firstPageImprinter FROM DigitalizationControl WHERE processed=0 ORDER BY entryDateTime DESC";

                using (SqlCommand cmd = new SqlCommand(q, conn_teste))
                {
                    using (SqlDataReader thisReader = cmd.ExecuteReader())
                    {
                        while (thisReader.Read())
                        {
                            string originalFilename = thisReader.GetString(0);
                            string imprinterFromDB = thisReader.GetString(1);

                            if (tiffName.ToLower().Contains(originalFilename.ToLower()))
                            {
                                imprinter = imprinterFromDB;

                                break;
                            }
                        }
                    }
                }
                q = null;
            }
            catch (Exception e)
            {
                LogMessageToFile("ERRO: ao obter o IMPRINTER (GetImprinterFromTiffName)", mainLogPath);
                LogMessageToFile(e.StackTrace, mainLogPath);
            }
            return imprinter;
        }

        // atualiza para processado
        private void SetImprinterAsProcessed(string imprinter)
        {
            try
            {
                string q = "UPDATE DigitalizationControl SET processed=1 WHERE firstPageImprinter=@imprinter";

                using (SqlCommand cmd = new SqlCommand(q, conn_teste))
                {
                    cmd.Parameters.Add("@imprinter", SqlDbType.NVarChar).Value = imprinter;

                    cmd.ExecuteNonQuery();
                }
                q = null;
            }
            catch (Exception e)
            {
                LogMessageToFile("ERRO: ao atualizar DigitalizationControl (SetImprinterAsProcessed)", mainLogPath);
                LogMessageToFile(e.StackTrace, mainLogPath);
            }
        }

        //obter anexos de um fornecedor para um determinado documento/facrura
        private ArrayList getAttachs(string nifEncontrado, string actualDocNameTemp, string PathImages)
        {
            ArrayList anexos = new ArrayList();
            try
            {
                string pathAnexos = PathImages + nifEncontrado + "\\Anexos\\";
                LogMessageToFile("A procurar anexos na pasta: " + pathAnexos, finalProcessingLogPath);
                DirectoryInfo di = new DirectoryInfo(pathAnexos);

                //acrescentado "if" 13-04-2012
                if (di.Exists)
                {
                    FileInfo[] todosAnexos = di.GetFiles("*.tif");
                    foreach (FileInfo fi in todosAnexos)
                    {
                        string nomeAnexo = fi.Name;
                        string[] delimit = { ".tif" };
                        string[] temp = nomeAnexo.Split(delimit, StringSplitOptions.RemoveEmptyEntries);
                        string nomeDocFromNomeAnexo = temp[0];
                        nomeDocFromNomeAnexo = nomeDocFromNomeAnexo + "_" + nifEncontrado + ".tif";

                        //se o nome do anexo é igual ao nome do documento. E.g. Nome Anexo = nomeDocumento.tif2Page.tif
                        string tempName = actualDocNameTemp.Remove(actualDocNameTemp.IndexOf('!'), actualDocNameTemp.Length - actualDocNameTemp.IndexOf('!') - 14);

                        //alterado 11-09-2012
                        //if (nomeDocFromNomeAnexo == tempName)
                        if (tempName.Contains(nomeDocFromNomeAnexo))
                            anexos.Add(fi);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessageToFile("ERRO: ao obter anexos (getAttachs)", finalProcessingLogPath);
                LogMessageToFile(ex.Message, finalProcessingLogPath);
                LogMessageToFile(ex.StackTrace, finalProcessingLogPath);
                throw new Exception(ex.Message);
            }
            return anexos;
        }

        //a partir do anexo .tif original, cria um .tif mais pequeno para cada página e guarda-as em "Processar/TempAnexos"
        private int resizeAndSaveTemporaryTifPagesAnexos(string imgSource, int count, ResolutionObject pdfResTemp, string nomeTif)
        {
            System.OperatingSystem osInfo = System.Environment.OSVersion;

            if (osInfo.Version.Major <= 5)
            {
                try
                {
                    //carregar o .tif original
                    Image photoImg = Image.FromFile(imgSource);
                    int pages = photoImg.GetFrameCount(FrameDimension.Page);

                    for (int i = 0; i < pages; i++)
                    {
                        Image thisPage = getTiffImage(photoImg, i);
                        //reduzir o tamanho do .tif original para metada - fica com uma profundidade de cor 24 bpp

                        //20-07-2012
                        Bitmap b = null;
                        if (pdfResTemp.DpiResolution.Equals(300))
                        {
                            b = new Bitmap(thisPage.Width, thisPage.Height);
                            b.SetResolution((float)pdfResTemp.DpiResolution, (float)pdfResTemp.DpiResolution); //24-07-2012
                        }
                        else
                        {
                            if (thisPage.Width > thisPage.Height)
                                b = new Bitmap(pdfResTemp.Height, pdfResTemp.Width);
                            else
                                b = new Bitmap(pdfResTemp.Width, pdfResTemp.Height);

                            b.SetResolution((float)pdfResTemp.DpiResolution, (float)pdfResTemp.DpiResolution); //24-07-2012
                        }

                        Graphics g = Graphics.FromImage((Image)b);
                        g.InterpolationMode = InterpolationMode.Default;

                        //20-07-2012
                        //g.DrawImage(photoImg, 0, 0, thisPage.Width, thisPage.Height);
                        if (pdfResTemp.DpiResolution.Equals(300))
                            g.DrawImage(photoImg, 0, 0, thisPage.Width, thisPage.Height);
                        else
                        {
                            if (thisPage.Width > thisPage.Height)
                                g.DrawImage(photoImg, 0, 0, pdfResTemp.Height, pdfResTemp.Width);
                            else
                                g.DrawImage(photoImg, 0, 0, pdfResTemp.Width, pdfResTemp.Height);
                        }


                        g.Dispose();
                        thisPage.Dispose();
                        Image smallerTifImage = (Image)b;

                        //converter novamente o .tif para uma profundidade de cor de 1 bpp
                        ImageCodecInfo myImageCodecInfo;
                        System.Drawing.Imaging.Encoder myEncoder;
                        EncoderParameter myEncoderParameter;
                        EncoderParameters myEncoderParameters;
                        Bitmap lastTifToSave = new Bitmap(smallerTifImage);
                        lastTifToSave.SetResolution((float)pdfResTemp.DpiResolution, (float)pdfResTemp.DpiResolution); //24-07-2012
                        smallerTifImage.Dispose();
                        myImageCodecInfo = GetEncoderInfo("image/tiff");
                        myEncoder = System.Drawing.Imaging.Encoder.ColorDepth;
                        myEncoderParameters = new EncoderParameters(1);
                        myEncoderParameter = new EncoderParameter(myEncoder, 1L);
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        #region Regedit | comentado por Tiago Borges a 30/06/2016 alteração para BD
                        //bool is64Bit = System.Environment.Is64BitOperatingSystem;
                        //Microsoft.Win32.RegistryKey regKey2 = null;
                        //if (!is64Bit)
                        //    regKey2 = Microsoft.Win32.Registry.LocalMachine;
                        //else
                        //    regKey2 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);

                        //Microsoft.Win32.RegistryKey ebcHive2 = regKey2.OpenSubKey(@"SOFTWARE\PI Portugal Informatico\FilePaths");
                        //string imagesPath = ebcHive2.GetValue("Processar").ToString();
                        #endregion
                        string imagesPath = Helpers.GetConfigFromDataBase("Processar").ToString();
                        string temporaryFolder = imagesPath + @"TempAnexos\";
                        lastTifToSave.Save(temporaryFolder + nomeTif + "temporaryAttachTifPage_" + count.ToString() + ".tif"); //11-09-2012
                        lastTifToSave.Dispose();

                        myEncoderParameter.Dispose();
                        myEncoderParameters.Dispose();
                        count++;
                    }
                    photoImg.Dispose();
                }
                catch (Exception ex)
                {
                    LogMessageToFile("ERRO: ao fazer o resize ao tif (resizeAndSaveTemporaryTifPages)", mainLogPath);
                    LogMessageToFile(ex.Message, mainLogPath);
                    LogMessageToFile(ex.StackTrace, mainLogPath);
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                try
                {
                    //carregar o .tif original
                    Image photoImg = Image.FromFile(imgSource);
                    int pages = photoImg.GetFrameCount(FrameDimension.Page);

                    for (int i = 0; i < pages; i++)
                    {
                        Image thisPage = getTiffImage(photoImg, i);
                        //reduzir o tamanho do .tif original para metada - fica com uma profundidade de cor 24 bpp

                        //20-07-2012
                        Bitmap b = null;
                        if (pdfResTemp.DpiResolution.Equals(300))
                        {
                            b = new Bitmap(thisPage.Width, thisPage.Height);
                            b.SetResolution((float)pdfResTemp.DpiResolution, (float)pdfResTemp.DpiResolution); //24-07-2012
                        }
                        else
                        {
                            if (thisPage.Width > thisPage.Height)
                                b = new Bitmap(pdfResTemp.Height, pdfResTemp.Width);
                            else
                                b = new Bitmap(pdfResTemp.Width, pdfResTemp.Height);

                            b.SetResolution((float)pdfResTemp.DpiResolution, (float)pdfResTemp.DpiResolution); //24-07-2012
                        }

                        Graphics g = Graphics.FromImage((Image)b);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        //20-07-2012
                        if (pdfResTemp.DpiResolution.Equals(300))
                            g.DrawImage(photoImg, 0, 0, thisPage.Width, thisPage.Height);
                        else
                        {
                            if (thisPage.Width > thisPage.Height)
                                g.DrawImage(photoImg, 0, 0, pdfResTemp.Height, pdfResTemp.Width);
                            else
                                g.DrawImage(photoImg, 0, 0, pdfResTemp.Width, pdfResTemp.Height);
                        }

                        g.Dispose();
                        thisPage.Dispose();
                        Image smallerTifImage = (Image)b;

                        //converter novamente o .tif para uma profundidade de cor de 1 bpp
                        ImageCodecInfo myImageCodecInfo;
                        System.Drawing.Imaging.Encoder myEncoder;
                        EncoderParameter myEncoderParameter;
                        EncoderParameters myEncoderParameters;
                        Bitmap lastTifToSave = new Bitmap(smallerTifImage);
                        lastTifToSave.SetResolution((float)pdfResTemp.DpiResolution, (float)pdfResTemp.DpiResolution); //24-07-2012
                        smallerTifImage.Dispose();
                        myImageCodecInfo = GetEncoderInfo("image/tiff");
                        myEncoder = System.Drawing.Imaging.Encoder.ColorDepth;
                        myEncoderParameters = new EncoderParameters(1);
                        myEncoderParameter = new EncoderParameter(myEncoder, 1L);
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        #region Regedit | comentado por Tiago Borges a 30/06/2016 alteração para BD
                        //bool is64Bit = System.Environment.Is64BitOperatingSystem;
                        //Microsoft.Win32.RegistryKey regKey2 = null;
                        //if (!is64Bit)
                        //    regKey2 = Microsoft.Win32.Registry.LocalMachine;
                        //else
                        //    regKey2 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                        //Microsoft.Win32.RegistryKey ebcHive2 = regKey2.OpenSubKey(@"SOFTWARE\PI Portugal Informatico\FilePaths");
                        //string imagesPath = ebcHive2.GetValue("Processar").ToString();
                        #endregion
                        string imagesPath = Helpers.GetConfigFromDataBase("Processar").ToString();
                        string temporaryFolder = imagesPath + @"TempAnexos\";
                        lastTifToSave.Save(temporaryFolder + nomeTif + "temporaryAttachTifPage_" + count.ToString() + ".tif", myImageCodecInfo, myEncoderParameters); //11-09-2012
                        lastTifToSave.Dispose();

                        myEncoderParameter.Dispose();
                        myEncoderParameters.Dispose();
                        count++;
                    }
                    photoImg.Dispose();
                }
                catch (Exception ex)
                {
                    LogMessageToFile("ERRO: ao fazer o resize ao tif (resizeAndSaveTemporaryTifPages)", mainLogPath);
                    LogMessageToFile(ex.Message, mainLogPath);
                    LogMessageToFile(ex.StackTrace, mainLogPath);
                    throw new Exception(ex.Message);
                }
            }

            return count;
        }

        //obter a pagina 'pageNumber' de um Tiff
        public Image getTiffImage(Image sourceImage, int pageNumber)
        {
            MemoryStream ms = null;
            Image returnImage = null;

            try
            {
                ms = new MemoryStream();
                Guid objGuid = sourceImage.FrameDimensionsList[0];
                FrameDimension objDimension = new FrameDimension(objGuid);
                sourceImage.SelectActiveFrame(objDimension, pageNumber);
                sourceImage.Save(ms, ImageFormat.Tiff);

                returnImage = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                returnImage = null;
            }
            return returnImage;
        }

        //usada na anterior
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}