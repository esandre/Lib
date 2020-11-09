using Lib.SQL.Adapter;

namespace Lib.SQL.MySQL
{
    internal abstract class Session : ISession
    {
        public abstract ITransaction BeginTransaction();
    }
}
