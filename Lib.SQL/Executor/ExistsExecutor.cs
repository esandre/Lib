using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public class ExistsExecutor : ExecutorAbstract<bool>
    {
        protected override bool ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var value = adapter.FetchValue(sql, parameters);
            return Convert.ToBoolean(value);
        }

        protected override async Task<bool> ExecuteOnAdapterAsync(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var value = await adapter.FetchValueAsync(sql, parameters);
            return Convert.ToBoolean(value);
        }
    }
}
