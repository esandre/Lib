using System;
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

        private static TransactionalDbAdapter OpenAdapter(SqliteConnectionStringBuilder connectionString)
        {
            if (connectionString.DataSource == ":memory:")
                return new TransactionalDbAdapter(new MemoryConnection(connectionString));

            CreationOrDeletionLock.EnterReadLock();

            try
            {
                var connection = new Connection(connectionString);
                return new TransactionalDbAdapter(new ThreadSafeConnection(connection));
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

        public Task<IAsyncCommandChannel> OpenAsync(SqliteConnectionStringBuilder connectionString)
            => Task.FromResult((IAsyncCommandChannel) OpenAdapter(connectionString));

        public ICommandChannel Create(
            SqliteConnectionStringBuilder connectionString, 
            string script, 
            bool eraseIfExists = false)
        {
            if (connectionString.DataSource == ":memory:") 
                return new TransactionalDbAdapter(new MemoryConnection(connectionString));

            CreationOrDeletionLock.EnterUpgradeableReadLock();

            try
            {
                if(eraseIfExists) DeleteIfExists(connectionString);

                CreationOrDeletionLock.EnterWriteLock();

                try
                {
                    var derivedConnectionString = new SqliteConnectionStringBuilder(connectionString.ToString()) 
                        { Mode = SqliteOpenMode.ReadWriteCreate };

                    var connection = new SqliteConnection(derivedConnectionString.ToString());
                    connection.Open();
                }
                finally
                {
                    CreationOrDeletionLock.ExitWriteLock();
                }

                return new TransactionalDbAdapter(new ThreadSafeConnection(new Connection(connectionString)));
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
            if (connectionString.DataSource == ":memory:") 
                return new TransactionalDbAdapter(new MemoryConnection(connectionString));

            CreationOrDeletionLock.EnterUpgradeableReadLock();

            try
            {
                if(eraseIfExists) DeleteIfExists(connectionString);

                CreationOrDeletionLock.EnterWriteLock();

                try
                {
                    var derivedConnectionString = new SqliteConnectionStringBuilder(connectionString.ToString()) 
                        { Mode = SqliteOpenMode.ReadWriteCreate };

                    var connection = new SqliteConnection(derivedConnectionString.ToString());
                    await connection.OpenAsync();
                }
                finally
                {
                    CreationOrDeletionLock.ExitWriteLock();
                }

                return new TransactionalDbAdapter(new ThreadSafeConnection(new Connection(connectionString)));
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
