using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public abstract class AsyncConnectionAbstract : IAsyncConnection
    {
        public Task CommitAsync() => Task.CompletedTask;
        public Task RollbackAsync() => Task.CompletedTask;

        public abstract Task<IAsyncSession> BeginTransactionAsync();
        public abstract Task OpenAsync();
        public abstract Task CloseAsync();
        public abstract ValueTask DisposeAsync();
        public abstract void Dispose();
        public abstract Task<long> LastInsertedIdAsync();
        public abstract Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
