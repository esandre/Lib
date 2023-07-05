using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL.Test
{
    public class TestAbstract
    {
        private const string TestServerKey = "MYSQL_TEST_SERVER";
        private const string TestPortKey = "MYSQL_TEST_PORT";
        private const string TestUserKey = "MYSQL_TEST_USER";
        private const string TestPasswordKey = "MYSQL_TEST_PASSWORD";
        private const string TestDatabaseKey = "MYSQL_TEST_DATABASE";

        [TestInitialize]
        public void Initialize()
        {
            var mysqlServer = Environment.GetEnvironmentVariable(TestServerKey) 
                              ?? throw new ConfigurationErrorsException(TestServerKey + " is not set in ENV vars");

            var mysqlPortVar = Environment.GetEnvironmentVariable(TestPortKey);
            var mysqlPort = mysqlPortVar is null ? 3306 : uint.Parse(mysqlPortVar);

            var mysqlUser = Environment.GetEnvironmentVariable(TestUserKey) 
                            ?? throw new ConfigurationErrorsException(TestUserKey + " is not set in ENV vars");


            var mysqlPassword = Environment.GetEnvironmentVariable(TestPasswordKey) ?? string.Empty;

            var mysqlDatabase = Environment.GetEnvironmentVariable(TestDatabaseKey) ?? Guid.NewGuid().ToString("N");

            var connectionString = new MySqlConnectionStringBuilder
            {
                Password = mysqlPassword, 
                UserID = mysqlUser, 
                Server = mysqlServer, 
                Port = mysqlPort, 
                Database = mysqlDatabase
            };

            Debug.WriteLine(connectionString.ToString());

            Credentials = connectionString;
        }

        [TestCleanup]
        public void CleanDb()
        {
            try
            {
                new MySQLCommandChannelFactory().Delete(Credentials);
            } catch
            {
                // ignored
            }
        }

        protected MySqlConnectionStringBuilder Credentials { get; private set; }
    }
}
