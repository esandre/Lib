using System.Data.SQLite;

namespace Lib.SQL.SQLite
{
    internal class MemoryConnection : Connection
    {
        public MemoryConnection(SQLiteConnectionStringBuilder connectionStringBuilder) 
            : base(connectionStringBuilder)
        {
        }

        public override void Close()
        {
        }

        public override void Dispose()
        {
            base.Close();
        }
    }
}
