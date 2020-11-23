using System;
using System.Threading.Tasks;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;

namespace Lib.SQL.Tables.Operation
{
    internal class TableInsert : TableOperation<Insert, int>, ITableInsert
    {
        private int _values;

        public TableInsert(Table table) : base(table, Insert.Into(table.Name, table.Columns), new AffectedLinesExecutor())
        {
        }

        public ITableInsert Values(params IConvertible[] values)
        {
            Statement.Values(values);
            _values ++;
            return this;
        }

        public override int ExecuteOn(ICommandChannel on)
        {
            var affectedLines = base.ExecuteOn(on);
            if (affectedLines != _values)
                throw new Exception($"Tentative d'insertion de {_values} lignes, seulement {affectedLines} insérée(s)");
            return affectedLines;
        }

        public override async Task<int> ExecuteOnAsync(IAsyncCommandChannel on)
        {
            var affectedLines = await base.ExecuteOnAsync(on);
            if (affectedLines != _values)
                throw new Exception($"Tentative d'insertion de {_values} lignes, seulement {affectedLines} insérée(s)");
            return affectedLines;
        }
    }
}
