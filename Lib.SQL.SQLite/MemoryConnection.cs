using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    internal class AsyncMemoryConnection : AsyncConnection
    {
        public AsyncMemoryConnection(SqliteConnectionStringBuilder connectionStringBuilder) 
            : base(connectionStringBuilder)
        {
        }

        public override Task CloseAsync() => Task.CompletedTask;

        public override async ValueTask DisposeAsync()
        {
            await base.CloseAsync();
        }
    }

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
