using System.Threading.Tasks;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;

namespace Lib.SQL.Tables.Operation
{
    internal abstract class TableOperation<TStatement, TResultType> : ITableOperation<TResultType>
        where TStatement : StatementAbstract
    {
        protected readonly TStatement Statement;
        private readonly IExecutor<TResultType> _executor;
        private readonly string _description;

        protected TableOperation(
            Table table, 
            TStatement statement, 
            IExecutor<TResultType> executor)
        {
            Statement = statement;
            _executor = executor;
            _description = table.Name; 
        }

        public virtual TResultType ExecuteOn(ICommandChannel on) 
            => _executor.ExecuteOnAdapter(on, Statement.Sql, Statement.Parameters);

        public virtual async Task<TResultType> ExecuteOnAsync(IAsyncCommandChannel on) 
            => await _executor.ExecuteOnAdapterAsync(on, Statement.Sql, Statement.Parameters);

        public string Sql => Statement.Sql;

        public override string ToString() => _description;
    }
}
