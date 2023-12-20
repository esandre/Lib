using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    public class MemorySqliteCommandChannelFactory :
        IAsyncCommandChannelFactory<MemorySqliteConnectionStringBuilder>
    {
        private static readonly ConcurrentDictionary<Guid, SqliteConnection> Instances 
            = new ConcurrentDictionary<Guid, SqliteConnection>();

        public async Task<IAsyncCommandChannel> CreateAsync(CreationParameters<MemorySqliteConnectionStringBuilder> creationParameters)
        {
            var mustPlayCreationScripts = true;
            var connection = Instances.AddOrUpdate(
                creationParameters.ConnectionString.MemoryInstanceGuid,
                guid => new SqliteConnection(creationParameters.ConnectionString.ConnectionString),
                (guid, previous) =>
                {
                    mustPlayCreationScripts = creationParameters.EraseIfExists;
                    return creationParameters.EraseIfExists ? new SqliteConnection(creationParameters.ConnectionString.ConnectionString) : previous;
                });

            var channel = new AsyncCommandChannel(new AsyncMemoryConnection(connection));

            if (mustPlayCreationScripts)
            {
                await channel.ExecuteAsync(creationParameters.Script);
                foreach (var creationParametersAdditionalScript in creationParameters.AdditionalScripts)
                    await channel.ExecuteAsync(creationParametersAdditionalScript);
            }

            return channel;
        }

        public void Delete(MemorySqliteConnectionStringBuilder connectionString)
        {
            var removed = Instances.TryRemove(connectionString.MemoryInstanceGuid, out var instance);
            if(removed) instance.Dispose();
        }
        public async Task DeleteAsync(MemorySqliteConnectionStringBuilder connectionString)
        {
            var removed = Instances.TryRemove(connectionString.MemoryInstanceGuid, out var instance);
            if (removed) await instance.DisposeAsync();
        }

        public bool Exists(MemorySqliteConnectionStringBuilder connectionString) =>
            Instances.ContainsKey(connectionString.MemoryInstanceGuid);

        public Task<bool> ExistsAsync(MemorySqliteConnectionStringBuilder connectionString)
            => Task.FromResult(Instances.ContainsKey(connectionString.MemoryInstanceGuid));

        public IAsyncCommandChannel OpenAsync(MemorySqliteConnectionStringBuilder connectionString)
            => new AsyncCommandChannel(new AsyncMemoryConnection(Instances[connectionString.MemoryInstanceGuid]));
    }
}
