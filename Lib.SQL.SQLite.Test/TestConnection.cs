﻿using System;
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
        private SqLiteCommandChannelFactory _commandChannelFactory;

        [TestInitialize]
        public void Initializa()
        {
            _commandChannelFactory = new SqLiteCommandChannelFactory();
        }

        [TestMethod]
        public void TestCreationSuccessFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationSuccessFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            _commandChannelFactory.Create(connString, Resources.TestCreationSuccess, true);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationFailFromFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            _commandChannelFactory.Create(connString, Resources.TestCreationFail, true);
        }

        [TestMethod]
        public void TestOpeningSuccess()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestOpeningSuccess.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            _commandChannelFactory.Create(connString, "", true);
        }

        [TestMethod]
        public void TestSuccessiveConnections()
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = ":memory:" };

            var adapter = _commandChannelFactory.Create(connString, "CREATE TABLE a (b TEXT)", true);
            adapter.Execute("INSERT INTO a VALUES ('c')");
            adapter = _commandChannelFactory.Create(connString, "CREATE TABLE a (b TEXT)", true);
            Assert.AreEqual(0, adapter.FetchLines("SELECT * FROM a").Count());
        }

        [TestMethod]
        public async Task TestMultithreading()
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var adapter = await _commandChannelFactory.CreateAsync(connString, "CREATE TABLE a (b TEXT)", true);

            var tasks = Enumerable.Range(1, 10).Select(async _ =>
            {
                foreach (var __ in Enumerable.Repeat(0, 500)) await adapter.ExecuteAsync("INSERT INTO a VALUES ('c')");
            });

            await Task.WhenAll(tasks);

            Assert.AreEqual(5000, (await adapter.FetchLinesAsync("SELECT * FROM a")).Count());
        }
    }
}