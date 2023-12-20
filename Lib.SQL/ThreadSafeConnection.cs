using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL
{
    public class SemaphoreOnOpeningConnection : IAsyncConnection
    {
        private int _openings;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private readonly IAsyncConnection _asyncConnectionImplementation;

        public SemaphoreOnOpeningConnection(IAsyncConnection asyncConnectionImplementation)
        {
            _asyncConnectionImplementation = asyncConnectionImplementation;
        }

        public async Task OpenAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (_openings == 0) await _asyncConnectionImplementation.OpenAsync();
                Interlocked.Increment(ref _openings);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task CloseAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                Interlocked.Decrement(ref _openings);
                if (_openings == 0) await _asyncConnectionImplementation.CloseAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public Task<IAsyncSession> BeginTransactionAsync() 
            => _asyncConnectionImplementation.BeginTransactionAsync();

        public Task CommitAsync() 
            => _asyncConnectionImplementation.CommitAsync();

        public Task RollbackAsync() 
            => _asyncConnectionImplementation.RollbackAsync();

        public ValueTask DisposeAsync() 
            => _asyncConnectionImplementation.DisposeAsync();

        public Task<long> LastInsertedIdAsync() 
            => _asyncConnectionImplementation.LastInsertedIdAsync();

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
}
