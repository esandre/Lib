using System;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    public class TableDelete : TableOperation<Delete, AffectedLinesExecutor, int>
    {
        public TableDelete(Table table) : base(table, Delete.From(table.Name))
        {
        }

        public TableDelete And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public TableDelete Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public TableDelete Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public TableDelete And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public TableDelete Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }
    }
}
