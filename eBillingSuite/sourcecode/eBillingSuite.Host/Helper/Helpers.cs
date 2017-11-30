using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eBillingSuite.Helper
{
    public class Helpers
    {
        public static string RemoveSpecialCharsForFilename(string input, string replaceChar)
        {
            string output = input
                .Replace("\\", replaceChar)
                .Replace("/", replaceChar)
                .Replace(":", replaceChar)
                .Replace("*", replaceChar)
                .Replace("?", replaceChar)
                .Replace("\"", replaceChar)
                .Replace("<", replaceChar)
                .Replace(">", replaceChar)
                .Replace("|", replaceChar);

            return output;
        }

        public static string GetConfigFromDataBase(string configName)
        {
            string path = "";

            using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnString("eBillingConfigurations")))
            {
                string q = "SELECT Dados FROM DigitalConfigurations WHERE Nome=@configName";

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    cmd.Parameters.Add("@configName", SqlDbType.NVarChar).Value = configName;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            path = reader.GetString(0);
                        }
                    }
                }
            }

            return path;
        }
    }
}