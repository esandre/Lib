﻿using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.QueryBuilder
{
    public class Delete : WhereFilteredStatement
    {
        public override string Sql {
            get
            {
                var whereSql = WhereSql;
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
