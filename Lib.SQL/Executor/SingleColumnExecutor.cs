using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lib.SQL.Adapter;
using Lib.SQL.Executor.Collections;

namespace Lib.SQL.Executor
{
    public class SingleColumnExecutor : ExecutorAbstract<object[]>
    {
        public string Column;

        protected override object[] ExecuteOnAdapter(DbAdapter adapter, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            if(string.IsNullOrWhiteSpace(Column)) throw new InvalidDataException("Définissez le nom de la colonne à récupérer avant de lancer la requête.");
            return new ResultLines(adapter.FetchLines(sql, parameters)).Select(line => line[Column]).ToArray();
        }
    }
}
