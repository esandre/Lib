using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class ExistsExecutor : IExecutor<bool>
    {
        public bool ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
        {
            var value = adapter.FetchValue(sql, parameters);
            return Convert.ToBoolean(value);
        }

        public async Task<bool> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
        {
            var value = await adapter.FetchValueAsync(sql, parameters);
            return Convert.ToBoolean(value);
        }
    }
}
