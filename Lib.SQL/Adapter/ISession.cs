namespace Lib.SQL.Adapter
{
    public interface ISession
    {
        ITransaction BeginTransaction();
    }

    public interface IAsyncSession
    {
        IAsyncTransaction BeginTransactionAsync();
    }
}