using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public interface IAsyncSession
    {
        Task<IAsyncSession> BeginTransactionAsync();

        Task CommitAsync();
        Task RollbackAsync();
        Task OpenAsync();
        Task CloseAsync();
    }
}