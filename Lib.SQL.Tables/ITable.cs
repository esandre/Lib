using System;
using System.Collections.Generic;
using Lib.SQL.Tables.Operation;

namespace Lib.SQL.Tables
{
    public interface ITable
    {
        ITableSelect<IConvertible> Select(string column);
        ITableSelect<IReadOnlyDictionary<string, IConvertible>> SelectLine(params string[] columns);
        ITableSelect<IReadOnlyList<IConvertible>> SelectColumn(string column);
        ITableSelect<IReadOnlyList<IReadOnlyDictionary<string, IConvertible>>> SelectLines(params string[] columns);
        IWhereFilterable<ITableOperation<bool>, bool> Exists();
        ITableInsert Insert();
        ITableUpdate Update();
        IWhereFilterable<ITableOperation<int>, int> Delete();
    }
}
