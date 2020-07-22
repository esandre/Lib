using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public abstract class ExecutorAbstract<TReturn>
    {
        public ICommandChannel Adapter;

        public TReturn Execute(string sql, IDictionary<string, object> parameters = null)
        {
            if(null == Adapter) throw new NullReferenceException("Vous devez spécifier un adaptateur sur lequel travailler");
            return ExecuteOnAdapter(Adapter, sql, parameters);
        }

        public async Task<TReturn> ExecuteAsync(string sql, IDictionary<string, object> parameters = null)
        {
            if (null == Adapter) throw new NullReferenceException("Vous devez spécifier un adaptateur sur lequel travailler");
            return await ExecuteOnAdapterAsync(Adapter, sql, parameters);
        }

        protected abstract TReturn ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
        protected abstract Task<TReturn> ExecuteOnAdapterAsync(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
