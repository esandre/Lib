using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Lib.SQL.Adapter.Session;

namespace Lib.SQL.SQLite
{
    internal class Connection : Session, IConnection
    {
        private SQLiteConnection _sqLiteConnection;
        private readonly string _connectionString;

        public Connection(SQLiteConnectionStringBuilder connectionStringBuilder)
        {
            _connectionString = connectionStringBuilder.ConnectionString;

            try
            {
                Open();
                Execute("PRAGMA schema_version");
            }
            catch (SQLiteException e)
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
            _sqLiteConnection ??= new SQLiteConnection(_connectionString).OpenAndReturn();
        }

        public virtual void Close()
        {
            if (_sqLiteConnection == null) return;
            _sqLiteConnection.Close();
            _sqLiteConnection.Dispose();
            _sqLiteConnection = null;
        }

        public override ITransaction BeginTransaction()
        {
            return new Savepoint(this);
        }

        public long LastInsertedId => _sqLiteConnection.LastInsertRowId;

        private SQLiteCommand CreateCommand(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var command = new SQLiteCommand(sql, _sqLiteConnection);
            if (parameters != null)
                command.Parameters.AddRange(
                    parameters.Select(parameter => new SQLiteParameter(parameter.Key, parameter.Value)).ToArray());
            return command;
        }

        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var command = CreateCommand(sql, parameters);

            lock (_sqLiteConnection)
            {
                return command.ExecuteNonQuery();
            }
        }

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using (var command = CreateCommand(sql, parameters))
            {
                return command.ExecuteScalar();
            }
        }

        public IDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return FetchLines(sql, parameters).FirstOrDefault();
        }

        public IEnumerable<IDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using (var command = CreateCommand(sql, parameters))
            using(var reader = command.ExecuteReader())
            {
                var output = new List<Dictionary<string, object>>();
                while (reader.Read())
                {
                    var line = Enumerable.Range(0, reader.FieldCount)
                        .ToDictionary(reader.GetName, reader.GetValue);
                    output.Add(line);
                }
                return output;
            }
        }
        
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
