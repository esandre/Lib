using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public interface IExecutor<TReturn>
    {
        TReturn ExecuteOnAdapter(ICommandChannel adapter, string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);

        Task<TReturn> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);
    }
}
