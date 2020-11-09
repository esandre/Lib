using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL
{
    public class TransactionalDbAdapter : ICommandChannel, IAsyncCommandChannel, IDisposable
    {
        private readonly IConnection _connection;

        private ISession Peek { get; set; }

        public TransactionalDbAdapter(IConnection connection)
        {
            connection.Open();
            connection.Close();
            Peek = connection;
            _connection = connection;
        }

        private void Open()
        {
            var connection = Peek as IConnection;
            connection?.Open();
        }

        private void Close()
        {
            var connection = Peek as IConnection;
            connection?.Close();
        }

        private void CommitPeek() => ((ITransaction)Peek).Commit();
        private void RollbackPeek() => ((ITransaction)Peek).Rollback();

        public async Task ExecuteInTransactionAsync(Func<ICommandChannel, Task<TransactionResult>> whatToDo)
        {
            await Task.Run(Open);
            var previousPeek = Peek;
            Peek = Peek.BeginTransaction();

            try
            {
                var result = await whatToDo(this);

                switch (result)
                {
                    case TransactionResult.Commit:
                        CommitPeek();
                        break;
                    case TransactionResult.Rollback:
                        RollbackPeek();
                        break;
                    default:
                        throw new ApplicationException("Valeur impossible en retour d'une transaction");
                }
            }
            catch (Exception)
            {
                await Task.Run(RollbackPeek);
                throw;
            }
            finally
            {
                Peek = previousPeek;
                await Task.Run(Close);
            }
        }

        public void ExecuteInTransaction(Func<ICommandChannel, TransactionResult> whatToDo)
        {
            Open();
            var previousPeek = Peek;
            Peek = Peek.BeginTransaction();

            try
            {
                var result = whatToDo.Invoke(this);

                switch (result)
                {
                    case TransactionResult.Commit:
                        CommitPeek();
                        break;
                    case TransactionResult.Rollback:
                        RollbackPeek();
                        break;
                    default:
                        throw new ApplicationException("Valeur impossible en retour d'une transaction");
                }
            }
            catch (Exception)
            {
                RollbackPeek();
                throw;
            }
            finally
            {
                Peek = previousPeek;
                Close();
            }
        }

        private T OpenCloseReturnSomething<T>(Func<T> what)
        {
            try
            {
                Open();
                return what.Invoke();
            }
            finally
            {
                Close();
            }
        }

        private async Task<T> OpenCloseReturnSomethingAsync<T>(Func<Task<T>> what)
        {
            try
            {
                await Task.Run(Open);
                return await what();
            }
            finally
            {
                await Task.Run(Close);
            }
        }

        public IConvertible LastInsertedId { get; private set; } = 0;
        public Task<IConvertible> LastInsertedIdAsync() => Task.FromResult((IConvertible) 0);

        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            OpenCloseReturnSomething(() =>
            {
                var output = _connection.Execute(sql, parameters);
                if (sql.Contains("INSERT")) LastInsertedId = _connection.LastInsertedId;
                return output;
            });

        public async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            await OpenCloseReturnSomethingAsync(async () =>
            {
                var output = await Task.Run(() => _connection.Execute(sql, parameters));
                if (sql.Contains("INSERT")) LastInsertedId = _connection.LastInsertedId;
                return output;
            });

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => OpenCloseReturnSomething(() => _connection.FetchValue(sql, parameters));

        public async Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => await OpenCloseReturnSomethingAsync(async () => await Task.Run(() => _connection.FetchValue(sql, parameters)));

        public IReadOnlyDictionary<string, object> FetchLine(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            OpenCloseReturnSomething(() => _connection.FetchLine(sql, parameters));

        public async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            await OpenCloseReturnSomethingAsync(async () => await Task.Run(() => _connection.FetchLine(sql, parameters)));

        public IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            OpenCloseReturnSomething(() => _connection.FetchLines(sql, parameters));

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            await OpenCloseReturnSomethingAsync(async () => await Task.Run(() => _connection.FetchLines(sql, parameters)));

        public void Dispose() => _connection.Dispose();
    }
}
