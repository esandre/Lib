using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public sealed class SingleValueExecutor : IExecutor<IConvertible>
    {
        public Task<IConvertible> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
            => adapter.FetchValueAsync(sql, parameters);
    }
}
