using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.SQLite.Test
{
    [TestClass]
    public sealed class TestConnection
    {
        private SqliteCommandChannelFactory _commandChannelFactory;
        private MemorySqliteCommandChannelFactory _memoryCommandChannelFactory;

        [TestInitialize]
        public void Initializa()
        {
            _commandChannelFactory = new SqliteCommandChannelFactory();
            _memoryCommandChannelFactory = new MemorySqliteCommandChannelFactory();
        }

        [TestMethod]
        public void TestCreationSuccessFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationSuccessFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            _commandChannelFactory.Create(new CreationParameters<SqliteConnectionStringBuilder>(connString, Resources.TestCreationSuccess, true));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationFailFromFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            _commandChannelFactory.Create(new CreationParameters<SqliteConnectionStringBuilder>(connString, Resources.TestCreationFail, true));
        }

        [TestMethod]
        public void TestOpeningSuccess()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestOpeningSuccess.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            _commandChannelFactory.Create(new CreationParameters<SqliteConnectionStringBuilder>(connString, "", true));
        }

        [TestMethod]
        public void TestIsolationOfMemoryConnections()
        {
            var connStringA = new MemorySqliteConnectionStringBuilder(Guid.NewGuid());
            var connStringB = new MemorySqliteConnectionStringBuilder(Guid.NewGuid());

            var adapter = _memoryCommandChannelFactory.Create(new CreationParameters<MemorySqliteConnectionStringBuilder>(connStringA, "CREATE TABLE a (b TEXT)", true));
            adapter.Execute("INSERT INTO a VALUES ('c')");

            adapter = _memoryCommandChannelFactory.Create(new CreationParameters<MemorySqliteConnectionStringBuilder>(connStringB, "CREATE TABLE a (b TEXT)", true));
            Assert.AreEqual(0, adapter.FetchLines("SELECT * FROM a").Count);
        }

        [TestMethod]
        public void TestPersistenceOfMemoryConnections()
        {
            var connString = new MemorySqliteConnectionStringBuilder();

            var adapter = _memoryCommandChannelFactory.Create(new CreationParameters<MemorySqliteConnectionStringBuilder>(connString, "CREATE TABLE a (b TEXT)", true));
            adapter.Execute("INSERT INTO a VALUES ('c')");

            adapter = _memoryCommandChannelFactory.Open(connString);
            adapter.Execute("INSERT INTO a VALUES ('d');");

            Assert.AreEqual(2, adapter.FetchLines("SELECT * FROM a").Count);
        }

        [TestMethod]
        public async Task TestPersistenceOfMemoryConnectionsAsync()
        {
            var connString = new MemorySqliteConnectionStringBuilder();

            var adapter = await _memoryCommandChannelFactory.CreateAsync(new CreationParameters<MemorySqliteConnectionStringBuilder>(connString, "CREATE TABLE a (b TEXT)", true));
            await adapter.ExecuteAsync("INSERT INTO a VALUES ('c')");

            adapter = _memoryCommandChannelFactory.OpenAsync(connString);
            await adapter.ExecuteAsync("INSERT INTO a VALUES ('d');");

            var lines = await adapter.FetchLinesAsync("SELECT * FROM a");
            Assert.AreEqual(2, lines.Count);
        }

        [TestMethod]
        public async Task TestPersistenceOfMemoryConnectionsSyncAndAsync()
        {
            var connString = new MemorySqliteConnectionStringBuilder();

            var syncAdapter = _memoryCommandChannelFactory.Create(new CreationParameters<MemorySqliteConnectionStringBuilder>(connString, "CREATE TABLE a (b TEXT)", true));
            syncAdapter.Execute("INSERT INTO a VALUES ('c')");

            var asyncAdapter = _memoryCommandChannelFactory.OpenAsync(connString);
            await asyncAdapter.ExecuteAsync("INSERT INTO a VALUES ('d');");

            var lines = await asyncAdapter.FetchLinesAsync("SELECT * FROM a");
            Assert.AreEqual(2, lines.Count);
        }

        [TestMethod]
        public async Task TestPersistenceOfMemoryConnectionsAsyncAndSync()
        {
            var connString = new MemorySqliteConnectionStringBuilder();

            var syncAdapter = _memoryCommandChannelFactory.Create(new CreationParameters<MemorySqliteConnectionStringBuilder>(connString, "CREATE TABLE a (b TEXT)", true));
            syncAdapter.Execute("INSERT INTO a VALUES ('c')");

            var asyncAdapter = _memoryCommandChannelFactory.OpenAsync(connString);
            await asyncAdapter.ExecuteAsync("INSERT INTO a VALUES ('d');");

            var lines = await asyncAdapter.FetchLinesAsync("SELECT * FROM a");
            Assert.AreEqual(2, lines.Count);
        }

        [TestMethod]
        public async Task TestMultithreading()
        {
            var connString = new MemorySqliteConnectionStringBuilder();
            var adapter = await _memoryCommandChannelFactory.CreateAsync(new CreationParameters<MemorySqliteConnectionStringBuilder>(connString, "CREATE TABLE a (b TEXT)", true));

            var tasks = Enumerable.Range(1, 10).Select(async _ =>
            {
                foreach (var __ in Enumerable.Repeat(0, 500)) 
                    await adapter.ExecuteAsync("INSERT INTO a VALUES ('c')");
            });

            await Task.WhenAll(tasks);

            Assert.AreEqual(5000, (await adapter.FetchLinesAsync("SELECT * FROM a")).Count);
        }
    }
}