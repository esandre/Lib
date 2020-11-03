using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    internal class MemoryConnection : Connection
    {
        public MemoryConnection(SqliteConnectionStringBuilder connectionStringBuilder) 
            : base(connectionStringBuilder)
        {
        }

        public override void Close()
        {
        }

        public override void Dispose()
        {
            base.Close();
        }
    }
}
