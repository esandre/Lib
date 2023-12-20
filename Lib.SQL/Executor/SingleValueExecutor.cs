using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public sealed class SingleValueExecutor : IExecutor<IConvertible>
    {
        public async Task<IConvertible> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
            => await adapter.FetchValueAsync(sql, parameters);
    }
}
