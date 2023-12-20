using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.MySQL
{
    internal class SemaphoreOnUsageConnection : IAsyncConnection
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private readonly IAsyncConnection _connection;

        public SemaphoreOnUsageConnection(IAsyncConnection connection)
        {
            _connection = connection;
        }

        public async Task<IAsyncSession> BeginTransactionAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                return await _connection.BeginTransactionAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task CommitAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                await _connection.CommitAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task RollbackAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                await _connection.RollbackAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public Task OpenAsync()
        {
            return _connection.OpenAsync();
        }

        public Task CloseAsync()
        {
            return _connection.CloseAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _connection.DisposeAsync();
        }

        public async Task<long> LastInsertedIdAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                return await _connection.LastInsertedIdAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await _semaphore.WaitAsync();

            try
            {
                return await _connection.ExecuteAsync(sql, parameters);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await _semaphore.WaitAsync();

            try
            {
                return await _connection.FetchValueAsync(sql, parameters);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await _semaphore.WaitAsync();

            try
            {
                return await _connection.FetchLineAsync(sql, parameters);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await _semaphore.WaitAsync();

            try
            {
                return await _connection.FetchLinesAsync(sql, parameters);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
