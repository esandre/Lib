using System;
using System.Collections.Generic;

namespace Lib.SQL.Executor.Collections
{
    [Serializable]
    public sealed class ResultLine : Dictionary<string, object>
    {
        public ResultLine(IEnumerable<KeyValuePair<string, object>> otherCollection)
        {
            if (otherCollection == null) throw new KeyNotFoundException();
            foreach (var keyValuePair in otherCollection) 
                Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
