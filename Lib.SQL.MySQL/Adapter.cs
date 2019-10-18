using System;
using System.IO;
using Lib.SQL.Adapter;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL
{
    public class Adapter : TransactionalDbAdapter
    {
        public Adapter(MySQLCredentials creds, string dbName) : base(new Connection(creds.Server, dbName, creds.Username, creds.Password, creds.Port))
        {
        }

        public static Adapter CreateFromScriptFile(MySQLCredentials creds, string dbName, string scriptUrl, bool eraseIfExists = false)
        {
            var uri = new Uri(scriptUrl);
            return CreateFromPlainScript(creds, dbName, File.ReadAllText(uri.LocalPath), eraseIfExists);
        }

        public static Adapter CreateFromPlainScript(MySQLCredentials creds, string dbName, string script, bool eraseIfExists = false)
        {
            return Create(creds, dbName, script, eraseIfExists);
        }

        private static MySqlConnection RootConnection(MySQLCredentials creds)
        {
            string connectionString = $"Server={creds.Server};Uid={creds.Username};Pwd={creds.Password};Port={creds.Port}";
            var conn = new MySqlConnection(connectionString);
            return conn;
        }

        public static bool DbExists(MySQLCredentials creds, string dbName)
        {
            using (var conn = RootConnection(creds))
            using (var command = conn.CreateCommand())
            {
                command.CommandText =
                    $"SHOW DATABASES LIKE '{dbName}'";

                conn.Open();
                return command.ExecuteScalar() != null;
            }
        }

        public static void DeleteDb(MySQLCredentials creds, string dbName)
        {
            using (var conn = RootConnection(creds))
            using (var command = conn.CreateCommand())
            {
                command.CommandText =
                    $"DROP DATABASE IF EXISTS {dbName}";

                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        private static void CreateDb(MySQLCredentials creds, string dbName)
        {
            using (var conn = RootConnection(creds))
            using (var command = conn.CreateCommand())
            {
                conn.Open();
                command.CommandText =
                    $"CREATE DATABASE IF NOT EXISTS {dbName}; USE {dbName}; ";
                command.ExecuteNonQuery();
            }
        }
        
        private static Adapter Create(MySQLCredentials creds, string dbName, string script, bool eraseIfExists = false)
        {
            if (DbExists(creds, dbName) && !eraseIfExists) return new Adapter(creds, dbName); 

            DeleteDb(creds, dbName);
            CreateDb(creds, dbName);

            var adapter = new Adapter(creds, dbName);
            if (!string.IsNullOrEmpty(script)) adapter.Execute(script);
            return adapter;
        }
    }
}
