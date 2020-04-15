using System.Configuration;

namespace Lib.SQL.MySQL
{
    /// <summary>
    /// Identifiants MySQL ne spécifiant pas de DB
    /// </summary>
    public struct MySQLCredentials
    {
        public string Server;
        public string Username;
        public string Password;
        public string Port;

        public MySQLCredentials(ConnectionStringSettings connectionString)
        {
            var parts = connectionString.ToDictionary();

            Server = parts["Server"];
            Username = parts["User Id"];
            Password = parts["Password"];
            Port = parts["Port"];
        }
    }
}
