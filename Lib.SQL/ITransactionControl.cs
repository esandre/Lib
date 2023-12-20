using System;
using System.Threading.Tasks;

namespace Lib.SQL
{
    public enum TransactionResult
    {
        Commit,
        Rollback
    }

    public interface IAsyncTransactionControl
    {
        Task ExecuteInTransactionAsync(Func<IAsyncCommandChannel, Task<TransactionResult>> whatToDo);
    }
}
