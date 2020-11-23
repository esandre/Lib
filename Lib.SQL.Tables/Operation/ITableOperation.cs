using System.Threading.Tasks;

namespace Lib.SQL.Tables.Operation
{
    public interface ITableOperation<TRecord>
    {
        TRecord ExecuteOn(ICommandChannel on);
        Task<TRecord> ExecuteOnAsync(IAsyncCommandChannel on);
        string Sql { get; }
    }
}
