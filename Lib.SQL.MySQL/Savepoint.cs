using Lib.SQL.Adapter;

namespace Lib.SQL.MySQL
{
    internal sealed class Savepoint : Session, ITransaction
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

        public void Commit()
        {
            if (!IsRoot) _connection.Execute("RELEASE SAVEPOINT `" + _id + "`");
            else _connection.Execute("COMMIT");
        }

        public void Rollback()
        {
            if (!IsRoot) _connection.Execute("ROLLBACK TO `" + _id + "`");
            else _connection.Execute("ROLLBACK");
        }

        public override ITransaction BeginTransaction()
        {
            return new Savepoint(this);
        }
    }
}
