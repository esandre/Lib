using System;
using System.Collections.Generic;

namespace Lib.SQL.Adapter.Session
{
    public interface IConnection : ISession, IDisposable
    {
        void Open();
        void Close();
        long LastInsertedId { get; }

        int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        IDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        IEnumerable<IDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
