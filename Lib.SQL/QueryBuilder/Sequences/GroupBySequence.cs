namespace Lib.SQL.QueryBuilder.Sequences
{
    internal class GroupBySequence
    {
        public string Sql { get; private set; }

        public GroupBySequence()
        {
            Sql = string.Empty;
        }

        public void Set(string expr, string havingExpr = "")
        {
            Sql = " GROUP BY " + expr + (string.IsNullOrWhiteSpace(havingExpr) ? "" : " HAVING " + havingExpr);
        }
    }
}
