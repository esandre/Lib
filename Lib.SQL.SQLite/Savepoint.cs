using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.SQLite
{
    internal class AsyncSavepoint : AsyncTransactionAbstract
    {
        private bool IsRoot => _id == 0;
        private readonly IAsyncConnection _connection;
        private readonly int _id;

        public static async Task<AsyncSavepoint> ConstructAsync(IAsyncConnection parent) 
            => await ConstructAsync(parent, 0);

        private static async Task<AsyncSavepoint> ConstructAsync(AsyncSavepoint parent) 
            => await ConstructAsync(parent._connection, parent._id + 1);

        private static async Task<AsyncSavepoint> ConstructAsync(IAsyncConnection parent, int id)
        {
            await parent.ExecuteAsync("SAVEPOINT '" + id + "'");
            return new AsyncSavepoint(parent, id);
        } 

        private AsyncSavepoint(IAsyncConnection parent, int id)
        {
            _id = id;
            _connection = parent;
        }

        public override async Task<IAsyncSession> BeginTransactionAsync() 
            => await ConstructAsync(this);

        public override async Task CommitAsync() 
            => await _connection.ExecuteAsync("RELEASE '" + _id + "'");

        public override async Task RollbackAsync()
        {
            if (!IsRoot) await _connection.ExecuteAsync("ROLLBACK TO '" + _id + "'");
            else await _connection.ExecuteAsync("ROLLBACK");
        }
    }
}
