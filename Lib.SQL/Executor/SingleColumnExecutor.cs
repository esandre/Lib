using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.SQL.Adapter;
using Lib.SQL.Executor.Collections;

namespace Lib.SQL.Executor
{
    public class SingleColumnExecutor : ExecutorAbstract<object[]>
    {
        protected override object[] ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var resultLines = new ResultLines(adapter.FetchLines(sql, parameters));
            return ParseResultLines(resultLines);
        }

        private static object[] ParseResultLines(ResultLines lines)
        {
            var columnName = lines.FirstOrDefault()?.First().Key;
            return columnName is null 
                ? new object[0] 
                : lines.Select(line => line[columnName]).ToArray();
        }

        protected override async Task<object[]> ExecuteOnAdapterAsync(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var lines = new ResultLines(await adapter.FetchLinesAsync(sql, parameters));
            return ParseResultLines(lines);
        }
    }
}
