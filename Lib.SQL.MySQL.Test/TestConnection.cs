using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL.Test
{
    [TestClass]
    public sealed class TestConnection : TestAbstract
    {
        [TestMethod]
        public void TestCreationSuccessFromPlainSql()
        {
            new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCreationSuccess, true));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromFromPlainSql()
        {
            new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCreationFail, true));
        }

        [TestMethod]
        public void TestOpeningSuccess()
        {
            new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "", true));
        }

        [TestMethod]
        public void TestSuccessiveConnections()
        {
            var adapter =new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "CREATE TABLE a (b TEXT)", true));
            adapter.Execute("INSERT INTO a VALUES ('c')");
            adapter = new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "CREATE TABLE a (b TEXT)", true));
            Assert.AreEqual(0, adapter.FetchLines("SELECT * FROM a").Count);
        }

        [TestMethod]
        public void TestMultithreading()
        {
            var mainAdapter = new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "CREATE TABLE a (b TEXT)", true));

            var t1 = new Thread(() =>
            {
                var adapter =  new MySQLCommandChannelFactory().Open(Credentials);
                foreach (var _ in Enumerable.Repeat(0, 50)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            var t2 = new Thread(() =>
            {
                var adapter = new MySQLCommandChannelFactory().Open(Credentials);
                foreach (var _ in Enumerable.Repeat(0, 50)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            var t3 = new Thread(() =>
            {
                var adapter = new MySQLCommandChannelFactory().Open(Credentials);
                foreach (var _ in Enumerable.Repeat(0, 50)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Assert.AreEqual(150, mainAdapter.FetchLines("SELECT * FROM a").Count);
        }
    }
}