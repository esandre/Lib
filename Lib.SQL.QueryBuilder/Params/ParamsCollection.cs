using System;
using System.Collections.Generic;

namespace Lib.SQL.QueryBuilder.Params
{
    public class ParamsCollection
    {
        public IDictionary<string, IConvertible> Params { get; } = new Dictionary<string, IConvertible>();

        public string GetIdentifier(IConvertible param)
        {
            if (param == null) return "NULL";

            var identifier = "@" + Math.Abs(param.GetHashCode());
            if(!Params.ContainsKey(identifier)) Params.Add(identifier, param);
            return identifier;
        }
    }
}
