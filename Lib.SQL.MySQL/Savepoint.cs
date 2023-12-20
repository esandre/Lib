using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.MySQL
{
    internal class AsyncSavepoint : IAsyncSession
    {
        private bool IsRoot => _id == 0;
        private readonly IAsyncConnection _connection;
        private readonly int _id;

        public static Task<AsyncSavepoint> ConstructAsync(IAsyncConnection parent)
            => ConstructAsync(parent, 0);

        private static async Task<AsyncSavepoint> ConstructAsync(IAsyncConnection parent, int id)
        {
            if(id == 0) await parent.ExecuteAsync("START TRANSACTION").ConfigureAwait(false);
            await parent.ExecuteAsync("SAVEPOINT `" + id + "`").ConfigureAwait(false);

            return new AsyncSavepoint(parent, id);
        }

        private static Task<AsyncSavepoint> ConstructAsync(AsyncSavepoint parent)
            => ConstructAsync(parent._connection, parent._id + 1);

        private AsyncSavepoint(IAsyncConnection parent, int id)
        {
            _id = id;
            _connection = parent;
        }

        public async Task CommitAsync()
        {
            if (!IsRoot) await _connection.ExecuteAsync("RELEASE SAVEPOINT `" + _id + "`").ConfigureAwait(false);
            else await _connection.ExecuteAsync("COMMIT").ConfigureAwait(false);
        }

        public async Task RollbackAsync()
        {
            if (!IsRoot) await _connection.ExecuteAsync("ROLLBACK TO `" + _id + "`").ConfigureAwait(false);
            else await _connection.ExecuteAsync("ROLLBACK").ConfigureAwait(false);
        }

        public Task OpenAsync() => Task.CompletedTask;

        public Task CloseAsync() => Task.CompletedTask;

        public async Task<IAsyncSession> BeginTransactionAsync() 
            => await ConstructAsync(this)
            .ConfigureAwait(false);
    }
}
