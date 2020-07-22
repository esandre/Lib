using System;
using System.Threading.Tasks;

namespace Lib.SQL.Adapter
{
    public enum TransactionResult
    {
        Commit,
        Rollback
    }

    public interface ITransactionControl
    {
        Task ExecuteInTransactionAsync(Func<ICommandChannel, Task<TransactionResult>> whatToDo);
        void ExecuteInTransaction(Func<ICommandChannel, TransactionResult> whatToDo);
    }
}
