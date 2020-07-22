using System;
using System.Data.SQLite;
using System.IO;
using Lib.SQL.Adapter;
using Lib.SQL.Adapter.Session;

namespace Lib.SQL.SQLite
{
    public class Adapter : TransactionalDbAdapter
    {
        private Adapter(IConnection connection) : base(connection)
        {
        }

        public static Adapter CreateFromScriptFile(SQLiteConnectionStringBuilder connectionStringBuilder, string scriptUrl, bool eraseIfExists = false)
        {
            var uri = new Uri(scriptUrl);
            return CreateFromPlainScript(connectionStringBuilder, File.ReadAllText(uri.LocalPath), eraseIfExists);
        }

        public static Adapter CreateFromPlainScript(SQLiteConnectionStringBuilder connectionStringBuilder, string script, bool eraseIfExists = false)
        {
            return Create(connectionStringBuilder, script, eraseIfExists);
        }

        private static readonly object CreationLock = new object();
        private static Adapter Create(SQLiteConnectionStringBuilder connectionStringBuilder, string script, bool eraseIfExists = false)
        {
            lock (CreationLock)
            {
                if (connectionStringBuilder.DataSource != ":memory:"
                    && File.Exists(connectionStringBuilder.DataSource) 
                    && eraseIfExists)
                {
                    File.Delete(connectionStringBuilder.DataSource);
                    SQLiteConnection.CreateFile(connectionStringBuilder.DataSource);
                }

                var adapter = Open(connectionStringBuilder);
                if (!string.IsNullOrEmpty(script)) adapter.Execute(script);
                return adapter;
            }
        }

        public static Adapter Open(SQLiteConnectionStringBuilder connectionStringBuilder, bool threadSafe = false)
        {
            if(connectionStringBuilder.DataSource == ":memory:") 
                return new Adapter(new MemoryConnection(connectionStringBuilder));

            var connection = new Connection(connectionStringBuilder);
            return new Adapter(threadSafe ? new ThreadSafeConnection(connection) : (IConnection) connection);
        }
    }
}
