namespace Lib.SQL.Adapter.Session
{
    public interface ISession
    {
        ITransaction BeginTransaction();
    }
}
