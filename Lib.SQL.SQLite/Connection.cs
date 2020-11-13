using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.SQL.Adapter;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    internal class AsyncConnection : AsyncConnectionAbstract
    {
        private SqliteCommand _lastInsertedCommand;
        private SqliteConnection _sqLiteConnection;
        private readonly string _connectionString;
        public AsyncConnection(SqliteConnectionStringBuilder connectionStringBuilder)
        {
            _connectionString = connectionStringBuilder.ConnectionString;
        }

        public override async Task<IAsyncSession> BeginTransactionAsync()
            => await AsyncSavepoint.ConstructAsync(this);

        public override async Task OpenAsync()
        {
            if (!(_sqLiteConnection is null)) return;

            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            _sqLiteConnection ??= connection;
            _lastInsertedCommand = new SqliteCommand("SELECT last_insert_rowid();", _sqLiteConnection);
        }

        public override async Task CloseAsync()
        {
            if (_sqLiteConnection == null) return;

            await _sqLiteConnection.CloseAsync();
            await _lastInsertedCommand.DisposeAsync();
            await _sqLiteConnection.DisposeAsync();
            _lastInsertedCommand = null;
            _sqLiteConnection = null;
        }

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

        public override async Task<long> LastInsertedIdAsync() => Convert.ToInt64(await _lastInsertedCommand.ExecuteScalarAsync());

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
            var command = new SqliteCommand(sql, _sqLiteConnection);

            try
            {
                if (parameters != null)
                    command.Parameters.AddRange(
                        parameters.Select(parameter => new SqliteParameter(parameter.Key, parameter.Value)).ToArray());

                return await whatToDo(command);
            }
            finally
            {
                try
                {
                    command.Dispose();
                }
                catch
                {
                    //ignore if dispose fails
                }
            }
        }
    }

    internal class Connection : ConnectionAbstract
    {
        private SqliteCommand _lastInsertedCommand;
        private SqliteConnection _sqLiteConnection;
        private readonly string _connectionString;

        public Connection(SqliteConnectionStringBuilder connectionStringBuilder)
        {
            _connectionString = connectionStringBuilder.ConnectionString;
        }

        public override void Open()
        {
            if (!(_sqLiteConnection is null)) return;

            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            _sqLiteConnection ??= connection;
            _lastInsertedCommand = new SqliteCommand("SELECT last_insert_rowid();", _sqLiteConnection);
        }

        public override void Close()
        {
            if (_sqLiteConnection == null) return;

            _sqLiteConnection.Close();
            _lastInsertedCommand.Dispose();
            _sqLiteConnection.Dispose();
            _lastInsertedCommand = null;
            _sqLiteConnection = null;
        }

        public override ISession BeginTransaction() => new Savepoint(this);

        public override long LastInsertedId => Convert.ToInt64(_lastInsertedCommand.ExecuteScalar());

        private TReturn ExecuteSomethingInCommand<TReturn>(Func<SqliteCommand, TReturn> whatToDo, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var command = new SqliteCommand(sql, _sqLiteConnection);

            if (parameters != null)
                command.Parameters.AddRange(
                    parameters.Select(parameter => new SqliteParameter(parameter.Key, parameter.Value)).ToArray());

            var result = whatToDo(command);

            try
            {
                command.Dispose();
            }
            catch
            {
                //ignore if dispose fails
            }

            return result;
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
