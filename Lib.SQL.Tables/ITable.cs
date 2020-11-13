using System.Collections.Generic;
using Lib.SQL.Tables.Operation;

namespace Lib.SQL.Tables
{
    public interface ITable
    {
        ITableSelect<object> Select(string column);
        ITableSelect<IReadOnlyDictionary<string, object>> SelectLine(params string[] columns);
        ITableSelect<IReadOnlyList<object>> SelectColumn(string column);
        ITableSelect<IReadOnlyList<IReadOnlyDictionary<string, object>>> SelectLines(params string[] columns);
        IWhereFilterable<ITableOperation<bool>> Exists();
        ITableInsert Insert();
        ITableUpdate Update();
        IWhereFilterable<ITableOperation<int>> Delete();
    }
}
