using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
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
