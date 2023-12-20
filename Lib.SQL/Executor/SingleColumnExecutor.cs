using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.SQL.Executor
{
    public class SingleColumnExecutor : IExecutor<IReadOnlyList<IConvertible>>
    {
        private static IReadOnlyList<IConvertible> ParseResultLines(IEnumerable<IReadOnlyDictionary<string, IConvertible>> lines)
        {
            if (lines is null) return Array.Empty<IConvertible>();

            var linesAsArray = lines.ToArray();
            var columnName = linesAsArray.FirstOrDefault()?.First().Key;

            return columnName is null 
                ? Array.Empty<IConvertible>() 
                : linesAsArray.Select(line => line[columnName]).ToArray();
        }

        public async Task<IReadOnlyList<IConvertible>> ExecuteOnAdapterAsync(IAsyncCommandChannel adapter, string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
        {
            var lines = await adapter.FetchLinesAsync(sql, parameters);
            return ParseResultLines(lines);
        }
    }
}
