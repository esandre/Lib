using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Patterns;
using Lib.SQL.Adapter;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    internal class AsyncConnection : IAsyncConnection
    {
        private readonly SqliteConnection _sqLiteConnection;
        private readonly SqliteCommand _lastInsertedCommand;
        private long _lastInsertedId;

        public AsyncConnection(SqliteConnection connection)
        {
            _sqLiteConnection = connection;
            _lastInsertedCommand = new SqliteCommand("SELECT last_insert_rowid();", _sqLiteConnection);
        }

        public async Task<IAsyncSession> BeginTransactionAsync()
            => await AsyncSavepoint.ConstructAsync(this);

        public Task CommitAsync() => Task.CompletedTask;

        public Task RollbackAsync() => Task.CompletedTask;

        public Task<long> LastInsertedIdAsync() => Task.FromResult(_lastInsertedId);
        public virtual Task OpenAsync() => _sqLiteConnection.OpenAsync();
        public virtual Task CloseAsync() => _sqLiteConnection.CloseAsync();

        public virtual async ValueTask DisposeAsync()
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

        public void Dispose()
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
        
        public Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => ExecuteSomethingInCommand(command => command.ExecuteNonQueryAsync(), sql, parameters);

        public Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => ExecuteSomethingInCommand(command => command.ExecuteScalarAsync(), sql, parameters);

        public async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null)
            => (await FetchLinesAsync(sql, parameters)).FirstOrDefault();

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
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

        private Task<TReturn> ExecuteSomethingInCommand<TReturn>(Func<SqliteCommand, Task<TReturn>> whatToDo, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return AsyncRectifyDispose<SqliteCommand>.UseDisposableResource(
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
