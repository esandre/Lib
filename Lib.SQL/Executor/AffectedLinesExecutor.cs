using System.Collections.Generic;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public class AffectedLinesExecutor : ExecutorAbstract<int>
    {
        protected override int ExecuteOnAdapter(DbAdapter adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return adapter.Execute(sql, parameters);
        }
    }
}
