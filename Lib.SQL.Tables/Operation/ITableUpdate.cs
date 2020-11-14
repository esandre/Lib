using System;
using System.Collections.Generic;

namespace Lib.SQL.Tables.Operation
{
    public interface ITableUpdate : IWhereFilterable<ITableUpdate, int>
    {
        ITableUpdate Set(string key, IConvertible value);
        ITableUpdate Set(IEnumerable<KeyValuePair<string, IConvertible>> values);
    }
}
