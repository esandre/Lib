using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    public class MemorySqliteCommandChannelFactory : 
        ICommandChannelFactory<MemorySqliteConnectionStringBuilder>,
        IAsyncCommandChannelFactory<MemorySqliteConnectionStringBuilder>
    {
        private static readonly ConcurrentDictionary<Guid, SqliteConnection> Instances 
            = new ConcurrentDictionary<Guid, SqliteConnection>();

        public ICommandChannel Create(MemorySqliteConnectionStringBuilder connectionString, string script, bool eraseIfExists = false)
        {
            var connection = Instances.AddOrUpdate(
                connectionString.MemoryInstanceGuid,
                guid => new SqliteConnection(connectionString.ConnectionString),
                (guid, previous) => previous
            );

            var channel = new CommandChannel(new MemoryConnection(connection));
            channel.Execute(script);
            return channel;
        }
        public async Task<IAsyncCommandChannel> CreateAsync(MemorySqliteConnectionStringBuilder connectionString, string script, bool eraseIfExists = false)
        {
            var connection = Instances.AddOrUpdate(
                connectionString.MemoryInstanceGuid,
                guid => new SqliteConnection(connectionString.ConnectionString),
                (guid, previous) => previous
            );

            var channel = new AsyncCommandChannel(new AsyncMemoryConnection(connection));
            await channel.ExecuteAsync(script);
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

        public ICommandChannel Open(MemorySqliteConnectionStringBuilder connectionString)
            => new CommandChannel(new MemoryConnection(Instances[connectionString.MemoryInstanceGuid]));

        public IAsyncCommandChannel OpenAsync(MemorySqliteConnectionStringBuilder connectionString)
            => new AsyncCommandChannel(new AsyncMemoryConnection(Instances[connectionString.MemoryInstanceGuid]));
    }
}
