using Lib.SQL.Operation.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Operation.QueryBuilder
{
    public class Exists : WhereFilteredStatement
    {
        private readonly Select _underlyingSelect;

        private Exists(string tableName)
            : base(tableName)
        {
            _underlyingSelect = Select.From(TableName, "1");
        }

        public static Exists InTable(string tableName)
        {
            return new Exists(tableName);
        }

        public override string Sql => "SELECT EXISTS (" + _underlyingSelect.Sql + WhereSql + ")";
    }
}
