﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    public class SqLiteCommandChannelFactory : 
        ICommandChannelFactory<SqliteConnectionStringBuilder>, 
        IAsyncCommandChannelFactory<SqliteConnectionStringBuilder>
    {
        private static readonly ReaderWriterLockSlim CreationOrDeletionLock = new ReaderWriterLockSlim();

        private static CommandChannel OpenAdapter(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:")
                return new CommandChannel(new MemoryConnection(connectionString));

            CreationOrDeletionLock.EnterReadLock();

            try
            {
                var connection = new Connection(connectionString);
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
                return new AsyncCommandChannel(new AsyncMemoryConnection(connectionString));

            CreationOrDeletionLock.EnterReadLock();

            try
            {
                var connection = new AsyncConnection(connectionString);
                return new AsyncCommandChannel(new AsyncThreadSafeConnection(connection));
            }
            finally
            {
                CreationOrDeletionLock.ExitReadLock();
            }
        }

        private static void DeleteIfExists(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:") return;

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

        public ICommandChannel Create(
            SqliteConnectionStringBuilder connectionString, 
            string script,
            bool eraseIfExists = false)
        {
            CreationOrDeletionLock.EnterUpgradeableReadLock();

            try
            {
                if(eraseIfExists) DeleteIfExists(connectionString);

                CreationOrDeletionLock.EnterWriteLock();

                try
                {
                    var derivedConnectionString = new SqliteConnectionStringBuilder(connectionString.ToString()) 
                        { Mode = SqliteOpenMode.ReadWriteCreate };

                    CommandChannel adapter;

                    if (connectionString.DataSource == ":memory:") 
                        adapter = new CommandChannel(new MemoryConnection(connectionString));
                    else
                    {
                        var connection = new SqliteConnection(derivedConnectionString.ToString());
                        connection.Open();
                        adapter = new CommandChannel(new ThreadSafeConnection(new Connection(connectionString)));
                    }

                    if(!string.IsNullOrWhiteSpace(script))
                        adapter.Execute(script);

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

        public async Task<IAsyncCommandChannel> CreateAsync(
            SqliteConnectionStringBuilder connectionString, 
            string script,
            bool eraseIfExists = false)
        {
            CreationOrDeletionLock.EnterUpgradeableReadLock();

            try
            {
                if(eraseIfExists) DeleteIfExists(connectionString);

                CreationOrDeletionLock.EnterWriteLock();

                try
                {
                    var derivedConnectionString = new SqliteConnectionStringBuilder(connectionString.ToString()) 
                        { Mode = SqliteOpenMode.ReadWriteCreate };

                    AsyncCommandChannel adapter;

                    if (connectionString.DataSource == ":memory:") 
                        adapter = new AsyncCommandChannel(new AsyncMemoryConnection(connectionString));
                    else
                    {
                        var connection = new SqliteConnection(derivedConnectionString.ToString());
                        connection.Open();
                        adapter = new AsyncCommandChannel(new AsyncThreadSafeConnection(new AsyncConnection(connectionString)));
                    }

                    if(!string.IsNullOrWhiteSpace(script))
                        await adapter.ExecuteAsync(script);

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
            if (connectionString.DataSource == ":memory:") return;

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
                throw new NotSupportedException(":memory: exists only for reference holder");

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
