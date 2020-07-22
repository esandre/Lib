using System;
using System.Collections.Generic;
using Lib.SQL.Executor;
using Lib.SQL.Operation.QueryBuilder;
using Lib.SQL.Operation.QueryBuilder.Operator;
using Lib.SQL.Operation.QueryBuilder.Sequences;
using Lib.SQL.Operation.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Operation
{
    public class TableUpdate : TableOperation<Update, AffectedLinesExecutor, int>
    {
        public TableUpdate(Table table) : base(table, Update.Table(table.Name))
        {
        }

        public TableUpdate Set(string key, IConvertible value)
        {
            Statement.Set(key, value);
            return this;
        }

        public TableUpdate Set(IEnumerable<KeyValuePair<string, IConvertible>> values)
        {
            Statement.Set(values);
            return this;
        }

        public TableUpdate OnError(OrType handler)
        {
            Statement.OnError(handler);
            return this;
        }

        public TableUpdate And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public TableUpdate Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public TableUpdate Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public TableUpdate And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public TableUpdate Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }
    }
}
