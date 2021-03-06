﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.SQL.Adapter;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL
{
    internal class AsyncConnection : AsyncConnectionAbstract
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

        public override async Task<IAsyncSession> BeginTransactionAsync() => await AsyncSavepoint.ConstructAsync(this);

        public override async Task OpenAsync() => await _dbCon.OpenAsync();
        public override async Task CloseAsync() => await _dbCon.CloseAsync();
        public override async ValueTask DisposeAsync() => await _dbCon.DisposeAsync();
        public override void Dispose() => _dbCon.Dispose();
        
        public override Task<long> LastInsertedIdAsync() => Task.FromResult(_lastInsertedId);

        public override async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await using var command = CreateCommand(sql, parameters);
            var result = await command.ExecuteNonQueryAsync();
            _lastInsertedId = command.LastInsertedId;
            return result;
        }

        public override async Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await using var command = CreateCommand(sql, parameters);
            var result = await command.ExecuteScalarAsync();
            _lastInsertedId = command.LastInsertedId;
            return result;
        }

        public override async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => (await FetchLinesAsync(sql, parameters)).First();

        public override async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            await using var command = CreateCommand(sql, parameters);
            await using var reader = command.ExecuteReader();

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

    internal class Connection : ConnectionAbstract
    {
        private readonly MySqlConnection _dbCon;
        private long _lastInsertedId;

        public Connection(MySqlConnectionStringBuilder sqlConnectionString)
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

        public override ISession BeginTransaction() => new Savepoint(this);

        public override void Dispose()
        {
            _dbCon.Dispose();
        }

        public override void Open()
        {
            _dbCon.Open();
        }

        public override void Close()
        {
            _dbCon.Close();
        }

        public override long LastInsertedId => _lastInsertedId;

        public override int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var command = CreateCommand(sql, parameters);
            var result = command.ExecuteNonQuery();
            _lastInsertedId = command.LastInsertedId;
            return result;
        }

        public override object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var command = CreateCommand(sql, parameters);
            var result = command.ExecuteScalar();
            _lastInsertedId = command.LastInsertedId;
            return result;
        }

        public override IReadOnlyDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => FetchLines(sql, parameters).First();

        public override IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var command = CreateCommand(sql, parameters);
            using var reader = command.ExecuteReader();

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
