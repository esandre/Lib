using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Lib.SQL
{
    public class CreationParameters<TConnectionString> 
    {
        public TConnectionString ConnectionString { get; }
        public string Script { get; }
        public bool EraseIfExists { get; }
        public IEnumerable<string> AdditionalScripts { get; set; }

        public CreationParameters(TConnectionString connectionString, string script, bool eraseIfExists = false)
        {
            ConnectionString = connectionString;
            Script = script;
            EraseIfExists = eraseIfExists;
            AdditionalScripts = new string[0];
        }
    }

    public interface ICommandChannelFactory<TConnectionString> 
        where TConnectionString : DbConnectionStringBuilder
    {
        ICommandChannel Open(TConnectionString connectionString);
        ICommandChannel Create(CreationParameters<TConnectionString> creationParameters);
        void Delete(TConnectionString connectionString);
        bool Exists(TConnectionString connectionString);
    }

    public interface IAsyncCommandChannelFactory<TConnectionString>
    {
        IAsyncCommandChannel OpenAsync(TConnectionString connectionString);
        Task<IAsyncCommandChannel> CreateAsync(CreationParameters<TConnectionString> creationParameters);
        Task DeleteAsync(TConnectionString connectionString);
        Task<bool> ExistsAsync(TConnectionString connectionString);
    }
}
