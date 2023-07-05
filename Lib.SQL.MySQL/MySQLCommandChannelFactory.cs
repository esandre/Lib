using System.Linq;
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

            await conn.OpenAsync().ConfigureAwait(false);
            command.CommandText =
                $"CREATE DATABASE IF NOT EXISTS `{connectionString.Database}`; USE `{connectionString.Database}`; ";
            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
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

        public ICommandChannel Create(CreationParameters<MySqlConnectionStringBuilder> creationParameters)
        {
            if (Exists(creationParameters.ConnectionString) && !creationParameters.EraseIfExists)
                return Open(creationParameters.ConnectionString); 

            Delete(creationParameters.ConnectionString);
            CreateDb(creationParameters.ConnectionString);

            var adapter = Open(creationParameters.ConnectionString);

            foreach (var script in creationParameters.AdditionalScripts.Prepend(creationParameters.Script))
            {
                if (!string.IsNullOrEmpty(script)) 
                    adapter.Execute(script);
            }
            
            return adapter;
        }

        public async Task<IAsyncCommandChannel> CreateAsync(CreationParameters<MySqlConnectionStringBuilder> creationParameters)
        {
            if (await ExistsAsync(creationParameters.ConnectionString).ConfigureAwait(false) 
                && !creationParameters.EraseIfExists)
                return OpenAsync(creationParameters.ConnectionString); 

            await DeleteAsync(creationParameters.ConnectionString).ConfigureAwait(false);
            await CreateDbAsync(creationParameters.ConnectionString).ConfigureAwait(false);

            var adapter = OpenAsync(creationParameters.ConnectionString);

            foreach (var script in creationParameters.AdditionalScripts.Prepend(creationParameters.Script))
            {
                if (!string.IsNullOrEmpty(script)) 
                    await adapter.ExecuteAsync(script).ConfigureAwait(false);
            }

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

            await conn.OpenAsync().ConfigureAwait(false);
            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
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

            await conn.OpenAsync().ConfigureAwait(false);
            return (await command.ExecuteScalarAsync().ConfigureAwait(false)) != null;
        }
    }
}
