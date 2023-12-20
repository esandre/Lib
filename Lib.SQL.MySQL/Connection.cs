using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.SQL.Adapter;
using MySqlConnector;

namespace Lib.SQL.MySQL
{
    internal class AsyncConnection : IAsyncConnection
    {
        private readonly MySqlConnection _dbCon;
        private long _lastInsertedId;

        public AsyncConnection(MySqlConnectionStringBuilder sqlConnectionString)
        {
            _dbCon = new MySqlConnection(sqlConnectionString.ConnectionString);
        }

        private MySqlCommand CreateCommand(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var command = new MySqlCommand(sql, _dbCon);
            if (parameters != null)
                command.Parameters.AddRange(
                    parameters.Select(parameter => new MySqlParameter(parameter.Key, parameter.Value)).ToArray());
            return command;
        }

        public async Task<IAsyncSession> BeginTransactionAsync() 
            => await AsyncSavepoint.ConstructAsync(this).ConfigureAwait(false);

        public Task CommitAsync() => Task.CompletedTask;
        public Task RollbackAsync() => Task.CompletedTask;

        public Task OpenAsync() => _dbCon.OpenAsync();
        public Task CloseAsync() => _dbCon.CloseAsync();
        public ValueTask DisposeAsync() => _dbCon.DisposeAsync();
        public void Dispose() => _dbCon.Dispose();
        
        public Task<long> LastInsertedIdAsync() => Task.FromResult(_lastInsertedId);

        public async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await using var command = CreateCommand(sql, parameters);
            var result = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            _lastInsertedId = command.LastInsertedId;
            return result;
        }

        public async Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await using var command = CreateCommand(sql, parameters);
            var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
            _lastInsertedId = command.LastInsertedId;
            return result;
        }

        public async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => (await FetchLinesAsync(sql, parameters).ConfigureAwait(false)).First();

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await using var command = CreateCommand(sql, parameters);
            await using var reader = await command.ExecuteReaderAsync();

            var output = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                var line = Enumerable.Range(0, reader.FieldCount)
                    .ToDictionary(reader.GetName, reader.GetValue);
                output.Add(line);
            }

            _lastInsertedId = command.LastInsertedId;

            return output;
        }
    }
}
