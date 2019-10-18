using System;
using System.IO;
using System.Linq;
using System.Threading;
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

            CreateSqlFileFromScriptDoSomethingAndDelete(Resources.TestCreationSuccess, script,
                () => Adapter.CreateFromScriptFile(db, script, true));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromScriptFile()
        {
            var script = Path.Combine(Path.GetTempPath(), "TestCreationFailFromScriptFile.sql");
            var db = Path.Combine(Path.GetTempPath(), "TestCreationFailFromScriptFile.s3db");

            CreateSqlFileFromScriptDoSomethingAndDelete(Resources.TestCreationFail, script,
                () => Adapter.CreateFromScriptFile(db, script, true));

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
            Adapter.CreateFromPlainScript(db, Resources.TestCreationSuccess, true);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromFromPlainSql()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestCreationFailFromFromPlainSql.s3db");
            Adapter.CreateFromPlainScript(db, Resources.TestCreationFail, true);
        }

        [TestMethod]
        public void TestOpeningSuccess()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestOpeningSuccess.s3db");
            Adapter.CreateFromPlainScript(db, "", true);
        }

        [TestMethod]
        public void TestDispose()
        {
            Adapter.CreateFromPlainScript(":memory:", "", true).Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [TestMethod]
        public void TestSuccessiveConnections()
        {
            var adapter = Adapter.CreateFromPlainScript(":memory:", "CREATE TABLE a (b TEXT)", true);
            adapter.Execute("INSERT INTO a VALUES ('c')");
            adapter = Adapter.CreateFromPlainScript(":memory:", "CREATE TABLE a (b TEXT)", true);
            Assert.AreEqual(0, adapter.FetchLines("SELECT * FROM a").Count());
        }

        [TestMethod]
        public void TestMultithreading()
        {
            var adapter = Adapter.CreateFromPlainScript(":memory:", "CREATE TABLE a (b TEXT)", true);

            var t1 = new Thread(() =>
            {
                foreach (var n in Enumerable.Repeat(0, 500)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            var t2 = new Thread(() =>
            {
                foreach (var n in Enumerable.Repeat(0, 500)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            var t3 = new Thread(() =>
            {
                foreach (var n in Enumerable.Repeat(0, 500)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Assert.AreEqual(1500, adapter.FetchLines("SELECT * FROM a").Count());
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), AllowDerivedTypes = true)]
        public void TestOpeningFail()
        {
            var db = Path.Combine(Path.GetTempPath(), "TestOpeningFail.s3db");
            Adapter.Open(db);
        }
    }
}