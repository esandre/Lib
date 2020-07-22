using System;
using System.Threading.Tasks;
using Lib.SQL.Adapter;
using Lib.SQL.Executor;
using Lib.SQL.Operation.QueryBuilder;
using Lib.SQL.Operation.QueryBuilder.Sequences;

namespace Lib.SQL.Operation
{
    public class TableInsert : TableOperation<Insert, AffectedLinesExecutor, int>
    {
        private int _values;

        public TableInsert(Table table) : base(table, Insert.Into(table.Name, table.Columns))
        {
        }

        public TableInsert Values(params IConvertible[] values)
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

        public override async Task<int> ExecuteOnAsync(ICommandChannel on)
        {
            var affectedLines = await base.ExecuteOnAsync(on);
            if (affectedLines != _values)
                throw new Exception($"Tentative d'insertion de {_values} lignes, seulement {affectedLines} insérée(s)");
            return affectedLines;
        }

        public IConvertible ExecuteOnAndReturnRowId(ICommandChannel on)
        {
            ExecuteOn(on);
            return on.LastInsertedId;
        }

        public async Task<IConvertible> ExecuteOnAndReturnRowIdAsync(ICommandChannel on)
        {
            await ExecuteOnAsync(on);
            return on.LastInsertedId;
        }

        public TableInsert OnError(OrType handler)
        {
            Statement.OnError(handler);
            return this;
        }
    }
}
