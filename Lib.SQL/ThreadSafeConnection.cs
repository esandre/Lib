using System.Collections.Generic;
using System.Threading;
using Lib.SQL.Adapter;

namespace Lib.SQL
{
    public class ThreadSafeConnection : IConnection
    {
        private int _openings;
        private readonly object _lock = new object();
        private readonly IConnection _connectionImplementation;

        public ThreadSafeConnection(IConnection connectionImplementation)
        {
            _connectionImplementation = connectionImplementation;
        }

        public ITransaction BeginTransaction() => _connectionImplementation.BeginTransaction();

        public void Dispose()
        {
            _connectionImplementation.Dispose();
        }

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
