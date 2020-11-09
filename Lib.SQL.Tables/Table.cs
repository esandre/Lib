using System.Collections.Generic;
using System.Linq;
using Lib.SQL.Executor;
using Lib.SQL.Tables.Operation;

namespace Lib.SQL.Tables
{
    public class Table
    {
        public readonly string Name;
        public readonly string[] Columns;

        public Table(string name, params string[] columns)
        {
            Name = name;
            Columns = columns;
        }

        public TableInsert Insert() => new TableInsert(this);

        public TableUpdate Update() => new TableUpdate(this);

        public TableDelete Delete() => new TableDelete(this);

        private TableSelect<TResultType> SelectCustom<TResultType>(
            IExecutor<TResultType> executor,
            params string[] columns) 
            => columns.Any() 
                ? new TableSelect<TResultType>(executor, this, columns) 
                : new TableSelect<TResultType>(executor, this);

        public TableSelect<object> Select(string column)
        {
            var executor = new SingleValueExecutor();
            return SelectCustom(executor, column);
        }

        public TableSelect<IReadOnlyDictionary<string, object>> SelectLine(params string[] columns)
        {
            var executor = new SingleLineExecutor();
            return SelectCustom(executor, columns);
        }

        public TableSelect<IReadOnlyList<object>> SelectColumn(string column) 
            => new TableSelect<IReadOnlyList<object>>(new SingleColumnExecutor(), this, new[] {column});

        public TableSelect<IReadOnlyList<IReadOnlyDictionary<string, object>>> SelectLines(params string[] columns)
        {
            var executor = new MultipleLinesExecutor();
            return SelectCustom(executor, columns);
        }

        public TableExists Exists() 
            => new TableExists(this);
    }
}
