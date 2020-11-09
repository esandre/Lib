using System;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    public class TableExists : TableOperation<Exists, bool>
    {
        public TableExists(Table table) : base(table, Exists.InTable(table.Name), new ExistsExecutor())
        {
        }

        public TableExists And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public TableExists Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public TableExists Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public TableExists And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public TableExists Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }
    }
}
