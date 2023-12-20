using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Patterns;
using Lib.SQL.Adapter;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    internal class AsyncConnection : AsyncConnectionAbstract
    {
        private readonly SqliteConnection _sqLiteConnection;
        private readonly SqliteCommand _lastInsertedCommand;
        private long _lastInsertedId;

        public AsyncConnection(SqliteConnection connection)
        {
            _sqLiteConnection = connection;
            _lastInsertedCommand = new SqliteCommand("SELECT last_insert_rowid();", _sqLiteConnection);
        }

        public override async Task<IAsyncSession> BeginTransactionAsync()
            => await AsyncSavepoint.ConstructAsync(this);

        public override Task<long> LastInsertedIdAsync() => Task.FromResult(_lastInsertedId);
        public override async Task OpenAsync() => await _sqLiteConnection.OpenAsync();
        public override async Task CloseAsync() => await _sqLiteConnection.CloseAsync();

        public override async ValueTask DisposeAsync()
        {
            try
            {
                await CloseAsync();
                _lastInsertedCommand.Dispose();
                await _sqLiteConnection.DisposeAsync();
            }
            catch
            {
                // ignored
            }
        }

        public override void Dispose()
        {
            try
            {
                _lastInsertedCommand.Dispose();
                _sqLiteConnection.Close();
                _sqLiteConnection.Dispose();
            }
            catch
            {
                // ignored
            }
        }
        
        public override async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => await ExecuteSomethingInCommand(async command => await command.ExecuteNonQueryAsync(), sql, parameters);

        public override async Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => await ExecuteSomethingInCommand(async command => await command.ExecuteScalarAsync(), sql, parameters);

        public override async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null)
            => (await FetchLinesAsync(sql, parameters)).FirstOrDefault();

        public override async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => await ExecuteSomethingInCommand(async command =>
            {
                await using var reader = await command.ExecuteReaderAsync();

                var output = new List<Dictionary<string, object>>();
                while (await reader.ReadAsync())
                {
                    var line = Enumerable.Range(0, reader.FieldCount)
                        .ToDictionary(reader.GetName, reader.GetValue);
                    output.Add(line);
                }
                return output;
            }, sql, parameters);

        private async Task<TReturn> ExecuteSomethingInCommand<TReturn>(Func<SqliteCommand, Task<TReturn>> whatToDo, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return await AsyncRectifyDispose<SqliteCommand>.UseDisposableResource(
                new SqliteCommand(sql, _sqLiteConnection),
                async command =>
                {
                    if (parameters != null)
                        command.Parameters.AddRange(
                            parameters.Select(parameter => new SqliteParameter(parameter.Key, parameter.Value))
                                .ToArray());

                    var result = await whatToDo(command);
                    _lastInsertedId = (long) await _lastInsertedCommand.ExecuteScalarAsync();
                    return result;
                });
        }
    }
}
