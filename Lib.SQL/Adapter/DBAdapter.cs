using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter.Session;

namespace Lib.SQL.Adapter
{
    public abstract class DbAdapter : ICommandChannel, IDisposable
    {
        private readonly IConnection _connection;

        protected DbAdapter(IConnection connection)
        {
            _connection = connection;
            connection.Open();
            connection.Close();
        }

        protected virtual void Open() => _connection.Open();
        protected virtual void Close() => _connection?.Close();

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

        public IDictionary<string, object> FetchLine(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            OpenCloseReturnSomething(() => _connection.FetchLine(sql, parameters));

        public async Task<IDictionary<string, object>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            await OpenCloseReturnSomethingAsync(async () => await Task.Run(() => _connection.FetchLine(sql, parameters)));

        public IEnumerable<IDictionary<string, object>> FetchLines(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            OpenCloseReturnSomething(() => _connection.FetchLines(sql, parameters));

        public async Task<IEnumerable<IDictionary<string, object>>> FetchLinesAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null) =>
            await OpenCloseReturnSomethingAsync(async () => await Task.Run(() => _connection.FetchLines(sql, parameters)));

        public void Dispose() => _connection.Dispose();
    }
}
