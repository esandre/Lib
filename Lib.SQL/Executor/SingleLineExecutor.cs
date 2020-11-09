using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class SingleLineExecutor : IExecutor<IReadOnlyDictionary<string, object>>
    {
        public IReadOnlyDictionary<string, object> ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => adapter.FetchLine(sql, parameters);

        public async Task<IReadOnlyDictionary<string, object>> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => await adapter.FetchLineAsync(sql, parameters);
    }
}
