using System;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    public interface IWhereFilterable<out TOperation>
    {
        public TOperation Where(string key, IBinaryOperator comparisonOperator, IConvertible value);
        public TOperation Or(string key, IBinaryOperator comparisonOperator, IConvertible value);
        public TOperation And(string key, IBinaryOperator comparisonOperator, IConvertible value);
        public TOperation Or(Action<SubSequence> sub);
        public TOperation And(Action<SubSequence> sub);
    }
}
