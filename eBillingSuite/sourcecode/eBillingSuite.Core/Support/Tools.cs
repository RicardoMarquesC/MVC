using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Support
{
    public class Tools
    {
        public static string GetDatabaseConnectionString(string connectionName)
        {
            var accountingDataConnStrConfig = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName];
            if (accountingDataConnStrConfig != null)
            {
                string accountingDataConnStr = accountingDataConnStrConfig.ConnectionString;

                if (!String.IsNullOrWhiteSpace(accountingDataConnStr))
                    return accountingDataConnStr;
            }

            return null;
        }

        public static List<string> ManageAccountingCSV(string csvPath, IEDigitalIntancesRepository _eDigitalInstancesRepository,
            IEDigitalDocTypeRepository _eDigitalDocTypeRepository, IEDigitalSuppliersRepository _eDigitalSuppliersRepository)
        {
            List<string> errors = new List<string>();

            try
            {
                List<AccountingDataDB> accountingDataToDB = new List<AccountingDataDB>();

                using (TextReader textReader = File.OpenText(csvPath))
                {
                    var csv = new CsvHelper.CsvReader(textReader);
                    csv.Configuration.Delimiter = ";";

                    var records = csv.GetRecords<AccountingDataCsv>().ToList();

                    if (records.Count == 0)
                        throw new Exception("O CSV não contém linhas");

                    int instanceID = -1;
                    Guid tipoDocumentoPKID = Guid.Empty;

                    int i = 0;
                    foreach (AccountingDataCsv line in records)
                    {
                        AccountingDataDB accountingDataDB = new AccountingDataDB(line);

                        if (i == 0)
                        {
                            // ver se a entidade (instancia) existe
                            var instance = _eDigitalInstancesRepository.Set.FirstOrDefault(x => x.VatNumber == line.Entidade);
                            if (instance == null)
                                errors.Add("A entidade com o NIF '" + line.Entidade + "' não existe.");
                            else
                                instanceID = instance.id;

                            // ver se o tipo de documento existe
                            var documentType = _eDigitalDocTypeRepository.Set.FirstOrDefault(x => x.FinalXmlDescription.ToLower() == line.TipoDoc.ToLower());
                            if (documentType == null)
                                errors.Add("O tipo de documento com o código '" + line.TipoDoc + "' não existe.");
                            else
                                tipoDocumentoPKID = documentType.pkid;
                        }

                        accountingDataDB.InstanciaID = instanceID;
                        accountingDataDB.TipoDocumentoPKID = tipoDocumentoPKID;

                        // ver se o fornecedor existe
                        var supplier = _eDigitalSuppliersRepository.Set.FirstOrDefault(x => x.Contribuinte == line.NIFTerceiro);
                        if (supplier == null)
                            errors.Add("O terceiro com o NIF '" + line.NIFTerceiro + "' não existe.");

                        accountingDataToDB.Add(accountingDataDB);

                        i++;
                    }
                }

                //if (errors.Count == 0)
                InsertOrUpdateAccountingData(accountingDataToDB);
            }
            catch (Exception e)
            {
                try
                {
                    File.Delete(csvPath);
                }
                catch (Exception)
                {
                }

                throw;
            }

            return errors;
        }

        private static void InsertOrUpdateAccountingData(List<AccountingDataDB> accountingDataToDB)
        {
            try
            {
                string accountingDataConnStr = Tools.GetDatabaseConnectionString("AccountingData");

                using (SqlConnection conn = new SqlConnection(accountingDataConnStr))
                {
                    string existsCombination = "SELECT ID FROM Terceiros WHERE InstanciaID = @InstanciaID and TipoDocumentoPKID = @TipoDocumentoPKID" +
                        " and ContribuinteTerceiro = @ContribuinteTerceiro";

                    string insert = "INSERT INTO Terceiros (InstanciaID, TipoDocumentoPKID, ContribuinteTerceiro, CodigoTerceiro, ContaTotalTerceiro," +
                        " ContaBaseNormal, CodIvaNormal, ContaIvaNormal, ContaBaseReduzida, CodIvaReduzida, ContaIvaReduzida, ContaBaseIntermedia, CodIvaIntermedia," +
                        " ContaIvaIntermedia, ContaIvaInterComunitaria, ContaImpostoSelo)" +
                        " VALUES (@InstanciaID, @TipoDocumentoPKID, @ContribuinteTerceiro, @CodigoTerceiro, @ContaTotalTerceiro, @ContaBaseNormal, @CodIvaNormal," +
                        " @ContaIvaNormal, @ContaBaseReduzida, @CodIvaReduzida, @ContaIvaReduzida, @ContaBaseIntermedia, @CodIvaIntermedia, @ContaIvaIntermedia," +
                        " @ContaIvaInterComunitaria, @ContaImpostoSelo)";

                    string update = "UPDATE Terceiros SET CodigoTerceiro = @CodigoTerceiro, ContaTotalTerceiro = @ContaTotalTerceiro, ContaBaseNormal = @ContaBaseNormal," +
                        " CodIvaNormal = @CodIvaNormal, ContaIvaNormal = @ContaIvaNormal, ContaBaseReduzida = @ContaBaseReduzida, CodIvaReduzida = @CodIvaReduzida," +
                        " ContaIvaReduzida = @ContaIvaReduzida, ContaBaseIntermedia = @ContaBaseIntermedia, CodIvaIntermedia = @CodIvaIntermedia," +
                        " ContaIvaIntermedia = @ContaIvaIntermedia, ContaIvaInterComunitaria = @ContaIvaInterComunitaria, ContaImpostoSelo = @ContaImpostoSelo WHERE ID = @id";

                    conn.Open();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (AccountingDataDB accountingDataDB in accountingDataToDB)
                            {
                                int accountingDataID = -1;

                                using (SqlCommand command = new SqlCommand(existsCombination, conn, transaction))
                                {
                                    command.Parameters.Add("@InstanciaID", System.Data.SqlDbType.Int).Value = accountingDataDB.InstanciaID;
                                    command.Parameters.Add("@TipoDocumentoPKID", System.Data.SqlDbType.UniqueIdentifier).Value = accountingDataDB.TipoDocumentoPKID;
                                    command.Parameters.Add("@ContribuinteTerceiro", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.NIFTerceiro;

                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                            accountingDataID = reader.GetInt32(0);
                                    }
                                }

                                if (accountingDataID == -1)
                                {
                                    using (SqlCommand command = new SqlCommand(insert, conn, transaction))
                                    {
                                        command.Parameters.Add("@InstanciaID", System.Data.SqlDbType.Int).Value = accountingDataDB.InstanciaID;
                                        command.Parameters.Add("@TipoDocumentoPKID", System.Data.SqlDbType.UniqueIdentifier).Value = accountingDataDB.TipoDocumentoPKID;
                                        command.Parameters.Add("@ContribuinteTerceiro", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.NIFTerceiro;
                                        command.Parameters.Add("@CodigoTerceiro", System.Data.SqlDbType.Int).Value = accountingDataDB.CodigoTerceiro;
                                        command.Parameters.Add("@ContaTotalTerceiro", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaTerceiro;
                                        command.Parameters.Add("@ContaBaseNormal", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaBase;
                                        command.Parameters.Add("@CodIvaNormal", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.CodIVA;
                                        command.Parameters.Add("@ContaIvaNormal", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVA;
                                        command.Parameters.Add("@ContaBaseReduzida", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaBaseRed;
                                        command.Parameters.Add("@CodIvaReduzida", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.CodIVARed;
                                        command.Parameters.Add("@ContaIvaReduzida", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVARed;
                                        command.Parameters.Add("@ContaBaseIntermedia", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaBaseInt;
                                        command.Parameters.Add("@CodIvaIntermedia", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.CodIVAInt;
                                        command.Parameters.Add("@ContaIvaIntermedia", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVAInt;
                                        command.Parameters.Add("@ContaIvaInterComunitaria", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVAintraC;
                                        command.Parameters.Add("@ContaImpostoSelo", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIS;

                                        command.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    using (SqlCommand command = new SqlCommand(update, conn, transaction))
                                    {
                                        command.Parameters.Add("@CodigoTerceiro", System.Data.SqlDbType.Int).Value = accountingDataDB.CodigoTerceiro;
                                        command.Parameters.Add("@ContaTotalTerceiro", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaTerceiro;
                                        command.Parameters.Add("@ContaBaseNormal", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaBase;
                                        command.Parameters.Add("@CodIvaNormal", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.CodIVA;
                                        command.Parameters.Add("@ContaIvaNormal", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVA;
                                        command.Parameters.Add("@ContaBaseReduzida", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaBaseRed;
                                        command.Parameters.Add("@CodIvaReduzida", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.CodIVARed;
                                        command.Parameters.Add("@ContaIvaReduzida", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVARed;
                                        command.Parameters.Add("@ContaBaseIntermedia", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaBaseInt;
                                        command.Parameters.Add("@CodIvaIntermedia", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.CodIVAInt;
                                        command.Parameters.Add("@ContaIvaIntermedia", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVAInt;
                                        command.Parameters.Add("@ContaIvaInterComunitaria", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIVAintraC;
                                        command.Parameters.Add("@ContaImpostoSelo", System.Data.SqlDbType.NVarChar).Value = accountingDataDB.ContaIS;
                                        command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = accountingDataID;

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception)
                            {
                            }
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public class AccountingDataCsv
        {
            public string Entidade { get; set; }
            public string NIFTerceiro { get; set; }
            public string CodigoTerceiro { get; set; }
            public string ContaTerceiro { get; set; }
            public string TipoDoc { get; set; }
            public string ContaBase { get; set; }
            public string CodIVA { get; set; }
            public string ContaIVA { get; set; }
            public string ContaIVAintraC { get; set; }
            public string ContaIS { get; set; }
            public string ContaBaseRed { get; set; }
            public string CodIVARed { get; set; }
            public string ContaIVARed { get; set; }
            public string ContaBaseInt { get; set; }
            public string CodIVAInt { get; set; }
            public string ContaIVAInt { get; set; }
        }

        public class AccountingDataDB : AccountingDataCsv
        {
            public AccountingDataDB(AccountingDataCsv line)
            {
                this.Entidade = line.Entidade;
                this.NIFTerceiro = line.NIFTerceiro;
                this.CodigoTerceiro = line.CodigoTerceiro;
                this.ContaTerceiro = line.ContaTerceiro;
                this.TipoDoc = line.TipoDoc;
                this.ContaBase = line.ContaBase;
                this.CodIVA = line.CodIVA;
                this.ContaIVA = line.ContaIVA;
                this.ContaIVAintraC = line.ContaIVAintraC;
                this.ContaIS = line.ContaIS;
                this.ContaBaseRed = line.ContaBaseRed;
                this.CodIVARed = line.CodIVARed;
                this.ContaIVARed = line.ContaIVARed;
                this.ContaBaseInt = line.ContaBaseInt;
                this.CodIVAInt = line.CodIVAInt;
                this.ContaIVAInt = line.ContaIVAInt;
            }

            public int InstanciaID { get; set; }
            public Guid TipoDocumentoPKID { get; set; }
        }
    }
}
