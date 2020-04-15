using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Lib.SQL.MySQL
{
    public static class ConnectionStringSettingsExtension
    {
        public static IDictionary<string, string> ToDictionary(this ConnectionStringSettings connectionString) =>
            connectionString.ConnectionString
                .Split(';')
                .Select(keyValue => keyValue.Split("="))
                .ToDictionary(subPart => subPart.First(), subPart => subPart.Last());
    }
}
