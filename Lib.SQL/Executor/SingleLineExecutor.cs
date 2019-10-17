using System.Collections.Generic;
using Lib.SQL.Adapter;
using Lib.SQL.Executor.Collections;

namespace Lib.SQL.Executor
{
    public class SingleLineExecutor : ExecutorAbstract<ResultLine>
    {
        protected override ResultLine ExecuteOnAdapter(DbAdapter adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return new ResultLine(adapter.FetchLine(sql, parameters));
        }
    }
}
