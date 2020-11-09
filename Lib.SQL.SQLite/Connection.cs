using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lib.SQL.Adapter;
using Microsoft.Data.Sqlite;

namespace Lib.SQL.SQLite
{
    internal class Connection : Session, IConnection
    {
        private SqliteCommand _lastInsertedCommand;
        private SqliteConnection _sqLiteConnection;
        private readonly string _connectionString;

        public Connection(SqliteConnectionStringBuilder connectionStringBuilder)
        {
            _connectionString = connectionStringBuilder.ConnectionString;

            try
            {
                Open();
                Execute("PRAGMA schema_version");
            }
            catch (SqliteException e)
            {
                throw new InvalidDataException("Database " + connectionStringBuilder.DataSource + " is not sqlite3", e);
            }
            finally
            {
                // ReSharper disable once DoNotCallOverridableMethodsInConstructor
                Close();
            }
        }

        public void Open()
        {
            if (!(_sqLiteConnection is null)) return;

            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            _sqLiteConnection ??= connection;
            _lastInsertedCommand = new SqliteCommand("SELECT last_insert_rowid();", _sqLiteConnection);
        }

        public virtual void Close()
        {
            if (_sqLiteConnection == null) return;

            _sqLiteConnection.Close();
            _lastInsertedCommand.Dispose();
            _sqLiteConnection.Dispose();
            _lastInsertedCommand = null;
            _sqLiteConnection = null;
        }

        public override ITransaction BeginTransaction() => new Savepoint(this);

        public long LastInsertedId { get; private set; }

        private TReturn ExecuteSomethingInCommand<TReturn>(Func<SqliteCommand, TReturn> whatToDo, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var command = new SqliteCommand(sql, _sqLiteConnection);

            if (parameters != null)
                command.Parameters.AddRange(
                    parameters.Select(parameter => new SqliteParameter(parameter.Key, parameter.Value)).ToArray());

            var result = whatToDo(command);

            try
            {
                LastInsertedId = (long) _lastInsertedCommand.ExecuteScalar();
            }
            catch
            {
                LastInsertedId = 0;
            }

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

        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null) 
            => ExecuteSomethingInCommand(command => command.ExecuteNonQuery(), sql, parameters);

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
            => ExecuteSomethingInCommand(command => command.ExecuteScalar(), sql, parameters);

        public IReadOnlyDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return FetchLines(sql, parameters).FirstOrDefault();
        }

        public IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
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

        private bool _isDisposed;

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);

            try
            {
                Close();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        ~Connection()
        {
            Dispose(false);
        }
    }
}
