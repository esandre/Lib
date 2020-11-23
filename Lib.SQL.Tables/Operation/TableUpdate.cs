using System;
using System.Collections.Generic;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    internal class TableUpdate : TableOperation<Update, int>, ITableUpdate
    {
        public TableUpdate(Table table) : base(table, Update.Table(table.Name), new AffectedLinesExecutor())
        {
        }

        public ITableUpdate Set(string key, IConvertible value)
        {
            Statement.Set(key, value);
            return this;
        }

        public ITableUpdate Set(IEnumerable<KeyValuePair<string, IConvertible>> values)
        {
            Statement.Set(values);
            return this;
        }

        public IWhereFilterable<ITableUpdate, int> And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public IWhereFilterable<ITableUpdate, int> Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<ITableUpdate, int> Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<ITableUpdate, int> And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<ITableUpdate, int> Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }
    }
}
