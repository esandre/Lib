using System;
using System.Collections.Generic;
using Lib.SQL.Adapter.Session;

namespace Lib.SQL.Adapter
{
    public abstract class DbAdapter : IDisposable
    {
        private readonly IConnection _connection;

        protected DbAdapter(IConnection connection)
        {
            _connection = connection;
            connection.Open();
            connection.Close();
        }

        protected virtual void Open()
        {
            _connection.Open();
        }

        protected virtual void Close()
        {
            _connection.Close();
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

        public long LastInsertedId { get; private set; }

        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return OpenCloseReturnSomething(() =>
            {
                var output = _connection.Execute(sql, parameters);
                if (sql.Contains("INSERT")) LastInsertedId = _connection.LastInsertedId;
                return output;
            });
        }

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return OpenCloseReturnSomething(() => _connection.FetchValue(sql, parameters));
        }

        public IDictionary<string, object> FetchLine(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return OpenCloseReturnSomething(() => _connection.FetchLine(sql, parameters));
        }

        public IEnumerable<IDictionary<string, object>> FetchLines(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return OpenCloseReturnSomething(() => _connection.FetchLines(sql, parameters));
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
