using System.Collections.Generic;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public sealed class SingleValueExecutor : ExecutorAbstract<object>
    {
        protected override object ExecuteOnAdapter(DbAdapter adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return adapter.FetchValue(sql, parameters);
        }
    }
}
