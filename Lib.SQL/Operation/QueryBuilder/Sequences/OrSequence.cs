namespace Lib.SQL.Operation.QueryBuilder.Sequences
{
    public enum OrType
    {
        Ignore,
        Replace,
        Abort,
        Fail
    }

    internal class OrSequence
    {
        public string Sql { get; private set; }

        public OrSequence()
        {
            Sql = string.Empty;
        }

        public void Set(OrType type)
        {
            switch (type)
            {
                case OrType.Ignore:
                    Sql = "OR IGNORE ";
                    break;
                case OrType.Replace:
                    Sql = "OR REPLACE ";
                    break;
                case OrType.Abort:
                    Sql = "OR ABORT ";
                    break;
                case OrType.Fail:
                    Sql = "OR FAIL ";
                    break;
            }
        }
    }
}
