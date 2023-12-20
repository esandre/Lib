using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class AffectedLinesExecutor : IExecutor<int>
    {
        public Task<int> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) 
            => adapter.ExecuteAsync(sql, parameters);
    }
}
