using System;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    internal class TableSelect<TResultType> : TableOperation<Select, TResultType>, ITableSelect<TResultType>
    {
        public TableSelect(IExecutor<TResultType> executor, Table table, string[] columns = null) 
            : base(table, FactorySelect(table, columns), executor)
        {
        }

        private static Select FactorySelect(Table table, string[] columns = null)
        {
            return columns == null
                ? Select.AllFrom(table.Name)
                : Select.From(table.Name, columns);
        }

        public IWhereFilterable<ITableSelect<TResultType>, TResultType> And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public IWhereFilterable<ITableSelect<TResultType>, TResultType> Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<ITableSelect<TResultType>, TResultType> Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<ITableSelect<TResultType>, TResultType> And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<ITableSelect<TResultType>, TResultType> Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }

        public ITableSelect<TResultType> Join(string tableName, string onClause, JoinType type = JoinType.Inner)
        {
            Statement.Join(tableName, onClause, type);
            return this;
        }

        public ITableSelect<TResultType> OrderBy(string expr, OrderDirection direction)
        {
            Statement.OrderBy(expr, direction);
            return this;
        }

        public ITableSelect<TResultType> GroupBy(string expr, string havingExpr = "")
        {
            Statement.GroupBy(expr, havingExpr);
            return this;
        }

        public ITableSelect<TResultType> Limit(int limit, int offset = 0)
        {
            Statement.Limit(limit, offset);
            return this;
        }

        public ITableSelect<TResultType> Distinct()
        {
            Statement.Distinct();
            return this;
        }
    }
}
