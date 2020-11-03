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
        [TestMethod]
        public void TestCreationSuccessFromScriptFile()
        {
            var script = Path.GetTempFileName();
            var db = Path.GetTempFileName();
            var connString = new SqliteConnectionStringBuilder { DataSource = db };

            CreateSqlFileFromScriptDoSomethingAndDelete(Resources.TestCreationSuccess, script,
                () => Adapter.CreateFromScriptFile(connString, script, true));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromScriptFile()
        {
            var script = Path.Combine(Path.GetTempPath(), "TestCreationFailFromScriptFile.sql");
            var db = Path.Combine(Path.GetTempPath(), "TestCreationFailFromScriptFile.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };

            CreateSqlFileFromScriptDoSomethingAndDelete(Resources.TestCreationFail, script,
                () => Adapter.CreateFromScriptFile(connString, script, true));
        }

        private static void CreateSqlFileFromScriptDoSomethingAndDelete(string script, string file, Action something)
        {
            try
            {
                File.WriteAllText(file, script);
                something.Invoke();
            }
            finally
            {
                File.Delete(file);
            }
        }

        [TestMethod]
        public void TestCreationSuccessFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationSuccessFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            Adapter.CreateFromPlainScript(connString, Resources.TestCreationSuccess, true);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationFailFromFromPlainSql.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            Adapter.CreateFromPlainScript(connString, Resources.TestCreationFail, true);
        }

        [TestMethod]
        public void TestOpeningSuccess()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestOpeningSuccess.s3db");
            var connString = new SqliteConnectionStringBuilder { DataSource = db };
            Adapter.CreateFromPlainScript(connString, "", true);
        }

        [TestMethod]
        public void TestDispose()
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            Adapter.CreateFromPlainScript(connString, "", true).Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [TestMethod]
        public void TestSuccessiveConnections()
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = ":memory:" };

            var adapter = Adapter.CreateFromPlainScript(connString, "CREATE TABLE a (b TEXT)", true);
            adapter.Execute("INSERT INTO a VALUES ('c')");
            adapter = Adapter.CreateFromPlainScript(connString, "CREATE TABLE a (b TEXT)", true);
            Assert.AreEqual(0, adapter.FetchLines("SELECT * FROM a").Count());
        }

        [TestMethod]
        public async Task TestMultithreading()
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var adapter = Adapter.CreateFromPlainScript(connString, "CREATE TABLE a (b TEXT)", true);

            var tasks = Enumerable.Range(1, 10).Select(async _ =>
            {
                foreach (var __ in Enumerable.Repeat(0, 500)) await adapter.ExecuteAsync("INSERT INTO a VALUES ('c')");
            });

            await Task.WhenAll(tasks);

            Assert.AreEqual(5000, (await adapter.FetchLinesAsync("SELECT * FROM a")).Count());
        }
    }
}