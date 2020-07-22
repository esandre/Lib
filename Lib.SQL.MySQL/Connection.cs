using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lib.SQL.Adapter.Session;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL
{
    public class Connection : IConnection
    {
        private readonly MySqlConnection _dbCon;

        public Connection(MySqlConnectionStringBuilder sqlConnectionString)
        {
            _dbCon = new MySqlConnection(sqlConnectionString.ConnectionString);
        }

        private IDbCommand CreateCommand(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            var command = new MySqlCommand(sql, _dbCon);
            if (parameters != null)
                command.Parameters.AddRange(
                    parameters.Select(parameter => new MySqlParameter(parameter.Key, parameter.Value)).ToArray());
            return command;
        }

        public ITransaction BeginTransaction()
        {
            return new Savepoint(this);
        }

        public void Dispose()
        {
            _dbCon.Dispose();
        }

        public void Open()
        {
            _dbCon.Open();
        }

        public void Close()
        {
            _dbCon.Close();
        }

        public long LastInsertedId => Convert.ToInt64(FetchValue("SELECT LAST_INSERT_ID();"));
        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return CreateCommand(sql, parameters).ExecuteNonQuery();
        }

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return CreateCommand(sql, parameters).ExecuteScalar();
        }

        public IDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            return FetchLines(sql, parameters).First();
        }

        public IEnumerable<IDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using (var command = CreateCommand(sql, parameters))
            using (var reader = command.ExecuteReader())
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
    }
}
