using System;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    internal class TableExists : TableOperation<Exists, bool>, IWhereFilterable<ITableOperation<bool>>
    {
        public TableExists(Table table) : base(table, Exists.InTable(table.Name), new ExistsExecutor())
        {
        }

        public ITableOperation<bool> And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public ITableOperation<bool> Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public ITableOperation<bool> Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public ITableOperation<bool> And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public ITableOperation<bool> Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }
    }
}
