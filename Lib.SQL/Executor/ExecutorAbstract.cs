using System;
using System.Collections.Generic;
using Lib.SQL.Adapter;

namespace Lib.SQL.Executor
{
    public abstract class ExecutorAbstract<TReturn>
    {
        public DbAdapter Adapter;

        public TReturn Execute(string sql, IDictionary<string, object> parameters = null)
        {
            if(null == Adapter) throw new NullReferenceException("Vous devez spécifier un adaptateur sur lequel travailler");
            return ExecuteOnAdapter(Adapter, sql, parameters);
        }

        protected abstract TReturn ExecuteOnAdapter(DbAdapter adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null);
    }
}
