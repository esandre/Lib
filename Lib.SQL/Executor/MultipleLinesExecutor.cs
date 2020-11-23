using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class MultipleLinesExecutor : 
        IExecutor<IReadOnlyList<IReadOnlyDictionary<string, object>>>
    {
        public IReadOnlyList<IReadOnlyDictionary<string, object>> ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => adapter.FetchLines(sql, parameters);

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> ExecuteOnAdapterAsync(
            IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters)
            => await adapter.FetchLinesAsync(sql, parameters);
    }
}
