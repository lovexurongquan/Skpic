/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：connect database factory.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Skpic.Factory
{
    /// <summary>
    ///
    /// </summary>
    public static class DbContextFactory
    {
        private static DbProviderFactory _df;

        /// <summary>
        /// Get a open connection by connection string and provider in config.
        /// </summary>
        public static IDbConnection GetConnection(string connectionName = "ConnectionString")
        {
            // get connection string in config.
            var connectionString = GetConnectionString(connectionName);
            var providerName = GetProviderName(connectionName);
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
        /// <param name="connectionName">connection name in config.</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName = "ConnectionString")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        /// <summary>
        /// get provider name by connectionName in config.
        /// </summary>
        /// <param name="connectionName">connection name in config.</param>
        /// <returns></returns>
        public static string GetProviderName(string connectionName = "ConnectionString")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
        }
    }
}