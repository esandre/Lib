using System.Data.Common;
using System.Threading.Tasks;

namespace Lib.SQL
{
    public interface ICommandChannelFactory<in TConnectionString> 
        where TConnectionString : DbConnectionStringBuilder
    {
        ICommandChannel Open(TConnectionString connectionString);
        ICommandChannel Create(TConnectionString connectionString, string script, bool eraseIfExists = false);
        void Delete(TConnectionString connectionString);
        bool Exists(TConnectionString connectionString);
    }

    public interface IAsyncCommandChannelFactory<in TConnectionString>
    {
        IAsyncCommandChannel OpenAsync(TConnectionString connectionString);
        Task<IAsyncCommandChannel> CreateAsync(TConnectionString connectionString, string script, bool eraseIfExists = false);
        Task DeleteAsync(TConnectionString connectionString);
        Task<bool> ExistsAsync(TConnectionString connectionString);
    }
}
