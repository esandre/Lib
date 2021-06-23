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

    internal class Connection : ConnectionAbstract
    {
        private readonly SqliteCommand _lastInsertedCommand;
        private readonly SqliteConnection _sqLiteConnection;
        private long _lastInsertedId;

        public Connection(SqliteConnection connection)
        {
            _sqLiteConnection = connection;
            _lastInsertedCommand = new SqliteCommand("SELECT last_insert_rowid();", _sqLiteConnection);
        }

        public override void Open()
        {
            _sqLiteConnection.Open();
        }

        public override void Close()
        {
            _sqLiteConnection.Close();
        }

        public override ISession BeginTransaction() => new Savepoint(this);

        public override long LastInsertedId => _lastInsertedId;

        private TReturn ExecuteSomethingInCommand<TReturn>(Func<SqliteCommand, TReturn> whatToDo, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return RectifyDispose<SqliteCommand>.UseDisposableResource(
                new SqliteCommand(sql, _sqLiteConnection),
                command =>
                {
                    if (parameters != null)
                        command.Parameters.AddRange(
                            parameters.Select(parameter => new SqliteParameter(parameter.Key, parameter.Value))
                                .ToArray());

                    var result = whatToDo(command);
                    _lastInsertedId = (long) _lastInsertedCommand.ExecuteScalar();
                    return result;
                });
        }

        public override int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => ExecuteSomethingInCommand(command => command.ExecuteNonQuery(), sql, parameters);

        public override object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => ExecuteSomethingInCommand(command => command.ExecuteScalar(), sql, parameters);

        public override IReadOnlyDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => FetchLines(sql, parameters).FirstOrDefault();

        public override IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => ExecuteSomethingInCommand(command =>
            {
                using var reader = command.ExecuteReader();

                var output = new List<Dictionary<string, object>>();
                while (reader.Read())
                {
                    var line = Enumerable.Range(0, reader.FieldCount)
                        .ToDictionary(reader.GetName, reader.GetValue);
                    output.Add(line);
                }
                return output;
            }, sql, parameters);


        public override void Dispose()
        {
            try
            {
                Close();
                _lastInsertedCommand.Dispose();
                _sqLiteConnection.Dispose();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}
