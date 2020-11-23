using System;
using System.Collections.Generic;
using Lib.SQL.QueryBuilder.Params;

namespace Lib.SQL.QueryBuilder
{
    public abstract class StatementAbstract
    {
        public abstract string Sql { get; }
        public IDictionary<string, IConvertible> Parameters => Params.Params;

        protected readonly string TableName;
        protected readonly ParamsCollection Params;

        protected StatementAbstract(string tableName)
        {
            TableName = tableName;
            Params = new ParamsCollection();
        }

        public override string ToString()
        {
            return Sql;
        }
    }
}
