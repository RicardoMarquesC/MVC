using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBillingSuite.Model;
using eBillingSuite.Repositories;
using Ninject;
using eBillingSuite.Model.Desmaterializacao;
using System.Data.SqlClient;
using System.Data.Common;

namespace eBillingSuite.Integrations
{
    public class AccountingData : IAccountingData
    {
        private const string insertSeqNumbersQuery = "INSERT INTO CompanySequentialNumbers (InstanciaID, Year, NumberControl) VALUES (@instanceID, @year, @json)";

        private const string emptyJsonSequentialNumbers = "{\"Janeiro\":1,\"Fevereiro\":1,\"Marco\":1,\"Abril\":1,\"Maio\":1,\"Junho\":1,\"Julho\":1,\"Agosto\":1,\"Setembro\":1,\"Outubro\":1,\"Novembro\":1,\"Dezembro\":1}";

        public void InsertInstanceEmptySequentialNumbers(int instanceCode, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // inserimos desde o ano anterior (pq pode haver faturas por tratar do ano anterior) e mais 4 anos para a frente
                            for (int year = DateTime.Now.Year - 1; year < DateTime.Now.Year + 5; year++)
                            {
                                using (SqlCommand command = new SqlCommand(insertSeqNumbersQuery, conn, transaction))
                                {
                                    command.Parameters.Add("@instanceID", System.Data.SqlDbType.Int).Value = instanceCode;
                                    command.Parameters.Add("@year", System.Data.SqlDbType.NVarChar).Value = year.ToString();
                                    command.Parameters.Add("@json", System.Data.SqlDbType.NVarChar).Value = emptyJsonSequentialNumbers;

                                    command.ExecuteNonQuery();
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
            catch (Exception e)
            {
                string msg = e.Message;

                throw;
            }
        }
    }
}
