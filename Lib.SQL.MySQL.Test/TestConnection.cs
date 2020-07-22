using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.MySQL.Test
{
    [TestClass]
    public sealed class TestConnection : TestAbstract
    {
        [TestMethod]
        public void TestCreationSuccessFromScriptFile()
        {
            var script = Path.GetTempFileName();

            CreateSqlFileFromScriptDoSomethingAndDelete(Resources.TestCreationSuccess, script,
                () => Adapter.CreateFromScriptFile(Credentials, script, true));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromScriptFile()
        {
            var script = Path.Combine(Path.GetTempPath(), "TestCreationFailFromScriptFile.sql");

            CreateSqlFileFromScriptDoSomethingAndDelete(Resources.TestCreationFail, script,
                () => Adapter.CreateFromScriptFile(Credentials, script, true));

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
            Adapter.CreateFromPlainScript(Credentials, Resources.TestCreationSuccess, true);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void TestCreationFailFromFromPlainSql()
        {
            Adapter.CreateFromPlainScript(Credentials, Resources.TestCreationFail, true);
        }

        [TestMethod]
        public void TestOpeningSuccess()
        {
            Adapter.CreateFromPlainScript(Credentials, "", true);
        }

        [TestMethod]
        public void TestDispose()
        {
            Adapter.CreateFromPlainScript(Credentials, "", true).Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [TestMethod]
        public void TestSuccessiveConnections()
        {
            var adapter = Adapter.CreateFromPlainScript(Credentials, "CREATE TABLE a (b TEXT)", true);
            adapter.Execute("INSERT INTO a VALUES ('c')");
            adapter = Adapter.CreateFromPlainScript(Credentials, "CREATE TABLE a (b TEXT)", true);
            Assert.AreEqual(0, adapter.FetchLines("SELECT * FROM a").Count());
        }

        [TestMethod]
        public void TestMultithreading()
        {
            var mainAdapter = Adapter.CreateFromPlainScript(Credentials, "CREATE TABLE a (b TEXT)", true);

            var t1 = new Thread(() =>
            {
                var adapter = Adapter.Open(Credentials);
                foreach (var n in Enumerable.Repeat(0, 50)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            var t2 = new Thread(() =>
            {
                var adapter = Adapter.Open(Credentials);
                foreach (var n in Enumerable.Repeat(0, 50)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            var t3 = new Thread(() =>
            {
                var adapter = Adapter.Open(Credentials);
                foreach (var n in Enumerable.Repeat(0, 50)) adapter.Execute("INSERT INTO a VALUES ('c')");
            });

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Assert.AreEqual(150, mainAdapter.FetchLines("SELECT * FROM a").Count());
        }
    }
}