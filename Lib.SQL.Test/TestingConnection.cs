using System.Collections.Generic;
using Lib.SQL.Adapter;
using Lib.SQL.Adapter.Session;

namespace Lib.SQL.Test
{
    internal class TestingAdapter : DbAdapter
    {
        public static readonly TestingAdapter Instance = new TestingAdapter();

        private TestingAdapter() : base(TestingConnection.Instance)
        {
        }
    }


    internal class TestingConnection : IConnection
    {
        public static readonly TestingConnection Instance = new TestingConnection();
        public ISession Parent { get; private set; }
        public IConnection Connection { get; private set; }

        public ITransaction BeginTransaction()
        {
            return null;
        }

        public long LastInsertedId { get; private set; }
        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            throw new System.NotImplementedException();
        }

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
        }

        public void Close()
        {
        }

        public void Dispose()
        {
        }
    }
}
