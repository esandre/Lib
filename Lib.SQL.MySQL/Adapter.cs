using System;
using System.IO;
using Lib.SQL.Adapter;
using Lib.SQL.Adapter.Session;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL
{
    public class Adapter : TransactionalDbAdapter
    {
        private Adapter(IConnection connection) : base(connection)
        {
        }

        public static Adapter CreateFromScriptFile(MySqlConnectionStringBuilder connectionString, string scriptUrl, bool eraseIfExists = false)
        {
            var uri = new Uri(scriptUrl);
            return CreateFromPlainScript(connectionString, File.ReadAllText(uri.LocalPath), eraseIfExists);
        }

        public static Adapter CreateFromPlainScript(MySqlConnectionStringBuilder connectionString, string script, bool eraseIfExists = false)
        {
            return Create(connectionString, script, eraseIfExists);
        }

        private static MySqlConnection RootConnection(MySqlConnectionStringBuilder connectionString)
        {
            var rootConnectionString = new MySqlConnectionStringBuilder(connectionString.ConnectionString) { Database = string.Empty };
            return new MySqlConnection(rootConnectionString.ConnectionString);
        }

        public static bool DbExists(MySqlConnectionStringBuilder connectionString)
        {
            using var conn = RootConnection(connectionString);
            using var command = conn.CreateCommand();

            command.CommandText =
                $"SHOW DATABASES LIKE '{connectionString.Database}'";

            conn.Open();
            return command.ExecuteScalar() != null;
        }

        public static void DeleteDb(MySqlConnectionStringBuilder connectionString)
        {
            using var conn = RootConnection(connectionString);
            using var command = conn.CreateCommand();
            command.CommandText =
                $"DROP DATABASE IF EXISTS {connectionString.Database}";

            conn.Open();
            command.ExecuteNonQuery();
        }

        private static void CreateDb(MySqlConnectionStringBuilder connectionString)
        {
            using var conn = RootConnection(connectionString);
            using var command = conn.CreateCommand();

            conn.Open();
            command.CommandText =
                $"CREATE DATABASE IF NOT EXISTS {connectionString.Database}; USE {connectionString.Database}; ";
            command.ExecuteNonQuery();
        }
        
        private static Adapter Create(MySqlConnectionStringBuilder connectionString, string script, bool eraseIfExists = false)
        {
            if (DbExists(connectionString) && !eraseIfExists)
                return Open(connectionString); 

            DeleteDb(connectionString);
            CreateDb(connectionString);

            var adapter = Open(connectionString);
            if (!string.IsNullOrEmpty(script)) adapter.Execute(script);
            return adapter;
        }

        public static Adapter Open(MySqlConnectionStringBuilder connectionString, bool threadSafe = false)
        {
            var connection = new Connection(connectionString);
            return new Adapter(threadSafe ? new ThreadSafeConnection(connection) : (IConnection)connection);
        }
    }
}
