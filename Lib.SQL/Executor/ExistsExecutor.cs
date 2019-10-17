using System;
using System.Collections.Generic;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public class ExistsExecutor : ExecutorAbstract<bool>
    {
        protected override bool ExecuteOnAdapter(DbAdapter adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var value = adapter.FetchValue(sql, parameters);
            return Convert.ToBoolean(value);
        }
    }
}
