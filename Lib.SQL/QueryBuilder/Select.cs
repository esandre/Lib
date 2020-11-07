using System.Linq;
using Lib.SQL.QueryBuilder.Sequences;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.QueryBuilder
{
    public class Select : WhereFilteredStatement
    {
        public override string Sql => "SELECT " + DistinctSql + _columns + " FROM " + TableName + JoinSql + 
                                      WhereSql + GroupBySql + OrderSql + LimitSql;

        private string DistinctSql => _distinct ? "DISTINCT " : string.Empty;
        private string JoinSql => _join.Sql;
        private string GroupBySql => _group.Sql;
        private string OrderSql => _order.Sql;
        private string LimitSql => _limit.Sql;

        private bool _distinct;
        private readonly string _columns;
        private readonly LimitSequence _limit = new LimitSequence();
        private readonly OrderSequence _order = new OrderSequence();
        private readonly GroupBySequence _group = new GroupBySequence();
        private readonly JoinSequence _join = new JoinSequence();

        private Select(string tableName, params string[] columns) : base(tableName)
        {
            _columns = columns.Aggregate(string.Empty, (current, column) => current + column + ", ").TrimEnd(',', ' ');
        }

        public static Select AllFrom(string tableName)
        {
            return new Select(tableName, "*");
        }

        public static Select From(string tableName, params string[] columns)
        {
            return new Select(tableName, columns);
        }

        public Select Distinct()
        {
            _distinct = true;
            return this;
        }

        public Select Join(string tableName, string onClause, JoinType type = JoinType.Inner)
        {
            _join.Add(tableName, onClause, type);
            return this;
        }

        public Select OrderBy(string expr, OrderDirection direction)
        {
            _order.Add(expr, direction);
            return this;
        }

        public Select GroupBy(string expr, string havingExpr = "")
        {
            _group.Set(expr, havingExpr);
            return this;
        }

        public Select Limit(int limit, int offset = 0)
        {
            _limit.Set(limit, offset);
            return this;
        }
    }
}
