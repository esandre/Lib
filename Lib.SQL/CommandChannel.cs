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

        public async Task<IConvertible> LastInsertedIdAsync() => await OpenCloseReturnSomethingAsync(async () => await _connection.LastInsertedIdAsync());
        private async Task CommitPeekAsync() => await Peek.CommitAsync();
        private async Task RollbackPeekAsync() => await Peek.RollbackAsync();

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

        public async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) 
            => await OpenCloseReturnSomethingAsync(async () => await _connection.ExecuteAsync(sql, parameters.Box()));

        public async Task<IConvertible> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) 
            => await OpenCloseReturnSomethingAsync(async () => (await _connection.FetchValueAsync(sql, parameters.Box())).AsConvertible());

        public async Task<IReadOnlyDictionary<string, IConvertible>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) =>
            await OpenCloseReturnSomethingAsync(async () => (await _connection.FetchLineAsync(sql, parameters.Box())).Unbox());

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, IConvertible>>> FetchLinesAsync(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) =>
            await OpenCloseReturnSomethingAsync(async () => (await _connection.FetchLinesAsync(sql, parameters.Box())).Unbox());

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

        private async Task OpenAsync() => await Peek.OpenAsync();
        private async Task CloseAsync() => await Peek.CloseAsync();

        public async ValueTask DisposeAsync() => await _connection.DisposeAsync();
        public void Dispose() => _connection.Dispose();
    }
}
