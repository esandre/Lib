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
        public async Task TestCreationSuccessFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationSuccessFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            await _commandChannelFactory.CreateAsync(new CreationParameters<SqliteConnectionStringBuilder>(connString, Resources.TestCreationSuccess, true));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public async Task TestCreationFailFromFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationFailFromFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            await _commandChannelFactory.CreateAsync(new CreationParameters<SqliteConnectionStringBuilder>(connString, Resources.TestCreationFail, true));
        }

        [TestMethod]
        public async Task TestOpeningSuccess()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestOpeningSuccess.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            await _commandChannelFactory.CreateAsync(new CreationParameters<SqliteConnectionStringBuilder>(connString, "", true));
        }

        [TestMethod]
        public async Task TestIsolationOfMemoryConnections()
        {
            var connStringA = new MemorySqliteConnectionStringBuilder(Guid.NewGuid());
            var connStringB = new MemorySqliteConnectionStringBuilder(Guid.NewGuid());

            var adapter = await _memoryCommandChannelFactory.CreateAsync(new CreationParameters<MemorySqliteConnectionStringBuilder>(connStringA, "CREATE TABLE a (b TEXT)", true));
            await adapter.ExecuteAsync("INSERT INTO a VALUES ('c')");

            adapter = await _memoryCommandChannelFactory.CreateAsync(
                new CreationParameters<MemorySqliteConnectionStringBuilder>(connStringB, "CREATE TABLE a (b TEXT)", true)
                );
            Assert.AreEqual(0, (await adapter.FetchLinesAsync("SELECT * FROM a")).Count);
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
        public async Task TestExceptionsNotSwallowedAsync()
        {
            var connString = new MemorySqliteConnectionStringBuilder();

            await _memoryCommandChannelFactory
                .CreateAsync(new CreationParameters<MemorySqliteConnectionStringBuilder>(connString, "CREATE TABLE a (b TEXT)", true));
            var asyncAdapter = _memoryCommandChannelFactory.OpenAsync(connString);

            await Assert.ThrowsExceptionAsync<SqliteException>(async () => await asyncAdapter.ExecuteAsync("BADREQUEST"));
            await Assert.ThrowsExceptionAsync<SqliteException>(async () => await asyncAdapter.FetchLinesAsync("BADREQUEST"));
            await Assert.ThrowsExceptionAsync<SqliteException>(async () => await asyncAdapter.FetchLineAsync("BADREQUEST"));
            await Assert.ThrowsExceptionAsync<SqliteException>(async () => await asyncAdapter.FetchValueAsync("BADREQUEST"));
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