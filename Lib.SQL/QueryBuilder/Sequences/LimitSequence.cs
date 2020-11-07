namespace Lib.SQL.QueryBuilder.Sequences
{
    internal class LimitSequence
    {
        public string Sql { get; private set; }

        public LimitSequence()
        {
            Sql = string.Empty;
        }

        public void Set(int limit, int offset = 0)
        {
            Sql = " LIMIT " + limit;
            if (offset != 0) Sql += " OFFSET " + offset;
        }
    }
}
