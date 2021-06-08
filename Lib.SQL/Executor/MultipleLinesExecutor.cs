using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class MultipleLinesExecutor : 
        IExecutor<IReadOnlyList<IReadOnlyDictionary<string, IConvertible>>>
    {
        public IReadOnlyList<IReadOnlyDictionary<string, IConvertible>> ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) 
            => adapter.FetchLines(sql, parameters);

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, IConvertible>>> ExecuteOnAdapterAsync(
            IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
            => await adapter.FetchLinesAsync(sql, parameters);
    }
}
