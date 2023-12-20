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

        public async Task<IAsyncSession> BeginTransactionAsync() 
            => await _asyncConnectionImplementation.BeginTransactionAsync();

        public async Task CommitAsync() 
            => await _asyncConnectionImplementation.CommitAsync();

        public async Task RollbackAsync() 
            => await _asyncConnectionImplementation.RollbackAsync();

        public async ValueTask DisposeAsync() 
            => await _asyncConnectionImplementation.DisposeAsync();

        public async Task<long> LastInsertedIdAsync() 
            => await _asyncConnectionImplementation.LastInsertedIdAsync();

        public async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => await _asyncConnectionImplementation.ExecuteAsync(sql, parameters);

        public async Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => await _asyncConnectionImplementation.FetchValueAsync(sql, parameters);

        public async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => await _asyncConnectionImplementation.FetchLineAsync(sql, parameters);

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => await _asyncConnectionImplementation.FetchLinesAsync(sql, parameters);

        public void Dispose() => _asyncConnectionImplementation?.Dispose();
    }
}
