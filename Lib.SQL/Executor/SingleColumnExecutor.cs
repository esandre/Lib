using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class SingleColumnExecutor : IExecutor<IReadOnlyList<IConvertible>>
    {
        public IReadOnlyList<IConvertible> ExecuteOnAdapter(ICommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
        {
            var resultLines = adapter.FetchLines(sql, parameters);
            return ParseResultLines(resultLines);
        }

        private static IReadOnlyList<IConvertible> ParseResultLines(IEnumerable<IReadOnlyDictionary<string, IConvertible>> lines)
        {
            var linesAsArray = lines.ToArray();
            var columnName = linesAsArray.FirstOrDefault()?.First().Key;
            return columnName is null 
                ? new IConvertible[0] 
                : linesAsArray.Select(line => line[columnName]).ToArray();
        }

        public async Task<IReadOnlyList<IConvertible>> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters)
        {
            var lines = await adapter.FetchLinesAsync(sql, parameters);
            return ParseResultLines(lines);
        }
    }
}
