using Lib.SQL.Adapter;

namespace Lib.SQL.SQLite
{
    internal abstract class Session : ISession
    {
        public abstract ITransaction BeginTransaction();
    }
}
