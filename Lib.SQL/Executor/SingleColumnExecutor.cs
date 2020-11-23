using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class SingleColumnExecutor : IExecutor<IReadOnlyList<object>>
    {
        public IReadOnlyList<object> ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var resultLines = adapter.FetchLines(sql, parameters);
            return ParseResultLines(resultLines);
        }

        private static IReadOnlyList<object> ParseResultLines(IEnumerable<IReadOnlyDictionary<string, object>> lines)
        {
            var linesAsArray = lines.ToArray();
            var columnName = linesAsArray.FirstOrDefault()?.First().Key;
            return columnName is null 
                ? new object[0] 
                : linesAsArray.Select(line => line[columnName]).ToArray();
        }

        public async Task<IReadOnlyList<object>> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var lines = await adapter.FetchLinesAsync(sql, parameters);
            return ParseResultLines(lines);
        }
    }
}
