using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class AffectedLinesExecutor : IExecutor<int>
    {
        public int ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => adapter.Execute(sql, parameters);

        public async Task<int> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => await adapter.ExecuteAsync(sql, parameters);
    }
}
