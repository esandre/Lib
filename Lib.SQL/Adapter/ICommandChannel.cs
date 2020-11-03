using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public interface ICommandChannel
    {
        IConvertible LastInsertedId { get; }

        int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);

        Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);

        object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);

        Task<object> FetchValueAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        IDictionary<string, object> FetchLine(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        Task<IDictionary<string, object>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        IEnumerable<IDictionary<string, object>> FetchLines(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        Task<IEnumerable<IDictionary<string, object>>> FetchLinesAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
