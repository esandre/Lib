using System;
using Lib.SQL.QueryBuilder.Operator;

namespace Lib.SQL.QueryBuilder.Sequences.Where
{
    public abstract class WhereFilteredStatement : StatementAbstract
    {
        private WhereSequence _where;
        protected string WhereSql => _where == null  ? string.Empty : " WHERE " + _where;

        protected WhereFilteredStatement(string tableName) : base(tableName)
        {
        }

        public WhereFilteredStatement And(Action<SubSequence> sub)
        {
            if (_where == null) throw new InvalidOperationException("La commande AND ne peut être appelée qu'après un WHERE");
            _where.And(sub);
            return this;
        }

        public WhereFilteredStatement Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            if (_where != null) throw new InvalidOperationException("La commande WHERE ne peut être appelée qu'une fois");
            _where = new WhereSequence(key, comparisonOperator, value, Params);
            return this;
        }

        public WhereFilteredStatement Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            if (_where == null) throw new InvalidOperationException("La commande OR ne peut être appelée qu'après un WHERE");
            _where.Or(key, comparisonOperator, value);
            return this;
        }

        public WhereFilteredStatement And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            if (_where == null) throw new InvalidOperationException("La commande AND ne peut être appelée qu'après un WHERE");
            _where.And(key, comparisonOperator, value);
            return this;
        }

        public WhereFilteredStatement Or(Action<SubSequence> sub)
        {
            if (_where == null) throw new InvalidOperationException("La commande OR ne peut être appelée qu'après un WHERE");
            _where.Or(sub);
            return this;
        }
    }
}
