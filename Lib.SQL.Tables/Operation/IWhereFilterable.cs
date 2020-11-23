using System;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    public interface IWhereFilterable<out TOperation, TRecord> 
        : ITableOperation<TRecord>
    {
        public IWhereFilterable<TOperation, TRecord> Where(string key, IBinaryOperator comparisonOperator, IConvertible value);
        public IWhereFilterable<TOperation, TRecord> Or(string key, IBinaryOperator comparisonOperator, IConvertible value);
        public IWhereFilterable<TOperation, TRecord> And(string key, IBinaryOperator comparisonOperator, IConvertible value);
        public IWhereFilterable<TOperation, TRecord> Or(Action<SubSequence> sub);
        public IWhereFilterable<TOperation, TRecord> And(Action<SubSequence> sub);
    }
}
