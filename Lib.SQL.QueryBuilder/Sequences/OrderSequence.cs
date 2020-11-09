namespace Lib.SQL.QueryBuilder.Sequences
{
    public enum OrderDirection : byte
    {
        Asc,
        Desc
    }

    internal class OrderSequence
    {
        public string Sql { get; private set; }

        public OrderSequence()
        {
            Sql = string.Empty;
        }

        public void Add(string expr, OrderDirection direction)
        {
            if (string.IsNullOrWhiteSpace(Sql)) Sql += " ORDER BY ";
            else Sql += ", ";

            Sql += expr + (direction == OrderDirection.Asc ? " ASC" : " DESC");
        }
    }
}
