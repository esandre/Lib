using System.Threading.Tasks;
using Lib.SQL.Adapter;

namespace Lib.SQL.MySQL
{
    internal class AsyncSavepoint : AsyncTransactionAbstract
    {
        private bool IsRoot => _id == 0;
        private readonly IAsyncConnection _connection;
        private readonly int _id;

        public static async Task<AsyncSavepoint> ConstructAsync(IAsyncConnection parent)
            => await ConstructAsync(parent, 0).ConfigureAwait(false);

        private static async Task<AsyncSavepoint> ConstructAsync(IAsyncConnection parent, int id)
        {
            if(id == 0) await parent.ExecuteAsync("START TRANSACTION").ConfigureAwait(false);
            await parent.ExecuteAsync("SAVEPOINT `" + id + "`").ConfigureAwait(false);

            return new AsyncSavepoint(parent, id);
        }

        private static async Task<AsyncSavepoint> ConstructAsync(AsyncSavepoint parent)
            => await ConstructAsync(parent._connection, parent._id + 1).ConfigureAwait(false);

        private AsyncSavepoint(IAsyncConnection parent, int id)
        {
            _id = id;
            _connection = parent;
        }

        public override async Task CommitAsync()
        {
            if (!IsRoot) await _connection.ExecuteAsync("RELEASE SAVEPOINT `" + _id + "`").ConfigureAwait(false);
            else await _connection.ExecuteAsync("COMMIT").ConfigureAwait(false);
        }

        public override async Task RollbackAsync()
        {
            if (!IsRoot) await _connection.ExecuteAsync("ROLLBACK TO `" + _id + "`").ConfigureAwait(false);
            else await _connection.ExecuteAsync("ROLLBACK").ConfigureAwait(false);
        }

        public override async Task<IAsyncSession> BeginTransactionAsync() 
            => await ConstructAsync(this)
            .ConfigureAwait(false);
    }

    internal class Savepoint : TransactionAbstract
    {
        private bool IsRoot => _id == 0;
        private readonly IConnection _connection;
        private readonly int _id;

        private Savepoint(IConnection parent, int id)
        {
            _id = id;
            _connection = parent;
            if(IsRoot) _connection.Execute("START TRANSACTION");
            _connection.Execute("SAVEPOINT `" + id + "`");
        }

        public Savepoint(IConnection parent) 
            : this(parent, 0)
        {
        }

        private Savepoint(Savepoint parent)
            : this(parent._connection, parent._id + 1)
        {
        }

        public override void Commit()
        {
            if (!IsRoot) _connection.Execute("RELEASE SAVEPOINT `" + _id + "`");
            else _connection.Execute("COMMIT");
        }

        public override void Rollback()
        {
            if (!IsRoot) _connection.Execute("ROLLBACK TO `" + _id + "`");
            else _connection.Execute("ROLLBACK");
        }

        public override ISession BeginTransaction() => new Savepoint(this);
    }
}
