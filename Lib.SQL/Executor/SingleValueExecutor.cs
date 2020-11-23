using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public sealed class SingleValueExecutor : IExecutor<object>
    {
        public object ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => adapter.FetchValue(sql, parameters);

        public async Task<object> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => await adapter.FetchValueAsync(sql, parameters);
    }
}
