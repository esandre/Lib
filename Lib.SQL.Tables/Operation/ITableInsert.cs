using System;

namespace Lib.SQL.Tables.Operation
{
    public interface ITableInsert : ITableOperation<int>
    {
        ITableInsert Values(params IConvertible[] values);
    }
}
