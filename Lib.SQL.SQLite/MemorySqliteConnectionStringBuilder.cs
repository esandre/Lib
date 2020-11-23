using System;
using System.Data.Common;

namespace Lib.SQL.SQLite
{
    public class MemorySqliteConnectionStringBuilder : DbConnectionStringBuilder
    {
        public Guid MemoryInstanceGuid { get; }

        public MemorySqliteConnectionStringBuilder() : this(Guid.NewGuid())
        {
        }

        public MemorySqliteConnectionStringBuilder(Guid memoryInstanceGuid)
        {
            MemoryInstanceGuid = memoryInstanceGuid;
        }
    }
}
