using System;
using Lib.SQL.Adapter.Session;

namespace Lib.SQL.Adapter
{
    public enum TransactionResult
    {
        Commit,
        Rollback
    }

    public class TransactionalDbAdapter : DbAdapter
    {
        private ISession Peek { get; set; }

        protected TransactionalDbAdapter(IConnection connection) : base(connection)
        {
            Peek = connection;
        }

        protected override void Open()
        {
            var connection = Peek as IConnection;
            connection?.Open();
        }

        protected override void Close()
        {
            var connection = Peek as IConnection;
            connection?.Close();
        }

        public void ExecuteInTransaction(Func<TransactionResult> whatToDo)
        {
            Open();
            var previousPeek = Peek;
            Peek = Peek.BeginTransaction();
            try
            {
                var result = whatToDo.Invoke();

                switch (result)
                {
                    case TransactionResult.Commit:
                        ((ITransaction)Peek).Commit();
                        break;
                    case TransactionResult.Rollback:
                        ((ITransaction)Peek).Rollback();
                        break;
                    default:
                        throw new ApplicationException("Valeur impossible en retour d'une transaction");
                }
            }
            catch (Exception)
            {
                ((ITransaction)Peek).Rollback();
            }
            finally
            {
                Peek = previousPeek;
                Close();
            }
        }
    }
}
