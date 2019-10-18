namespace Lib.SQL.SQLite
{
    class MemoryConnection : Connection
    {
        public MemoryConnection() : base(":memory:")
        {
        }

        public override void Close()
        {
        }
    }
}
