using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;
using Lib.SQL.Executor.Collections;

namespace Lib.SQL.Executor
{
    public class SingleLineExecutor : ExecutorAbstract<ResultLine>
    {
        protected override ResultLine ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => new ResultLine(adapter.FetchLine(sql, parameters));

        protected override async Task<ResultLine> ExecuteOnAdapterAsync(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => new ResultLine(await adapter.FetchLineAsync(sql, parameters));
    }
}
