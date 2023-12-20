using System;
using System.Threading.Tasks;
using Lib.SQL.Tables.Operation;

namespace Lib.SQL.Tables
{
    public static class ExecuteAndReturnRowIdExtensions
    {
        public static async Task<IConvertible> ExecuteOnAndReturnRowIdAsync(this ITableInsert insert, IAsyncCommandChannel on)
        {
            await insert.ExecuteOnAsync(on);
            return await on.LastInsertedIdAsync();
        }
    }
}
