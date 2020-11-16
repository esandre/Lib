using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    public class SqliteCommandChannelFactory : 
        ICommandChannelFactory<SqliteConnectionStringBuilder>, 
        IAsyncCommandChannelFactory<SqliteConnectionStringBuilder>
    {
        private static readonly ReaderWriterLockSlim CreationOrDeletionLock = new ReaderWriterLockSlim();

        private static CommandChannel OpenAdapter(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:") 
                throw new NotSupportedException("Use " + nameof(MemorySqliteCommandChannelFactory));

            CreationOrDeletionLock.EnterReadLock();

            try
            {
                var connection = new Connection(new SqliteConnection(connectionString.ToString()));
                return new CommandChannel(new ThreadSafeConnection(connection));
            }
            finally
            {
                CreationOrDeletionLock.ExitReadLock();
            }
        }

        private static AsyncCommandChannel OpenAdapterAsync(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:") 
                throw new NotSupportedException("Use " + nameof(MemorySqliteCommandChannelFactory));

            CreationOrDeletionLock.EnterReadLock();

            try
            {
                var connection = new AsyncConnection(new SqliteConnection(connectionString.ToString()));
                return new AsyncCommandChannel(new AsyncThreadSafeConnection(connection));
            }
            finally
            {
                CreationOrDeletionLock.ExitReadLock();
            }
        }

        private static void DeleteIfExists(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:") 
                throw new NotSupportedException("Use " + nameof(MemorySqliteCommandChannelFactory));

            CreationOrDeletionLock.EnterWriteLock();

            try
            {
                if (!File.Exists(connectionString.DataSource)) return;
                File.Delete(connectionString.DataSource);
            }
            finally
            {
                CreationOrDeletionLock.ExitWriteLock();
            }
        }

        public ICommandChannel Open(SqliteConnectionStringBuilder connectionString)
            => OpenAdapter(connectionString);

        public IAsyncCommandChannel OpenAsync(SqliteConnectionStringBuilder connectionString)
            => OpenAdapterAsync(connectionString);

        public ICommandChannel Create(CreationParameters<SqliteConnectionStringBuilder> creationParameters)
        {
            if (creationParameters.ConnectionString.DataSource == ":memory:") 
                throw new NotSupportedException("Use " + nameof(MemorySqliteCommandChannelFactory));

            CreationOrDeletionLock.EnterUpgradeableReadLock();

            try
            {
                if(creationParameters.EraseIfExists) DeleteIfExists(creationParameters.ConnectionString);

                CreationOrDeletionLock.EnterWriteLock();

                try
                {
                    var derivedConnectionString = new SqliteConnectionStringBuilder(creationParameters.ConnectionString.ToString()) 
                        { Mode = SqliteOpenMode.ReadWriteCreate };

                    var connection = new SqliteConnection(derivedConnectionString.ToString());
                    connection.Open();
                    var adapter = new CommandChannel(new ThreadSafeConnection(new Connection(new SqliteConnection(creationParameters.ConnectionString.ToString()))));

                    foreach (var script in creationParameters.AdditionalScripts.Prepend(creationParameters.Script))
                    {
                        if(!string.IsNullOrWhiteSpace(script))
                            adapter.Execute(script);
                    }

                    return adapter;
                }
                finally
                {
                    CreationOrDeletionLock.ExitWriteLock();
                }
            }
            finally
            {
                CreationOrDeletionLock.ExitUpgradeableReadLock();
            }
        }

        public async Task<IAsyncCommandChannel> CreateAsync(CreationParameters<SqliteConnectionStringBuilder> creationParameters)
        {
            if (creationParameters.ConnectionString.DataSource == ":memory:") 
                throw new NotSupportedException("Use " + nameof(MemorySqliteCommandChannelFactory));

            CreationOrDeletionLock.EnterUpgradeableReadLock();

            try
            {
                if(creationParameters.EraseIfExists) DeleteIfExists(creationParameters.ConnectionString);

                CreationOrDeletionLock.EnterWriteLock();

                try
                {
                    var derivedConnectionString = new SqliteConnectionStringBuilder(creationParameters.ConnectionString.ToString()) 
                        { Mode = SqliteOpenMode.ReadWriteCreate };

                    var connection = new SqliteConnection(derivedConnectionString.ToString());
                    connection.Open();
                    var adapter = new AsyncCommandChannel(
                        new AsyncThreadSafeConnection(
                            new AsyncConnection(
                                new SqliteConnection(creationParameters.ConnectionString.ToString())
                            )
                        )
                    );

                    foreach (var script in creationParameters.AdditionalScripts.Prepend(creationParameters.Script))
                    {
                        if(!string.IsNullOrWhiteSpace(script))
                            await adapter.ExecuteAsync(script);
                    }

                    return adapter;
                }
                finally
                {
                    CreationOrDeletionLock.ExitWriteLock();
                }
            }
            finally
            {
                CreationOrDeletionLock.ExitUpgradeableReadLock();
            }
        }

        public void Delete(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:") 
                throw new NotSupportedException("Use " + nameof(MemorySqliteCommandChannelFactory));

            CreationOrDeletionLock.EnterUpgradeableReadLock();

            try
            {
                if (!File.Exists(connectionString.DataSource)) return;
                
                CreationOrDeletionLock.EnterWriteLock();

                try
                {
                    File.Delete(connectionString.DataSource);
                }
                finally
                {
                    CreationOrDeletionLock.ExitWriteLock();
                }
            }
            finally
            {
                CreationOrDeletionLock.ExitUpgradeableReadLock();
            }
        }

        public Task DeleteAsync(SqliteConnectionStringBuilder connectionString)
        {
            Delete(connectionString);
            return Task.CompletedTask;
        }

        public bool Exists(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:") 
                throw new NotSupportedException("Use " + nameof(MemorySqliteCommandChannelFactory));

            CreationOrDeletionLock.EnterReadLock();

            try
            {
                return File.Exists(connectionString.DataSource);
            }
            finally
            {
                CreationOrDeletionLock.ExitReadLock();
            }
        }

        public Task<bool> ExistsAsync(SqliteConnectionStringBuilder connectionString) =>
            Task.FromResult(Exists(connectionString));
    }
}
