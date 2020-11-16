using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public interface IConnection : ISession, IDisposable
    {
        long LastInsertedId { get; }
        int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        IReadOnlyDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }

    public interface IAsyncConnection : IAsyncSession, IAsyncDisposable, IDisposable
    {
        Task<long> LastInsertedIdAsync();
        Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
