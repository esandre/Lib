namespace Lib.SQL.Adapter.Session
{
    public interface ITransaction : ISession
    {
        void Commit();
        void Rollback();
    }
}
