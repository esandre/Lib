using System.Linq;
using Lib.SQL.Executor;
using Lib.SQL.Executor.Collections;
using Lib.SQL.Operation;

namespace Lib.SQL
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

        public TableSelect<TExecutor, TResultType> SelectCustom<TExecutor, TResultType>(params string[] columns) 
            where TExecutor : ExecutorAbstract<TResultType>, new() =>
            columns.Any() 
                ? new TableSelect<TExecutor, TResultType>(this, columns) 
                : new TableSelect<TExecutor, TResultType>(this);

        public TableSelect<SingleValueExecutor, object> Select(string column) 
            => SelectCustom<SingleValueExecutor, object>(column);

        public TableSelect<SingleLineExecutor, ResultLine> SelectLine(params string[] columns) 
            => SelectCustom<SingleLineExecutor, ResultLine>(columns);

        public TableSelect<SingleColumnExecutor, object[]> SelectColumn(string column) 
            => new TableSelect<SingleColumnExecutor, object[]>(this, new[] {column});

        public TableSelect<MultipleLinesExecutor, ResultLines> SelectLines(params string[] columns) 
            => SelectCustom<MultipleLinesExecutor, ResultLines>(columns);

        public TableExists Exists() 
            => new TableExists(this);
    }
}
