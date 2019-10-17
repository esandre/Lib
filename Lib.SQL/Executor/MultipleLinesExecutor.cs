using System.Collections.Generic;
using Lib.SQL.Adapter;
using Lib.SQL.Executor.Collections;

namespace Lib.SQL.Executor
{
    public class MultipleLinesExecutor : ExecutorAbstract<ResultLines>
    {
        protected override ResultLines ExecuteOnAdapter(DbAdapter adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return new ResultLines(adapter.FetchLines(sql, parameters));
        }
    }
}
