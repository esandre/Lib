using System.Collections.Generic;
using System.Linq;
using Lib.SQL.Executor;
using Lib.SQL.Tables.Operation;

namespace Lib.SQL.Tables
{
    public class Table : ITable
    {
        public readonly string Name;
        public readonly string[] Columns;

        public Table(string name, params string[] columns)
        {
            Name = name;
            Columns = columns;
        }

        public ITableInsert Insert() => new TableInsert(this);

        public ITableUpdate Update() => new TableUpdate(this);

        public IWhereFilterable<ITableOperation<int>> Delete() => new TableDelete(this);

        public ITableSelect<object> Select(string column)
        {
            var executor = new SingleValueExecutor();
            return SelectCustom(executor, column);
        }

        public ITableSelect<IReadOnlyDictionary<string, object>> SelectLine(params string[] columns)
        {
            var executor = new SingleLineExecutor();
            return SelectCustom(executor, columns);
        }

        public ITableSelect<IReadOnlyList<object>> SelectColumn(string column) 
            => new TableSelect<IReadOnlyList<object>>(new SingleColumnExecutor(), this, new[] {column});

        public ITableSelect<IReadOnlyList<IReadOnlyDictionary<string, object>>> SelectLines(params string[] columns)
        {
            var executor = new MultipleLinesExecutor();
            return SelectCustom(executor, columns);
        }

        public IWhereFilterable<ITableOperation<bool>> Exists() 
            => new TableExists(this);

        private ITableSelect<TResultType> SelectCustom<TResultType>(
            IExecutor<TResultType> executor,
            params string[] columns) 
            => columns.Any() 
                ? new TableSelect<TResultType>(executor, this, columns) 
                : new TableSelect<TResultType>(executor, this);
    }
}
