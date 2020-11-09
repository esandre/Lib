using System;
using System.Collections.Generic;
using System.Linq;
using Lib.SQL.QueryBuilder.Params;
using Lib.SQL.QueryBuilder.Sequences;

namespace Lib.SQL.QueryBuilder
{
    public class Insert : StatementAbstract
    {
        public override string Sql => "INSERT " + _orSequence.Sql + "INTO " + TableName + ColumnsSql + " VALUES " + BatchesSql;

        private readonly IEnumerable<string> _columns;
        private readonly OrSequence _orSequence = new OrSequence();
        private readonly IList<InsertBatch> _batches = new List<InsertBatch>(); 

        public static Insert Into(string tableName, params string[] columns)
        {
            return new Insert(tableName, columns);
        }

        public Insert Values(params IConvertible[] values)
        {
            _batches.Add(new InsertBatch(_columns, values, Params));
            return this;
        }

        public Insert OnError(OrType handler)
        {
            _orSequence.Set(handler);
            return this;
        }

        private string ColumnsSql
        {
            get { return _columns.Aggregate(" (", (current, column) => current + (column + ", ")).TrimEnd(' ', ',') + ")"; }
        }

        private string BatchesSql
        {
            get
            {
                if(!_batches.Any()) throw new InvalidOperationException("Un INSERT doit compter au moins une ligne");
                return _batches.Aggregate("", (current, batch) => current + (batch.Sql + ", ")).TrimEnd(' ', ',');
            }
        }

        private Insert(string tableName, IEnumerable<string> columns)
            : base(tableName)
        {
            var array = columns as string[] ?? columns.ToArray();

            if(!array.Any()) throw new InvalidOperationException("Une insertion doit compter au moins une colonne");
            _columns = array;
        }
    }

    internal class InsertBatch
    {
        public InsertBatch(IEnumerable<string> columns, IEnumerable<IConvertible> values, ParamsCollection paramsCollection)
        {
            var colArray = columns as string[] ?? columns.ToArray();
            var valuesArray = values as IConvertible[] ?? values.ToArray();

            if(colArray.Count() != valuesArray.Count()) throw new InvalidOperationException("La ligne doit compter " + colArray.Count() + " colonnes");

            Sql = valuesArray.Aggregate("(", (current, value) => current + (paramsCollection.GetIdentifier(value) + ", ")).TrimEnd(' ', ',') + ")";
        }

        public string Sql { get; }
    }
}
