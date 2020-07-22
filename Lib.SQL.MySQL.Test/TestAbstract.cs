using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL.Test
{
    public class TestAbstract
    {
        protected static MySqlConnectionStringBuilder Credentials
            => new MySqlConnectionStringBuilder { Password = "2zRpZ4fnGcVynUh2", UserID = "cali_badges", Server = "192.168.1.7", Port = 3306, Database = "tmp"};
    }
}
