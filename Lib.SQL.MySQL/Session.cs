using Lib.SQL.Adapter.Session;

namespace Lib.SQL.MySQL
{
    internal abstract class Session : ISession
    {
        public abstract ITransaction BeginTransaction();
    }
}
