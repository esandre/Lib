using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public interface ISession
    {
        ISession BeginTransaction();
        void Commit();
        void Rollback();
        void Open();
        void Close();
    }

    public interface IAsyncSession
    {
        Task<IAsyncSession> BeginTransactionAsync();

        Task CommitAsync();
        Task RollbackAsync();
        Task OpenAsync();
        Task CloseAsync();
    }
}