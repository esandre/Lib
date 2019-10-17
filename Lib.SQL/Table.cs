using System;
using System.Linq;
using Lib.SQL.Adapter;
using Lib.SQL.Executor;
using Lib.SQL.Executor.Collections;
using Lib.SQL.Operation;

namespace Lib.SQL
{
    public class Table
    {
        public readonly string Name;
        public readonly string[] Columns;

        public readonly Func<DbAdapter> Adapter;

        public Table(Func<DbAdapter> adapter, string name, params string[] columns)
        {
            Adapter = adapter;
            Name = name;
            Columns = columns;
        }

        public void ExecuteInTransaction(Func<TransactionResult> whatToDo)
        {
            var invokee = Adapter.Invoke();
            var adapter = invokee as TransactionalDbAdapter;
            if (adapter != null) adapter.ExecuteInTransaction(whatToDo);
            else throw new NotSupportedException("L'adaptateur fourni ne supporte pas les transactions");
        }
        
        public long LastInsertedId => Adapter.Invoke().LastInsertedId;

        public TableInsert Insert()
        {
            return new TableInsert(this);
        }

        public TableUpdate Update()
        {
            return new TableUpdate(this);
        }

        public TableDelete Delete()
        {
            return new TableDelete(this);
        }

        public TableSelect<TExecutor, TResultType> SelectCustom<TExecutor, TResultType>(params string[] columns) 
            where TExecutor : ExecutorAbstract<TResultType>, new()
        {
            return columns.Any() 
                ? new TableSelect<TExecutor, TResultType>(this, columns) 
                : new TableSelect<TExecutor, TResultType>(this);
        }

        public TableSelect<SingleValueExecutor, object> Select(string column)
        {
            return SelectCustom<SingleValueExecutor, object>(column);
        }

        public TableSelect<SingleLineExecutor, ResultLine> SelectLine(params string[] columns)
        {
            return SelectCustom<SingleLineExecutor, ResultLine>(columns);
        }

        public TableSelect<SingleColumnExecutor, object[]> SelectColumn(string column)
        {
            var select = new TableSelect<SingleColumnExecutor, object[]>(this, new[] {column});
            select.Executor.Column = column;
            return select;
        }

        public TableSelect<MultipleLinesExecutor, ResultLines> SelectLines(params string[] columns)
        {
            return SelectCustom<MultipleLinesExecutor, ResultLines>(columns);
        }

        public TableExists Exists()
        {
            return new TableExists(this);
        }
    }
}
