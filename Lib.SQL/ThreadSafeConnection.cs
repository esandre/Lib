using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL
{
    public class AsyncThreadSafeConnection : IAsyncConnection
    {
        private int _openings;
        private readonly object _lock = new object();
        private readonly IAsyncConnection _asyncConnectionImplementation;

        public AsyncThreadSafeConnection(IAsyncConnection asyncConnectionImplementation)
        {
            _asyncConnectionImplementation = asyncConnectionImplementation;
        }

        public async Task OpenAsync()
        {
            Monitor.Enter(_lock);

            try
            {
                if (_openings == 0) await _asyncConnectionImplementation.OpenAsync();
                Interlocked.Increment(ref _openings);
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        public async Task CloseAsync()
        {
            Monitor.Enter(_lock);

            try
            {
                Interlocked.Decrement(ref _openings);
                if (_openings == 0) await _asyncConnectionImplementation.CloseAsync();
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        public Task<IAsyncSession> BeginTransactionAsync() => _asyncConnectionImplementation.BeginTransactionAsync();
        public Task CommitAsync() => _asyncConnectionImplementation.CommitAsync();
        public Task RollbackAsync() => _asyncConnectionImplementation.RollbackAsync();
        public ValueTask DisposeAsync() => _asyncConnectionImplementation.DisposeAsync();
        public Task<long> LastInsertedIdAsync() => _asyncConnectionImplementation.LastInsertedIdAsync();
        public Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _asyncConnectionImplementation.ExecuteAsync(sql, parameters);
        public Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _asyncConnectionImplementation.FetchValueAsync(sql, parameters);
        public Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _asyncConnectionImplementation.FetchLineAsync(sql, parameters);
        public Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _asyncConnectionImplementation.FetchLinesAsync(sql, parameters);
        public void Dispose() => _asyncConnectionImplementation?.Dispose();
    }

    public class ThreadSafeConnection : IConnection
    {
        private int _openings;
        private readonly object _lock = new object();
        private readonly IConnection _connectionImplementation;

        public ThreadSafeConnection(IConnection connectionImplementation)
        {
            _connectionImplementation = connectionImplementation;
        }

        public ISession BeginTransaction() => _connectionImplementation.BeginTransaction();
        public void Commit() => _connectionImplementation.Commit();
        public void Rollback() => _connectionImplementation.Rollback();
        public void Dispose() => _connectionImplementation.Dispose();

        public void Open()
        {
            lock (_lock)
            {
                if (_openings == 0) _connectionImplementation.Open();
                Interlocked.Increment(ref _openings);
            }
        }

        public void Close()
        {
            lock (_lock)
            {
                Interlocked.Decrement(ref _openings);
                if (_openings == 0) _connectionImplementation.Close();
            }
        }

        public long LastInsertedId 
            => _connectionImplementation.LastInsertedId;

        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _connectionImplementation.Execute(sql, parameters);

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _connectionImplementation.FetchValue(sql, parameters);

        public IReadOnlyDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _connectionImplementation.FetchLine(sql, parameters);

        public IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => _connectionImplementation.FetchLines(sql, parameters);
    }
}
