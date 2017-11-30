using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace eBillingSuite.Helper
{
    public class DatabaseConnection
    {
        internal static string GetConnString(string nameDataBase)
        {
            string connection = "";
            ConnectionStringSettingsCollection connSetCol = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings conn in connSetCol)
            {
                if (conn.Name.Equals(nameDataBase))
                    connection = conn.ConnectionString.ToString();
            }

            // decode connection string

            connection = Encryption.Base64Decode(connection);


            return connection;
        }
    }
}