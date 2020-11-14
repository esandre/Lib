using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL
{
    public class MySQLCommandChannelFactory : 
        ICommandChannelFactory<MySqlConnectionStringBuilder>, 
        IAsyncCommandChannelFactory<MySqlConnectionStringBuilder>
    {
        private static MySqlConnection RootConnection(MySqlConnectionStringBuilder connectionString)
        {
            var rootConnectionString = new MySqlConnectionStringBuilder(connectionString.ConnectionString) { Database = string.Empty };
            return new MySqlConnection(rootConnectionString.ConnectionString);
        }

        private static async Task CreateDbAsync(MySqlConnectionStringBuilder connectionString)
        {
            await using var conn = RootConnection(connectionString);
            await using var command = conn.CreateCommand();

            await conn.OpenAsync();
            command.CommandText =
                $"CREATE DATABASE IF NOT EXISTS `{connectionString.Database}`; USE `{connectionString.Database}`; ";
            await command.ExecuteNonQueryAsync();
        }

        private static void CreateDb(MySqlConnectionStringBuilder connectionString)
        {
            using var conn = RootConnection(connectionString);
            using var command = conn.CreateCommand();

            conn.Open();
            command.CommandText =
                $"CREATE DATABASE IF NOT EXISTS `{connectionString.Database}`; USE `{connectionString.Database}`; ";
            command.ExecuteNonQuery();
        }

        public ICommandChannel Open(MySqlConnectionStringBuilder connectionString)
        {
            var connection = new Connection(connectionString);
            return new CommandChannel(new ThreadSafeConnection(connection));
        }

        public IAsyncCommandChannel OpenAsync(MySqlConnectionStringBuilder connectionString)
        {
            var connection = new AsyncConnection(connectionString);
            return new AsyncCommandChannel(new AsyncThreadSafeConnection(connection));
        }

        public ICommandChannel Create(MySqlConnectionStringBuilder connectionString, string script, bool eraseIfExists = false)
        {
            if (Exists(connectionString) && !eraseIfExists)
                return Open(connectionString); 

            Delete(connectionString);
            CreateDb(connectionString);

            var adapter = Open(connectionString);
            if (!string.IsNullOrEmpty(script)) adapter.Execute(script);
            return adapter;
        }

        public async Task<IAsyncCommandChannel> CreateAsync(MySqlConnectionStringBuilder connectionString, string script, bool eraseIfExists = false)
        {
            if (await ExistsAsync(connectionString) && !eraseIfExists)
                return OpenAsync(connectionString); 

            await DeleteAsync(connectionString);
            await CreateDbAsync(connectionString);

            var adapter = OpenAsync(connectionString);
            if (!string.IsNullOrEmpty(script)) await adapter.ExecuteAsync(script);
            return adapter;
        }

        public void Delete(MySqlConnectionStringBuilder connectionString)
        {
            using var conn = RootConnection(connectionString);
            using var command = conn.CreateCommand();
            command.CommandText =
                $"DROP DATABASE IF EXISTS `{connectionString.Database}`";

            conn.Open();
            command.ExecuteNonQuery();
        }

        public async Task DeleteAsync(MySqlConnectionStringBuilder connectionString)
        {
            await using var conn = RootConnection(connectionString);
            await using var command = conn.CreateCommand();
            command.CommandText =
                $"DROP DATABASE IF EXISTS `{connectionString.Database}`";

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public bool Exists(MySqlConnectionStringBuilder connectionString)
        {
            using var conn = RootConnection(connectionString);
            using var command = conn.CreateCommand();

            command.CommandText =
                $"SHOW DATABASES LIKE '{connectionString.Database}'";

            conn.Open();
            return command.ExecuteScalar() != null;
        }

        public async Task<bool> ExistsAsync(MySqlConnectionStringBuilder connectionString)
        {
            await using var conn = RootConnection(connectionString);
            await using var command = conn.CreateCommand();

            command.CommandText =
                $"SHOW DATABASES LIKE '{connectionString.Database}'";

            await conn.OpenAsync();
            return await command.ExecuteScalarAsync() != null;
        }
    }
}
