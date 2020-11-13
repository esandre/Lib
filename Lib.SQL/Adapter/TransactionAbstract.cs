using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public abstract class AsyncTransactionAbstract : IAsyncSession
    {
        public Task OpenAsync() => Task.CompletedTask;
        public Task CloseAsync() => Task.CompletedTask;

        public abstract Task<IAsyncSession> BeginTransactionAsync();
        public abstract Task CommitAsync();
        public abstract Task RollbackAsync();
    }

    public abstract class TransactionAbstract : ISession
    {
        public abstract ISession BeginTransaction();

        public abstract void Commit();
        public abstract void Rollback();

        public void Open()
        {
        }

        public void Close()
        {
        }
    }
}
