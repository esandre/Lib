using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public interface IAsyncConnection : IAsyncSession, IAsyncDisposable, IDisposable
    {
        Task<long> LastInsertedIdAsync();
        Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
