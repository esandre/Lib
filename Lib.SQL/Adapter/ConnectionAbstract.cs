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
        public abstract Task<long> LastInsertedIdAsync();
        public abstract Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }

    public abstract class ConnectionAbstract : IConnection
    {
        public abstract ISession BeginTransaction();

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public abstract void Open();
        public abstract void Close();
        public abstract void Dispose();
        public abstract long LastInsertedId { get; }
        public abstract int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract IReadOnlyDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        public abstract IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
