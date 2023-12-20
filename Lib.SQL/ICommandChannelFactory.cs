using System;
using System.Collections.Generic;
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
            AdditionalScripts = Array.Empty<string>();
        }
    }

    public interface IAsyncCommandChannelFactory<TConnectionString>
    {
        IAsyncCommandChannel OpenAsync(TConnectionString connectionString);
        Task<IAsyncCommandChannel> CreateAsync(CreationParameters<TConnectionString> creationParameters);
        Task DeleteAsync(TConnectionString connectionString);
        Task<bool> ExistsAsync(TConnectionString connectionString);
    }
}
