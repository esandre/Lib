using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public class AffectedLinesExecutor : ExecutorAbstract<int>
    {
        protected override int ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => adapter.Execute(sql, parameters);

        protected override async Task<int> ExecuteOnAdapterAsync(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => await adapter.ExecuteAsync(sql, parameters);
    }
}
