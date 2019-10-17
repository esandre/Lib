using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;

namespace Lib.SQL
{
    public abstract class TableOperation<TStatement, TExecutor, TResultType>
        where TStatement : StatementAbstract
        where TExecutor : ExecutorAbstract<TResultType>, new()
    {
        internal readonly TExecutor Executor;
        protected readonly TStatement Statement;
        private readonly string _description;

        protected TableOperation(Table table, TStatement statement)
        {
            Statement = statement;
            Executor = new TExecutor {Adapter = table.Adapter.Invoke()};
            _description = table.Name; 
        }

        public TResultType Execute()
        {
            return Executor.Execute(Statement.Sql, Statement.Parameters);
        }

        public string Sql => Statement.Sql;

        public override string ToString()
        {
            return _description;
        }
    }
}
