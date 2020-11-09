using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public interface ITransaction : ISession
    {
        void Commit();
        void Rollback();
    }

    public interface IAsyncTransaction : IAsyncSession
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
