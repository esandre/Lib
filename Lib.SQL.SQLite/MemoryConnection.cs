using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    internal class MemoryConnection : Connection
    {
        public MemoryConnection(SqliteConnection connection)
         : base(connection)
        {
            base.Open();
        }

        public override void Open()
        {
        }

        public override void Close()
        {
        }

        public override void Dispose()
        {
            base.Close();
            base.Dispose();
        }
    }

    internal class AsyncMemoryConnection : AsyncConnection
    {
        public AsyncMemoryConnection(SqliteConnection connection)
            : base(connection)
        {
            Task.WaitAll(base.OpenAsync());
        }

        public override Task OpenAsync() => Task.CompletedTask;

        public override Task CloseAsync() => Task.CompletedTask;

        public override async ValueTask DisposeAsync()
        {
            await base.CloseAsync();
            await base.DisposeAsync();
        }
    }
}
