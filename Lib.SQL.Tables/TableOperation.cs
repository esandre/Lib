using System.Threading.Tasks;
using Lib.SQL.Adapter;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;

namespace Lib.SQL.Tables
{
    public abstract class TableOperation<TStatement, TExecutor, TResultType>
        where TStatement : StatementAbstract
        where TExecutor : ExecutorAbstract<TResultType>, new()
    {
        protected readonly TStatement Statement;
        private readonly string _description;

        protected TableOperation(Table table, TStatement statement)
        {
            Statement = statement;
            _description = table.Name; 
        }

        public virtual TResultType ExecuteOn(ICommandChannel on) 
            => new TExecutor { Adapter = on }.Execute(Statement.Sql, Statement.Parameters);

        public virtual async Task<TResultType> ExecuteOnAsync(ICommandChannel on) 
            => await new TExecutor { Adapter = on }.ExecuteAsync(Statement.Sql, Statement.Parameters);

        public string Sql => Statement.Sql;

        public override string ToString() => _description;
    }
}
