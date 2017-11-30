using eBillingSuite.Model.eBillingConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Support
{
    public class EbcConfiguration
    {
        public static string GetConfiguration(string configName)
        {
            string config = String.Empty;
            using (eBillingConfigurations configEntity = new eBillingConfigurations())
            {
                var installFolderConfig = configEntity.EBC_Configurations
                .FirstOrDefault(c => c.Name.ToLower() == configName.ToLower());

                config = installFolderConfig.Data;
            }
            return config;
        }

        //public static void LogMessageToFile(string msg, string path)
        //{
        //	StreamWriter sw = File.AppendText(path);
        //	try
        //	{
        //		string logLine = System.String.Format("{0:G}:" + System.DateTime.Now.Millisecond.ToString() + ": {1}.", System.DateTime.Now, msg);
        //		sw.WriteLine(logLine);
        //	}
        //	finally
        //	{
        //		sw.Close();
        //	}
        //}
    }
}
