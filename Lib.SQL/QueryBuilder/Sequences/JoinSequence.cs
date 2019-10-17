namespace Lib.SQL.QueryBuilder.Sequences
{
    public enum JoinType
    {
        Inner,
        Left,
        LeftOuter
    }

    internal class JoinSequence
    {
        public string Sql { get; private set; }

        public JoinSequence()
        {
            Sql = string.Empty;
        }

        public void Add(string tableName, string onClause, JoinType type = JoinType.Inner)
        {
            var joinStatement = "JOIN ";
            if (type == JoinType.LeftOuter) joinStatement = "OUTER " + joinStatement;
            if (type != JoinType.Inner) joinStatement = "LEFT " + joinStatement;
            

            Sql += ' ' + joinStatement + tableName + " ON (" + onClause + ")";
        }
    }
}
