using System.Collections.Generic;
using Lib.SQL.Operation.QueryBuilder.Params;

namespace Lib.SQL.Operation.QueryBuilder
{
    public abstract class StatementAbstract
    {
        public abstract string Sql { get; }
        public IDictionary<string, object> Parameters => Params.Params;

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
