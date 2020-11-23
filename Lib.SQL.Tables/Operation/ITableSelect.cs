using Lib.SQL.QueryBuilder.Sequences;

namespace Lib.SQL.Tables.Operation
{
    public interface ITableSelect<TResultType> : IWhereFilterable<ITableSelect<TResultType>, TResultType>
    {
        ITableSelect<TResultType> Join(string tableName, string onClause, JoinType type = JoinType.Inner);
        ITableSelect<TResultType> OrderBy(string expr, OrderDirection direction);
        ITableSelect<TResultType> GroupBy(string expr, string havingExpr = "");
        ITableSelect<TResultType> Limit(int limit, int offset = 0);
        ITableSelect<TResultType> Distinct();
    }
}
