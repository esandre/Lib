using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class SingleLineExecutor : IExecutor<IReadOnlyDictionary<string, IConvertible>>
    {
        public Task<IReadOnlyDictionary<string, IConvertible>> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
            => adapter.FetchLineAsync(sql, parameters);
    }
}
