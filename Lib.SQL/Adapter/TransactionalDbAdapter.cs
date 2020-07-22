using System;
using System.Threading.Tasks;
using Lib.SQL.Adapter.Session;

namespace Lib.SQL.Adapter
{
    public class TransactionalDbAdapter : DbAdapter, ITransactionControl
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

        private void CommitPeek() => ((ITransaction)Peek).Commit();
        private void RollbackPeek() => ((ITransaction)Peek).Rollback();

        public async Task ExecuteInTransactionAsync(Func<ICommandChannel, Task<TransactionResult>> whatToDo)
        {
            await Task.Run(Open);
            var previousPeek = Peek;
            Peek = Peek.BeginTransaction();

            try
            {
                var result = await whatToDo(this);

                switch (result)
                {
                    case TransactionResult.Commit:
                        await Task.Run(CommitPeek);
                        break;
                    case TransactionResult.Rollback:
                        await Task.Run(RollbackPeek);
                        break;
                    default:
                        throw new ApplicationException("Valeur impossible en retour d'une transaction");
                }
            }
            catch (Exception)
            {
                await Task.Run(RollbackPeek);
                throw;
            }
            finally
            {
                Peek = previousPeek;
                await Task.Run(Close);
            }
        }

        public void ExecuteInTransaction(Func<ICommandChannel, TransactionResult> whatToDo)
        {
            Open();
            var previousPeek = Peek;
            Peek = Peek.BeginTransaction();

            try
            {
                var result = whatToDo.Invoke(this);

                switch (result)
                {
                    case TransactionResult.Commit:
                        CommitPeek();
                        break;
                    case TransactionResult.Rollback:
                        RollbackPeek();
                        break;
                    default:
                        throw new ApplicationException("Valeur impossible en retour d'une transaction");
                }
            }
            catch (Exception)
            {
                RollbackPeek();
                throw;
            }
            finally
            {
                Peek = previousPeek;
                Close();
            }
        }
    }
}
