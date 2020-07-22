using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public sealed class SingleValueExecutor : ExecutorAbstract<object>
    {
        protected override object ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => adapter.FetchValue(sql, parameters);

        protected override async Task<object> ExecuteOnAdapterAsync(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => await adapter.FetchValueAsync(sql, parameters);
    }
}
