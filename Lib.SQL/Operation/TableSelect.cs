using System;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Operation
{
    public class TableSelect<TExecutor, TResultType> : TableOperation<Select, TExecutor, TResultType> 
        where TExecutor : ExecutorAbstract<TResultType>, new()
    {
        public TableSelect(Table table, string[] columns = null) : base(table, FactorySelect(table, columns))
        {
        }

        private static Select FactorySelect(Table table, string[] columns = null)
        {
            return columns == null
                ? Select.AllFrom(table.Name)
                : Select.From(table.Name, columns)
            ;
        }

        public TableSelect<TExecutor, TResultType> And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public TableSelect<TExecutor, TResultType> Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public TableSelect<TExecutor, TResultType> Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public TableSelect<TExecutor, TResultType> And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public TableSelect<TExecutor, TResultType> Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }

        public TableSelect<TExecutor, TResultType> Join(string tableName, string onClause, JoinType type = JoinType.Inner)
        {
            Statement.Join(tableName, onClause, type);
            return this;
        }

        public TableSelect<TExecutor, TResultType> OrderBy(string expr, OrderDirection direction)
        {
            Statement.OrderBy(expr, direction);
            return this;
        }

        public TableSelect<TExecutor, TResultType> GroupBy(string expr, string havingExpr = "")
        {
            Statement.GroupBy(expr, havingExpr);
            return this;
        }

        public TableSelect<TExecutor, TResultType> Limit(int limit, int offset = 0)
        {
            Statement.Limit(limit, offset);
            return this;
        }

        public TableSelect<TExecutor, TResultType> Distinct()
        {
            Statement.Distinct();
            return this;
        }
    }
}
