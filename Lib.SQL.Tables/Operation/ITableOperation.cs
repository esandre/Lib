using System.Threading.Tasks;

namespace Lib.SQL.Tables.Operation
{
    public interface ITableOperation<TRecord>
    {
        Task<TRecord> ExecuteOnAsync(IAsyncCommandChannel on);
        string Sql { get; }
    }
}
