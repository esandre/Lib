using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.QueryBuilder
{
    public class Delete : WhereFilteredStatement
    {
        public override string Sql {
            get
            {
                var whereSql = WhereSql;
                if (whereSql == string.Empty) throw new System.Exception("You cannot call delete without Where. At least call Where(1, IsEqualWith, 1)");
                return "DELETE FROM " + TableName + whereSql;
            }
        }

        public static Delete From(string tableName)
        {
            return new Delete(tableName);
        }

        private Delete(string tableName) : base(tableName)
        {
        }
    }
}
