using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace Skpic.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public static class TemplateFactory
    {
        private static DbProviderFactory _df;

        /// <summary>  
        /// Get a open connection by connection string and provider in config.
        /// </summary>  
        public static IDbConnection GetConnection(string templateFilePath, string connectionName = "ConnectionString")
        {
            // get connection string in config.
            var connectionString = GetConnectionString(templateFilePath,connectionName);
            var providerName = GetProviderName(templateFilePath,connectionName);
            if (connectionString == null || providerName == null)
            {
                throw new Exception();
            }

            if (_df == null)
                _df = DbProviderFactories.GetFactory(providerName);
            var connection = _df.CreateConnection();
            if (connection == null)
            {
                throw new Exception("connection provider have some problem. please check it.");
            }
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }

        /// <summary>
        /// get connection string by connectionName in config.
        /// </summary>
        /// <param name="templateFilePath">the tt template file path.</param>
        /// <param name="connectionName">connection name in config.</param>
        /// <returns></returns>
        public static string GetConnectionString(string templateFilePath,string connectionName = "ConnectionString")
        {
            string directoryName = Path.GetDirectoryName(templateFilePath);

            string str = Directory.GetFiles(directoryName, "*.config").FirstOrDefault() ??
                         Directory.GetParent(directoryName).GetFiles("*.config").FirstOrDefault().FullName;
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
            {
                ExeConfigFilename = str
            }, ConfigurationUserLevel.None);
            
            var connectionString = ((ConnectionStringsSection)configuration.GetSection("connectionStrings")).ConnectionStrings[connectionName].ConnectionString;

            return connectionString;
        }

        /// <summary>
        /// get provider name by connectionName in config.
        /// </summary>
        /// <param name="templateFilePath">the tt template file path.</param>
        /// <param name="connectionName">connection name in config.</param>
        /// <returns></returns>
        public static string GetProviderName(string templateFilePath, string connectionName = "ConnectionString")
        {
            string directoryName = Path.GetDirectoryName(templateFilePath);

            string str = Directory.GetFiles(directoryName, "*.config").FirstOrDefault() ??
                         Directory.GetParent(directoryName).GetFiles("*.config").FirstOrDefault().FullName;
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
            {
                ExeConfigFilename = str
            }, ConfigurationUserLevel.None);


            var connectionString = ((ConnectionStringsSection)configuration.GetSection("connectionStrings")).ConnectionStrings[connectionName].ProviderName;

            return connectionString;
        }

        

    }
}
