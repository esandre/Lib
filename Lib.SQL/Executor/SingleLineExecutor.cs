﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class SingleLineExecutor : IExecutor<IReadOnlyDictionary<string, IConvertible>>
    {
        public IReadOnlyDictionary<string, IConvertible> ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null) 
            => adapter.FetchLine(sql, parameters);

        public async Task<IReadOnlyDictionary<string, IConvertible>> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
            => await adapter.FetchLineAsync(sql, parameters);
    }
}
