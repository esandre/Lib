using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public interface ICommandChannel
    {
        IConvertible LastInsertedId { get; }

        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);

        public Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);

        public Task<object> FetchValueAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        public IDictionary<string, object> FetchLine(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        public Task<IDictionary<string, object>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        public IEnumerable<IDictionary<string, object>> FetchLines(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);

        public Task<IEnumerable<IDictionary<string, object>>> FetchLinesAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
