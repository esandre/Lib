using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;
using Lib.SQL.Executor.Collections;

namespace Lib.SQL.Executor
{
    public class MultipleLinesExecutor : ExecutorAbstract<ResultLines>
    {
        protected override ResultLines ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => new ResultLines(adapter.FetchLines(sql, parameters));

        protected override async Task<ResultLines> ExecuteOnAdapterAsync(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => new ResultLines(await adapter.FetchLinesAsync(sql, parameters));
    }
}
