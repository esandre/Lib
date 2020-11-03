using System;
using System.IO;
using Lib.SQL.Adapter;
using Lib.SQL.Adapter.Session;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    public class Adapter : TransactionalDbAdapter
    {
        private Adapter(IConnection connection) : base(connection)
        {
        }

        public static Adapter CreateFromScriptFile(SqliteConnectionStringBuilder connectionStringBuilder, string scriptUrl, bool eraseIfExists = false)
        {
            var uri = new Uri(scriptUrl);
            return CreateFromPlainScript(connectionStringBuilder, File.ReadAllText(uri.LocalPath), eraseIfExists);
        }

        public static Adapter CreateFromPlainScript(SqliteConnectionStringBuilder connectionStringBuilder, string script, bool eraseIfExists = false)
        {
            return Create(connectionStringBuilder, script, eraseIfExists);
        }

        private static readonly object CreationLock = new object();
        private static Adapter Create(SqliteConnectionStringBuilder connectionStringBuilder, string script, bool eraseIfExists = false)
        {
            lock (CreationLock)
            {
                if (connectionStringBuilder.DataSource != ":memory:"
                    && File.Exists(connectionStringBuilder.DataSource) 
                    && eraseIfExists)
                {
                    File.Delete(connectionStringBuilder.DataSource);

                    var derivedConnectionString = new SqliteConnectionStringBuilder(connectionStringBuilder.ToString()) 
                        { Mode = SqliteOpenMode.ReadWriteCreate };
                    new SqliteConnection(derivedConnectionString.ToString()).Open();
                }

                var adapter = Open(connectionStringBuilder);
                if (!string.IsNullOrEmpty(script)) adapter.Execute(script);
                return adapter;
            }
        }

        public static Adapter Open(SqliteConnectionStringBuilder connectionStringBuilder, bool threadSafe = false)
        {
            if(connectionStringBuilder.DataSource == ":memory:") 
                return new Adapter(new MemoryConnection(connectionStringBuilder));

            var connection = new Connection(connectionStringBuilder);
            return new Adapter(threadSafe ? new ThreadSafeConnection(connection) : (IConnection) connection);
        }
    }
}
