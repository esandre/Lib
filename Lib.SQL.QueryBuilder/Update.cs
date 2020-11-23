using System;
using System.Collections.Generic;
using System.Linq;
using Lib.SQL.QueryBuilder.Sequences;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.QueryBuilder
{
    public class Update : WhereFilteredStatement
    {
        public override string Sql => "UPDATE " + _orSequence.Sql + TableName + ColumnsSql + WhereSql;

        private readonly IDictionary<string, string> _columns = new Dictionary<string, string>();
        private readonly OrSequence _orSequence = new OrSequence();

        public static Update Table(string tableName)
        {
            return new Update(tableName);
        }

        public Update Set(string key, IConvertible value)
        {
            if (_columns.ContainsKey(key)) _columns[key] = Params.GetIdentifier(value);
            else _columns.Add(key, Params.GetIdentifier(value));

            return this;
        }

        public Update Set(IEnumerable<KeyValuePair<string, IConvertible>> values)
        {
            foreach (var value in values) Set(value.Key, value.Value);
            return this;
        }

        public Update OnError(OrType handler)
        {
            _orSequence.Set(handler);
            return this;
        }

        private string ColumnsSql
        {
            get
            {
                if(!_columns.Any()) throw new InvalidOperationException("Un UPDATE doit modifier au minimum une colonne !");
                return _columns.Aggregate(" SET ", (current, column) => current + (column.Key + " = " + column.Value) + ", ").TrimEnd(',', ' ');
            }
        }

        private Update(string tableName)
            : base(tableName)
        {
        }
    }
}
