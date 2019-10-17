using System;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Params;

namespace Lib.SQL.QueryBuilder.Sequences.Where
{
    public class SubSequence
    {
        private WhereSequence _sequence;
        private readonly ParamsCollection _parentCollection;

        internal SubSequence(ParamsCollection parentCollection)
        {
            _parentCollection = parentCollection;
        }

        public WhereSequence Where(string key, IBinaryOperator binOperator, IConvertible value)
        {
            _sequence = new WhereSequence(key, binOperator, value, _parentCollection);
            return _sequence;
        }

        public override string ToString()
        {
            return _sequence?.ToString() ?? string.Empty;
        }
    }

    public class WhereSequence
    {
        private string _sequence;
        private readonly ParamsCollection _parentCollection;

        public WhereSequence(string key, IBinaryOperator binOperator, IConvertible value, ParamsCollection parentCollection)
        {
            _parentCollection = parentCollection;
            _sequence = binOperator.ToString(key, parentCollection.GetIdentifier(value));
        }

        public WhereSequence And(string key, IBinaryOperator binOperator, IConvertible value)
        {
            _sequence += " AND " + binOperator.ToString(key, _parentCollection.GetIdentifier(value));
            return this;
        }

        public WhereSequence Or(string key, IBinaryOperator binOperator, IConvertible value)
        {
            _sequence += " OR " + binOperator.ToString(key, _parentCollection.GetIdentifier(value));
            return this;
        }

        public WhereSequence And(Action<SubSequence> subSequence)
        {
            var sequence = new SubSequence(_parentCollection);
            subSequence.Invoke(sequence);
            _sequence += " AND (" + sequence + ")";

            return this;
        }

        public WhereSequence Or(Action<SubSequence> subSequence)
        {
            var sequence = new SubSequence(_parentCollection);
            subSequence.Invoke(sequence);
            _sequence += " OR (" + sequence + ")";

            return this;
        }

        public override string ToString()
        {
            return _sequence;
        }
    }
}
