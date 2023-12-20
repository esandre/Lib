using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL
{
    public class AsyncCommandChannel : IAsyncCommandChannel, IAsyncDisposable, IDisposable
    {
        private IAsyncSession Peek { get; set; }
        private readonly IAsyncConnection _connection;

        public AsyncCommandChannel(IAsyncConnection connection)
        {
            _connection = connection;
            Peek = connection;
        }

        public async Task<IConvertible> LastInsertedIdAsync() => await OpenCloseReturnSomethingAsync(() => _connection.LastInsertedIdAsync());
        private Task CommitPeekAsync() => Peek.CommitAsync();
        private Task RollbackPeekAsync() => Peek.RollbackAsync();

        public async Task ExecuteInTransactionAsync(Func<IAsyncCommandChannel, Task<TransactionResult>> whatToDo)
        {
            await OpenAsync();
            var previousPeek = Peek;
            Peek = await Peek.BeginTransactionAsync();

            try
            {
                var result = await whatToDo(this);

                switch (result)
                {
                    case TransactionResult.Commit:
                        await CommitPeekAsync();
                        break;
                    case TransactionResult.Rollback:
                        await RollbackPeekAsync();
                        break;
                    default:
                        throw new ApplicationException("Valeur impossible en retour d'une transaction");
                }
            }
            catch (Exception)
            {
                await RollbackPeekAsync();
                throw;
            }
            finally
            {
                Peek = previousPeek;
                await CloseAsync();
            }
        }

        public Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) 
            => OpenCloseReturnSomethingAsync(() => _connection.ExecuteAsync(sql, parameters.Box()));

        public Task<IConvertible> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) 
            => OpenCloseReturnSomethingAsync(async () => (await _connection.FetchValueAsync(sql, parameters.Box())).AsConvertible());

        public Task<IReadOnlyDictionary<string, IConvertible>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) =>
            OpenCloseReturnSomethingAsync(async () => (await _connection.FetchLineAsync(sql, parameters.Box())).Unbox());

        public Task<IReadOnlyList<IReadOnlyDictionary<string, IConvertible>>> FetchLinesAsync(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) =>
            OpenCloseReturnSomethingAsync(async () => (await _connection.FetchLinesAsync(sql, parameters.Box())).Unbox());

        private async Task<T> OpenCloseReturnSomethingAsync<T>(Func<Task<T>> what)
        {
            try
            {
                await OpenAsync();
                return await what();
            }
            finally
            {
                await CloseAsync();
            }
        }

        private Task OpenAsync() => Peek.OpenAsync();
        private Task CloseAsync() => Peek.CloseAsync();

        public ValueTask DisposeAsync() => _connection.DisposeAsync();
        public void Dispose() => _connection.Dispose();
    }
}
