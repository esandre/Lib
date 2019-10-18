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

        public static Adapter CreateFromScriptFile(string fileUrl, string scriptUrl, bool eraseIfExists = false)
        {
            var uri = new Uri(scriptUrl);
            return CreateFromPlainScript(fileUrl, File.ReadAllText(uri.LocalPath), eraseIfExists);
        }

        public static Adapter CreateFromPlainScript(string fileUrl, string script, bool eraseIfExists = false)
        {
            return Create(fileUrl, script, eraseIfExists);
        }

        private static readonly object CreationLock = new object();
        private static Adapter Create(string fileUrl, string script, bool eraseIfExists = false)
        {
            lock (CreationLock)
            {
                if (File.Exists(fileUrl) && !eraseIfExists)
                {
                    return Open(fileUrl);
                }

                if (fileUrl != ":memory:")
                {
                    File.Delete(fileUrl);
                    SQLiteConnection.CreateFile(fileUrl);
                }

                var adapter = Open(fileUrl);
                if (!string.IsNullOrEmpty(script)) adapter.Execute(script);
                return adapter;
            }
        }

        public static Adapter Open(string fileUrl)
        {
            return new Adapter(fileUrl != ":memory:" ? new Connection(fileUrl) : new MemoryConnection());
        }
    }
}
