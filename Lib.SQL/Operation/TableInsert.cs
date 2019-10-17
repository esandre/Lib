using System;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Sequences;

namespace Lib.SQL.Operation
{
    public class TableInsert : TableOperation<Insert, AffectedLinesExecutor, int>
    {
        private readonly Table _table;
        private int _values;

        public TableInsert(Table table) : base(table, Insert.Into(table.Name, table.Columns))
        {
            _table = table;
        }

        public TableInsert Values(params IConvertible[] values)
        {
            Statement.Values(values);
            _values ++;
            return this;
        }

        public new int Execute()
        {
            var affectedLines = base.Execute();
            if (affectedLines != _values)
                throw new Exception(string.Format("Tentative d'insertion de {0} lignes, seulement {1} insérée(s)", _values, affectedLines));
            return affectedLines;
        }

        public long ExecuteAndReturnRowId()
        {
             Execute();
            return _table.LastInsertedId;
        }

        public TableInsert OnError(OrType handler)
        {
            Statement.OnError(handler);
            return this;
        }
    }
}
